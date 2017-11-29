using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zvezdichka.Data.EntityConfigurations.Extensions;
using Zvezdichka.Data.Models;

namespace Zvezdichka.Data.EntityConfigurations
{
    public class ImageSourceConfiguration : DbEntityConfiguration<ImageSource>
    {
        public override void Configure(EntityTypeBuilder<ImageSource> entity)
        {
            entity
                .ToTable("ImageSources");

            entity
                .HasOne(c => c.Product)
                .WithMany(p => p.ImageSources);
        }
    }
}