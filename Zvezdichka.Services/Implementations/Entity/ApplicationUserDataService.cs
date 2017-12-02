using Zvezdichka.Data;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts.Entity;

namespace Zvezdichka.Services.Implementations.Entity
{
    /// <summary>
    /// Could still use UserManager<ApplicationUser> but this data service gives you the ability to chain eager loading with the Join(navProp) method
    /// </summary>
    public class ApplicationUserDataService : GenericDataService<ApplicationUser>, IApplicationUserDataService
    {
        public ApplicationUserDataService(ZvezdichkaDbContext dbContext) : base(dbContext)
        {
        }
    }
}