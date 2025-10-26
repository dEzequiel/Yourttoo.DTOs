using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yourttoo.DTOs.Common.Constants;

namespace Yourttoo.Api.DataAccess.Configuration.User
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<Yourttoo.DTOs.Models.Users.User>
    {
        public void Configure(EntityTypeBuilder<Yourttoo.DTOs.Models.Users.User> builder)
        {
            // Configuración básica de la tabla
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);

            // Configuración de propiedades con tipos explícitos
            builder.Property<string>(u => u.Username)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property<string>(u => u.FirstName)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property<string>(u => u.LastName)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property<string>(u => u.Language)
                .IsRequired()
                .HasMaxLength(10)
                .HasDefaultValue(Languages.Default);

            builder.Property<string>(u => u.TimeZone)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue(TimeZones.Europe.MADRID);

            builder.Property<string>(u => u.AccountId)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property<string>(u => u.Email)
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property<string>(u => u.PasswordHash)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property<string>(u => u.PasswordSalt)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property<string>(u => u.AccessToken)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property<DateTime?>(u => u.AccessTokenExpiry)
                .IsRequired(false);

            builder.Property<DateTime?>(u => u.LastLoginAt)
                .IsRequired(false);

            builder.Property<int>(u => u.FailedLoginAttempts)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property<string>(u => u.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue(UserStatus.ACTIVE);

            // Configuración de propiedades de auditoría
            builder.Property<string>(u => u.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property<DateTime>(u => u.CreatedAt)
                .IsRequired();

            builder.Property<string>(u => u.UpdatedBy)
                .HasMaxLength(100);

            // Configuración de Roles como JSON
            builder.Property(u => u.Roles)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
                .HasMaxLength(1000);

            // Índices básicos
            builder.HasIndex(u => u.Username)
                .IsUnique()
                .HasDatabaseName("IX_Users_Username");

            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("IX_Users_Email");

            builder.HasIndex(u => u.Status)
                .HasDatabaseName("IX_Users_Status");

            // Configuración de valores por defecto
            builder.Property<DateTime>(u => u.CreatedAt)
                .ValueGeneratedOnAdd();
        }
    }
}