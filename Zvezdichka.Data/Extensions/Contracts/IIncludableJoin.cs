using System.Linq;

namespace Zvezdichka.Data.Extensions.Contracts
{
    public interface IIncludableJoin<out TEntity, out TProperty> : IQueryable<TEntity>
    {
    }
}
