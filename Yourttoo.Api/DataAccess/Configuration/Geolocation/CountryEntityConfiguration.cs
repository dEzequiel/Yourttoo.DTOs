using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yourttoo.DTOs.Models.Geolocation;

namespace Yourttoo.Api.DataAccess.Configuration.Geolocation
{
    public class CountryEntityConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            // Configuración de la tabla
            builder.ToTable("Countries");

            builder.HasKey(c => c.Id);

            // Configuración de propiedades específicas de Country
            builder.Property(c => c.Currency)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(c => c.CurrencySymbol)
                .IsRequired()
                .HasMaxLength(5);

            builder.Property(c => c.Language)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.LanguageCode)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(c => c.TimeZone)
                .IsRequired()
                .HasMaxLength(50);

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
            builder.HasIndex(c => c.Currency);
            builder.HasIndex(c => c.LanguageCode);
            builder.HasIndex(c => c.TimeZone);
        }
    }
}
