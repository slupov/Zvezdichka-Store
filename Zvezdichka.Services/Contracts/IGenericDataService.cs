using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Zvezdichka.Services.Extensions.Contracts;

namespace Zvezdichka.Services.Contracts
{
    /// <summary>
    /// The repository for an entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericDataService<T> where T : class
    {
        IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties);

        IList<T> GetList(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties);

        T GetSingle(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties);

        void Add(params T[] items);

        void Update(params T[] items);

        void Remove(params T[] items);

        bool Any(Func<T, bool> where,
            params Expression<Func<T, object>>[] navigationProperties);

        bool Any();

        /// <summary>
        /// The starting point for eager loading.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="navigationProperty">The parent navigation property</param>
        /// <returns></returns>
        IIncludableJoin<T, TProperty> Join<TProperty>(Expression<Func<T, TProperty>> navigationProperty);
    }
}