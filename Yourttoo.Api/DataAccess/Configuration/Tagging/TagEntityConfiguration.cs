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

            builder.OwnsMany(t => t.Name, name =>
            {
                name.ToJson();
                name.Property(n => n.Content).IsRequired().HasMaxLength(500);
                name.Property(n => n.Language).IsRequired().HasMaxLength(10);
            });

            builder.OwnsMany(t => t.Description, desc =>
            {
                desc.ToJson();
                desc.Property(d => d.Content).IsRequired().HasMaxLength(2000);
                desc.Property(d => d.Language).IsRequired().HasMaxLength(10);
            });

            builder.OwnsMany(t => t.Label, label =>
            {
                label.ToJson();
                label.Property(l => l.Content).IsRequired().HasMaxLength(100);
                label.Property(l => l.Language).IsRequired().HasMaxLength(10);
            });

            builder.HasMany(t => t.Categories)
                .WithMany()
                .UsingEntity("TagCategoryTag");
        }
    }
}
