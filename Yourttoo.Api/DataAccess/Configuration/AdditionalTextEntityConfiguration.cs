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


            builder.OwnsOne(t => t.Title, name =>
            {
                name.ToJson();
                name.OwnsMany(n => n.Texts, text =>
                {
                    text.Property(t => t.Language).IsRequired().HasMaxLength(10);
                    text.Property(t => t.Text).HasMaxLength(500);
                });
            });

            builder.OwnsOne(t => t.Content, desc =>
            {
                desc.ToJson();
                desc.OwnsMany(d => d.Texts, text =>
                {
                    text.Property(t => t.Language).IsRequired().HasMaxLength(10);
                    text.Property(t => t.Text).HasMaxLength(2000);
                });
            });

        }
    }
}
