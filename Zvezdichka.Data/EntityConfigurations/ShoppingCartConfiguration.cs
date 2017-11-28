using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zvezdichka.Data.EntityConfigurations.Extensions;
using Zvezdichka.Data.Models;

namespace Zvezdichka.Data.EntityConfigurations
{
    public class ShoppingCartConfiguration : DbEntityConfiguration<ShoppingCart>
    {
        public override void Configure(EntityTypeBuilder<ShoppingCart> entity)
        {
            entity.ToTable("ShoppingCarts");

            entity
                .HasMany(c => c.CartItems)
                .WithOne(ci => ci.ShoppingCart);

            //user has a single cart only
            entity
                .HasIndex(c => c.UserId)
                .IsUnique();
        }
    }
}