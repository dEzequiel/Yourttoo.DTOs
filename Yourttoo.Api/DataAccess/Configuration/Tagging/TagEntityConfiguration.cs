using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yourttoo.DTOs.Models.Tagging;
using Yourttoo.DTOs.Common.Constants;

namespace Yourttoo.Api.DataAccess.Configuration.Tagging
{
    public class TagEntityConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            // Configuración de la tabla
            builder.ToTable("Tags", t =>
            {
                t.HasCheckConstraint("CK_Tags_Type", 
                    $"Type IN ('{TagTypes.CATEGORY}', '{TagTypes.FEATURE}', '{TagTypes.THEME}', " +
                    $"'{TagTypes.TOPIC}', '{TagTypes.DESTINATION}', '{TagTypes.SERVICE}', " +
                    $"'{TagTypes.AUDIENCE}', '{TagTypes.STYLE}', '{TagTypes.PRICE_RANGE}', " +
                    $"'{TagTypes.SEARCH_KEYWORD}')");

                t.HasCheckConstraint("CK_Tags_Status", 
                    $"Status IN ('{ItemStatus.ACTIVE}', '{ItemStatus.INACTIVE}', '{ItemStatus.PENDING}', " +
                    $"'{ItemStatus.DELETED}', '{ItemStatus.ARCHIVED}', '{ItemStatus.DRAFT}', " +
                    $"'{ItemStatus.APPROVED}', '{ItemStatus.REJECTED}', '{ItemStatus.SUSPENDED}', " +
                    $"'{ItemStatus.EXPIRED}', '{ItemStatus.COMPLETED}', '{ItemStatus.CANCELLED}', " +
                    $"'{ItemStatus.PROCESSING}', '{ItemStatus.ON_HOLD}', '{ItemStatus.NEW}', " +
                    $"'{ItemStatus.UPDATED}', '{ItemStatus.VERIFIED}', '{ItemStatus.UNVERIFIED}')");
            });

            builder.HasKey(t => t.Id);

            // Configuración de propiedades específicas de Tag
            builder.Property(t => t.Key)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Category)
                .HasMaxLength(100);

            builder.Property(t => t.SubCategory)
                .HasMaxLength(100);

            builder.Property(t => t.Type)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue(TagTypes.SEARCH_KEYWORD);

            builder.Property(t => t.IconUrl)
                .HasMaxLength(500);

            builder.Property(t => t.ImageUrl)
                .HasMaxLength(500);

            builder.Property(t => t.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue(ItemStatus.ACTIVE);

            // Configuración de propiedades de auditoría (heredadas de IEntity)
            builder.Property(t => t.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.CreatedAt)
                .IsRequired();

            builder.Property(t => t.UpdatedBy)
                .HasMaxLength(100);

            // Configuración de propiedades multiidioma
            builder.OwnsOne(t => t.Name, name =>
            {
                name.ToJson();
                name.OwnsMany(n => n.Texts, text =>
                {
                    text.Property(t => t.Language).IsRequired().HasMaxLength(10);
                    text.Property(t => t.Text).HasMaxLength(200);
                });
            });

            builder.OwnsOne(t => t.Description, desc =>
            {
                desc.ToJson();
                desc.OwnsMany(d => d.Texts, text =>
                {
                    text.Property(t => t.Language).IsRequired().HasMaxLength(10);
                    text.Property(t => t.Text).HasMaxLength(1000);
                });
            });

            // Índices para optimizar consultas
            builder.HasIndex(t => t.Key)
                .IsUnique()
                .HasDatabaseName("IX_Tags_Key");

            builder.HasIndex(t => t.Value)
                .HasDatabaseName("IX_Tags_Value");

            builder.HasIndex(t => t.Category)
                .HasDatabaseName("IX_Tags_Category");

            builder.HasIndex(t => t.SubCategory)
                .HasDatabaseName("IX_Tags_SubCategory");

            builder.HasIndex(t => t.Type)
                .HasDatabaseName("IX_Tags_Type");

            builder.HasIndex(t => t.Status)
                .HasDatabaseName("IX_Tags_Status");

            // Índice compuesto para consultas comunes
            builder.HasIndex(t => new { t.Type, t.Status })
                .HasDatabaseName("IX_Tags_Type_Status");

            builder.HasIndex(t => new { t.Category, t.SubCategory })
                .HasDatabaseName("IX_Tags_Category_SubCategory");

            // Índice para auditoría
            builder.HasIndex(t => t.CreatedAt)
                .HasDatabaseName("IX_Tags_CreatedAt");

            builder.HasIndex(t => t.UpdatedAt)
                .HasDatabaseName("IX_Tags_UpdatedAt");

            // Configuración de valores por defecto - manejados en el código C#
            builder.Property(t => t.CreatedAt)
                .ValueGeneratedOnAdd();

            // Configuración de comentarios para documentación
            builder.Property(t => t.Key)
                .HasComment("Unique identifier key for the tag");

            builder.Property(t => t.Value)
                .HasComment("Display value of the tag");

            builder.Property(t => t.Category)
                .HasComment("Main category classification of the tag");

            builder.Property(t => t.SubCategory)
                .HasComment("Sub-category classification of the tag");

            builder.Property(t => t.Type)
                .HasComment("Type of tag (Category, Feature, Theme, etc.)");

            builder.Property(t => t.Status)
                .HasComment("Current status of the tag");

            builder.Property(t => t.IconUrl)
                .HasComment("URL to the icon image for the tag");

            builder.Property(t => t.ImageUrl)
                .HasComment("URL to the main image for the tag");
        }
    }
}
