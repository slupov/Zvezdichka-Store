using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zvezdichka.Data.EntityConfigurations.Extensions;
using Zvezdichka.Data.Models;

namespace Zvezdichka.Data.EntityConfigurations
{
    public class CartsConfiguration : DbEntityConfiguration<Cart>
    {
        public override void Configure(EntityTypeBuilder<Cart> entity)
        {
            entity.ToTable("Carts");

            entity
                .HasOne(c => c.Product)
                .WithMany(p => p.Carts);

            entity.HasOne(c => c.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.UserId);
        }
    }
}