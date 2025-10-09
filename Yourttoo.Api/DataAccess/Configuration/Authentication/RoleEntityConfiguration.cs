using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yourttoo.DTOs.Models.Authentication;

namespace Yourttoo.Api.DataAccess.Configuration.Authentication
{
    public class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(r => r.Description)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(r => r.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(r => r.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(r => r.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Índice único
            builder.HasIndex(r => r.Name)
                .IsUnique();

            // Relaciones
            builder.HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
