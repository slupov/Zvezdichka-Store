using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zvezdichka.Data;
using Zvezdichka.Data.Helpers;
using Zvezdichka.Data.Models;
using Zvezdichka.Services;
using Zvezdichka.Services.Contracts;
using Zvezdichka.Services.Contracts.Entity;
using Zvezdichka.Services.Implementations;
using Zvezdichka.Services.Implementations.Entity;

namespace Zvezdichka.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // DI - This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ZvezdichkaDbContext>(options =>
                options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    // Password settings
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 3;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredUniqueChars = 2;

                    //user settings
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ZvezdichkaDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "ZvezdichkaCookie";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
                // Requires `using Microsoft.AspNetCore.Authentication.Cookies;`
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            });

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IProductsDataService, ProductsDataService>();
            services.AddTransient<ICategoriesDataService, CategoriesDataService>();
            services.AddTransient<IRatingsDataService, RatingsDataService>();
            services.AddTransient<ICommentsDataService, CommentsDataService>();

            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = this.Configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = this.Configuration["Authentication:Facebook:AppSecret"];
            });

            services.AddAutoMapper();

            services.AddMvc(options =>
            {
                //adds global antiforgery defense for server data alterations from outside
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //scope seed db
            Task.Run(() =>
            {
                using (var serviceScope =
                    app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetService<ZvezdichkaDbContext>();
                    context.Database.Migrate();
                    context.EnsureSeedData();
                }
            });

            CreateRoles(serviceProvider).Wait();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = {"Admin", "Manager", "Member"};

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //TODO: Remove this in production
            //Here you could create a super user who will maintain the web app
            var username = this.Configuration.GetSection("UserSettings")["AdminUsername"];
            var email = this.Configuration.GetSection("UserSettings")["AdminEmail"];

            var superUser = new ApplicationUser
            {
                UserName = username,
                Email = email
            };

            //Ensure you have these values in your appsettings.json file
            string userPwd = this.Configuration.GetSection("UserSettings")["AdminPassword"];
            var user = await userManager.FindByNameAsync(
                this.Configuration.GetSection("UserSettings")["AdminUsername"]);

            if (user == null)
            {
                var createSuperUser = await userManager.CreateAsync(superUser, userPwd);
                if (createSuperUser.Succeeded)
                {
                    //here we tie the new user to the role
                    await userManager.AddToRoleAsync(superUser, "Admin");
                }
            }
        }
    }
}