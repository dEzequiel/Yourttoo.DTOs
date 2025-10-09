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
            builder.OwnsMany(g => g.Name, name =>
            {
                name.ToJson();
                name.Property(n => n.Content).IsRequired().HasMaxLength(200);
                name.Property(n => n.Language).IsRequired().HasMaxLength(10);
            });

            builder.OwnsMany(g => g.Description, desc =>
            {
                desc.ToJson();
                desc.Property(d => d.Content).HasMaxLength(1000);
                desc.Property(d => d.Language).IsRequired().HasMaxLength(10);
            });

            // Índices específicos
            builder.HasIndex(z => z.PromotionArea);
            builder.HasIndex(z => z.PromotionAreaPriority);
        }
    }
}
