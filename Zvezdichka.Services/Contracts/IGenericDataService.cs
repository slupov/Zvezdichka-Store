using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Zvezdichka.Services.Contracts
{
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
    }
}