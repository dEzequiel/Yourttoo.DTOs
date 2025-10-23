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

            builder.Property(c => c.Continent)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.ZoneId)
                .IsRequired();

            builder.HasOne(c => c.Zone)
                .WithMany(z => z.Countries)
                .HasForeignKey("ZoneId")
                .OnDelete(DeleteBehavior.Restrict);

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
            builder.HasIndex(c => c.Currency);
            builder.HasIndex(c => c.LanguageCode);
            builder.HasIndex(c => c.TimeZone);
            builder.HasIndex(c => c.Continent);
            builder.HasIndex(c => c.ZoneId);
        }
    }
}
