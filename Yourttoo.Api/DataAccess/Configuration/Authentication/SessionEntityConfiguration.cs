using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yourttoo.DTOs.Models.Authentication;

namespace Yourttoo.Api.DataAccess.Configuration.Authentication
{
    public class SessionEntityConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.SessionId)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(s => s.UserId)
                .IsRequired();

            builder.Property(s => s.IpAddress)
                .HasMaxLength(45); // IPv6 max length

            builder.Property(s => s.UserAgent)
                .HasMaxLength(500);

            builder.Property(s => s.CreatedAt)
                .IsRequired();

            builder.Property(s => s.ExpiresAt)
                .IsRequired();

            builder.Property(s => s.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Índices para optimizar consultas
            builder.HasIndex(s => s.SessionId)
                .IsUnique();

            builder.HasIndex(s => s.UserId);

            builder.HasIndex(s => s.ExpiresAt);

            builder.HasIndex(s => s.IsActive);

            // Relación con User
            builder.HasOne(s => s.User)
                .WithMany(u => u.Sessions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Nombre de tabla
            builder.ToTable("Sessions");
        }
    }
}
