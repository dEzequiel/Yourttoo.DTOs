using AutoMapper;
using Yourttoo.DTOs.DTOs.Geolocation.Zone;
using Yourttoo.DTOs.Models.Geolocation;
using Yourttoo.DTOs.Common;
using Yourttoo.DTOs.DTOs.Geolocation.Country;
using Yourttoo.DTOs.Requests.Geolocation.Zone;
using Yourttoo.DTOs.Requests.Geolocation.Country;
using Yourttoo.DTOs.DTOs.Geolocation.City;
using Yourttoo.DTOs.Requests.Geolocation.City;
using Yourttoo.DTOs.DTOs.Geolocation.Airport;
using Yourttoo.DTOs.Requests.Geolocation.Airport;
using Yourttoo.DTOs.DTOs.Geolocation.Region;
using Yourttoo.DTOs.Requests.Geolocation.Region;

namespace Yourttoo.Api.Mappings
{
    public class GeolocationProfile : Profile
    {
        public GeolocationProfile()
        {
            #region Zone Mappings

            CreateMap<Zone, ZoneDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom((src, dest, destMember, context) =>
                    src.Name.GetText(context.Items["Language"] as string)))
                .ForMember(dest => dest.Description, opt => opt.MapFrom((src, dest, destMember, context) =>
                    src.Description.GetText(context.Items["Language"] as string)))
                .ForMember(dest => dest.Language, opt => opt.MapFrom((src, dest, destMember, context) =>
                    context.Items["Language"] as string ?? string.Empty));
            CreateMap<ZoneDTO, Zone>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom((src, dest, destMember, context) =>
                    new MultiLanguageText
                    {
                        Texts = new List<LanguageText>
                            { new LanguageText { Language = context.Items["Language"] as string, Text = src.Name } }
                    }))
                .ForMember(dest => dest.Description, opt => opt.MapFrom((src, dest, destMember, context) =>
                    new MultiLanguageText
                    {
                        Texts = new List<LanguageText>
                        {
                            new LanguageText { Language = context.Items["Language"] as string, Text = src.Description }
                        }
                    }));

            CreateMap<CreateZoneRequest, Zone>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            CreateMap<UpdateZoneRequest, Zone>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            CreateMap<PatchZoneRequest, Zone>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<Zone, ZoneDetailDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            #endregion

            #region Country Mappings

