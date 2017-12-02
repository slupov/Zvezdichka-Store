using Zvezdichka.Data;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts.Entity;

namespace Zvezdichka.Services.Implementations.Entity
{
    public class CommentsDataService : GenericDataService<Comment>, ICommentsDataService
    {
        public CommentsDataService(ZvezdichkaDbContext dbContext) : base(dbContext)
        {
        }
    }
}