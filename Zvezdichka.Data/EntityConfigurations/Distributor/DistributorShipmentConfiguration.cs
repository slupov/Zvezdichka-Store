using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zvezdichka.Data.EntityConfigurations.Extensions;
using Zvezdichka.Data.Models.Distributors;

namespace Zvezdichka.Data.EntityConfigurations.Distributor
{
    public class DistributorShipmentConfiguration : DbEntityConfiguration<DistributorShipment>
    {
        public override void Configure(EntityTypeBuilder<DistributorShipment> entity)
        {
            entity
                .Property(x => x.Date)
                .HasDefaultValue(DateTime.Now);
        }
    }
}