using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zvezdichka.Data.EntityConfigurations.Extensions;
using Zvezdichka.Data.Models;

namespace Zvezdichka.Data.EntityConfigurations
{
    public class CartItemConfiguration : DbEntityConfiguration<CartItem>
    {
        public override void Configure(EntityTypeBuilder<CartItem> entity)
        {
            entity.ToTable("CartItems");

            entity
                .HasOne(c => c.Product)
                .WithMany(p => p.Carts);

            entity.HasOne(c => c.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.UserId);
        }
    }
}