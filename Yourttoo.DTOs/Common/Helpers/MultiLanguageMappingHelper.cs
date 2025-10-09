using Yourttoo.DTOs.Common.Extensions;
using Yourttoo.DTOs.DTOs.Tagging.TagCategory;
using Yourttoo.DTOs.DTOs.Tagging.Tag;
using Yourttoo.DTOs.DTOs.AdditionalText;
using Yourttoo.DTOs.DTOs.FrequentlyAskedQuestion.Section;
using Yourttoo.DTOs.DTOs.FrequentlyAskedQuestion.Content;
using Yourttoo.DTOs.DTOs.Geolocation.Zone;
using Yourttoo.DTOs.Models.Tagging;
using Yourttoo.DTOs.Models;
using Yourttoo.DTOs.Models.FrequentlyAskedQuestions;
using Yourttoo.DTOs.Models.Geolocation;

namespace Yourttoo.DTOs.Common.Helpers
{
    /// <summary>
    /// Helper class for mapping multi-language entities to DTOs
    /// </summary>
    public static class MultiLanguageMappingHelper
    {
        /// <summary>
        /// Maps TagCategory entity to TagCategoryDTO with language selection
        /// </summary>
        /// <param name="category">TagCategory entity</param>
        /// <param name="language">Preferred language code</param>
        /// <returns>TagCategoryDTO with content in the specified language</returns>
        public static TagCategoryDTO MapToTagCategoryDto(TagCategory category, string? language)
        {
            return new TagCategoryDTO
            {
                Id = category.Id,
                Code = category.Code,
                Filtering = category.Filtering,
                Tags = category.Tags?.Select(t => t.Id).ToList() ?? new List<Guid>(),
                Name = category.Name.GetByLanguageOrDefault(language),
                Description = category.Description.GetByLanguageOrDefault(language)
            };
        }

        /// <summary>
        /// Maps list of TagCategory entities to TagCategoryDTO list with language selection
        /// </summary>
        /// <param name="categories">List of TagCategory entities</param>
        /// <param name="language">Preferred language code</param>
        /// <returns>List of TagCategoryDTO with content in the specified language</returns>
        public static List<TagCategoryDTO> MapToTagCategoryDtos(IEnumerable<TagCategory> categories, string? language)
        {
            return categories.Select(category => MapToTagCategoryDto(category, language)).ToList();
        }

        /// <summary>
        /// Maps Tag entity to TagDTO with language selection
        /// </summary>
        /// <param name="tag">Tag entity</param>
        /// <param name="language">Preferred language code</param>
        /// <returns>TagDTO with content in the specified language</returns>
        public static TagDTO MapToTagDto(Tag tag, string? language)
        {
            return new TagDTO
            {
                Id = tag.Id,
                Code = tag.Code,
                Slug = tag.Slug,
                Status = tag.Status,
                Icon = tag.Icon,
                Categories = tag.Categories?.Select(c => c.Id).ToList() ?? new List<Guid>(),
                Name = tag.Name.GetByLanguageOrDefault(language),
                Description = tag.Description.GetByLanguageOrDefault(language),
                Label = tag.Label.GetByLanguageOrDefault(language)
            };
        }

        /// <summary>
        /// Maps list of Tag entities to TagDTO list with language selection
        /// </summary>
        /// <param name="tags">List of Tag entities</param>
        /// <param name="language">Preferred language code</param>
        /// <returns>List of TagDTO with content in the specified language</returns>
        public static List<TagDTO> MapToTagDtos(IEnumerable<Tag> tags, string? language)
        {
            return tags.Select(tag => MapToTagDto(tag, language)).ToList();
        }

        /// <summary>
        /// Maps AdditionalText entity to AdditionalTextDTO with language selection
        /// </summary>
        /// <param name="additionalText">AdditionalText entity</param>
        /// <param name="language">Preferred language code</param>
        /// <returns>AdditionalTextDTO with content in the specified language</returns>
        public static AdditionalTextDTO MapToAdditionalTextDto(AdditionalText additionalText, string? language)
        {
            return new AdditionalTextDTO
            {
                Id = additionalText.Id,
                Status = additionalText.Status,
                Title = additionalText.Title.GetByLanguageOrDefault(language),
                Content = additionalText.Content?.GetByLanguageOrDefault(language),
                CreatedBy = additionalText.CreatedBy,
                CreatedAt = additionalText.CreatedAt,
                UpdatedBy = additionalText.UpdatedBy,
                UpdatedAt = additionalText.UpdatedAt
            };
        }