            CreateMap<Country, CountryDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom((src, dest, destMember, context) =>
                    src.Name.GetText(context.Items["Language"] as string)))
                .ForMember(dest => dest.Description, opt => opt.MapFrom((src, dest, destMember, context) =>
                    src.Description.GetText(context.Items["Language"] as string)))
                .ForMember(dest => dest.Language, opt => opt.MapFrom((src, dest, destMember, context) =>
                    context.Items["Language"] as string ?? string.Empty));

            CreateMap<Country, CountryDetailDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<CreateCountryRequest, Country>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.CurrencySymbol, opt => opt.MapFrom(src => src.CurrencySymbol))
                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode))
                .ForMember(dest => dest.TimeZone, opt => opt.MapFrom(src => src.TimeZone))
                .ForMember(dest => dest.Continent, opt => opt.MapFrom(src => src.Continent))
                .ForMember(dest => dest.ZoneId, opt => opt.MapFrom(src => src.ZoneId))
                .ForMember(dest => dest.Zone, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<UpdateCountryRequest, Country>()  
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.CurrencySymbol, opt => opt.MapFrom(src => src.CurrencySymbol))
                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageCode))
                .ForMember(dest => dest.TimeZone, opt => opt.MapFrom(src => src.TimeZone))
                .ForMember(dest => dest.Continent, opt => opt.MapFrom(src => src.Continent))
                .ForMember(dest => dest.ZoneId, opt => opt.MapFrom(src => src.ZoneId))
                .ForMember(dest => dest.Zone, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            CreateMap<PatchCountryRequest, Country>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Zone, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            CreateMap<DeleteCountryRequest, Country>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            #endregion
        
            #region City Mappings

            CreateMap<City, CityDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom((src, dest, destMember, context) =>
                    src.Name.GetText(context.Items["Language"] as string)))
                .ForMember(dest => dest.Description, opt => opt.MapFrom((src, dest, destMember, context) =>
                    src.Description.GetText(context.Items["Language"] as string)))
                .ForMember(dest => dest.Language, opt => opt.MapFrom((src, dest, destMember, context) =>
                    context.Items["Language"] as string ?? string.Empty));

            CreateMap<City, CityDetailDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<CreateCityRequest, City>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.CountryId))
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.CountryCode))
                .ForMember(dest => dest.ZoneId, opt => opt.MapFrom(src => src.ZoneId))
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.Zone, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<UpdateCityRequest, City>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.CountryId))
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.CountryCode))
                .ForMember(dest => dest.ZoneId, opt => opt.MapFrom(src => src.ZoneId))
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.Zone, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<PatchCityRequest, City>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.CountryId))
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.CountryCode))
                .ForMember(dest => dest.ZoneId, opt => opt.MapFrom(src => src.ZoneId))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.Zone, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<DeleteCityRequest, City>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            #endregion

            #region Airport Mappings

            CreateMap<Airport, AirportDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom((src, dest, destMember, context) =>
                    src.Name.GetText(context.Items["Language"] as string)))
                .ForMember(dest => dest.Description, opt => opt.MapFrom((src, dest, destMember, context) =>
                    src.Description.GetText(context.Items["Language"] as string)))
                .ForMember(dest => dest.Language, opt => opt.MapFrom((src, dest, destMember, context) =>
                    context.Items["Language"] as string ?? string.Empty));

            CreateMap<Airport, AirportDetailDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<CreateAirportRequest, Airport>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.CityId))
                .ForMember(dest => dest.ZoneId, opt => opt.MapFrom(src => src.ZoneId))
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.CountryId))
                .ForMember(dest => dest.IataCode, opt => opt.MapFrom(src => src.IataCode))
                .ForMember(dest => dest.IcaoCode, opt => opt.MapFrom(src => src.IcaoCode))
                .ForMember(dest => dest.TimeZone, opt => opt.MapFrom(src => src.TimeZone))
                .ForMember(dest => dest.GMTOffset, opt => opt.MapFrom(src => src.GmtOffset ?? 0))
                .ForMember(dest => dest.DSTOffset, opt => opt.MapFrom(src => src.DstOffset ?? 0))
                .ForMember(dest => dest.City, opt => opt.Ignore())
                .ForMember(dest => dest.Zone, opt => opt.Ignore())
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<UpdateAirportRequest, Airport>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.CityId))
                .ForMember(dest => dest.ZoneId, opt => opt.MapFrom(src => src.ZoneId))
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.CountryId))
                .ForMember(dest => dest.IataCode, opt => opt.MapFrom(src => src.IataCode))
                .ForMember(dest => dest.IcaoCode, opt => opt.MapFrom(src => src.IcaoCode))
                .ForMember(dest => dest.TimeZone, opt => opt.MapFrom(src => src.TimeZone))
                .ForMember(dest => dest.GMTOffset, opt => opt.MapFrom(src => src.GmtOffset ?? 0))
                .ForMember(dest => dest.DSTOffset, opt => opt.MapFrom(src => src.DstOffset ?? 0))
                .ForMember(dest => dest.City, opt => opt.Ignore())
                .ForMember(dest => dest.Zone, opt => opt.Ignore())
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<PatchAirportRequest, Airport>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.CityId))
                .ForMember(dest => dest.ZoneId, opt => opt.MapFrom(src => src.ZoneId))
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.CountryId))
                .ForMember(dest => dest.IataCode, opt => opt.MapFrom(src => src.IataCode))
                .ForMember(dest => dest.IcaoCode, opt => opt.MapFrom(src => src.IcaoCode))
                .ForMember(dest => dest.TimeZone, opt => opt.MapFrom(src => src.TimeZone))
                .ForMember(dest => dest.GMTOffset, opt => opt.MapFrom(src => src.GmtOffset ?? 0))
                .ForMember(dest => dest.DSTOffset, opt => opt.MapFrom(src => src.DstOffset ?? 0))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.City, opt => opt.Ignore())
                .ForMember(dest => dest.Zone, opt => opt.Ignore())
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<DeleteAirportRequest, Airport>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<Country, CountryDetailDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<CreateRegionRequest, Region>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.CountryId))
                .ForMember(dest => dest.ZoneId, opt => opt.MapFrom(src => src.ZoneId))
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.Zone, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<UpdateRegionRequest, Region>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.CountryId))
                .ForMember(dest => dest.ZoneId, opt => opt.MapFrom(src => src.ZoneId))
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.Zone, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<PatchRegionRequest, Region>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.CountryId))
                .ForMember(dest => dest.ZoneId, opt => opt.MapFrom(src => src.ZoneId))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.Zone, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<DeleteRegionRequest, Region>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            #endregion
        }
    }
}