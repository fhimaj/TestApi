using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestApi.Domain.Entities.Posts;

namespace TestApi.Infrastructure.Configurations
{
    public class PostConfiguration : BaseEntityConfiguration<Post>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Post> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Title)
                .HasColumnType("CLOB")
                .IsRequired(false);
            builder.Property(e => e.Description)
                .HasColumnType("CLOB")
                .IsRequired(false);
            builder.Property(e => e.Content)
                .HasColumnType("CLOB")
                .IsRequired(false);
            builder.Property(e => e.Url)
               .HasColumnType("CLOB")
               .IsRequired(false);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Posts)
                .HasForeignKey(x => x.UserId)
                .IsRequired();
        }
    }
}
