using System;
using System.Linq.Expressions;

namespace Zvezdichka.Data.Extensions.Contracts
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// The starting point for eager loading.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="navigationProperty">The parent navigation property</param>
        /// <returns></returns>
        IIncludableJoin<TEntity, TProperty> Join<TProperty>(Expression<Func<TEntity, TProperty>> navigationProperty);
    }
}
