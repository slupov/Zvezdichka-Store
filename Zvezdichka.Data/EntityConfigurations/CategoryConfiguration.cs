using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zvezdichka.Data.EntityConfigurations.Extensions;
using Zvezdichka.Data.Models;

namespace Zvezdichka.Data.EntityConfigurations
{
    public class CategoryConfiguration : DbEntityConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> entity)
        {
            entity.ToTable("Categories");

            entity
                .HasIndex(x => x.Name)
                .IsUnique();
        }
    }
}
