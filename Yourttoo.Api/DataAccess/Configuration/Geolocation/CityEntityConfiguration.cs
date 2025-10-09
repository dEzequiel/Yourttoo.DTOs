using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yourttoo.DTOs.Models.Geolocation;

namespace Yourttoo.Api.DataAccess.Configuration.Geolocation
{
    public class CityEntityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            // Configuración de la tabla
            builder.ToTable("Cities");

            builder.HasKey(c => c.Id);

            // Configuración de propiedades específicas
            builder.Property(c => c.CountryCode)
                .IsRequired()
                .HasMaxLength(10);

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
            builder.HasOne(c => c.Region)
                .WithMany()
                .HasForeignKey("RegionId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Country)
                .WithMany()
                .HasForeignKey("CountryId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Zone)
                .WithMany()
                .HasForeignKey("ZoneId")
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de propiedades de clave foránea
            builder.Property<Guid>("RegionId")
                .IsRequired();

            builder.Property<Guid>("CountryId")
                .IsRequired();

            builder.Property<Guid>("ZoneId")
                .IsRequired();

            // Índices específicos
            builder.HasIndex(c => c.CountryCode);
            builder.HasIndex("RegionId");
            builder.HasIndex("CountryId");
            builder.HasIndex("ZoneId");
        }
    }
}
