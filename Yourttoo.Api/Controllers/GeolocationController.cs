using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Yourttoo.Api.DataAccess;
using Yourttoo.DTOs.DTOs.Geolocation.Zone;
using Yourttoo.DTOs.Requests.Geolocation.Zone;
using Yourttoo.DTOs.DTOs.Geolocation.Country;
using Yourttoo.DTOs.Requests.Geolocation.Country;
using Yourttoo.DTOs.DTOs.Geolocation.City;
using Yourttoo.DTOs.Requests.Geolocation.City;
using Yourttoo.DTOs.DTOs.Geolocation.Airport;
using Yourttoo.DTOs.Requests.Geolocation.Airport;
using Yourttoo.DTOs.DTOs.Geolocation.Region;
using Yourttoo.DTOs.Requests.Geolocation.Region;
using Yourttoo.DTOs.DTOs.Geolocation.Country;
using Yourttoo.DTOs.Requests.Geolocation.Country;
using Yourttoo.DTOs.Shared.API;
using Yourttoo.DTOs.Shared.Pagination;
using Yourttoo.DTOs.Models.Geolocation;
using Yourttoo.DTOs.Common.Constants;
using Yourttoo.Api.Attributes;

namespace Yourttoo.Api.Controllers
{
    [Route("/store/geolocation")]
    [ApiController]
    [LanguageFromHeader]
    public class GeolocationController : ControllerBase
    {
        private readonly ILogger<GeolocationController> _logger;
        private readonly IMapper _mapper;
        private readonly DatabaseContext _context;

        public GeolocationController(ILogger<GeolocationController> logger, IMapper mapper, DatabaseContext context)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        #region Zone Endpoints

