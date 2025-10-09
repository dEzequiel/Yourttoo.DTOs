using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Yourttoo.Api.DataAccess;
using Yourttoo.Api.Attributes;
using Yourttoo.Api.Extensions;
using Yourttoo.DTOs.DTOs.FrequentlyAskedQuestion.Section;
using Yourttoo.DTOs.DTOs.FrequentlyAskedQuestion.Content;
using Yourttoo.DTOs.Requests.FrequentlyAskedQuestion.Section;
using Yourttoo.DTOs.Requests.FrequentlyAskedQuestion.Content;
using Yourttoo.DTOs.Shared.API;
using Yourttoo.DTOs.Shared.Pagination;
using Yourttoo.DTOs.Models.FrequentlyAskedQuestions;
using Yourttoo.DTOs.Common;
using Yourttoo.DTOs.Common.Constants;
using Yourttoo.DTOs.Common.Helpers;

namespace Yourttoo.Api.Controllers
{
    [Route("/store/faq")]
    [ApiController]
    [LanguageFromHeader]
    public class FAQController : ControllerBase
    {
        private readonly ILogger<FAQController> _logger;
        private readonly IMapper _mapper;
        private readonly DatabaseContext _context;

        public FAQController(ILogger<FAQController> logger, IMapper mapper, DatabaseContext context)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        #region FAQ Section Endpoints

