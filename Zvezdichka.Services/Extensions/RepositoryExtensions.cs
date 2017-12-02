using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Zvezdichka.Services.Extensions.Contracts;
using Zvezdichka.Services.Extensions.Implementation;

namespace Zvezdichka.Services.Extensions
{
    /// <summary>
    /// Allows usage of multiple Includes/ThenIncludes and their method chaining
    /// </summary>
    public static class RepositoryExtensions
    {
        public static IIncludableJoin<TEntity, TProperty> Join<TEntity, TProperty>(
            this IQueryable<TEntity> query,
            Expression<Func<TEntity, TProperty>> propToExpand)
            where TEntity : class
        {
            return new IncludableJoin<TEntity, TProperty>(query.Include(propToExpand));
        }

        public static IIncludableJoin<TEntity, TProperty> ThenJoin<TEntity, TPreviousProperty, TProperty>(
            this IIncludableJoin<TEntity, TPreviousProperty> query,
            Expression<Func<TPreviousProperty, TProperty>> propToExpand)
            where TEntity : class
        {
            IIncludableQueryable<TEntity, TPreviousProperty> queryable =
                ((IncludableJoin<TEntity, TPreviousProperty>) query).GetQuery();
            return new IncludableJoin<TEntity, TProperty>(queryable.ThenInclude(propToExpand));
        }

        public static IIncludableJoin<TEntity, TProperty> ThenJoin<TEntity, TPreviousProperty, TProperty>(
            this IIncludableJoin<TEntity, ICollection<TPreviousProperty>> query,
            Expression<Func<TPreviousProperty, TProperty>> propToExpand)
            where TEntity : class
        {
            var queryable = ((IncludableJoin<TEntity, ICollection<TPreviousProperty>>) query).GetQuery();
            var include = queryable.ThenInclude(propToExpand);
            return new IncludableJoin<TEntity, TProperty>(include);
        }
    }
}