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

            builder.Property(a => a.IataCode)
                .IsRequired()
                .HasMaxLength(3);

            builder.Property(a => a.IcaoCode)
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

            // Configuración de propiedades de clave foránea
            builder.Property<Guid?>("CityId")
                .IsRequired(false);

            builder.Property<Guid?>("ZoneId")
                .IsRequired(false);

            builder.Property<Guid?>("CountryId")
                .IsRequired(false);

            // Índices específicos
            builder.HasIndex(a => a.IataCode)
                .IsUnique();

            builder.HasIndex(a => a.IcaoCode)
                .IsUnique();

            builder.HasIndex(a => a.TimeZone);
            builder.HasIndex("CityId");
            builder.HasIndex("ZoneId");
            builder.HasIndex("CountryId");
        }
    }
}
