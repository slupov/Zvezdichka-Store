using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Zvezdichka.Data;
using Zvezdichka.Data.Models;
using Zvezdichka.Web.Models;

namespace Zvezdichka.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IServiceProvider serviceProvider;

        public AdminController(UserManager<ApplicationUser> userManager, IServiceProvider serviceProvider)
        {
            this.userManager = userManager;
            this.serviceProvider = serviceProvider;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Users()
        {
            var usersWithRoles = new List<UserWithRolesModel>();
            var users = this.userManager.Users;

            foreach (var user in users)
                usersWithRoles.Add(new UserWithRolesModel()
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Roles = this.userManager.GetRolesAsync(user).GetAwaiter().GetResult().ToArray()
                });

            return View(usersWithRoles);
        }

        //manage user
        public IActionResult EditUser(string username)
        {
            var user = this.userManager.FindByNameAsync(username).GetAwaiter().GetResult();

            if (user == null)
            {
                return NotFound();
            }

            this.ViewData["ReturnUrl"] = "/Admin/Users";

            var roles = this.serviceProvider
                .GetRequiredService<RoleManager<IdentityRole>>()
                .Roles
                .Select(x => x.Name);

            this.ViewData["AppRoles"] = roles;

            return View(new UserWithRolesModel()
            {
                Username = user.UserName,
                Email = user.Email,
                Roles = this.userManager.GetRolesAsync(user).GetAwaiter().GetResult().ToArray()
            });
        }

        public async Task<IActionResult> DeleteUser(string username)
        {
            //delete user
            var user = await this.userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound();
            }

            await this.userManager.DeleteAsync(user);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChangeUserRole(string username, string newRole)
        {
            var user = await this.userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound();
            }

            var isInRole = this.userManager.IsInRoleAsync(user, newRole).GetAwaiter().GetResult();

            if (isInRole)
            {
                //remove role
                await this.userManager.RemoveFromRoleAsync(user, newRole);
                return RedirectToAction(nameof(EditUser), new {username = username});
            }

            //add role
            await this.userManager.AddToRoleAsync(user, newRole);
            return RedirectToAction(nameof(EditUser), new {username = username});
        }
    }
}