using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zvezdichka.Data.EntityConfigurations.Extensions;
using Zvezdichka.Data.Models;
using Zvezdichka.Data.Models.Checkout;

namespace Zvezdichka.Data.EntityConfigurations.Checkout
{
    public class PaymentOptionConfiguration : DbEntityConfiguration<PaymentOption>
    {
        public override void Configure(EntityTypeBuilder<PaymentOption> entity)
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