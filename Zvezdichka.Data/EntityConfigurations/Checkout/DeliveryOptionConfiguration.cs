using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zvezdichka.Data.EntityConfigurations.Extensions;
using Zvezdichka.Data.Models;
using Zvezdichka.Data.Models.Checkout;

namespace Zvezdichka.Data.EntityConfigurations.Checkout
{
    public class DeliveryOptionConfiguration : DbEntityConfiguration<DeliveryOption>
    {
        public override void Configure(EntityTypeBuilder<DeliveryOption> entity)
        {
            entity
                .HasIndex(x => x.Name)
                .IsUnique();

            entity
                .Property(x => x.Name)
                .HasDefaultValue("");
        }
    }
}