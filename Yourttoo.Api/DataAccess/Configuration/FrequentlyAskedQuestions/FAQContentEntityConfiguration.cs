using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yourttoo.DTOs.Models.FrequentlyAskedQuestions;

namespace Yourttoo.Api.DataAccess.Configuration.FrequentlyAskedQuestions
{
    public class FAQContentEntityConfiguration : IEntityTypeConfiguration<FAQContent>
    {
        public void Configure(EntityTypeBuilder<FAQContent> builder)
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

            builder.OwnsMany(t => t.Content, desc =>
            {
                desc.ToJson();
                desc.Property(d => d.Content).IsRequired().HasMaxLength(2000);
                desc.Property(d => d.Language).IsRequired().HasMaxLength(10);
            });

            builder.HasOne(t => t.FAQSection)
                .WithMany(t => t.Contents)
                .HasForeignKey(t => t.FAQSectionId);
        }
    }
}