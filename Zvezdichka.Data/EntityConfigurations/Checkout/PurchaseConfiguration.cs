using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zvezdichka.Data.EntityConfigurations.Extensions;
using Zvezdichka.Data.Models.Checkout;

namespace Zvezdichka.Data.EntityConfigurations.Checkout
{
    public class PurchaseConfiguration : DbEntityConfiguration<Purchase>
    {
        public override void Configure(EntityTypeBuilder<Purchase> entity)
        {
            entity
                .HasOne(x => x.Customer)
                .WithMany(x => x.Purchases)
                .HasForeignKey(x => x.CustomerId);

            entity
                .Property(x => x.IsOnline)
                .HasDefaultValue(false);
        }
    }
}