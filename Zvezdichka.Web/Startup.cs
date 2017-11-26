﻿using System;
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
using Microsoft.Extensions.Options;
using Zvezdichka.Data;
using Zvezdichka.Data.Helpers;
using Zvezdichka.Data.Models;
using Zvezdichka.Services;
using Zvezdichka.Services.Contracts;
using Zvezdichka.Services.Contracts.Entity;
using Zvezdichka.Services.Implementations;
using Zvezdichka.Services.Implementations.Entity;
using Zvezdichka.Web.Extensions.Helpers.Secrets;

namespace Zvezdichka.Web
{
    public class Startup
    {
        public AppKeyConfig AppConfigs { get; }

        public Startup(IConfiguration configuration, IOptions<AppKeyConfig> appkeys)
        {
            this.Configuration = configuration;
            this.AppConfigs = appkeys.Value;
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

//            //Configure app secrets
            services.Configure<AppKeyConfig>(this.Configuration.GetSection("AppKeys"));

            //todo: do the same with the superuser (put the info in the appsecrets)

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IProductsDataService, ProductsDataService>();
            services.AddTransient<ICategoriesDataService, CategoriesDataService>();
            services.AddTransient<IRatingsDataService, RatingsDataService>();
            services.AddTransient<ICommentsDataService, CommentsDataService>();

            //Add external login options
            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = this.Configuration.GetSection("AppKeys")["FacebookAppId"];
                facebookOptions.AppSecret = this.Configuration.GetSection("AppKeys")["FacebookAppSecret"];
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
                    "areas",
                    "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
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
                    await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            //Here you could create a super user who will maintain the web app
            var username = this.Configuration.GetSection("UserSettings")["AdminUsername"];
            var email = this.Configuration.GetSection("UserSettings")["AdminEmail"];

            var superUser = new ApplicationUser
            {
                UserName = username,
                Email = email
            };

            //Ensure you have these values in your appsettings.json or secrets.json file
            var userPwd = this.Configuration.GetSection("UserSettings")["AdminPassword"];
            var user = await userManager.FindByNameAsync(
                this.Configuration.GetSection("UserSettings")["AdminUsername"]);

            if (user == null)
            {
                var createSuperUser = await userManager.CreateAsync(superUser, userPwd);
                if (createSuperUser.Succeeded)
                    await userManager.AddToRoleAsync(superUser, "Admin");
            }
        }
    }
}