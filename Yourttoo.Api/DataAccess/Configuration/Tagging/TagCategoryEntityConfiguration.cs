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

            builder.OwnsMany(tc => tc.Name, name => name.ToJson());
            builder.OwnsMany(tc => tc.Description, desc => desc.ToJson());

            builder.HasMany(tc => tc.Tags)
                .WithMany(t => t.Categories)
                .UsingEntity("TagCategoryTag");
        }
    }
}
