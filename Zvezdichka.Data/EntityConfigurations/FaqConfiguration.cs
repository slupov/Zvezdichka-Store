using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zvezdichka.Data.EntityConfigurations.Extensions;
using Zvezdichka.Data.Models;

namespace Zvezdichka.Data.EntityConfigurations
{
    public class FaqConfiguration : DbEntityConfiguration<Faq>
    {
        public override void Configure(EntityTypeBuilder<Faq> entity)
        {
            entity.ToTable("Faqs");

            entity
                .Property(x => x.Answer)
                .HasDefaultValue("");

            entity
                .Property(x => x.Title)
                .HasDefaultValue("");
        }
    }
}