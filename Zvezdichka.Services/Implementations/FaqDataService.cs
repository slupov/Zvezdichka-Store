using Zvezdichka.Data;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts.Entity;

namespace Zvezdichka.Services.Implementations
{
    public class FaqDataService : GenericDataService<Faq>, IFaqDataService
    {
        public FaqDataService(ZvezdichkaDbContext dbContext) : base(dbContext)
        {
        }
    }
}