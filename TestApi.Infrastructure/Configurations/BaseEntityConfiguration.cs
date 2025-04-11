using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestApi.Domain.Entities;

namespace TestApi.Infrastructure.Configurations
{
    public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                    .HasColumnType("NUMBER(10)")
                    .IsRequired();

            builder.Property(e => e.InsertDate)
                   .HasColumnType("date")
                   .IsRequired();

            builder.Property(e => e.UpdateDate)
                   .HasColumnType("date")
                   .IsRequired();

            builder.Property(e => e.InsertedBy)
                    .HasColumnType("NUMBER(10)")
                    .IsRequired();

            builder.Property(e => e.UpdatedBy)
                .HasColumnType("NUMBER(10)")
                .IsRequired();

            builder.Property(e => e.Deleted)
                 .HasDefaultValue(0)
                 .HasColumnType("NUMBER");
        }

        protected abstract void ConfigureEntity(EntityTypeBuilder<T> builder);
    }
}
