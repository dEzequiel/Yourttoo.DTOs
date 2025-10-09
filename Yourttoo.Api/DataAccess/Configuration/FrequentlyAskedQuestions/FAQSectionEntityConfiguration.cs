using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yourttoo.DTOs.Models.FrequentlyAskedQuestions;

namespace Yourttoo.Api.DataAccess.Configuration.FrequentlyAskedQuestions
{
    public class FAQSectionEntityConfiguration : IEntityTypeConfiguration<FAQSection>
    {
        public void Configure(EntityTypeBuilder<FAQSection> builder)
        {
            builder.Property(t => t.CreatedAt)
               .HasDefaultValueSql("datetime('now')");

            builder.Property(t => t.UpdatedAt)
                .HasDefaultValueSql("datetime('now')");

            builder.HasIndex(t => new { t.CreatedBy, t.CreatedAt });


            builder.OwnsMany(t => t.Title, name =>
            {
                name.ToJson();
                name.Property(n => n.Content).IsRequired().HasMaxLength(500);
                name.Property(n => n.Language).IsRequired().HasMaxLength(10);
            });

            builder.HasMany(t => t.Contents)
                .WithOne(t => t.FAQSection)
                .HasForeignKey(t => t.FAQSectionId);
        }
    }
}