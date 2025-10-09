using AutoMapper;
using Yourttoo.DTOs.DTOs.AdditionalText;
using Yourttoo.DTOs.Models;
using Yourttoo.DTOs.Common;

namespace Yourttoo.Api.Mappings
{
    public class AdditionalTextProfile : Profile
    {
        public AdditionalTextProfile() {
            CreateMap<AdditionalText, AdditionalTextDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.FirstOrDefault() ?? new MultiLanguageText()))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content!.FirstOrDefault() ?? new MultiLanguageText()));
            CreateMap<AdditionalTextDTO, AdditionalText>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => new List<MultiLanguageText> { src.Title }))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => new List<MultiLanguageText> { src.Content! }));
            CreateMap<AdditionalText, AdditionalTextDetailDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content));
            CreateMap<AdditionalTextDetailDTO, AdditionalText>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content));
        }
    }
}