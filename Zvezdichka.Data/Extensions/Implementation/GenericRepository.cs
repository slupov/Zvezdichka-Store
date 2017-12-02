using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Zvezdichka.Data.Extensions.Contracts;

namespace Zvezdichka.Data.Extensions.Implementation
{
    public abstract class GenericRepository<TInterface, TEntity> : IRepository<TInterface>
        where TEntity : class, new()
        where TInterface : class
    {
        protected DbSet<TEntity> _dbSet;

        protected GenericRepository(ZvezdichkaDbContext dbContext)
        {
            this._dbSet = dbContext.Set<TEntity>();
        }

        public IIncludableJoin<TInterface, TProperty> Join<TProperty>(
            Expression<Func<TInterface, TProperty>> navigationProperty)
        {
            return ((IQueryable<TInterface>) this._dbSet).Join(navigationProperty);
        }
    }
}