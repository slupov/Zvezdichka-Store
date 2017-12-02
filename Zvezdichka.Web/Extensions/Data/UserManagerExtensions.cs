using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Zvezdichka.Data;
using Zvezdichka.Data.Extensions;
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
            return Task.Run(() =>
            {
                ApplicationUser item = null;

                using (var context = new ZvezdichkaDbContext(new DbContextOptions<ZvezdichkaDbContext>()))
                {
                    IQueryable<ApplicationUser> dbQuery = context.Set<ApplicationUser>();

                    //Apply eager loading
                    foreach (Expression<Func<ApplicationUser, object>> navigationProperty in navigationProperties)
                    {
                        dbQuery = dbQuery.Include(navigationProperty);
                    }

                    Console.WriteLine(new string('-',20));
                    Console.WriteLine(dbQuery.ToSql());

                    item = dbQuery
                        .AsNoTracking() //Don't track any changes for the selected item
                        .FirstOrDefault(u => u.UserName == username); //Apply where clause
                }

                return item;
            }).GetAwaiter().GetResult();
        }

        #region Todo

        /// <summary>
        /// Includes passed navigation property. If its a Select() to a dependency it will be included too. 
        /// </summary>
        /// <returns>The queryable to the db table with navigationProperty and its dependencies included </returns>
        private static IQueryable<ApplicationUser> NestedInclude(this IQueryable<ApplicationUser> dbQuery,
            Expression<Func<ApplicationUser, object>> navigationProperty)
        {
            if (navigationProperty.NodeType == ExpressionType.Lambda
                && navigationProperty.Body.NodeType == ExpressionType.MemberAccess)
            {
                return dbQuery.Include(navigationProperty);
            }
            else if (navigationProperty.NodeType == ExpressionType.Lambda &&
                     navigationProperty.Body.NodeType == ExpressionType.Call
                ) //using select call for nested dependencies
                //navigationProperty.DependencyCollection.Select(dc => dc.Dependency)
            {
                if (!(navigationProperty.Body is MethodCallExpression call))
                {
                    throw new ArgumentException("Not a method call");
                }

                foreach (Expression argument in call.Arguments)
                {
                    if (argument is LambdaExpression)
                    {
                        dbQuery = dbQuery.Include(navigationProperty);
                        //could be going further down
                    }
                }

                return dbQuery;
            }

            return dbQuery;
        }

        #endregion

        public static async Task<ApplicationUser> FindByEmailAsync(this UserManager<ApplicationUser> manager,
            string email, params Expression<Func<ApplicationUser, object>>[] navigationProperties)
        {
            return Task.Run(() =>
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
            }).GetAwaiter().GetResult();
        }

        public static async Task<ApplicationUser> FindByIdAsync(this UserManager<ApplicationUser> manager, string id,
            params Expression<Func<ApplicationUser, object>>[] navigationProperties)
        {
            return Task.Run(() =>
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
            }).GetAwaiter().GetResult();
        }
    }
}