using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Zvezdichka.Data;
using Zvezdichka.Data.Models;

namespace Zvezdichka.Web.Extensions.Data
{
    /// <summary>
    /// Class extending UserManager in order to add eagerly loaded navigation properties via expressions
    /// </summary>
    public static class UserManagerExtensions
    {
        public static async Task<ApplicationUser> FindByNameAsync(this UserManager<ApplicationUser> manager,
            string username, params Expression<Func<ApplicationUser, object>>[] navigationProperties)
        {
            await Task.Run(() =>
            {
                ApplicationUser item = null;
                using (var context = new ZvezdichkaDbContext(new DbContextOptions<ZvezdichkaDbContext>()))
                {
                    IQueryable<ApplicationUser> dbQuery = context.Set<ApplicationUser>();

                    //Apply eager loading
                    foreach (Expression<Func<ApplicationUser, object>> navigationProperty in navigationProperties)
                        dbQuery = dbQuery.Include<ApplicationUser, object>(navigationProperty);

                    item = dbQuery
                        .AsNoTracking() //Don't track any changes for the selected item
                        .FirstOrDefault(u => u.UserName == username); //Apply where clause
                }
                return item;
            });

            return null;
        }

        public static async Task<ApplicationUser> FindByEmailAsync(this UserManager<ApplicationUser> manager,
            string email, params Expression<Func<ApplicationUser, object>>[] navigationProperties)
        {
            await Task.Run(() =>
            {
                ApplicationUser item = null;
                using (var context = new ZvezdichkaDbContext(new DbContextOptions<ZvezdichkaDbContext>()))
                {
                    IQueryable<ApplicationUser> dbQuery = context.Set<ApplicationUser>();

                    //Apply eager loading
                    foreach (Expression<Func<ApplicationUser, object>> navigationProperty in navigationProperties)
                        dbQuery = dbQuery.Include<ApplicationUser, object>(navigationProperty);

                    item = dbQuery
                        .AsNoTracking() //Don't track any changes for the selected item
                        .FirstOrDefault(u => u.Email == email); //Apply where clause
                }
                return item;
            });

            return null;
        }

        public static async Task<ApplicationUser> FindByIdAsync(this UserManager<ApplicationUser> manager, string id,
            params Expression<Func<ApplicationUser, object>>[] navigationProperties)
        {
            await Task.Run(() =>
            {
                ApplicationUser item = null;
                using (var context = new ZvezdichkaDbContext(new DbContextOptions<ZvezdichkaDbContext>()))
                {
                    IQueryable<ApplicationUser> dbQuery = context.Set<ApplicationUser>();

                    //Apply eager loading
                    foreach (Expression<Func<ApplicationUser, object>> navigationProperty in navigationProperties)
                        dbQuery = dbQuery.Include<ApplicationUser, object>(navigationProperty);

                    item = dbQuery
                        .AsNoTracking() //Don't track any changes for the selected item
                        .FirstOrDefault(u => u.Id == id); //Apply where clause
                }
                return item;
            });

            return null;
        }
    }
}