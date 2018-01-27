using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zvezdichka.Data.EntityConfigurations.Extensions;

namespace Zvezdichka.Data.EntityConfigurations.Distributor
{
    public class DistributorConfiguration : DbEntityConfiguration<Models.Distributors.Distributor>
    {
        public override void Configure(EntityTypeBuilder<Models.Distributors.Distributor> entity)
        {
            entity
                .HasIndex(x => x.Name)
                .IsUnique();
        }
    }
}