        /// <summary>
        /// Maps list of AdditionalText entities to AdditionalTextDTO list with language selection
        /// </summary>
        /// <param name="additionalTexts">List of AdditionalText entities</param>
        /// <param name="language">Preferred language code</param>
        /// <returns>List of AdditionalTextDTO with content in the specified language</returns>
        public static List<AdditionalTextDTO> MapToAdditionalTextDtos(IEnumerable<AdditionalText> additionalTexts, string? language)
        {
            return additionalTexts.Select(additionalText => MapToAdditionalTextDto(additionalText, language)).ToList();
        }

        /// <summary>
        /// Maps AdditionalText entity to AdditionalTextDetailDTO (preserves all languages)
        /// </summary>
        /// <param name="additionalText">AdditionalText entity</param>
        /// <returns>AdditionalTextDetailDTO with all language content</returns>
        public static AdditionalTextDetailDTO MapToAdditionalTextDetailDto(AdditionalText additionalText)
        {
            return new AdditionalTextDetailDTO
            {
                Id = additionalText.Id,
                Status = additionalText.Status,
                Title = additionalText.Title,
                Content = additionalText.Content
            };
        }

        /// <summary>
        /// Maps list of AdditionalText entities to AdditionalTextDetailDTO list (preserves all languages)
        /// </summary>
        /// <param name="additionalTexts">List of AdditionalText entities</param>
        /// <returns>List of AdditionalTextDetailDTO with all language content</returns>
        public static List<AdditionalTextDetailDTO> MapToAdditionalTextDetailDtos(IEnumerable<AdditionalText> additionalTexts)
        {
            return additionalTexts.Select(additionalText => MapToAdditionalTextDetailDto(additionalText)).ToList();
        }

        /// <summary>
        /// Maps FAQSection entity to FAQSectionDTO with language selection
        /// </summary>
        /// <param name="section">FAQSection entity</param>
        /// <param name="language">Preferred language code</param>
        /// <returns>FAQSectionDTO with content in the specified language</returns>
        public static FAQSectionDTO MapToFAQSectionDto(FAQSection section, string? language)
        {
            return new FAQSectionDTO
            {
                Id = section.Id,
                Title = section.Title.GetByLanguageOrDefault(language),
                Category = section.Category,
                Type = section.Type,
                CreatedBy = section.CreatedBy,
                CreatedAt = section.CreatedAt,
                UpdatedBy = section.UpdatedBy,
                UpdatedAt = section.UpdatedAt,
                Contents = section.Contents?.Select(c => c.Id).ToList() ?? new List<Guid>()
            };
        }

        /// <summary>
        /// Maps list of FAQSection entities to FAQSectionDTO list with language selection
        /// </summary>
        /// <param name="sections">List of FAQSection entities</param>
        /// <param name="language">Preferred language code</param>
        /// <returns>List of FAQSectionDTO with content in the specified language</returns>
        public static List<FAQSectionDTO> MapToFAQSectionDtos(IEnumerable<FAQSection> sections, string? language)
        {
            return sections.Select(section => MapToFAQSectionDto(section, language)).ToList();
        }

        /// <summary>
        /// Maps FAQContent entity to FAQContentDTO with language selection
        /// </summary>
        /// <param name="content">FAQContent entity</param>
        /// <param name="language">Preferred language code</param>
        /// <returns>FAQContentDTO with content in the specified language</returns>
        public static FAQContentDTO MapToFAQContentDto(FAQContent content, string? language)
        {
            return new FAQContentDTO
            {
                Id = content.Id,
                Title = content.Title.GetByLanguageOrDefault(language),
                Content = content.Content.GetByLanguageOrDefault(language),
                Status = content.Status,
                CreatedAt = content.CreatedAt,
                FAQSectionId = content.FAQSectionId
            };
        }

