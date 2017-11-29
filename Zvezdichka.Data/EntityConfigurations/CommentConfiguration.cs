using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zvezdichka.Data.EntityConfigurations.Extensions;
using Zvezdichka.Data.Models;

namespace Zvezdichka.Data.EntityConfigurations
{
    public class CommentConfiguration : DbEntityConfiguration<Comment>
    {
        public override void Configure(EntityTypeBuilder<Comment> entity)
        {
            entity.ToTable("Comments");

            entity
                .HasOne(c => c.Product)
                .WithMany(p => p.Comments);
        }
    }
}
