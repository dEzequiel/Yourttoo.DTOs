using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yourttoo.DTOs.Models.Tagging;

namespace Yourttoo.Api.DataAccess.Configuration.Tagging
{
    public class TagCategoryEntityConfiguration : IEntityTypeConfiguration<TagCategory>
    {
        public void Configure(EntityTypeBuilder<TagCategory> builder)
        {
            builder.HasIndex(tc => tc.Code).IsUnique();

            builder.Property(t => t.CreatedAt)
                .HasDefaultValueSql("datetime('now')");

            builder.Property(t => t.UpdatedAt)
                .HasDefaultValueSql("datetime('now')");

            builder.OwnsOne(tc => tc.Name, name =>
            {
                name.ToJson();
                name.OwnsMany(n => n.Texts, text =>
                {
                    text.Property(t => t.Language).IsRequired().HasMaxLength(10);
                    text.Property(t => t.Text).HasMaxLength(500);
                });
            });

            builder.OwnsOne(tc => tc.Description, desc =>
            {
                desc.ToJson();
                desc.OwnsMany(d => d.Texts, text =>
                {
                    text.Property(t => t.Language).IsRequired().HasMaxLength(10);
                    text.Property(t => t.Text).HasMaxLength(2000);
                });
            });

            builder.HasMany(tc => tc.Tags)
                .WithMany(t => t.Categories)
                .UsingEntity("TagCategoryTag");
        }
    }
}
