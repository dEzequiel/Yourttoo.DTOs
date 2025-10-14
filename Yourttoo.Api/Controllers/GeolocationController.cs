using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Yourttoo.Api.DataAccess;
using Yourttoo.DTOs.DTOs.Geolocation.Zone;
using Yourttoo.DTOs.Requests.Geolocation.Zone;
using Yourttoo.DTOs.DTOs.Geolocation.Country;
using Yourttoo.DTOs.Requests.Geolocation.Country;
using Yourttoo.DTOs.Shared.API;
using Yourttoo.DTOs.Shared.Pagination;
using Yourttoo.DTOs.Models.Geolocation;
using Yourttoo.DTOs.Common;
using Yourttoo.DTOs.Common.Constants;
using Yourttoo.DTOs.Common.Helpers;
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
        public async Task<IActionResult> GetZones([FromQuery] QueryZoneRequest request, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation($"GeolocationController --> GetZones --> Start: {DateTime.UtcNow}");

            // Extraer idioma del header Accept-Language
            var acceptLanguage = Request.Headers["Accept-Language"].FirstOrDefault();
            var language = LanguageHelper.ExtractLanguageFromAcceptLanguage(acceptLanguage);

            // Log de parámetros de la request
            _logger.LogInformation("Request Parameters: SearchTerm={SearchTerm}, Status={Status}, Category={Category}, PromotionArea={PromotionArea}, Language={Language}, Page={Page}, PageSize={PageSize}",
                request.SearchTerm, request.Status, request.Category, request.PromotionArea, language, page, pageSize);

            try
            {
                var query = _context.Zones.AsNoTracking();

                // Aplicar filtros
                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    if (!string.IsNullOrWhiteSpace(language))
                    {
                        // Filtrar por idioma específico y buscar en ese idioma
                        var searchLanguage = Languages.GetValidLanguageOrDefault(language);
                        query = query.Where(z => z.Name.Any(n => n.Language == searchLanguage && n.Content.Contains(request.SearchTerm)) ||
                                                z.Description.Any(d => d.Language == searchLanguage && d.Content.Contains(request.SearchTerm)));
                    }
                    else
                    {
                        // Buscar en todos los idiomas
                        query = query.Where(z => z.Name.Any(n => n.Content.Contains(request.SearchTerm)) ||
                                                z.Description.Any(d => d.Content.Contains(request.SearchTerm)));
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

                var zoneDtos = MultiLanguageMappingHelper.MapToZoneDtos(zones, language);

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
        [ProducesResponseType(typeof(ZoneDTO), StatusCodes.Status200OK)]
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

                var zoneDto = _mapper.Map<ZoneDetailDTO>(zone);
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
                var zone = new Zone
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
                    PromotionArea = request.PromotionArea,
                    PromotionAreaPriority = request.PromotionAreaPriority,
                    CreatedBy = request.CreatedBy ?? "System",
                    CreatedAt = DateTime.UtcNow
                };

                _context.Zones.Add(zone);
                await _context.SaveChangesAsync();

                var zoneDto = _mapper.Map<ZoneDTO>(zone);
                _logger.LogInformation("GeolocationController --> CreateZone --> End at {EndTime}", DateTime.UtcNow);
                return CreatedAtAction(nameof(GetZoneById),
                    new { id = zone.Id },
                    new ApiResponse<ZoneDTO>(zoneDto));
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
                zone.Description = request.Description ?? new();
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

                var zoneDto = _mapper.Map<ZoneDTO>(zone);
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
                if (request.PromotionAreaPriority.HasValue) zone.PromotionAreaPriority = request.PromotionAreaPriority.Value;

                zone.UpdatedBy = request.CreatedBy ?? "System";
                zone.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var zoneDto = _mapper.Map<ZoneDTO>(zone);
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
