using AutoMapper;
using Yourttoo.DTOs.DTOs.FrequentlyAskedQuestion.Section;
using Yourttoo.DTOs.DTOs.FrequentlyAskedQuestion.Content;
using Yourttoo.DTOs.Models.FrequentlyAskedQuestions;
using Yourttoo.DTOs.Common;

namespace Yourttoo.Api.Mappings
{
    public class FAQProfile : Profile
    {
        public FAQProfile()
        {
            // FAQSection mappings
            CreateMap<FAQSection, FAQSectionDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom((src, dest, destMember, context) => 
                    src.Title.GetText(context.Items["Language"] as string) ?? string.Empty))
                .ForMember(dest => dest.Contents, opt => opt.MapFrom(src => src.Contents.Select(c => c.Id).ToList()));

            CreateMap<FAQSection, FAQSectionDetailDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom((src, dest, destMember, context) => 
                    src.Title.GetText(context.Items["Language"] as string) ?? string.Empty))
                .ForMember(dest => dest.Contents, opt => opt.MapFrom(src => src.Contents.Select(c => c.Id).ToList()));

            // FAQContent mappings
            CreateMap<FAQContent, FAQContentDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom((src, dest, destMember, context) => 
                    src.Title.GetText(context.Items["Language"] as string) ?? string.Empty))
                .ForMember(dest => dest.Content, opt => opt.MapFrom((src, dest, destMember, context) => 
                    src.Content.GetText(context.Items["Language"] as string) ?? string.Empty));

            CreateMap<FAQContent, FAQContentDetailDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom((src, dest, destMember, context) => 
                    src.Title.GetText(context.Items["Language"] as string) ?? string.Empty))
                .ForMember(dest => dest.Content, opt => opt.MapFrom((src, dest, destMember, context) => 
                    src.Content.GetText(context.Items["Language"] as string) ?? string.Empty));

            // Reverse mappings for creation/updates
            CreateMap<FAQSectionDTO, FAQSection>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Contents, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.MapFrom((src, dest, destMember, context) => 
                    new MultiLanguageText
                    {
                        Texts = new List<LanguageText>
                        {
                            new LanguageText { Language = context.Items["Language"] as string, Text = src.Title.GetText(context.Items["Language"] as string) ?? string.Empty }
                        }
                    }));

            CreateMap<FAQContentDTO, FAQContent>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.FAQSection, opt => opt.Ignore())
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
