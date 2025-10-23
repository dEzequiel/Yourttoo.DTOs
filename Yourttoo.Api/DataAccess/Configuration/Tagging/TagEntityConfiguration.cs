using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yourttoo.DTOs.Models.Tagging;

namespace Yourttoo.Api.DataAccess.Configuration.Tagging
{
    public class TagEntityConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.Property(t => t.Status)
                .IsRequired()
                .HasDefaultValue("active");

            builder.Property(t => t.CreatedAt)
                .HasDefaultValueSql("datetime('now')");

            builder.Property(t => t.UpdatedAt)
                .HasDefaultValueSql("datetime('now')");

            builder.HasIndex(t => t.Code).IsUnique();
            builder.HasIndex(t => t.Slug).IsUnique();
            builder.HasIndex(t => t.Status);
            builder.HasIndex(t => new { t.CreatedBy, t.CreatedAt });

            builder.OwnsOne(t => t.Name, name =>
            {
                name.ToJson();
                name.OwnsMany(n => n.Texts, text =>
                {
                    text.Property(t => t.Language).IsRequired().HasMaxLength(10);
                    text.Property(t => t.Text).HasMaxLength(500);
                });
            });

            builder.OwnsOne(t => t.Description, desc =>
            {
                desc.ToJson();
                desc.OwnsMany(d => d.Texts, text =>
                {
                    text.Property(t => t.Language).IsRequired().HasMaxLength(10);
                    text.Property(t => t.Text).HasMaxLength(2000);
                });
            });

            builder.OwnsOne(t => t.Label, label =>
            {
                label.ToJson();
                label.OwnsMany(l => l.Texts, text =>
                {
                    text.Property(t => t.Language).IsRequired().HasMaxLength(10);
                    text.Property(t => t.Text).HasMaxLength(100);
                });
            });

            builder.HasMany(t => t.Categories)
                .WithMany()
                .UsingEntity("TagCategoryTag");
        }
    }
}
