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
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.FirstOrDefault() ?? new MultiLanguageText()))
                .ForMember(dest => dest.Contents, opt => opt.MapFrom(src => src.Contents.Select(c => c.Id).ToList()));

            CreateMap<FAQSection, FAQSectionDetailDTO>()
                .ForMember(dest => dest.Contents, opt => opt.MapFrom(src => src.Contents.Select(c => c.Id).ToList()));

            // FAQContent mappings
            CreateMap<FAQContent, FAQContentDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.FirstOrDefault() ?? new MultiLanguageText()))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content.FirstOrDefault() ?? new MultiLanguageText()));

            CreateMap<FAQContent, FAQContentDetailDTO>();

            // Reverse mappings for creation/updates
            CreateMap<FAQSectionDTO, FAQSection>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Contents, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => new List<MultiLanguageText> { src.Title }));

            CreateMap<FAQContentDTO, FAQContent>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.FAQSection, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => new List<MultiLanguageText> { src.Title }))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => new List<MultiLanguageText> { src.Content! }));
        }
    }
}
