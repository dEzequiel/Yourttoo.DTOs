using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yourttoo.DTOs.Models.Authentication;

namespace Yourttoo.Api.DataAccess.Configuration.Authentication
{
    public class UserRoleEntityConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRoles");

            builder.HasKey(ur => ur.Id);

            builder.Property(ur => ur.UserId)
                .IsRequired();

            builder.Property(ur => ur.RoleId)
                .IsRequired();

            builder.Property(ur => ur.AssignedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(ur => ur.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Índice compuesto único para evitar duplicados
            builder.HasIndex(ur => new { ur.UserId, ur.RoleId })
                .IsUnique();

            // Relaciones
            builder.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
