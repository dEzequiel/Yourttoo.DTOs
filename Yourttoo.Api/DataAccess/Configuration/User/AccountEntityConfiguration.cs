using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yourttoo.DTOs.Models.Users;
using Yourttoo.DTOs.Common.Constants;

namespace Yourttoo.Api.DataAccess.Configuration.User
{
    public class AccountEntityConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            // Configuración básica de la tabla
            builder.ToTable("Accounts");
            builder.HasKey(a => a.Email);

            // Configuración de propiedades con tipos explícitos
            builder.Property<string>(a => a.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property<string>(a => a.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue(UserStatus.ACTIVE);

            builder.Property<DateTime?>(a => a.LastLoginAt)
                .IsRequired(false);

            builder.Property<int>(a => a.FailedLoginAttempts)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property<string>(a => a.PasswordHash)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property<string>(a => a.PasswordSalt)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property<string>(a => a.PasswordChangedAt)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property<bool>(a => a.IsTwoFactorEnabled)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property<string>(a => a.TwoFactorEnabledAt)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property<string>(a => a.TwoFactorMethod)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property<string>(a => a.LastPasswordResetAt)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property<string>(a => a.PasswordResetToken)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property<string>(a => a.PasswordResetTokenExpiresAt)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property<string>(a => a.AccountType)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue(AccountTypes.AGENCY);

            // Configuración de ApiKeys como JSON
            builder.Property(a => a.ApiKeys)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
                .HasMaxLength(2000);

            // Índices básicos
            builder.HasIndex(a => a.Email)
                .IsUnique()
                .HasDatabaseName("IX_Accounts_Email");

            builder.HasIndex(a => a.Status)
                .HasDatabaseName("IX_Accounts_Status");

            builder.HasIndex(a => a.AccountType)
                .HasDatabaseName("IX_Accounts_AccountType");
        }
    }
}