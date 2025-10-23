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


            builder.OwnsOne(t => t.Title, name =>
            {
                name.ToJson();
                name.OwnsMany(n => n.Texts, text =>
                {
                    text.Property(t => t.Language).IsRequired().HasMaxLength(10);
                    text.Property(t => t.Text).HasMaxLength(500);
                });
            });

            builder.HasMany(t => t.Contents)
                .WithOne(t => t.FAQSection)
                .HasForeignKey(t => t.FAQSectionId);
        }
    }
}