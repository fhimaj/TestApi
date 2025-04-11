using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestApi.Domain.Entities.Users;

namespace TestApi.Infrastructure.Configurations
{
    public class UserConfiguration : BaseEntityConfiguration<User>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.FirstName)
                .HasColumnType("CLOB")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.LastName)
                .HasColumnType("CLOB")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.DOB)
                .HasColumnType("date")
                .IsRequired();

            builder.Property(e => e.Email)
                .HasColumnType("CLOB")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.UserName)
               .HasColumnType("CLOB")
               .IsRequired()
               .HasMaxLength(50);

            builder.Property(e => e.PasswordHash)
                .IsRequired(); builder.Property(e => e.PasswordHash)
                .HasColumnType("CLOB")
                .IsRequired();

            builder.Property(e => e.Preferences)
                    .HasColumnType("CLOB")
                    .HasDefaultValue()
                    .IsRequired(false);
        }
    }
}
