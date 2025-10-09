using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yourttoo.DTOs.Models.Geolocation;

namespace Yourttoo.Api.DataAccess.Configuration.Geolocation
{
    public class AirportEntityConfiguration : IEntityTypeConfiguration<Airport>
    {
        public void Configure(EntityTypeBuilder<Airport> builder)
        {
            // Configuración de la tabla
            builder.ToTable("Airports");

            builder.HasKey(a => a.Id);

            // Configuración de propiedades específicas de Airport
            builder.Property(a => a.CountryCode)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(a => a.IATACode)
                .IsRequired()
                .HasMaxLength(3);

            builder.Property(a => a.ICAOCode)
                .IsRequired()
                .HasMaxLength(4);

            builder.Property(a => a.TimeZone)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.GMTOffset)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(a => a.DSTOffset)
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

            // Configuración de relaciones
            builder.HasOne(a => a.City)
                .WithMany()
                .HasForeignKey("CityId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Zone)
                .WithMany()
                .HasForeignKey("ZoneId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Country)
                .WithMany()
                .HasForeignKey("CountryId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Region)
                .WithMany()
                .HasForeignKey("RegionId")
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de propiedades de clave foránea
            builder.Property<Guid>("CityId")
                .IsRequired();

            builder.Property<Guid>("ZoneId")
                .IsRequired();

            builder.Property<Guid>("CountryId")
                .IsRequired();

            builder.Property<Guid>("RegionId")
                .IsRequired();

            // Índices específicos
            builder.HasIndex(a => a.IATACode)
                .IsUnique();

            builder.HasIndex(a => a.ICAOCode)
                .IsUnique();

            builder.HasIndex(a => a.CountryCode);
            builder.HasIndex(a => a.TimeZone);
            builder.HasIndex("CityId");
            builder.HasIndex("ZoneId");
            builder.HasIndex("CountryId");
            builder.HasIndex("RegionId");
        }
    }
}
