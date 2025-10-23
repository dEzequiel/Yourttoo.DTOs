using AutoMapper;
using Yourttoo.DTOs.DTOs.AdditionalText;
using Yourttoo.DTOs.Models;
using Yourttoo.DTOs.Common;

namespace Yourttoo.Api.Mappings
{
    public class AdditionalTextProfile : Profile
    {
        public AdditionalTextProfile()
        {
            CreateMap<AdditionalText, AdditionalTextDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom((src, dest, destMember, context) => 
                    src.Title.GetText(context.Items["Language"] as string) ?? string.Empty))
                .ForMember(dest => dest.Content, opt => opt.MapFrom((src, dest, destMember, context) => 
                    src.Content?.GetText(context.Items["Language"] as string) ?? string.Empty));

            CreateMap<AdditionalTextDTO, AdditionalText>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.MapFrom((src, dest, destMember, context) => 
                    new MultiLanguageText
                    {
                        Texts = new List<LanguageText>
                        {
                            new LanguageText { Language = context.Items["Language"] as string, Text = src.Title.GetText(context.Items["Language"] as string) ?? string.Empty }
                        }
                    }))
                .ForMember(dest => dest.Content, opt => opt.MapFrom((src, dest, destMember, context) => 
                    new MultiLanguageText
                    {
                        Texts = new List<LanguageText>
                        {
                            new LanguageText { Language = context.Items["Language"] as string, Text = src.Content?.GetText(context.Items["Language"] as string) ?? string.Empty }
                        }
                    }));

            CreateMap<AdditionalText, AdditionalTextDetailDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom((src, dest, destMember, context) => 
                    src.Title.GetText(context.Items["Language"] as string) ?? string.Empty))
                .ForMember(dest => dest.Content, opt => opt.MapFrom((src, dest, destMember, context) => 
                    src.Content?.GetText(context.Items["Language"] as string) ?? string.Empty));

            CreateMap<AdditionalTextDetailDTO, AdditionalText>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.MapFrom((src, dest, destMember, context) => 
                    new MultiLanguageText
                    {
                        Texts = new List<LanguageText>
                        {
                            new LanguageText { Language = context.Items["Language"] as string, Text = src.Title.GetText(context.Items["Language"] as string) ?? string.Empty }
                        }
                    }))
                .ForMember(dest => dest.Content, opt => opt.MapFrom((src, dest, destMember, context) => 
                    new MultiLanguageText
                    {
                        Texts = new List<LanguageText>
                        {
                            new LanguageText { Language = context.Items["Language"] as string, Text = src.Content?.GetText(context.Items["Language"] as string) ?? string.Empty }
                        }
                    }));
        }
    }
}