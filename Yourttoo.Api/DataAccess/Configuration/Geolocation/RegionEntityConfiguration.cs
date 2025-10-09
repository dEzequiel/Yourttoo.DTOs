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
            builder.Property<Guid>("CountryId")
                .IsRequired();

            builder.Property<Guid>("ZoneId")
                .IsRequired();

            // Índices para las relaciones
            builder.HasIndex("CountryId");
            builder.HasIndex("ZoneId");
        }
    }
}