        [HttpGet("sections")]
        [ProducesResponseType(typeof(PaginatedApiResponse<FAQSectionDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFAQSections(
            [FromQuery] QueryFAQSectionRequest request,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation("FAQController --> GetFAQSections --> Start at {StartTime}", DateTime.UtcNow);
            
            var language = HttpContext.GetLanguage();
            _logger.LogInformation("GetFAQSections: SearchTerm={SearchTerm}, Category={Category}, Type={Type}, Language={Language}, Page={Page}, PageSize={PageSize}", 
                request.SearchTerm, request.Category, request.Type, language, page, pageSize);
            
            try
            {
                var query = _context.FAQSection
                    .Include(fs => fs.Contents)
                    .AsNoTracking();

                // Aplicar filtros
                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    var searchLanguage = Languages.GetValidLanguageOrDefault(language);
                    query = query.Where(fs => fs.Title.Any(t => t.Language == searchLanguage && t.Content.Contains(request.SearchTerm)));
                }

                if (!string.IsNullOrWhiteSpace(request.Category))
                    query = query.Where(fs => fs.Category == request.Category);

                if (!string.IsNullOrWhiteSpace(request.Type))
                    query = query.Where(fs => fs.Type == request.Type);

                var totalCount = await query.CountAsync();
                var sections = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var sectionDtos = MultiLanguageMappingHelper.MapToFAQSectionDtos(sections, language);
                var pagination = new PaginatedParameters(page, pageSize, totalCount);

                _logger.LogInformation("FAQController --> GetFAQSections --> End at {EndTime}", DateTime.UtcNow);
                return Ok(new PaginatedApiResponse<FAQSectionDTO>(sectionDtos, pagination));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving FAQ sections");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpGet("sections/{id}")]
        [ProducesResponseType(typeof(FAQSectionDetailDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFAQSectionById(Guid id)
        {
            _logger.LogInformation("FAQController --> GetFAQSectionById --> Start: {Id}", id);
            
            try
            {
                var section = await _context.FAQSection
                    .Include(fs => fs.Contents)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(fs => fs.Id == id);

                if (section == null)
                    return NotFound(new ApiResponse<string>("Sección FAQ no encontrada"));

                var sectionDto = _mapper.Map<FAQSectionDetailDTO>(section);
                _logger.LogInformation("FAQController --> GetFAQSectionById --> End: {Id}", id);
                return Ok(new ApiResponse<FAQSectionDetailDTO>(sectionDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving FAQ section by id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPost("sections")]
        [ProducesResponseType(typeof(FAQSectionDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateFAQSection([FromBody] CreateFAQSectionRequest request)
        {
            _logger.LogInformation("FAQController --> CreateFAQSection --> Start");
            
            try
            {
                var section = new FAQSection
                {
                    Id = Guid.NewGuid(),
                    Title = request.Title,
                    Category = request.Category,
                    Type = request.Type,
                    CreatedBy = "system", // TODO: Get from authentication context
                    CreatedAt = DateTime.UtcNow
                };

                _context.FAQSection.Add(section);
                await _context.SaveChangesAsync();

                var sectionDto = _mapper.Map<FAQSectionDTO>(section);
                _logger.LogInformation("FAQController --> CreateFAQSection --> End: {Id}", section.Id);
                return CreatedAtAction(nameof(GetFAQSectionById), new { id = section.Id }, new ApiResponse<FAQSectionDTO>(sectionDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating FAQ section");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPut("sections/{id}")]
        [ProducesResponseType(typeof(FAQSectionDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateFAQSection(Guid id, [FromBody] UpdateFAQSectionRequest request)
        {
            _logger.LogInformation("FAQController --> UpdateFAQSection --> Start: {Id}", id);
            
            try
            {
                var section = await _context.FAQSection.FindAsync(id);
                if (section == null)
                {
                    return NotFound(new ApiResponse<string>("Sección FAQ no encontrada"));
                }

                section.Title = request.Title;
                section.Category = request.Category;
                section.Type = request.Type;
                section.UpdatedBy = "system"; // TODO: Get from authentication context
                section.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var sectionDto = _mapper.Map<FAQSectionDTO>(section);
                _logger.LogInformation("FAQController --> UpdateFAQSection --> End: {Id}", id);
                return Ok(new ApiResponse<FAQSectionDTO>(sectionDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating FAQ section: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpDelete("sections/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteFAQSection(Guid id)
        {
            _logger.LogInformation("FAQController --> DeleteFAQSection --> Start: {Id}", id);
            
            try
            {
                var section = await _context.FAQSection
                    .Include(fs => fs.Contents)
                    .FirstOrDefaultAsync(fs => fs.Id == id);

                if (section == null)
                {
                    return NotFound(new ApiResponse<string>("Sección FAQ no encontrada"));
                }

                // Verificar si tiene contenidos asociados
                if (section.Contents.Any())
                {
                    return BadRequest(new ApiResponse<string>("No se puede eliminar la sección porque tiene contenidos asociados"));
                }

                _context.FAQSection.Remove(section);
                await _context.SaveChangesAsync();

                _logger.LogInformation("FAQController --> DeleteFAQSection --> End: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting FAQ section: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPost("sections/bulk")]
        [ProducesResponseType(typeof(List<FAQSectionDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BulkCreateFAQSections([FromBody] BulkCreateFAQSectionRequest request)
        {
            _logger.LogInformation("FAQController --> BulkCreateFAQSections --> Start");
            
            try
            {
                var sections = new List<FAQSection>();
                var createdBy = "system"; // TODO: Get from authentication context
                var createdAt = DateTime.UtcNow;

                foreach (var sectionRequest in request.Sections)
                {
                    var section = new FAQSection
                    {
                        Id = Guid.NewGuid(),
                        Title = sectionRequest.Title,
                        Category = sectionRequest.Category,
                        Type = sectionRequest.Type,
                        CreatedBy = createdBy,
                        CreatedAt = createdAt
                    };
                    sections.Add(section);
                }

                _context.FAQSection.AddRange(sections);
                await _context.SaveChangesAsync();

                var sectionDtos = _mapper.Map<List<FAQSectionDTO>>(sections);
                _logger.LogInformation("FAQController --> BulkCreateFAQSections --> End: {Count} sections created", sections.Count);
                return CreatedAtAction(nameof(GetFAQSections), new ApiResponse<List<FAQSectionDTO>>(sectionDtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error bulk creating FAQ sections");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        #endregion

        #region FAQ Content Endpoints

        [HttpGet("contents")]
        [ProducesResponseType(typeof(PaginatedApiResponse<FAQContentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFAQContents(
            [FromQuery] QueryFAQContentRequest request,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation("FAQController --> GetFAQContents --> Start at {StartTime}", DateTime.UtcNow);
            
            var language = HttpContext.GetLanguage();
            _logger.LogInformation("GetFAQContents: SearchTerm={SearchTerm}, Status={Status}, Slug={Slug}, FAQSectionId={FAQSectionId}, Language={Language}, Page={Page}, PageSize={PageSize}", 
                request.SearchTerm, request.Status, request.Slug, request.FAQSectionId, language, page, pageSize);
            
            try
            {
                var query = _context.FAQContent
                    .Include(fc => fc.FAQSection)
                    .AsNoTracking();

                // Aplicar filtros
                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    var searchLanguage = Languages.GetValidLanguageOrDefault(language);
                    query = query.Where(fc => fc.Title.Any(t => t.Language == searchLanguage && t.Content.Contains(request.SearchTerm)) ||
                                            fc.Content.Any(c => c.Language == searchLanguage && c.Content.Contains(request.SearchTerm)));
                }

                if (!string.IsNullOrWhiteSpace(request.Status))
                    query = query.Where(fc => fc.Status == request.Status);

                if (!string.IsNullOrWhiteSpace(request.Slug))
                    query = query.Where(fc => fc.Slug == request.Slug);

                if (request.FAQSectionId.HasValue)
                    query = query.Where(fc => fc.FAQSectionId == request.FAQSectionId.Value);

                var totalCount = await query.CountAsync();
                var contents = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var contentDtos = MultiLanguageMappingHelper.MapToFAQContentDtos(contents, language);
                var pagination = new PaginatedParameters(page, pageSize, totalCount);

                _logger.LogInformation("FAQController --> GetFAQContents --> End at {EndTime}", DateTime.UtcNow);
                return Ok(new PaginatedApiResponse<FAQContentDTO>(contentDtos, pagination));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving FAQ contents");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpGet("contents/{id}")]
        [ProducesResponseType(typeof(FAQContentDetailDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFAQContentById(Guid id)
        {
            _logger.LogInformation("FAQController --> GetFAQContentById --> Start: {Id}", id);
            
            try
            {
                var content = await _context.FAQContent
                    .Include(fc => fc.FAQSection)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(fc => fc.Id == id);

                if (content == null)
                {
                    return NotFound(new ApiResponse<string>("Contenido FAQ no encontrado"));
                }

                var contentDto = _mapper.Map<FAQContentDetailDTO>(content);
                _logger.LogInformation("FAQController --> GetFAQContentById --> End: {Id}", id);
                return Ok(new ApiResponse<FAQContentDetailDTO>(contentDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving FAQ content by id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPost("contents")]
        [ProducesResponseType(typeof(FAQContentDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateFAQContent([FromBody] CreateFAQContentRequest request)
        {
            _logger.LogInformation("FAQController --> CreateFAQContent --> Start");
            
            try
            {
                // Verificar que la sección existe
                var sectionExists = await _context.FAQSection
                    .AnyAsync(fs => fs.Id == request.FAQSectionId);

                if (!sectionExists)
                {
                    return BadRequest(new ApiResponse<string>("La sección FAQ especificada no existe"));
                }

                var content = new FAQContent
                {
                    Id = Guid.NewGuid(),
                    Title = request.Title,
                    Content = request.Content,
                    Status = request.Status,
                    Slug = request.Slug,
                    FAQSectionId = request.FAQSectionId,
                    CreatedBy = "system", // TODO: Get from authentication context
                    CreatedAt = DateTime.UtcNow
                };

                _context.FAQContent.Add(content);
                await _context.SaveChangesAsync();

                var contentDto = _mapper.Map<FAQContentDTO>(content);
                _logger.LogInformation("FAQController --> CreateFAQContent --> End: {Id}", content.Id);
                return CreatedAtAction(nameof(GetFAQContentById), new { id = content.Id }, new ApiResponse<FAQContentDTO>(contentDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating FAQ content");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPut("contents/{id}")]
        [ProducesResponseType(typeof(FAQContentDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateFAQContent(Guid id, [FromBody] UpdateFAQContentRequest request)
        {
            _logger.LogInformation("FAQController --> UpdateFAQContent --> Start: {Id}", id);
            
            try
            {
                var content = await _context.FAQContent.FindAsync(id);
                if (content == null)
                {
                    return NotFound(new ApiResponse<string>("Contenido FAQ no encontrado"));
                }

                // Verificar que la sección existe
                var sectionExists = await _context.FAQSection
                    .AnyAsync(fs => fs.Id == request.FAQSectionId);

                if (!sectionExists)
                {
                    return BadRequest(new ApiResponse<string>("La sección FAQ especificada no existe"));
                }

                content.Title = request.Title;
                content.Content = request.Content;
                content.Status = request.Status;
                content.Slug = request.Slug;
                content.FAQSectionId = request.FAQSectionId;
                content.UpdatedBy = "system"; // TODO: Get from authentication context
                content.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var contentDto = _mapper.Map<FAQContentDTO>(content);
                _logger.LogInformation("FAQController --> UpdateFAQContent --> End: {Id}", id);
                return Ok(new ApiResponse<FAQContentDTO>(contentDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating FAQ content: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpDelete("contents/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteFAQContent(Guid id)
        {
            _logger.LogInformation("FAQController --> DeleteFAQContent --> Start: {Id}", id);
            
            try
            {
                var content = await _context.FAQContent.FindAsync(id);
                if (content == null)
                {
                    return NotFound(new ApiResponse<string>("Contenido FAQ no encontrado"));
                }

                _context.FAQContent.Remove(content);
                await _context.SaveChangesAsync();

                _logger.LogInformation("FAQController --> DeleteFAQContent --> End: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting FAQ content: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPost("contents/bulk")]
        [ProducesResponseType(typeof(List<FAQContentDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BulkCreateFAQContents([FromBody] BulkCreateFAQContentRequest request)
        {
            _logger.LogInformation("FAQController --> BulkCreateFAQContents --> Start");
            
            try
            {
                var contents = new List<FAQContent>();
                var createdBy = "system"; // TODO: Get from authentication context
                var createdAt = DateTime.UtcNow;

                // Verificar que todas las secciones existen
                var sectionIds = request.Contents.Select(c => c.FAQSectionId).Distinct().ToList();
                var existingSections = await _context.FAQSection
                    .Where(fs => sectionIds.Contains(fs.Id))
                    .Select(fs => fs.Id)
                    .ToListAsync();

                var missingSections = sectionIds.Except(existingSections).ToList();
                if (missingSections.Any())
                {
                    return BadRequest(new ApiResponse<string>($"Las siguientes secciones FAQ no existen: {string.Join(", ", missingSections)}"));
                }

                foreach (var contentRequest in request.Contents)
                {
                    var content = new FAQContent
                    {
                        Id = Guid.NewGuid(),
                        Title = contentRequest.Title,
                        Content = contentRequest.Content,
                        Status = contentRequest.Status,
                        Slug = contentRequest.Slug,
                        FAQSectionId = contentRequest.FAQSectionId,
                        CreatedBy = createdBy,
                        CreatedAt = createdAt
                    };
                    contents.Add(content);
                }

                _context.FAQContent.AddRange(contents);
                await _context.SaveChangesAsync();

                var contentDtos = _mapper.Map<List<FAQContentDTO>>(contents);
                _logger.LogInformation("FAQController --> BulkCreateFAQContents --> End: {Count} contents created", contents.Count);
                return CreatedAtAction(nameof(GetFAQContents), new ApiResponse<List<FAQContentDTO>>(contentDtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error bulk creating FAQ contents");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        #endregion
    }
}
