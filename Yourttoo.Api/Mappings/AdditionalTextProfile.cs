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
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.FirstOrDefault() ?? new IdiomaticText()))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content!.FirstOrDefault() ?? new IdiomaticText()));
            CreateMap<AdditionalTextDTO, AdditionalText>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => new List<IdiomaticText> { src.Title }))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => new List<IdiomaticText> { src.Content! }));
            CreateMap<AdditionalText, AdditionalTextDetailDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content));
            CreateMap<AdditionalTextDetailDTO, AdditionalText>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content));
        }
    }
}