        [HttpGet("zones")]
        [ProducesResponseType(typeof(PaginatedApiResponse<ZoneDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetZones([FromQuery] QueryZoneRequest request, [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation($"GeolocationController --> GetZones --> Start: {DateTime.UtcNow}");

            // Extraer idioma del header Accept-Language

            // Log de parámetros de la request
            _logger.LogInformation(
                "Request Parameters: SearchTerm={SearchTerm}, Status={Status}, Category={Category}, PromotionArea={PromotionArea}, Language={Language}, Page={Page}, PageSize={PageSize}",
                request.SearchTerm, request.Status, request.Category, request.PromotionArea,
                HttpContext.Items["Language"] as string, page, pageSize);

            try
            {
                var query = _context.Zones.AsNoTracking();

                // Aplicar filtros
                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    if (!string.IsNullOrWhiteSpace(HttpContext.Items["Language"] as string))
                    {
                        // Filtrar por idioma específico y buscar en ese idioma
                        var searchLanguage =
                            Languages.GetValidLanguageOrDefault(HttpContext.Items["Language"] as string);
                        query = query.Where(z =>
                            z.Name.GetText(searchLanguage) != null &&
                            z.Name.GetText(searchLanguage).Contains(request.SearchTerm) ||
                            z.Description.GetText(searchLanguage) != null &&
                            z.Description.GetText(searchLanguage).Contains(request.SearchTerm));
                    }
                    else
                    {
                        // Buscar en todos los idiomas
                        query = query.Where(z =>
                            z.Name.GetText(Languages.Default) != null &&
                            z.Name.GetText(Languages.Default).Contains(request.SearchTerm) ||
                            z.Description.GetText(Languages.Default) != null && z.Description.GetText(Languages.Default)
                                .Contains(request.SearchTerm));
                    }
                }

                if (!string.IsNullOrWhiteSpace(request.Status))
                {
                    query = query.Where(z => z.Status == request.Status);
                }

                if (!string.IsNullOrWhiteSpace(request.Category))
                {
                    query = query.Where(z => z.Category == request.Category);
                }

                if (!string.IsNullOrWhiteSpace(request.PromotionArea))
                {
                    query = query.Where(z => z.PromotionArea == request.PromotionArea);
                }

                if (request.MinPromotionAreaPriority.HasValue)
                {
                    query = query.Where(z => z.PromotionAreaPriority >= request.MinPromotionAreaPriority.Value);
                }

                if (request.MaxPromotionAreaPriority.HasValue)
                {
                    query = query.Where(z => z.PromotionAreaPriority <= request.MaxPromotionAreaPriority.Value);
                }

                if (request.MinAveragePrice.HasValue)
                {
                    query = query.Where(z => z.AveragePrice >= request.MinAveragePrice.Value);
                }

                if (request.MaxAveragePrice.HasValue)
                {
                    query = query.Where(z => z.AveragePrice <= request.MaxAveragePrice.Value);
                }

                if (request.MinLatitude.HasValue)
                {
                    query = query.Where(z => z.Latitude >= request.MinLatitude.Value);
                }

                if (request.MaxLatitude.HasValue)
                {
                    query = query.Where(z => z.Latitude <= request.MaxLatitude.Value);
                }

                if (request.MinLongitude.HasValue)
                {
                    query = query.Where(z => z.Longitude >= request.MinLongitude.Value);
                }

                if (request.MaxLongitude.HasValue)
                {
                    query = query.Where(z => z.Longitude <= request.MaxLongitude.Value);
                }

                var totalCount = await query.CountAsync();
                var zones = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                var language = HttpContext.Items["Language"] as string;
                var zoneDtos = _mapper.Map<List<Zone>, List<ZoneDTO>>(zones, opt =>
                    opt.Items["Language"] = language);

                var pagination = new PaginatedParameters(page, pageSize, totalCount);

                _logger.LogInformation("GeolocationController --> GetZones --> End at {EndTime}", DateTime.UtcNow);
                return Ok(new PaginatedApiResponse<ZoneDTO>(zoneDtos, pagination));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving zones");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpGet("zones/{id}")]
        [ProducesResponseType(typeof(ZoneDetailDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetZoneById(Guid id)
        {
            _logger.LogInformation("GeolocationController --> GetZoneById --> Start: {Id}", id);
            try
            {
                var zone = await _context.Zones.FindAsync(id);
                if (zone == null)
                {
                    return NotFound(new ApiResponse<string>("Zone not found"));
                }

                var zoneDto = _mapper.Map<Zone, ZoneDetailDTO>(zone);

                _logger.LogInformation("GeolocationController --> GetZoneById --> End: {Id}", id);
                return Ok(new ApiResponse<ZoneDetailDTO>(zoneDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving zone by id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPost("zones")]
        [ProducesResponseType(typeof(ZoneDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateZone([FromBody] CreateZoneRequest request)
        {
            _logger.LogInformation("GeolocationController --> CreateZone --> Start");
            try
            {
                var zone = _mapper.Map<CreateZoneRequest, Zone>(request);
                await _context.Zones.AddAsync(zone);
                await _context.SaveChangesAsync();

                // Obtener el idioma del HttpContext (establecido por el atributo LanguageFromHeader)
                var language = HttpContext.Items["Language"] as string;

                var zoneDto = _mapper.Map<Zone, ZoneDTO>(zone, opt =>
                    opt.Items["Language"] = language);
                _logger.LogInformation("GeolocationController --> CreateZone --> End at {EndTime}", DateTime.UtcNow);
                return CreatedAtAction(nameof(GetZoneById), new { id = zone.Id }, new ApiResponse<ZoneDTO>(zoneDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating zone");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPut("zones/{id}")]
        [ProducesResponseType(typeof(ZoneDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateZone(Guid id, [FromBody] UpdateZoneRequest request)
        {
            _logger.LogInformation("GeolocationController --> UpdateZone --> Start: {Id}", id);
            try
            {
                if (id != request.Id)
                {
                    return BadRequest(new ApiResponse<string>("El ID de la URL no coincide con el ID del request"));
                }

                var zone = await _context.Zones.FindAsync(id);
                if (zone == null)
                {
                    return NotFound(new ApiResponse<string>("Zone not found"));
                }

                zone.Latitude = request.Latitude;
                zone.Longitude = request.Longitude;
                zone.AveragePrice = request.AveragePrice;
                zone.Name = request.Name;
                zone.Description = request.Description;
                zone.ImageUrl = request.ImageUrl;
                zone.IconUrl = request.IconUrl;
                zone.BackgroundColor = request.BackgroundColor;
                zone.Status = request.Status;
                zone.ThumbnailUrl = request.ThumbnailUrl;
                zone.Category = request.Category;
                zone.PromotionArea = request.PromotionArea;
                zone.PromotionAreaPriority = request.PromotionAreaPriority;
                zone.UpdatedBy = request.UpdatedBy ?? "System";
                zone.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                var language = HttpContext.Items["Language"] as string;
                var zoneDto = _mapper.Map<Zone, ZoneDTO>(zone, opt => 
                    opt.Items["Language"] = language);

                _logger.LogInformation("GeolocationController --> UpdateZone --> End: {Id}", id);
                return Ok(new ApiResponse<ZoneDTO>(zoneDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating zone: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPatch("zones/{id}")]
        [ProducesResponseType(typeof(ZoneDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchZone(Guid id, [FromBody] PatchZoneRequest request)
        {
            _logger.LogInformation("GeolocationController --> PatchZone --> Start: {Id}", id);
            try
            {
                if (id != request.Id)
                {
                    return BadRequest(new ApiResponse<string>("El ID de la URL no coincide con el ID del request"));
                }

                var zone = await _context.Zones.FindAsync(id);
                if (zone == null)
                {
                    return NotFound(new ApiResponse<string>("Zone not found"));
                }

                // Actualizar solo los campos proporcionados
                if (request.Latitude.HasValue) zone.Latitude = request.Latitude.Value;
                if (request.Longitude.HasValue) zone.Longitude = request.Longitude.Value;
                if (request.AveragePrice.HasValue) zone.AveragePrice = request.AveragePrice.Value;
                if (request.Name != null) zone.Name = request.Name;
                if (request.Description != null) zone.Description = request.Description;
                if (request.ImageUrl != null) zone.ImageUrl = request.ImageUrl;
                if (request.IconUrl != null) zone.IconUrl = request.IconUrl;
                if (request.BackgroundColor != null) zone.BackgroundColor = request.BackgroundColor;
                if (request.Status != null) zone.Status = request.Status;
                if (request.ThumbnailUrl != null) zone.ThumbnailUrl = request.ThumbnailUrl;
                if (request.Category != null) zone.Category = request.Category;
                if (request.PromotionArea != null) zone.PromotionArea = request.PromotionArea;
                if (request.PromotionAreaPriority.HasValue)
                    zone.PromotionAreaPriority = request.PromotionAreaPriority.Value;

                zone.UpdatedBy = request.CreatedBy ?? "System";
                zone.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var language = HttpContext.Items["Language"] as string;
                var zoneDto = _mapper.Map<Zone, ZoneDTO>(zone, opt => 
                    opt.Items["Language"] = language);
                _logger.LogInformation("GeolocationController --> PatchZone --> End: {Id}", id);
                return Ok(new ApiResponse<ZoneDTO>(zoneDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error patching zone: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpDelete("zones/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteZone(Guid id)
        {
            _logger.LogInformation("GeolocationController --> DeleteZone --> Start: {Id}", id);
            try
            {
                var zone = await _context.Zones.FindAsync(id);
                if (zone == null)
                {
                    return NotFound(new ApiResponse<string>("Zone not found"));
                }

                _context.Zones.Remove(zone);
                await _context.SaveChangesAsync();

                _logger.LogInformation("GeolocationController --> DeleteZone --> End: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting zone: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        #endregion

        #region Country Endpoints

        [HttpGet("countries")]
        [ProducesResponseType(typeof(PaginatedApiResponse<CountryDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries([FromQuery] QueryCountryRequest request, [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation($"GeolocationController --> GetCountries --> Start: {DateTime.UtcNow}");

            // Extraer idioma del header Accept-Language
            var language = HttpContext.Items["Language"] as string;


            // Log de parámetros de la request
            _logger.LogInformation(
                "Request Parameters: SearchTerm={SearchTerm}, Status={Status}, Category={Category}, Currency={Currency}, LanguageCode={LanguageCode}, Language={Language}, Page={Page}, PageSize={PageSize}",
                request.SearchTerm, request.Status, request.Category, request.Currency, request.LanguageCode, language,
                page, pageSize);

            try
            {
                var query = _context.Countries.AsNoTracking();

                // Aplicar filtros
                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    if (!string.IsNullOrWhiteSpace(language))
                    {
                        // Filtrar por idioma específico y buscar en ese idioma
                        query = query.Where(c =>
                            c.Name.GetText(language) != null &&
                            c.Name.GetText(language).Contains(request.SearchTerm) ||
                            c.Description.GetText(language) != null &&
                            c.Description.GetText(language).Contains(request.SearchTerm));
                    }
                    else
                    {
                        // Buscar en todos los idiomas
                        query = query.Where(c => c.Name.GetText(Languages.Default) != null &&
                                                 c.Name.GetText(Languages.Default).Contains(request.SearchTerm) ||
                                                 c.Description.GetText(Languages.Default) != null &&
                                                 c.Description.GetText(Languages.Default).Contains(request.SearchTerm));
                    }
                }

                if (!string.IsNullOrWhiteSpace(request.Status))
                {
                    query = query.Where(c => c.Status == request.Status);
                }

                if (!string.IsNullOrWhiteSpace(request.Category))
                {
                    query = query.Where(c => c.Category == request.Category);
                }

                if (!string.IsNullOrWhiteSpace(request.Currency))
                {
                    query = query.Where(c => c.Currency == request.Currency);
                }

                if (!string.IsNullOrWhiteSpace(request.LanguageCode))
                {
                    query = query.Where(c => c.LanguageCode == request.LanguageCode);
                }

                if (!string.IsNullOrWhiteSpace(request.TimeZone))
                {
                    query = query.Where(c => c.TimeZone == request.TimeZone);
                }

                if (request.MinAveragePrice.HasValue)
                {
                    query = query.Where(c => c.AveragePrice >= request.MinAveragePrice.Value);
                }

                if (request.MaxAveragePrice.HasValue)
                {
                    query = query.Where(c => c.AveragePrice <= request.MaxAveragePrice.Value);
                }

                if (request.MinLatitude.HasValue)
                {
                    query = query.Where(c => c.Latitude >= request.MinLatitude.Value);
                }

                if (request.MaxLatitude.HasValue)
                {
                    query = query.Where(c => c.Latitude <= request.MaxLatitude.Value);
                }

                if (request.MinLongitude.HasValue)
                {
                    query = query.Where(c => c.Longitude >= request.MinLongitude.Value);
                }

                if (request.MaxLongitude.HasValue)
                {
                    query = query.Where(c => c.Longitude <= request.MaxLongitude.Value);
                }

                var totalCount = await query.CountAsync();
                var countries = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                var countryDtos = _mapper.Map<List<Country>, List<CountryDTO>>(countries, opt =>
                    opt.Items["Language"] = language);

                var pagination = new PaginatedParameters(page, pageSize, totalCount);

                _logger.LogInformation("GeolocationController --> GetCountries --> End at {EndTime}", DateTime.UtcNow);
                return Ok(new PaginatedApiResponse<CountryDTO>(countryDtos, pagination));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving countries");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpGet("countries/{id}")]
        [ProducesResponseType(typeof(CountryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountryById(Guid id)
        {
            _logger.LogInformation("GeolocationController --> GetCountryById --> Start: {Id}", id);
            try
            {
                var country = await _context.Countries.FindAsync(id);
                if (country == null)
                {
                    return NotFound(new ApiResponse<string>("Country not found"));
                }

                var countryDto = _mapper.Map<Country, CountryDetailDTO>(country);
                _logger.LogInformation("GeolocationController --> GetCountryById --> End: {Id}", id);
                return Ok(new ApiResponse<CountryDetailDTO>(countryDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving country by id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPost("countries")]
        [ProducesResponseType(typeof(CountryDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryRequest request)
        {
            _logger.LogInformation("GeolocationController --> CreateCountry --> Start");
            try
            {
                var country = _mapper.Map<CreateCountryRequest, Country>(request);
                _context.Countries.Add(country);
                await _context.SaveChangesAsync();

                // Obtener el idioma del HttpContext (establecido por el atributo LanguageFromHeader)
                var language = HttpContext.Items["Language"] as string;

                var countryDto = _mapper.Map<Country, CountryDTO>(country, opt =>
                    opt.Items["Language"] = language);
                _logger.LogInformation("GeolocationController --> CreateCountry --> End at {EndTime}", DateTime.UtcNow);
                return CreatedAtAction(nameof(GetCountryById),
                    new { id = country.Id },
                    new ApiResponse<CountryDTO>(countryDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating country");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPut("countries/{id}")]
        [ProducesResponseType(typeof(CountryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountry(Guid id, [FromBody] UpdateCountryRequest request)
        {
            _logger.LogInformation("GeolocationController --> UpdateCountry --> Start: {Id}", id);
            try
            {
                if (id != request.Id)
                {
                    return BadRequest(new ApiResponse<string>("El ID de la URL no coincide con el ID del request"));
                }

                var country = await _context.Countries.FindAsync(id);
                if (country == null)
                {
                    return NotFound(new ApiResponse<string>("Country not found"));
                }

                country.Latitude = request.Latitude;
                country.Longitude = request.Longitude;
                country.AveragePrice = request.AveragePrice;
                country.Name = request.Name;
                country.Description = request.Description;
                country.ImageUrl = request.ImageUrl;
                country.IconUrl = request.IconUrl;
                country.BackgroundColor = request.BackgroundColor;
                country.Status = request.Status;
                country.ThumbnailUrl = request.ThumbnailUrl;
                country.Category = request.Category;
                country.Currency = request.Currency;
                country.CurrencySymbol = request.CurrencySymbol;
                country.LanguageCode = request.LanguageCode;
                country.TimeZone = request.TimeZone;
                country.UpdatedBy = request.CreatedBy ?? "System";
                country.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Obtener el idioma del HttpContext (establecido por el atributo LanguageFromHeader)
                var language = HttpContext.Items["Language"] as string;

                var countryDto = _mapper.Map<CountryDTO>(country, opt =>
                    opt.Items["Language"] = language);
                _logger.LogInformation("GeolocationController --> UpdateCountry --> End: {Id}", id);
                return Ok(new ApiResponse<CountryDTO>(countryDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating country: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPatch("countries/{id}")]
        [ProducesResponseType(typeof(CountryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchCountry(Guid id, [FromBody] PatchCountryRequest request)
        {
            _logger.LogInformation("GeolocationController --> PatchCountry --> Start: {Id}", id);
            try
            {
                if (id != request.Id)
                {
                    return BadRequest(new ApiResponse<string>("El ID de la URL no coincide con el ID del request"));
                }

                var country = await _context.Countries.FindAsync(id);
                if (country == null)
                {
                    return NotFound(new ApiResponse<string>("Country not found"));
                }

                // Actualizar solo los campos proporcionados
                if (request.Latitude != null) country.Latitude = request.Latitude;
                if (request.Longitude != null) country.Longitude = request.Longitude;
                if (request.AveragePrice != null) country.AveragePrice = request.AveragePrice;
                if (request.Name != null) country.Name = request.Name;
                if (request.Description != null) country.Description = request.Description;
                if (request.ImageUrl != null) country.ImageUrl = request.ImageUrl;
                if (request.IconUrl != null) country.IconUrl = request.IconUrl;
                if (request.BackgroundColor != null) country.BackgroundColor = request.BackgroundColor;
                if (request.Status != null) country.Status = request.Status;
                if (request.ThumbnailUrl != null) country.ThumbnailUrl = request.ThumbnailUrl;
                if (request.Category != null) country.Category = request.Category;
                if (request.Currency != null) country.Currency = request.Currency;
                if (request.CurrencySymbol != null) country.CurrencySymbol = request.CurrencySymbol;
                if (request.LanguageCode != null) country.LanguageCode = request.LanguageCode;
                if (request.TimeZone != null) country.TimeZone = request.TimeZone;

                country.UpdatedBy = request.CreatedBy ?? "System";
                country.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Obtener el idioma del HttpContext (establecido por el atributo LanguageFromHeader)
                var language = HttpContext.Items["Language"] as string;

                var countryDto = _mapper.Map<CountryDTO>(country, opt =>
                    opt.Items["Language"] = language);
                _logger.LogInformation("GeolocationController --> PatchCountry --> End: {Id}", id);
                return Ok(new ApiResponse<CountryDTO>(countryDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error patching country: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpDelete("countries/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCountry(Guid id)
        {
            _logger.LogInformation("GeolocationController --> DeleteCountry --> Start: {Id}", id);
            try
            {
                var country = await _context.Countries.FindAsync(id);
                if (country == null)
                {
                    return NotFound(new ApiResponse<string>("Country not found"));
                }

                _context.Countries.Remove(country);
                await _context.SaveChangesAsync();

                _logger.LogInformation("GeolocationController --> DeleteCountry --> End: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting country: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }


        // method to get countries by zoneId
        [HttpGet("countries/zone/{zoneId}")]
        [ProducesResponseType(typeof(List<CountryDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountriesByZoneId(Guid zoneId)
        {
            _logger.LogInformation("GeolocationController --> GetCountriesByZoneId --> Start: {ZoneId}", zoneId);
            try
            {
                var countries = await _context.Countries.Where(c => c.ZoneId == zoneId).ToListAsync();
                
                var language = HttpContext.Items["Language"] as string;
                var countryDtos = _mapper.Map<List<Country>, List<CountryDTO>>(countries, opt => 
                    opt.Items["Language"] = language);
                _logger.LogInformation("GeolocationController --> GetCountriesByZoneId --> End: {ZoneId}", zoneId);
                return Ok(new ApiResponse<List<CountryDTO>>(countryDtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving countries by zone id: {ZoneId}", zoneId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }
        #endregion
    
        #region City Endpoints

        [HttpGet("cities")]
        [ProducesResponseType(typeof(PaginatedApiResponse<CityDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCities([FromQuery] QueryCityRequest request, [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation($"GeolocationController --> GetCities --> Start: {DateTime.UtcNow}");

            var language = HttpContext.Items["Language"] as string;

            _logger.LogInformation(
                "Request Parameters: SearchTerm={SearchTerm}, Status={Status}, Category={Category}, CountryId={CountryId}, CountryCode={CountryCode}, ZoneId={ZoneId}, Language={Language}, Page={Page}, PageSize={PageSize}",
                request.SearchTerm, request.Status, request.Category, request.CountryId, request.CountryCode, request.ZoneId, language,
                page, pageSize);

            try
            {
                var query = _context.Cities.AsNoTracking();

                // Aplicar filtros
                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    if (!string.IsNullOrWhiteSpace(language))
                    {
                        query = query.Where(c =>
                            c.Name.GetText(language) != null &&
                            c.Name.GetText(language).Contains(request.SearchTerm) ||
                            c.Description.GetText(language) != null &&
                            c.Description.GetText(language).Contains(request.SearchTerm));
                    }
                    else
                    {
                        query = query.Where(c => c.Name.GetText(Languages.Default) != null &&
                                                 c.Name.GetText(Languages.Default).Contains(request.SearchTerm) ||
                                                 c.Description.GetText(Languages.Default) != null &&
                                                 c.Description.GetText(Languages.Default).Contains(request.SearchTerm));
                    }
                }

                if (!string.IsNullOrWhiteSpace(request.Status))
                {
                    query = query.Where(c => c.Status == request.Status);
                }

                if (!string.IsNullOrWhiteSpace(request.Category))
                {
                    query = query.Where(c => c.Category == request.Category);
                }

                if (request.CountryId.HasValue)
                {
                    query = query.Where(c => c.CountryId == request.CountryId.Value);
                }

                if (!string.IsNullOrWhiteSpace(request.CountryCode))
                {
                    query = query.Where(c => c.CountryCode == request.CountryCode);
                }

                if (request.ZoneId.HasValue)
                {
                    query = query.Where(c => c.ZoneId == request.ZoneId.Value);
                }

                if (request.MinAveragePrice.HasValue)
                {
                    query = query.Where(c => c.AveragePrice >= request.MinAveragePrice.Value);
                }

                if (request.MaxAveragePrice.HasValue)
                {
                    query = query.Where(c => c.AveragePrice <= request.MaxAveragePrice.Value);
                }

                if (request.MinLatitude.HasValue)
                {
                    query = query.Where(c => c.Latitude >= request.MinLatitude.Value);
                }

                if (request.MaxLatitude.HasValue)
                {
                    query = query.Where(c => c.Latitude <= request.MaxLatitude.Value);
                }

                if (request.MinLongitude.HasValue)
                {
                    query = query.Where(c => c.Longitude >= request.MinLongitude.Value);
                }

                if (request.MaxLongitude.HasValue)
                {
                    query = query.Where(c => c.Longitude <= request.MaxLongitude.Value);
                }

                var totalCount = await query.CountAsync();
                var cities = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                var cityDtos = _mapper.Map<List<City>, List<CityDTO>>(cities, opt =>
                    opt.Items["Language"] = language);

                var pagination = new PaginatedParameters(page, pageSize, totalCount);

                _logger.LogInformation("GeolocationController --> GetCities --> End at {EndTime}", DateTime.UtcNow);
                return Ok(new PaginatedApiResponse<CityDTO>(cityDtos, pagination));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cities");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpGet("cities/{id}")]
        [ProducesResponseType(typeof(CityDetailDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCityById(Guid id)
        {
            _logger.LogInformation("GeolocationController --> GetCityById --> Start: {Id}", id);
            try
            {
                var city = await _context.Cities.FindAsync(id);
                if (city == null)
                {
                    return NotFound(new ApiResponse<string>("City not found"));
                }

                var cityDto = _mapper.Map<City, CityDetailDTO>(city);

                _logger.LogInformation("GeolocationController --> GetCityById --> End: {Id}", id);
                return Ok(new ApiResponse<CityDetailDTO>(cityDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving city by id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPost("cities")]
        [ProducesResponseType(typeof(CityDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCity([FromBody] CreateCityRequest request)
        {
            _logger.LogInformation("GeolocationController --> CreateCity --> Start");
            try
            {
                var city = _mapper.Map<CreateCityRequest, City>(request);
                _context.Cities.Add(city);
                await _context.SaveChangesAsync();

                var language = HttpContext.Items["Language"] as string;

                var cityDto = _mapper.Map<City, CityDTO>(city, opt =>
                    opt.Items["Language"] = language);
                _logger.LogInformation("GeolocationController --> CreateCity --> End at {EndTime}", DateTime.UtcNow);
                return CreatedAtAction(nameof(GetCityById),
                    new { id = city.Id },
                    new ApiResponse<CityDTO>(cityDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating city");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPut("cities/{id}")]
        [ProducesResponseType(typeof(CityDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCity(Guid id, [FromBody] UpdateCityRequest request)
        {
            _logger.LogInformation("GeolocationController --> UpdateCity --> Start: {Id}", id);
            try
            {
                if (id != request.Id)
                {
                    return BadRequest(new ApiResponse<string>("El ID de la URL no coincide con el ID del request"));
                }

                var city = await _context.Cities.FindAsync(id);
                if (city == null)
                {
                    return NotFound(new ApiResponse<string>("City not found"));
                }

                city.Latitude = request.Latitude;
                city.Longitude = request.Longitude;
                city.AveragePrice = request.AveragePrice;
                city.Name = request.Name;
                city.Description = request.Description;
                city.ImageUrl = request.ImageUrl;
                city.IconUrl = request.IconUrl;
                city.BackgroundColor = request.BackgroundColor;
                city.Status = request.Status;
                city.ThumbnailUrl = request.ThumbnailUrl;
                city.Category = request.Category;
                city.CountryId = request.CountryId;
                city.CountryCode = request.CountryCode;
                city.ZoneId = request.ZoneId;
                city.UpdatedBy = request.UpdatedBy ?? "System";
                city.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var language = HttpContext.Items["Language"] as string;
                var cityDto = _mapper.Map<City, CityDTO>(city, opt =>
                    opt.Items["Language"] = language);

                _logger.LogInformation("GeolocationController --> UpdateCity --> End: {Id}", id);
                return Ok(new ApiResponse<CityDTO>(cityDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating city: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPatch("cities/{id}")]
        [ProducesResponseType(typeof(CityDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchCity(Guid id, [FromBody] PatchCityRequest request)
        {
            _logger.LogInformation("GeolocationController --> PatchCity --> Start: {Id}", id);
            try
            {
                if (id != request.Id)
                {
                    return BadRequest(new ApiResponse<string>("El ID de la URL no coincide con el ID del request"));
                }

                var city = await _context.Cities.FindAsync(id);
                if (city == null)
                {
                    return NotFound(new ApiResponse<string>("City not found"));
                }

                // Actualizar solo los campos proporcionados
                if (request.Latitude.HasValue) city.Latitude = request.Latitude.Value;
                if (request.Longitude.HasValue) city.Longitude = request.Longitude.Value;
                if (request.AveragePrice.HasValue) city.AveragePrice = request.AveragePrice.Value;
                if (request.Name != null) city.Name = request.Name;
                if (request.Description != null) city.Description = request.Description;
                if (request.ImageUrl != null) city.ImageUrl = request.ImageUrl;
                if (request.IconUrl != null) city.IconUrl = request.IconUrl;
                if (request.BackgroundColor != null) city.BackgroundColor = request.BackgroundColor;
                if (request.Status != null) city.Status = request.Status;
                if (request.ThumbnailUrl != null) city.ThumbnailUrl = request.ThumbnailUrl;
                if (request.Category != null) city.Category = request.Category;
                if (request.CountryId.HasValue) city.CountryId = request.CountryId.Value;
                if (request.CountryCode != null) city.CountryCode = request.CountryCode;
                if (request.ZoneId.HasValue) city.ZoneId = request.ZoneId.Value;

                city.UpdatedBy = request.CreatedBy ?? "System";
                city.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var language = HttpContext.Items["Language"] as string;
                var cityDto = _mapper.Map<City, CityDTO>(city, opt =>
                    opt.Items["Language"] = language);
                _logger.LogInformation("GeolocationController --> PatchCity --> End: {Id}", id);
                return Ok(new ApiResponse<CityDTO>(cityDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error patching city: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpDelete("cities/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCity(Guid id)
        {
            _logger.LogInformation("GeolocationController --> DeleteCity --> Start: {Id}", id);
            try
            {
                var city = await _context.Cities.FindAsync(id);
                if (city == null)
                {
                    return NotFound(new ApiResponse<string>("City not found"));
                }

                _context.Cities.Remove(city);
                await _context.SaveChangesAsync();

                _logger.LogInformation("GeolocationController --> DeleteCity --> End: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting city: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpGet("cities/country/{countryId}")]
        [ProducesResponseType(typeof(List<CityDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCitiesByCountryId(Guid countryId)
        {
            _logger.LogInformation("GeolocationController --> GetCitiesByCountryId --> Start: {CountryId}", countryId);
            try
            {
                var cities = await _context.Cities.Where(c => c.CountryId == countryId).ToListAsync();
                
                var language = HttpContext.Items["Language"] as string;
                var cityDtos = _mapper.Map<List<City>, List<CityDTO>>(cities, opt => 
                    opt.Items["Language"] = language);
                _logger.LogInformation("GeolocationController --> GetCitiesByCountryId --> End: {CountryId}", countryId);
                return Ok(new ApiResponse<List<CityDTO>>(cityDtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cities by country id: {CountryId}", countryId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpGet("cities/zone/{zoneId}")]
        [ProducesResponseType(typeof(List<CityDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCitiesByZoneId(Guid zoneId)
        {
            _logger.LogInformation("GeolocationController --> GetCitiesByZoneId --> Start: {ZoneId}", zoneId);
            try
            {
                var cities = await _context.Cities.Where(c => c.ZoneId == zoneId).ToListAsync();
                
                var language = HttpContext.Items["Language"] as string;
                var cityDtos = _mapper.Map<List<City>, List<CityDTO>>(cities, opt => 
                    opt.Items["Language"] = language);
                _logger.LogInformation("GeolocationController --> GetCitiesByZoneId --> End: {ZoneId}", zoneId);
                return Ok(new ApiResponse<List<CityDTO>>(cityDtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cities by zone id: {ZoneId}", zoneId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        #endregion
    
        #region Airport Endpoints

        [HttpGet("airports")]
        [ProducesResponseType(typeof(PaginatedApiResponse<AirportDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAirports([FromQuery] QueryAirportRequest request, [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation($"GeolocationController --> GetAirports --> Start: {DateTime.UtcNow}");

            var language = HttpContext.Items["Language"] as string;

            _logger.LogInformation(
                "Request Parameters: SearchTerm={SearchTerm}, Status={Status}, Category={Category}, CityId={CityId}, ZoneId={ZoneId}, CountryId={CountryId}, IataCode={IataCode}, IcaoCode={IcaoCode}, TimeZone={TimeZone}, Language={Language}, Page={Page}, PageSize={PageSize}",
                request.SearchTerm, request.Status, request.Category, request.CityId, request.ZoneId, request.CountryId, 
                request.IataCode, request.IcaoCode, request.TimeZone, language, page, pageSize);

            try
            {
                var query = _context.Airports.AsNoTracking();

                // Aplicar filtros
                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    if (!string.IsNullOrWhiteSpace(language))
                    {
                        query = query.Where(a =>
                            a.Name.GetText(language) != null &&
                            a.Name.GetText(language).Contains(request.SearchTerm) ||
                            a.Description.GetText(language) != null &&
                            a.Description.GetText(language).Contains(request.SearchTerm));
                    }
                    else
                    {
                        query = query.Where(a => a.Name.GetText(Languages.Default) != null &&
                                                 a.Name.GetText(Languages.Default).Contains(request.SearchTerm) ||
                                                 a.Description.GetText(Languages.Default) != null &&
                                                 a.Description.GetText(Languages.Default).Contains(request.SearchTerm));
                    }
                }

                if (!string.IsNullOrWhiteSpace(request.Status))
                {
                    query = query.Where(a => a.Status == request.Status);
                }

                if (!string.IsNullOrWhiteSpace(request.Category))
                {
                    query = query.Where(a => a.Category == request.Category);
                }

                if (request.CityId.HasValue)
                {
                    query = query.Where(a => a.CityId == request.CityId.Value);
                }

                if (request.ZoneId.HasValue)
                {
                    query = query.Where(a => a.ZoneId == request.ZoneId.Value);
                }

                if (request.CountryId.HasValue)
                {
                    query = query.Where(a => a.CountryId == request.CountryId.Value);
                }

                if (!string.IsNullOrWhiteSpace(request.IataCode))
                {
                    query = query.Where(a => a.IataCode == request.IataCode);
                }

                if (!string.IsNullOrWhiteSpace(request.IcaoCode))
                {
                    query = query.Where(a => a.IcaoCode == request.IcaoCode);
                }

                if (!string.IsNullOrWhiteSpace(request.TimeZone))
                {
                    query = query.Where(a => a.TimeZone == request.TimeZone);
                }

                if (request.MinAveragePrice.HasValue)
                {
                    query = query.Where(a => a.AveragePrice >= request.MinAveragePrice.Value);
                }

                if (request.MaxAveragePrice.HasValue)
                {
                    query = query.Where(a => a.AveragePrice <= request.MaxAveragePrice.Value);
                }

                if (request.MinLatitude.HasValue)
                {
                    query = query.Where(a => a.Latitude >= request.MinLatitude.Value);
                }

                if (request.MaxLatitude.HasValue)
                {
                    query = query.Where(a => a.Latitude <= request.MaxLatitude.Value);
                }

                if (request.MinLongitude.HasValue)
                {
                    query = query.Where(a => a.Longitude >= request.MinLongitude.Value);
                }

                if (request.MaxLongitude.HasValue)
                {
                    query = query.Where(a => a.Longitude <= request.MaxLongitude.Value);
                }

                var totalCount = await query.CountAsync();
                var airports = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                var airportDtos = _mapper.Map<List<Airport>, List<AirportDTO>>(airports, opt =>
                    opt.Items["Language"] = language);

                var pagination = new PaginatedParameters(page, pageSize, totalCount);

                _logger.LogInformation("GeolocationController --> GetAirports --> End at {EndTime}", DateTime.UtcNow);
                return Ok(new PaginatedApiResponse<AirportDTO>(airportDtos, pagination));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving airports");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpGet("airports/{id}")]
        [ProducesResponseType(typeof(AirportDetailDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAirportById(Guid id)
        {
            _logger.LogInformation("GeolocationController --> GetAirportById --> Start: {Id}", id);
            try
            {
                var airport = await _context.Airports.FindAsync(id);
                if (airport == null)
                {
                    return NotFound(new ApiResponse<string>("Airport not found"));
                }

                var airportDto = _mapper.Map<Airport, AirportDetailDTO>(airport);

                _logger.LogInformation("GeolocationController --> GetAirportById --> End: {Id}", id);
                return Ok(new ApiResponse<AirportDetailDTO>(airportDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving airport by id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPost("airports")]
        [ProducesResponseType(typeof(AirportDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAirport([FromBody] CreateAirportRequest request)
        {
            _logger.LogInformation("GeolocationController --> CreateAirport --> Start");
            try
            {
                var airport = _mapper.Map<CreateAirportRequest, Airport>(request);
                _context.Airports.Add(airport);
                await _context.SaveChangesAsync();

                var language = HttpContext.Items["Language"] as string;

                var airportDto = _mapper.Map<Airport, AirportDTO>(airport, opt =>
                    opt.Items["Language"] = language);
                _logger.LogInformation("GeolocationController --> CreateAirport --> End at {EndTime}", DateTime.UtcNow);
                return CreatedAtAction(nameof(GetAirportById),
                    new { id = airport.Id },
                    new ApiResponse<AirportDTO>(airportDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating airport");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPut("airports/{id}")]
        [ProducesResponseType(typeof(AirportDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAirport(Guid id, [FromBody] UpdateAirportRequest request)
        {
            _logger.LogInformation("GeolocationController --> UpdateAirport --> Start: {Id}", id);
            try
            {
                if (id != request.Id)
                {
                    return BadRequest(new ApiResponse<string>("El ID de la URL no coincide con el ID del request"));
                }

                var airport = await _context.Airports.FindAsync(id);
                if (airport == null)
                {
                    return NotFound(new ApiResponse<string>("Airport not found"));
                }

                airport.Latitude = request.Latitude;
                airport.Longitude = request.Longitude;
                airport.AveragePrice = request.AveragePrice;
                airport.Name = request.Name;
                airport.Description = request.Description;
                airport.ImageUrl = request.ImageUrl;
                airport.IconUrl = request.IconUrl;
                airport.BackgroundColor = request.BackgroundColor;
                airport.Status = request.Status;
                airport.ThumbnailUrl = request.ThumbnailUrl;
                airport.Category = request.Category;
                airport.CityId = request.CityId;
                airport.ZoneId = request.ZoneId;
                airport.CountryId = request.CountryId;
                airport.IataCode = request.IataCode;
                airport.IcaoCode = request.IcaoCode;
                airport.TimeZone = request.TimeZone;
                airport.GMTOffset = request.GmtOffset ?? 0;
                airport.DSTOffset = request.DstOffset ?? 0;
                airport.UpdatedBy = request.UpdatedBy ?? "System";
                airport.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var language = HttpContext.Items["Language"] as string;
                var airportDto = _mapper.Map<Airport, AirportDTO>(airport, opt =>
                    opt.Items["Language"] = language);

                _logger.LogInformation("GeolocationController --> UpdateAirport --> End: {Id}", id);
                return Ok(new ApiResponse<AirportDTO>(airportDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating airport: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPatch("airports/{id}")]
        [ProducesResponseType(typeof(AirportDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchAirport(Guid id, [FromBody] PatchAirportRequest request)
        {
            _logger.LogInformation("GeolocationController --> PatchAirport --> Start: {Id}", id);
            try
            {
                if (id != request.Id)
                {
                    return BadRequest(new ApiResponse<string>("El ID de la URL no coincide con el ID del request"));
                }

                var airport = await _context.Airports.FindAsync(id);
                if (airport == null)
                {
                    return NotFound(new ApiResponse<string>("Airport not found"));
                }

                // Actualizar solo los campos proporcionados
                if (request.Latitude.HasValue) airport.Latitude = request.Latitude.Value;
                if (request.Longitude.HasValue) airport.Longitude = request.Longitude.Value;
                if (request.AveragePrice.HasValue) airport.AveragePrice = request.AveragePrice.Value;
                if (request.Name != null) airport.Name = request.Name;
                if (request.Description != null) airport.Description = request.Description;
                if (request.ImageUrl != null) airport.ImageUrl = request.ImageUrl;
                if (request.IconUrl != null) airport.IconUrl = request.IconUrl;
                if (request.BackgroundColor != null) airport.BackgroundColor = request.BackgroundColor;
                if (request.Status != null) airport.Status = request.Status;
                if (request.ThumbnailUrl != null) airport.ThumbnailUrl = request.ThumbnailUrl;
                if (request.Category != null) airport.Category = request.Category;
                if (request.CityId.HasValue) airport.CityId = request.CityId.Value;
                if (request.ZoneId.HasValue) airport.ZoneId = request.ZoneId.Value;
                if (request.CountryId.HasValue) airport.CountryId = request.CountryId.Value;
                if (request.IataCode != null) airport.IataCode = request.IataCode;
                if (request.IcaoCode != null) airport.IcaoCode = request.IcaoCode;
                if (request.TimeZone != null) airport.TimeZone = request.TimeZone;
                if (request.GmtOffset.HasValue) airport.GMTOffset = request.GmtOffset.Value;
                if (request.DstOffset.HasValue) airport.DSTOffset = request.DstOffset.Value;

                airport.UpdatedBy = request.CreatedBy ?? "System";
                airport.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var language = HttpContext.Items["Language"] as string;
                var airportDto = _mapper.Map<Airport, AirportDTO>(airport, opt =>
                    opt.Items["Language"] = language);
                _logger.LogInformation("GeolocationController --> PatchAirport --> End: {Id}", id);
                return Ok(new ApiResponse<AirportDTO>(airportDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error patching airport: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpDelete("airports/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAirport(Guid id)
        {
            _logger.LogInformation("GeolocationController --> DeleteAirport --> Start: {Id}", id);
            try
            {
                var airport = await _context.Airports.FindAsync(id);
                if (airport == null)
                {
                    return NotFound(new ApiResponse<string>("Airport not found"));
                }

                _context.Airports.Remove(airport);
                await _context.SaveChangesAsync();

                _logger.LogInformation("GeolocationController --> DeleteAirport --> End: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting airport: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpGet("airports/city/{cityId}")]
        [ProducesResponseType(typeof(List<AirportDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAirportsByCityId(Guid cityId)
        {
            _logger.LogInformation("GeolocationController --> GetAirportsByCityId --> Start: {CityId}", cityId);
            try
            {
                var airports = await _context.Airports.Where(a => a.CityId == cityId).ToListAsync();
                
                var language = HttpContext.Items["Language"] as string;
                var airportDtos = _mapper.Map<List<Airport>, List<AirportDTO>>(airports, opt => 
                    opt.Items["Language"] = language);
                _logger.LogInformation("GeolocationController --> GetAirportsByCityId --> End: {CityId}", cityId);
                return Ok(new ApiResponse<List<AirportDTO>>(airportDtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving airports by city id: {CityId}", cityId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpGet("airports/country/{countryId}")]
        [ProducesResponseType(typeof(List<AirportDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAirportsByCountryId(Guid countryId)
        {
            _logger.LogInformation("GeolocationController --> GetAirportsByCountryId --> Start: {CountryId}", countryId);
            try
            {
                var airports = await _context.Airports.Where(a => a.CountryId == countryId).ToListAsync();
                
                var language = HttpContext.Items["Language"] as string;
                var airportDtos = _mapper.Map<List<Airport>, List<AirportDTO>>(airports, opt => 
                    opt.Items["Language"] = language);
                _logger.LogInformation("GeolocationController --> GetAirportsByCountryId --> End: {CountryId}", countryId);
                return Ok(new ApiResponse<List<AirportDTO>>(airportDtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving airports by country id: {CountryId}", countryId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpGet("airports/zone/{zoneId}")]
        [ProducesResponseType(typeof(List<AirportDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAirportsByZoneId(Guid zoneId)
        {
            _logger.LogInformation("GeolocationController --> GetAirportsByZoneId --> Start: {ZoneId}", zoneId);
            try
            {
                var airports = await _context.Airports.Where(a => a.ZoneId == zoneId).ToListAsync();
                
                var language = HttpContext.Items["Language"] as string;
                var airportDtos = _mapper.Map<List<Airport>, List<AirportDTO>>(airports, opt => 
                    opt.Items["Language"] = language);
                _logger.LogInformation("GeolocationController --> GetAirportsByZoneId --> End: {ZoneId}", zoneId);
                return Ok(new ApiResponse<List<AirportDTO>>(airportDtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving airports by zone id: {ZoneId}", zoneId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        #endregion
    
        #region Region Endpoints

        [HttpGet("regions")]
        [ProducesResponseType(typeof(PaginatedApiResponse<RegionDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRegions([FromQuery] QueryRegionRequest request, [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation($"GeolocationController --> GetRegions --> Start: {DateTime.UtcNow}");

            var language = HttpContext.Items["Language"] as string;

            _logger.LogInformation(
                "Request Parameters: SearchTerm={SearchTerm}, Status={Status}, Category={Category}, CountryId={CountryId}, ZoneId={ZoneId}, Language={Language}, Page={Page}, PageSize={PageSize}",
                request.SearchTerm, request.Status, request.Category, request.CountryId, request.ZoneId, language, page, pageSize);

            try
            {
                var query = _context.Regions.AsNoTracking();

                // Aplicar filtros
                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    if (!string.IsNullOrWhiteSpace(language))
                    {
                        query = query.Where(r =>
                            r.Name.GetText(language) != null &&
                            r.Name.GetText(language).Contains(request.SearchTerm) ||
                            r.Description.GetText(language) != null &&
                            r.Description.GetText(language).Contains(request.SearchTerm));
                    }
                    else
                    {
                        query = query.Where(r => r.Name.GetText(Languages.Default) != null &&
                                                 r.Name.GetText(Languages.Default).Contains(request.SearchTerm) ||
                                                 r.Description.GetText(Languages.Default) != null &&
                                                 r.Description.GetText(Languages.Default).Contains(request.SearchTerm));
                    }
                }

                if (!string.IsNullOrWhiteSpace(request.Status))
                {
                    query = query.Where(r => r.Status == request.Status);
                }

                if (!string.IsNullOrWhiteSpace(request.Category))
                {
                    query = query.Where(r => r.Category == request.Category);
                }

                if (request.CountryId.HasValue)
                {
                    query = query.Where(r => r.CountryId == request.CountryId.Value);
                }

                if (request.ZoneId.HasValue)
                {
                    query = query.Where(r => r.ZoneId == request.ZoneId.Value);
                }

                if (request.MinAveragePrice.HasValue)
                {
                    query = query.Where(r => r.AveragePrice >= request.MinAveragePrice.Value);
                }

                if (request.MaxAveragePrice.HasValue)
                {
                    query = query.Where(r => r.AveragePrice <= request.MaxAveragePrice.Value);
                }

                if (request.MinLatitude.HasValue)
                {
                    query = query.Where(r => r.Latitude >= request.MinLatitude.Value);
                }

                if (request.MaxLatitude.HasValue)
                {
                    query = query.Where(r => r.Latitude <= request.MaxLatitude.Value);
                }

                if (request.MinLongitude.HasValue)
                {
                    query = query.Where(r => r.Longitude >= request.MinLongitude.Value);
                }

                if (request.MaxLongitude.HasValue)
                {
                    query = query.Where(r => r.Longitude <= request.MaxLongitude.Value);
                }

                var totalCount = await query.CountAsync();
                var regions = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                var regionDtos = _mapper.Map<List<Region>, List<RegionDTO>>(regions, opt =>
                    opt.Items["Language"] = language);

                var pagination = new PaginatedParameters(page, pageSize, totalCount);

                _logger.LogInformation("GeolocationController --> GetRegions --> End at {EndTime}", DateTime.UtcNow);
                return Ok(new PaginatedApiResponse<RegionDTO>(regionDtos, pagination));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving regions");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpGet("regions/{id}")]
        [ProducesResponseType(typeof(RegionDetailDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRegionById(Guid id)
        {
            _logger.LogInformation("GeolocationController --> GetRegionById --> Start: {Id}", id);
            try
            {
                var region = await _context.Regions.FindAsync(id);
                if (region == null)
                {
                    return NotFound(new ApiResponse<string>("Region not found"));
                }

                var regionDto = _mapper.Map<Region, RegionDetailDTO>(region);

                _logger.LogInformation("GeolocationController --> GetRegionById --> End: {Id}", id);
                return Ok(new ApiResponse<RegionDetailDTO>(regionDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving region by id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPost("regions")]
        [ProducesResponseType(typeof(RegionDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRegion([FromBody] CreateRegionRequest request)
        {
            _logger.LogInformation("GeolocationController --> CreateRegion --> Start");
            try
            {
                var region = _mapper.Map<CreateRegionRequest, Region>(request);
                _context.Regions.Add(region);
                await _context.SaveChangesAsync();

                var language = HttpContext.Items["Language"] as string;

                var regionDto = _mapper.Map<Region, RegionDTO>(region, opt =>
                    opt.Items["Language"] = language);
                _logger.LogInformation("GeolocationController --> CreateRegion --> End at {EndTime}", DateTime.UtcNow);
                return CreatedAtAction(nameof(GetRegionById),
                    new { id = region.Id },
                    new ApiResponse<RegionDTO>(regionDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating region");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPut("regions/{id}")]
        [ProducesResponseType(typeof(RegionDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRegion(Guid id, [FromBody] UpdateRegionRequest request)
        {
            _logger.LogInformation("GeolocationController --> UpdateRegion --> Start: {Id}", id);
            try
            {
                if (id != request.Id)
                {
                    return BadRequest(new ApiResponse<string>("El ID de la URL no coincide con el ID del request"));
                }

                var region = await _context.Regions.FindAsync(id);
                if (region == null)
                {
                    return NotFound(new ApiResponse<string>("Region not found"));
                }

                region.Latitude = request.Latitude;
                region.Longitude = request.Longitude;
                region.AveragePrice = request.AveragePrice;
                region.Name = request.Name;
                region.Description = request.Description;
                region.ImageUrl = request.ImageUrl;
                region.IconUrl = request.IconUrl;
                region.BackgroundColor = request.BackgroundColor;
                region.Status = request.Status;
                region.ThumbnailUrl = request.ThumbnailUrl;
                region.Category = request.Category;
                region.CountryId = request.CountryId;
                region.ZoneId = request.ZoneId;
                region.UpdatedBy = request.UpdatedBy ?? "System";
                region.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var language = HttpContext.Items["Language"] as string;
                var regionDto = _mapper.Map<Region, RegionDTO>(region, opt =>
                    opt.Items["Language"] = language);

                _logger.LogInformation("GeolocationController --> UpdateRegion --> End: {Id}", id);
                return Ok(new ApiResponse<RegionDTO>(regionDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating region: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPatch("regions/{id}")]
        [ProducesResponseType(typeof(RegionDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchRegion(Guid id, [FromBody] PatchRegionRequest request)
        {
            _logger.LogInformation("GeolocationController --> PatchRegion --> Start: {Id}", id);
            try
            {
                if (id != request.Id)
                {
                    return BadRequest(new ApiResponse<string>("El ID de la URL no coincide con el ID del request"));
                }

                var region = await _context.Regions.FindAsync(id);
                if (region == null)
                {
                    return NotFound(new ApiResponse<string>("Region not found"));
                }

                // Actualizar solo los campos proporcionados
                if (request.Latitude.HasValue) region.Latitude = request.Latitude.Value;
                if (request.Longitude.HasValue) region.Longitude = request.Longitude.Value;
                if (request.AveragePrice.HasValue) region.AveragePrice = request.AveragePrice.Value;
                if (request.Name != null) region.Name = request.Name;
                if (request.Description != null) region.Description = request.Description;
                if (request.ImageUrl != null) region.ImageUrl = request.ImageUrl;
                if (request.IconUrl != null) region.IconUrl = request.IconUrl;
                if (request.BackgroundColor != null) region.BackgroundColor = request.BackgroundColor;
                if (request.Status != null) region.Status = request.Status;
                if (request.ThumbnailUrl != null) region.ThumbnailUrl = request.ThumbnailUrl;
                if (request.Category != null) region.Category = request.Category;
                if (request.CountryId.HasValue) region.CountryId = request.CountryId.Value;
                if (request.ZoneId.HasValue) region.ZoneId = request.ZoneId.Value;

                region.UpdatedBy = request.CreatedBy ?? "System";
                region.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var language = HttpContext.Items["Language"] as string;
                var regionDto = _mapper.Map<Region, RegionDTO>(region, opt =>
                    opt.Items["Language"] = language);
                _logger.LogInformation("GeolocationController --> PatchRegion --> End: {Id}", id);
                return Ok(new ApiResponse<RegionDTO>(regionDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error patching region: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpDelete("regions/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRegion(Guid id)
        {
            _logger.LogInformation("GeolocationController --> DeleteRegion --> Start: {Id}", id);
            try
            {
                var region = await _context.Regions.FindAsync(id);
                if (region == null)
                {
                    return NotFound(new ApiResponse<string>("Region not found"));
                }

                _context.Regions.Remove(region);
                await _context.SaveChangesAsync();

                _logger.LogInformation("GeolocationController --> DeleteRegion --> End: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting region: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpGet("regions/country/{countryId}")]
        [ProducesResponseType(typeof(List<RegionDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRegionsByCountryId(Guid countryId)
        {
            _logger.LogInformation("GeolocationController --> GetRegionsByCountryId --> Start: {CountryId}", countryId);
            try
            {
                var regions = await _context.Regions.Where(r => r.CountryId == countryId).ToListAsync();
                
                var language = HttpContext.Items["Language"] as string;
                var regionDtos = _mapper.Map<List<Region>, List<RegionDTO>>(regions, opt => 
                    opt.Items["Language"] = language);
                _logger.LogInformation("GeolocationController --> GetRegionsByCountryId --> End: {CountryId}", countryId);
                return Ok(new ApiResponse<List<RegionDTO>>(regionDtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving regions by country id: {CountryId}", countryId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpGet("regions/zone/{zoneId}")]
        [ProducesResponseType(typeof(List<RegionDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRegionsByZoneId(Guid zoneId)
        {
            _logger.LogInformation("GeolocationController --> GetRegionsByZoneId --> Start: {ZoneId}", zoneId);
            try
            {
                var regions = await _context.Regions.Where(r => r.ZoneId == zoneId).ToListAsync();
                
                var language = HttpContext.Items["Language"] as string;
                var regionDtos = _mapper.Map<List<Region>, List<RegionDTO>>(regions, opt => 
                    opt.Items["Language"] = language);
                _logger.LogInformation("GeolocationController --> GetRegionsByZoneId --> End: {ZoneId}", zoneId);
                return Ok(new ApiResponse<List<RegionDTO>>(regionDtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving regions by zone id: {ZoneId}", zoneId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        #endregion

        #region Country Endpoints

        [HttpGet("countries")]
        [ProducesResponseType(typeof(PaginatedApiResponse<CountryDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries([FromQuery] QueryCountryRequest request, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation($"GeolocationController --> GetCountries --> Start: {DateTime.UtcNow}");

            // Extraer idioma del header Accept-Language
            var acceptLanguage = Request.Headers["Accept-Language"].FirstOrDefault();
            var language = LanguageHelper.ExtractLanguageFromAcceptLanguage(acceptLanguage);

            // Log de parámetros de la request
            _logger.LogInformation("Request Parameters: SearchTerm={SearchTerm}, Status={Status}, Category={Category}, Currency={Currency}, LanguageCode={LanguageCode}, Language={Language}, Page={Page}, PageSize={PageSize}",
                request.SearchTerm, request.Status, request.Category, request.Currency, request.LanguageCode, language, page, pageSize);

            try
            {
                var query = _context.Countries.AsNoTracking();

                // Aplicar filtros
                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    if (!string.IsNullOrWhiteSpace(language))
                    {
                        // Filtrar por idioma específico y buscar en ese idioma
                        var searchLanguage = Languages.GetValidLanguageOrDefault(language);
                        query = query.Where(c => c.Name.Any(n => n.Language == searchLanguage && n.Content.Contains(request.SearchTerm)) ||
                                                c.Description.Any(d => d.Language == searchLanguage && d.Content.Contains(request.SearchTerm)));
                    }
                    else
                    {
                        // Buscar en todos los idiomas
                        query = query.Where(c => c.Name.Any(n => n.Content.Contains(request.SearchTerm)) ||
                                                c.Description.Any(d => d.Content.Contains(request.SearchTerm)));
                    }
                }

                if (!string.IsNullOrWhiteSpace(request.Status))
                {
                    query = query.Where(c => c.Status == request.Status);
                }

                if (!string.IsNullOrWhiteSpace(request.Category))
                {
                    query = query.Where(c => c.Category == request.Category);
                }

                if (!string.IsNullOrWhiteSpace(request.Currency))
                {
                    query = query.Where(c => c.Currency == request.Currency);
                }

                if (!string.IsNullOrWhiteSpace(request.LanguageCode))
                {
                    query = query.Where(c => c.LanguageCode == request.LanguageCode);
                }

                if (!string.IsNullOrWhiteSpace(request.TimeZone))
                {
                    query = query.Where(c => c.TimeZone == request.TimeZone);
                }

                if (request.MinAveragePrice.HasValue)
                {
                    query = query.Where(c => c.AveragePrice >= request.MinAveragePrice.Value);
                }

                if (request.MaxAveragePrice.HasValue)
                {
                    query = query.Where(c => c.AveragePrice <= request.MaxAveragePrice.Value);
                }

                if (request.MinLatitude.HasValue)
                {
                    query = query.Where(c => c.Latitude >= request.MinLatitude.Value);
                }

                if (request.MaxLatitude.HasValue)
                {
                    query = query.Where(c => c.Latitude <= request.MaxLatitude.Value);
                }

                if (request.MinLongitude.HasValue)
                {
                    query = query.Where(c => c.Longitude >= request.MinLongitude.Value);
                }

                if (request.MaxLongitude.HasValue)
                {
                    query = query.Where(c => c.Longitude <= request.MaxLongitude.Value);
                }

                var totalCount = await query.CountAsync();
                var countries = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                var countryDtos = MultiLanguageMappingHelper.MapToCountryDtos(countries, language);

                var pagination = new PaginatedParameters(page, pageSize, totalCount);

                _logger.LogInformation("GeolocationController --> GetCountries --> End at {EndTime}", DateTime.UtcNow);
                return Ok(new PaginatedApiResponse<CountryDTO>(countryDtos, pagination));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving countries");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpGet("countries/{id}")]
        [ProducesResponseType(typeof(CountryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountryById(Guid id)
        {
            _logger.LogInformation("GeolocationController --> GetCountryById --> Start: {Id}", id);
            try
            {
                var country = await _context.Countries.FindAsync(id);
                if (country == null)
                {
                    return NotFound(new ApiResponse<string>("Country not found"));
                }

                var countryDto = _mapper.Map<CountryDetailDTO>(country);
                _logger.LogInformation("GeolocationController --> GetCountryById --> End: {Id}", id);
                return Ok(new ApiResponse<CountryDetailDTO>(countryDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving country by id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPost("countries")]
        [ProducesResponseType(typeof(CountryDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryRequest request)
        {
            _logger.LogInformation("GeolocationController --> CreateCountry --> Start");
            try
            {
                var country = new Country
                {
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    AveragePrice = request.AveragePrice,
                    Name = request.Name,
                    Description = request.Description ?? new(),
                    ImageUrl = request.ImageUrl,
                    IconUrl = request.IconUrl,
                    BackgroundColor = request.BackgroundColor,
                    Status = request.Status,
                    ThumbnailUrl = request.ThumbnailUrl,
                    Category = request.Category,
                    Currency = request.Currency,
                    CurrencySymbol = request.CurrencySymbol,
                    Language = request.Language,
                    LanguageCode = request.LanguageCode,
                    TimeZone = request.TimeZone,
                    CreatedBy = request.CreatedBy ?? "System",
                    CreatedAt = DateTime.UtcNow
                };

                _context.Countries.Add(country);
                await _context.SaveChangesAsync();

                var countryDto = _mapper.Map<CountryDTO>(country);
                _logger.LogInformation("GeolocationController --> CreateCountry --> End at {EndTime}", DateTime.UtcNow);
                return CreatedAtAction(nameof(GetCountryById),
                    new { id = country.Id },
                    new ApiResponse<CountryDTO>(countryDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating country");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPut("countries/{id}")]
        [ProducesResponseType(typeof(CountryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountry(Guid id, [FromBody] UpdateCountryRequest request)
        {
            _logger.LogInformation("GeolocationController --> UpdateCountry --> Start: {Id}", id);
            try
            {
                if (id != request.Id)
                {
                    return BadRequest(new ApiResponse<string>("El ID de la URL no coincide con el ID del request"));
                }

                var country = await _context.Countries.FindAsync(id);
                if (country == null)
                {
                    return NotFound(new ApiResponse<string>("Country not found"));
                }

                country.Latitude = request.Latitude;
                country.Longitude = request.Longitude;
                country.AveragePrice = request.AveragePrice;
                country.Name = request.Name;
                country.Description = request.Description ?? new();
                country.ImageUrl = request.ImageUrl;
                country.IconUrl = request.IconUrl;
                country.BackgroundColor = request.BackgroundColor;
                country.Status = request.Status;
                country.ThumbnailUrl = request.ThumbnailUrl;
                country.Category = request.Category;
                country.Currency = request.Currency;
                country.CurrencySymbol = request.CurrencySymbol;
                country.Language = request.Language;
                country.LanguageCode = request.LanguageCode;
                country.TimeZone = request.TimeZone;
                country.UpdatedBy = request.CreatedBy ?? "System";
                country.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var countryDto = _mapper.Map<CountryDTO>(country);
                _logger.LogInformation("GeolocationController --> UpdateCountry --> End: {Id}", id);
                return Ok(new ApiResponse<CountryDTO>(countryDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating country: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPatch("countries/{id}")]
        [ProducesResponseType(typeof(CountryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchCountry(Guid id, [FromBody] PatchCountryRequest request)
        {
            _logger.LogInformation("GeolocationController --> PatchCountry --> Start: {Id}", id);
            try
            {
                if (id != request.Id)
                {
                    return BadRequest(new ApiResponse<string>("El ID de la URL no coincide con el ID del request"));
                }

                var country = await _context.Countries.FindAsync(id);
                if (country == null)
                {
                    return NotFound(new ApiResponse<string>("Country not found"));
                }

                // Actualizar solo los campos proporcionados
                if (request.Latitude.HasValue) country.Latitude = request.Latitude.Value;
                if (request.Longitude.HasValue) country.Longitude = request.Longitude.Value;
                if (request.AveragePrice.HasValue) country.AveragePrice = request.AveragePrice.Value;
                if (request.Name != null) country.Name = request.Name;
                if (request.Description != null) country.Description = request.Description;
                if (request.ImageUrl != null) country.ImageUrl = request.ImageUrl;
                if (request.IconUrl != null) country.IconUrl = request.IconUrl;
                if (request.BackgroundColor != null) country.BackgroundColor = request.BackgroundColor;
                if (request.Status != null) country.Status = request.Status;
                if (request.ThumbnailUrl != null) country.ThumbnailUrl = request.ThumbnailUrl;
                if (request.Category != null) country.Category = request.Category;
                if (request.Currency != null) country.Currency = request.Currency;
                if (request.CurrencySymbol != null) country.CurrencySymbol = request.CurrencySymbol;
                if (request.Language != null) country.Language = request.Language;
                if (request.LanguageCode != null) country.LanguageCode = request.LanguageCode;
                if (request.TimeZone != null) country.TimeZone = request.TimeZone;

                country.UpdatedBy = request.CreatedBy ?? "System";
                country.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var countryDto = _mapper.Map<CountryDTO>(country);
                _logger.LogInformation("GeolocationController --> PatchCountry --> End: {Id}", id);
                return Ok(new ApiResponse<CountryDTO>(countryDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error patching country: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpDelete("countries/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCountry(Guid id)
        {
            _logger.LogInformation("GeolocationController --> DeleteCountry --> Start: {Id}", id);
            try
            {
                var country = await _context.Countries.FindAsync(id);
                if (country == null)
                {
                    return NotFound(new ApiResponse<string>("Country not found"));
                }

                _context.Countries.Remove(country);
                await _context.SaveChangesAsync();

                _logger.LogInformation("GeolocationController --> DeleteCountry --> End: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting country: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        #endregion
    }
}
