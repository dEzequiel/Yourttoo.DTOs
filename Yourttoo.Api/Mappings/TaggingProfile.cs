using AutoMapper;
using Yourttoo.DTOs.DTOs.Tagging.TagCategory;
using Yourttoo.DTOs.DTOs.Tagging.Tag;
using Yourttoo.DTOs.Models.Tagging;
using Yourttoo.DTOs.Common;

namespace Yourttoo.Api.Mappings
{
    public class TaggingProfile : Profile
    {
        public TaggingProfile() 
        {
            CreateMap<TagCategory, TagCategoryDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.FirstOrDefault() ?? new MultiLanguageText()))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.FirstOrDefault() ?? new MultiLanguageText()))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags != null && src.Tags.Any() ? src.Tags.Select(t => t.Id).ToList() : new List<Guid>()));

            CreateMap<TagCategoryDTO, TagCategory>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new List<MultiLanguageText> { src.Name }))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => new List<MultiLanguageText> { src.Description }))
                .ForMember(dest => dest.Tags, opt => opt.Ignore()) // No mapear tags en la dirección inversa
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<TagCategory, TagCategoryDetailDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags != null && src.Tags.Any() ? src.Tags.Select(t => t.Id).ToList() : new List<Guid>()));

            CreateMap<TagCategoryDetailDTO, TagCategory>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Tags, opt => opt.Ignore()) // No mapear tags en la dirección inversa
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<Tag, TagDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.FirstOrDefault() ?? new MultiLanguageText()))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.FirstOrDefault() ?? new MultiLanguageText()))
                .ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.Label.FirstOrDefault() ?? new MultiLanguageText()))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories != null ? src.Categories.Select(c => c.Id).ToList() : new List<Guid>()));

            CreateMap<TagDTO, Tag>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new List<MultiLanguageText> { src.Name }))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => new List<MultiLanguageText> { src.Description }))
                .ForMember(dest => dest.Label, opt => opt.MapFrom(src => new List<MultiLanguageText> { src.Label }))
                .ForMember(dest => dest.Categories, opt => opt.Ignore()) // No mapear categorías en la dirección inversa
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<Tag, TagDetailDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.Label))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories != null ? src.Categories.Select(c => c.Id).ToList() : new List<Guid>()));

            CreateMap<TagDetailDTO, Tag>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.Label))
                .ForMember(dest => dest.Categories, opt => opt.Ignore()) // No mapear categorías en la dirección inversa
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
}
