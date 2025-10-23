using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yourttoo.DTOs.Models.Geolocation;

namespace Yourttoo.Api.DataAccess.Configuration.Geolocation
{
    public class RegionEntityConfiguration : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            // Configuración de la tabla
            builder.ToTable("Regions");

            builder.HasKey(r => r.Id);

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

            // Configuración de relaciones
            builder.HasOne(r => r.Country)
                .WithMany()
                .HasForeignKey("CountryId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Zone)
                .WithMany()
                .HasForeignKey("ZoneId")
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de propiedades de clave foránea
            builder.Property<Guid?>("CountryId")
                .IsRequired(false);

            builder.Property<Guid?>("ZoneId")
                .IsRequired(false);

            // Índices para las relaciones
            builder.HasIndex("CountryId");
            builder.HasIndex("ZoneId");
        }
    }
}
