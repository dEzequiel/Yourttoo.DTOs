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
                .IsRequired(false)
                .HasMaxLength(10);

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
            // builder.HasOne(c => c.Region)
            //     .WithMany()
            //     .HasForeignKey("RegionId")
            //     .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Country)
                .WithMany()
                .HasForeignKey("CountryId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Zone)
                .WithMany()
                .HasForeignKey("ZoneId")
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de propiedades de clave foránea
            builder.Property<Guid?>("RegionId")
                .IsRequired(false);

            builder.Property<Guid?>("CountryId")
                .IsRequired(false);

            builder.Property<Guid?>("ZoneId")
                .IsRequired(false);

            // Índices específicos
            builder.HasIndex(c => c.CountryCode);
            builder.HasIndex("RegionId");
            builder.HasIndex("CountryId");
            builder.HasIndex("ZoneId");
        }
    }
}
