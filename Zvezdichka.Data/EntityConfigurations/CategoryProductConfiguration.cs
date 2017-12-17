using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zvezdichka.Data.Models.Mapping;
using Zvezdichka.Data.EntityConfigurations.Extensions;

namespace Zvezdichka.Data.EntityConfigurations
{
    public class CategoryProductConfiguration : DbEntityConfiguration<CategoryProduct>
    {
        public override void Configure(EntityTypeBuilder<CategoryProduct> entity)
        {
            entity.HasKey(bc => new
                {
                    bc.ProductId,
                    bc.CategoryId,
            });

            entity.HasOne(bc => bc.Category)
                .WithMany(b => b.Products)
                .HasForeignKey(bc => bc.CategoryId);

            entity.HasOne(bc => bc.Product)
                .WithMany(c => c.Categories)
                .HasForeignKey(bc => bc.ProductId);
        }
    }
}