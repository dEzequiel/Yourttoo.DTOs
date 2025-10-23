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
                .ForMember(dest => dest.Name, opt => opt.MapFrom((src, dest, destMember, context) => 
                    src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom((src, dest, destMember, context) => 
                    src.Description))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags != null && src.Tags.Any() ? src.Tags.Select(t => t.Id).ToList() : new List<Guid>()));

            CreateMap<TagCategoryDTO, TagCategory>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Tags, opt => opt.Ignore()) // No mapear tags en la dirección inversa
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom((src, dest, destMember, context) => 
                    new MultiLanguageText
                    {
                        Texts = new List<LanguageText>
                        {
                            new LanguageText { Language = context.Items["Language"] as string, Text = src.Name.GetText(context.Items["Language"] as string) ?? string.Empty }
                        }
                    }))
                .ForMember(dest => dest.Description, opt => opt.MapFrom((src, dest, destMember, context) => 
                    new MultiLanguageText
                    {
                        Texts = new List<LanguageText>
                        {
                            new LanguageText { Language = context.Items["Language"] as string, Text = src.Description.GetText(context.Items["Language"] as string) ?? string.Empty }
                        }
                    }));

            CreateMap<TagCategory, TagCategoryDetailDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom((src, dest, destMember, context) => 
                    src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom((src, dest, destMember, context) => 
                    src.Description))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags != null && src.Tags.Any() ? src.Tags.Select(t => t.Id).ToList() : new List<Guid>()));

            CreateMap<TagCategoryDetailDTO, TagCategory>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Tags, opt => opt.Ignore()) // No mapear tags en la dirección inversa
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom((src, dest, destMember, context) => 
                    new MultiLanguageText
                    {
                        Texts = new List<LanguageText>
                        {
                            new LanguageText { Language = context.Items["Language"] as string, Text = src.Name.GetText(context.Items["Language"] as string) ?? string.Empty }
                        }
                    }))
                .ForMember(dest => dest.Description, opt => opt.MapFrom((src, dest, destMember, context) => 
                    new MultiLanguageText
                    {
                        Texts = new List<LanguageText>
                        {
                            new LanguageText { Language = context.Items["Language"] as string, Text = src.Description.GetText(context.Items["Language"] as string) ?? string.Empty }
                        }
                    }));

            CreateMap<Tag, TagDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom((src, dest, destMember, context) => 
                    src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom((src, dest, destMember, context) => 
                    src.Description))
                .ForMember(dest => dest.Label, opt => opt.MapFrom((src, dest, destMember, context) => 
                    src.Label))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories != null ? src.Categories.Select(c => c.Id).ToList() : new List<Guid>()));

            CreateMap<TagDTO, Tag>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom((src, dest, destMember, context) => 
                    new MultiLanguageText
                    {
                        Texts = new List<LanguageText>
                        {
                            new LanguageText { Language = context.Items["Language"] as string, Text = src.Name.GetText(context.Items["Language"] as string) ?? string.Empty }
                        }
                    }))
                .ForMember(dest => dest.Description, opt => opt.MapFrom((src, dest, destMember, context) => 
                    new MultiLanguageText
                    {
                        Texts = new List<LanguageText>
                        {
                            new LanguageText { Language = context.Items["Language"] as string, Text = src.Description.GetText(context.Items["Language"] as string) ?? string.Empty }
                        }
                    }))
                .ForMember(dest => dest.Label, opt => opt.MapFrom((src, dest, destMember, context) => 
                    new MultiLanguageText
                    {
                        Texts = new List<LanguageText>
                        {
                            new LanguageText { Language = context.Items["Language"] as string, Text = src.Label.GetText(context.Items["Language"] as string) ?? string.Empty     }
                        }
                    }))
                .ForMember(dest => dest.Categories, opt => opt.Ignore()) // No mapear categorías en la dirección inversa
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<Tag, TagDetailDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom((src, dest, destMember, context) => 
                    src.Name.GetText(context.Items["Language"] as string) ?? string.Empty))
                .ForMember(dest => dest.Description, opt => opt.MapFrom((src, dest, destMember, context) => 
                    src.Description.GetText(context.Items["Language"] as string) ?? string.Empty))
                .ForMember(dest => dest.Label, opt => opt.MapFrom((src, dest, destMember, context) => 
                    src.Label.GetText(context.Items["Language"] as string) ?? string.Empty))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories != null ? src.Categories.Select(c => c.Id).ToList() : new List<Guid>()));

            CreateMap<TagDetailDTO, Tag>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Categories, opt => opt.Ignore()) // No mapear categorías en la dirección inversa
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom((src, dest, destMember, context) => 
                    new MultiLanguageText
                    {
                        Texts = new List<LanguageText>
                        {
                            new LanguageText { Language = context.Items["Language"] as string, Text = src.Name.GetText(context.Items["Language"] as string) ?? string.Empty }
                        }
                    }))
                .ForMember(dest => dest.Description, opt => opt.MapFrom((src, dest, destMember, context) => 
                    new MultiLanguageText
                    {
                        Texts = new List<LanguageText>
                        {
                            new LanguageText { Language = context.Items["Language"] as string, Text = src.Description.GetText(context.Items["Language"] as string) ?? string.Empty }
                        }
                    }))
                .ForMember(dest => dest.Label, opt => opt.MapFrom((src, dest, destMember, context) => 
                    new MultiLanguageText
                    {
                        Texts = new List<LanguageText>
                        {
                            new LanguageText { Language = context.Items["Language"] as string, Text = src.Label.GetText(context.Items["Language"] as string) ?? string.Empty }
                        }
                    }));
        }
    }
}
