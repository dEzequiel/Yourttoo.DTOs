using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Yourttoo.Api.DataAccess;
using Yourttoo.DTOs.DTOs.AdditionalText;
using Yourttoo.DTOs.Requests.AdditionalText;
using Yourttoo.DTOs.Shared.API;
using Yourttoo.DTOs.Shared.Pagination;
using Yourttoo.DTOs.Models;
using Yourttoo.DTOs.Common;
using Yourttoo.DTOs.Common.Constants;
using Yourttoo.DTOs.Common.Helpers;


namespace Yourttoo.Api.Controllers
{
    [Route("/store/additionaltext")]
    [ApiController]
    public class AdditionalTextController : ControllerBase
    {
        private readonly ILogger<AdditionalTextController> _logger;
        private readonly IMapper _mapper;
        private readonly DatabaseContext _context;
        public AdditionalTextController(ILogger<AdditionalTextController> logger, IMapper mapper, DatabaseContext context)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedApiResponse<AdditionalTextDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAdditionalTexts([FromQuery] QueryAdditionalTextRequest request, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation($"AdditionalTextController --> GetAdditionalTexts --> Start: {DateTime.UtcNow}");

            // Extraer idioma del header Accept-Language
            var acceptLanguage = Request.Headers["Accept-Language"].FirstOrDefault();
            var language = LanguageHelper.ExtractLanguageFromAcceptLanguage(acceptLanguage);

            // Log de parámetros de la request
            _logger.LogInformation("Request Parameters: SearchTerm={SearchTerm}, Status={Status}, ExtractedLanguage={Language}, Page={Page}, PageSize={PageSize}",
                request.SearchTerm, request.Status, language, page, pageSize);

            try
            {
                var query = _context.AdditionalText.AsNoTracking();

                // Aplicar filtros
                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    if (!string.IsNullOrWhiteSpace(language))
                    {
                        // Filtrar por idioma específico y buscar en ese idioma (SQLite compatible)
                        var searchLanguage = Languages.GetValidLanguageOrDefault(language);
                        query = query.Where(tc => tc.Title.Any(n => n.Language == searchLanguage && n.Content.Contains(request.SearchTerm)) ||
                                                tc.Content.Any(d => d.Language == searchLanguage && d.Content.Contains(request.SearchTerm)));
                    }
                    else
                    {
                        // Buscar en todos los idiomas
                        query = query.Where(tc => tc.Title.Any(n => n.Content.Contains(request.SearchTerm)) ||
                                                tc.Content.Any(d => d.Content.Contains(request.SearchTerm)));
                    }
                }

                if (!string.IsNullOrWhiteSpace(request.Status))
                {
                    query = query.Where(at => at.Status == request.Status);
                }

                var totalCount = await query.CountAsync();
                var additionalTexts = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                var additionalTextDtos = MultiLanguageMappingHelper.MapToAdditionalTextDtos(additionalTexts, language);

                var pagination = new PaginatedParameters(page, pageSize, totalCount);

                _logger.LogInformation("AdditionalTextController --> GetAdditionalTexts --> End at {EndTime}", DateTime.UtcNow);
                return Ok(new PaginatedApiResponse<AdditionalTextDTO>(additionalTextDtos, pagination));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving additional texts");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AdditionalTextDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAdditionalTextById(Guid id)
        {
            _logger.LogInformation("AdditionalTextController --> GetAdditionalTextById --> Start: {Id}", id);
            try
            {
                var additionalText = await _context.AdditionalText.FindAsync(id);
                if (additionalText == null)
                {
                    return NotFound(new ApiResponse<string>("Additional text not found"));
                }

                var additionalTextDto = _mapper.Map<AdditionalTextDetailDTO>(additionalText);
                _logger.LogInformation("AdditionalTextController --> GetAdditionalTextById --> End: {Id}", id);
                return Ok(new ApiResponse<AdditionalTextDetailDTO>(additionalTextDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving additional text by id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(AdditionalTextDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAdditionalText([FromBody] CreateAdditionalTextRequest request)
        {
            _logger.LogInformation("AdditionalTextController --> CreateAdditionalText --> Start");
            try
            {
                var additionalText = new AdditionalText
                {
                    Title = request.Title,
                    Content = request.Content,
                    Status = request.Status,
                    CreatedBy = "System", // TODO: Obtener del contexto de usuario
                    CreatedAt = DateTime.UtcNow
                };

                _context.AdditionalText.Add(additionalText);
                await _context.SaveChangesAsync();

                var additionalTextDto = _mapper.Map<AdditionalTextDTO>(additionalText);
                _logger.LogInformation("AdditionalTextController --> CreateAdditionalText --> End at {EndTime}", DateTime.UtcNow);
                return CreatedAtAction(nameof(GetAdditionalTextById),
                    new { id = additionalText.Id },
                    new ApiResponse<AdditionalTextDTO>(additionalTextDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating additional text");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAdditionalText(Guid id)
        {
            _logger.LogInformation("AdditionalTextController --> DeleteAdditionalText --> Start: {Id}", id);
            try
            {
                var additionalText = await _context.AdditionalText.FindAsync(id);
                if (additionalText == null)
                {
                    return NotFound(new ApiResponse<string>("Additional text not found"));
                }

                _context.AdditionalText.Remove(additionalText);
                await _context.SaveChangesAsync();

                _logger.LogInformation("AdditionalTextController --> DeleteAdditionalText --> End: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting additional text: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }

        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(AdditionalTextDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAdditionalText(Guid id, [FromBody] UpdateAdditionalTextRequest request)
        {
            _logger.LogInformation("AdditionalTextController --> UpdateAdditionalText --> Start: {Id}", id);
            try {
                if (id != request.Id) {
                    return BadRequest(new ApiResponse<string>("El ID de la URL no coincide con el ID del request"));
                }

                var additionalText = await _context.AdditionalText.FindAsync(id);
                if (additionalText == null) {
                    return NotFound(new ApiResponse<string>("Additional text not found"));
                }

                additionalText.Title = request.Title;
                additionalText.Content = request.Content;
                additionalText.Status = request.Status;
                additionalText.UpdatedBy = "System"; // TODO: Obtener del contexto de usuario
                additionalText.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var additionalTextDto = _mapper.Map<AdditionalTextDTO>(additionalText);
                _logger.LogInformation("AdditionalTextController --> UpdateAdditionalText --> End: {Id}", id);
                return Ok(new ApiResponse<AdditionalTextDTO>(additionalTextDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating additional text: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }
    }
}