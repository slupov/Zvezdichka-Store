using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Zvezdichka.Services.Extensions.Contracts;

namespace Zvezdichka.Services.Extensions.Implementation
{
    /// <summary>
    /// Custom version of IncludableQueryable. Holds the tree of previous properties to allow for strongly type later includes.
    /// </summary>
    /// <typeparam name="TEntity">The entity model for the database.</typeparam>
    /// <typeparam name="TPreviousProperty">Previous navigation property. Used for strongly typed later includes</typeparam>
    public class IncludableJoin<TEntity, TPreviousProperty> : IIncludableJoin<TEntity, TPreviousProperty>
    {
        private readonly IIncludableQueryable<TEntity, TPreviousProperty> query;

        public IncludableJoin(IIncludableQueryable<TEntity, TPreviousProperty> query)
        {
            this.query = query;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return this.query.GetEnumerator();
        }

        public Expression Expression => this.query.Expression;
        public Type ElementType => this.query.ElementType;
        public IQueryProvider Provider => this.query.Provider;

        internal IIncludableQueryable<TEntity, TPreviousProperty> GetQuery()
        {
            return this.query;
        }
    }
}
