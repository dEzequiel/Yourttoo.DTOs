using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yourttoo.DTOs.Models;
using Yourttoo.DTOs.Models.Tagging;

namespace Yourttoo.Api.DataAccess.Configuration
{
    public class AdditionalTextEntityConfiguration : IEntityTypeConfiguration<AdditionalText>
    {
        public void Configure(EntityTypeBuilder<AdditionalText> builder)
        {
            builder.Property(t => t.Status)
                .IsRequired()
                .HasDefaultValue("published");

            builder.Property(t => t.CreatedAt)
               .HasDefaultValueSql("datetime('now')");

            builder.Property(t => t.UpdatedAt)
                .HasDefaultValueSql("datetime('now')");

            builder.HasIndex(t => t.Status);
            builder.HasIndex(t => new { t.CreatedBy, t.CreatedAt });


            builder.OwnsMany(t => t.Title, name =>
            {
                name.ToJson();
                name.Property(n => n.Content).IsRequired().HasMaxLength(500);
                name.Property(n => n.Language).IsRequired().HasMaxLength(10);
            });

            builder.OwnsMany(t => t.Content, desc =>
            {
                desc.ToJson();
                desc.Property(d => d.Content).IsRequired().HasMaxLength(2000);
                desc.Property(d => d.Language).IsRequired().HasMaxLength(10);
            });

        }
    }
}
