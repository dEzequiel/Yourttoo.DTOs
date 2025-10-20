using AutoMapper;
using Yourttoo.DTOs.DTOs.Geolocation.Zone;
using Yourttoo.DTOs.Models.Geolocation;
using Yourttoo.DTOs.Common;
using Yourttoo.DTOs.DTOs.Geolocation.Country;

namespace Yourttoo.Api.Mappings
{
    public class GeolocationProfile : Profile
    {
        public GeolocationProfile()
        {
            CreateMap<Zone, ZoneDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.FirstOrDefault() ?? new MultiLanguageText()))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.FirstOrDefault() ?? new MultiLanguageText()));
            CreateMap<ZoneDTO, Zone>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new List<MultiLanguageText> { src.Name }))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => new List<MultiLanguageText> { src.Description }))
                    .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            CreateMap<Zone, ZoneDetailDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
            CreateMap<ZoneDetailDTO, Zone>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<Country, CountryDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.FirstOrDefault() ?? new MultiLanguageText()))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.FirstOrDefault() ?? new MultiLanguageText()));

            CreateMap<CountryDTO, Country>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new List<MultiLanguageText> { src.Name }))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => new List<MultiLanguageText> { src.Description }))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<Country, CountryDetailDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<CountryDetailDTO, Country>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        }
    }
}