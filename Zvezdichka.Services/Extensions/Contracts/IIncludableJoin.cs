using System.Linq;

namespace Zvezdichka.Services.Extensions.Contracts
{
    public interface IIncludableJoin<out TEntity, out TProperty> : IQueryable<TEntity>
    {
    }
}
