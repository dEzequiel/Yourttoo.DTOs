using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yourttoo.DTOs.Models.Geolocation;

namespace Yourttoo.Api.DataAccess.Configuration.Geolocation
{
    public class ZoneEntityConfiguration : IEntityTypeConfiguration<Zone>
    {
        public void Configure(EntityTypeBuilder<Zone> builder)
        {
            // Configuración de la tabla
            builder.ToTable("Zones");

            builder.HasKey(z => z.Id);

            // Configuración de propiedades específicas de Zone
            builder.Property(z => z.PromotionArea)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(z => z.PromotionAreaPriority)
                .IsRequired()
                .HasDefaultValue(0);

            // Configuración de propiedades multiidioma
            builder.OwnsOne(g => g.Name, name =>
            {
                name.ToJson();
                name.OwnsMany(n => n.Texts, text =>
                {
                    text.Property(t => t.Language).IsRequired().HasMaxLength(10);
                    text.Property(t => t.Text).HasMaxLength(200);
                });
            });

            builder.OwnsOne(g => g.Description, desc =>
            {
                desc.ToJson();
                desc.OwnsMany(d => d.Texts, text =>
                {
                    text.Property(t => t.Language).IsRequired().HasMaxLength(10);
                    text.Property(t => t.Text).HasMaxLength(1000);
                });
            });

            // Índices específicos
            builder.HasIndex(z => z.PromotionArea);
            builder.HasIndex(z => z.PromotionAreaPriority);
        }
    }
}
