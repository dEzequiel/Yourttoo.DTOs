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

            builder.HasOne(t => t.FAQSection)
                .WithMany(t => t.Contents)
                .HasForeignKey(t => t.FAQSectionId);
        }
    }
}