        /// <summary>
        /// Maps list of FAQContent entities to FAQContentDTO list with language selection
        /// </summary>
        /// <param name="contents">List of FAQContent entities</param>
        /// <param name="language">Preferred language code</param>
        /// <returns>List of FAQContentDTO with content in the specified language</returns>
        public static List<FAQContentDTO> MapToFAQContentDtos(IEnumerable<FAQContent> contents, string? language)
        {
            return contents.Select(content => MapToFAQContentDto(content, language)).ToList();
        }

        /// <summary>
        /// Maps FAQSection entity to FAQSectionDetailDTO (preserves all languages)
        /// </summary>
        /// <param name="section">FAQSection entity</param>
        /// <returns>FAQSectionDetailDTO with all language content</returns>
        public static FAQSectionDetailDTO MapToFAQSectionDetailDto(FAQSection section)
        {
            return new FAQSectionDetailDTO
            {
                Id = section.Id,
                Title = section.Title,
                Category = section.Category,
                Type = section.Type,
                Contents = section.Contents?.Select(c => c.Id).ToList() ?? new List<Guid>()
            };
        }

        /// <summary>
        /// Maps FAQContent entity to FAQContentDetailDTO (preserves all languages)
        /// </summary>
        /// <param name="content">FAQContent entity</param>
        /// <returns>FAQContentDetailDTO with all language content</returns>
        public static FAQContentDetailDTO MapToFAQContentDetailDto(FAQContent content)
        {
            return new FAQContentDetailDTO
            {
                Id = content.Id,
                Title = content.Title,
                Content = content.Content,
                Status = content.Status,
                Slug = content.Slug,
                FAQSectionId = content.FAQSectionId
            };
        }

        /// <summary>
        /// Maps Zone entity to ZoneDTO with language selection
        /// </summary>
        /// <param name="zone">Zone entity</param>
        /// <param name="language">Preferred language code</param>
        /// <returns>ZoneDTO with content in the specified language</returns>
        public static ZoneDTO MapToZoneDto(Zone zone, string? language)
        {
            return new ZoneDTO
            {
                Id = zone.Id,
                Latitude = zone.Latitude,
                Longitude = zone.Longitude,
                AveragePrice = zone.AveragePrice,
                Name = zone.Name.GetByLanguageOrDefault(language),
                Description = zone.Description?.GetByLanguageOrDefault(language),
                ImageUrl = zone.ImageUrl,
                BackgroundColor = zone.BackgroundColor,
                Status = zone.Status,
                ThumbnailUrl = zone.ThumbnailUrl,
                PromotionAreaPriority = zone.PromotionAreaPriority
            };
        }

        /// <summary>
        /// Maps list of Zone entities to ZoneDTO list with language selection
        /// </summary>
        /// <param name="zones">List of Zone entities</param>
        /// <param name="language">Preferred language code</param>
        /// <returns>List of ZoneDTO with content in the specified language</returns>
        public static List<ZoneDTO> MapToZoneDtos(IEnumerable<Zone> zones, string? language)
        {
            return zones.Select(zone => MapToZoneDto(zone, language)).ToList();
        }

        /// <summary>
        /// Maps Zone entity to ZoneDetailDTO (preserves all languages)
        /// </summary>
        /// <param name="zone">Zone entity</param>
        /// <returns>ZoneDetailDTO with all language content</returns>
        public static ZoneDetailDTO MapToZoneDetailDto(Zone zone)
        {
            return new ZoneDetailDTO
            {
                Id = zone.Id,
                Latitude = zone.Latitude,
                Longitude = zone.Longitude,
                AveragePrice = zone.AveragePrice,
                Name = zone.Name,
                Description = zone.Description,
                ImageUrl = zone.ImageUrl,
                IconUrl = zone.IconUrl,
                BackgroundColor = zone.BackgroundColor,
                Status = zone.Status,
                ThumbnailUrl = zone.ThumbnailUrl,
                Category = zone.Category,
                PromotionArea = zone.PromotionArea,
                PromotionAreaPriority = zone.PromotionAreaPriority
            };
        }
    }

    
}
