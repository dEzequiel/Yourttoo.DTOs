using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Yourttoo.Api.DataAccess;
using Yourttoo.DTOs.Shared.API;
using Yourttoo.DTOs.Shared.Pagination;
using Yourttoo.DTOs.Models.Tagging;
using Yourttoo.DTOs.DTOs.Tagging.Tag;
using Yourttoo.DTOs.Requests.Tagging.Tag;
using Yourttoo.DTOs.Common.Constants;
using Yourttoo.Api.Attributes;

namespace Yourttoo.Api.Controllers
{
    [Route("/store/tagging/tags")]
    [ApiController]
    [LanguageFromHeader]
    public class TaggingController : ControllerBase
    {
        private readonly ILogger<TaggingController> _logger;
        private readonly IMapper _mapper;
        private readonly DatabaseContext _context;

        public TaggingController(ILogger<TaggingController> logger, IMapper mapper, DatabaseContext context)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        #region Tag Endpoints

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedApiResponse<TagDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTags([FromQuery] QueryTagRequest request, [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation($"TaggingController --> GetTags --> Start: {DateTime.UtcNow}");

            var language = HttpContext.Items["Language"] as string;

            _logger.LogInformation(
                "Request Parameters: SearchTerm={SearchTerm}, Key={Key}, Value={Value}, Category={Category}, SubCategory={SubCategory}, Type={Type}, Status={Status}, Language={Language}, Page={Page}, PageSize={PageSize}",
                request.SearchTerm, request.Key, request.Value, request.Category, request.SubCategory, 
                request.Type, request.Status, language, page, pageSize);

            try
            {
                var query = _context.Tags.AsNoTracking();

                // Aplicar filtros
                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    if (!string.IsNullOrWhiteSpace(language))
                    {
                        query = query.Where(t =>
                            (t.Name != null && t.Name.GetText(language) != null &&
                            t.Name.GetText(language)!.Contains(request.SearchTerm)) ||
                            (t.Description != null && t.Description.GetText(language) != null &&
                            t.Description.GetText(language)!.Contains(request.SearchTerm)) ||
                            (t.Key != null && t.Key.Contains(request.SearchTerm)) ||
                            (t.Value != null && t.Value.Contains(request.SearchTerm)));
                    }
                    else
                    {
                        query = query.Where(t => 
                            (t.Name != null && t.Name.GetText(Languages.Default) != null &&
                            t.Name.GetText(Languages.Default)!.Contains(request.SearchTerm)) ||
                            (t.Description != null && t.Description.GetText(Languages.Default) != null &&
                            t.Description.GetText(Languages.Default)!.Contains(request.SearchTerm)) ||
                            (t.Key != null && t.Key.Contains(request.SearchTerm)) ||
                            (t.Value != null && t.Value.Contains(request.SearchTerm)));
                    }
                }

                if (!string.IsNullOrWhiteSpace(request.Key))
                {
                    query = query.Where(t => t.Key != null && t.Key.Contains(request.Key));
                }

                if (!string.IsNullOrWhiteSpace(request.Value))
                {
                    query = query.Where(t => t.Value != null && t.Value.Contains(request.Value));
                }

                if (!string.IsNullOrWhiteSpace(request.Category))
                {
                    query = query.Where(t => t.Category == request.Category);
                }

                if (!string.IsNullOrWhiteSpace(request.SubCategory))
                {
                    query = query.Where(t => t.SubCategory == request.SubCategory);
                }

                if (!string.IsNullOrWhiteSpace(request.Type))
                {
                    query = query.Where(t => t.Type == request.Type);
                }

                if (!string.IsNullOrWhiteSpace(request.Status))
                {
                    query = query.Where(t => t.Status == request.Status);
                }

                var totalCount = await query.CountAsync();
                var tags = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                var tagDtos = _mapper.Map<List<Tag>, List<TagDTO>>(tags, opt =>
                    opt.Items["Language"] = language);

                var pagination = new PaginatedParameters(page, pageSize, totalCount);

                _logger.LogInformation("TaggingController --> GetTags --> End at {EndTime}", DateTime.UtcNow);
                return Ok(new PaginatedApiResponse<TagDTO>(tagDtos, pagination));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tags");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TagDetailDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTagById(Guid id)
        {
            _logger.LogInformation("TaggingController --> GetTagById --> Start: {Id}", id);
            try
            {
                var tag = await _context.Tags.FindAsync(id);
                if (tag == null)
                {
                    return NotFound(new ApiResponse<string>("Tag not found"));
                }

                var language = HttpContext.Items["Language"] as string;
                var tagDto = _mapper.Map<Tag, TagDetailDTO>(tag, opt =>
                    opt.Items["Language"] = language);

                _logger.LogInformation("TaggingController --> GetTagById --> End: {Id}", id);
                return Ok(new ApiResponse<TagDetailDTO>(tagDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tag by id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(TagDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTag([FromBody] CreateTagRequest request)
        {
            _logger.LogInformation("TaggingController --> CreateTag --> Start");
            try
            {
                var tag = _mapper.Map<CreateTagRequest, Tag>(request);
                
                // Establecer fechas de auditoría manualmente
                tag.CreatedAt = DateTime.UtcNow;
                tag.CreatedBy = request.CreatedBy ?? "System";
                
                await _context.Tags.AddAsync(tag);
                await _context.SaveChangesAsync();

                var language = HttpContext.Items["Language"] as string;
                var tagDto = _mapper.Map<Tag, TagDTO>(tag, opt =>
                    opt.Items["Language"] = language);

                _logger.LogInformation("TaggingController --> CreateTag --> End at {EndTime}", DateTime.UtcNow);
                return CreatedAtAction(nameof(GetTagById), new { id = tag.Id }, new ApiResponse<TagDTO>(tagDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating tag");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TagDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTag(Guid id, [FromBody] UpdateTagRequest request)
        {
            _logger.LogInformation("TaggingController --> UpdateTag --> Start: {Id}", id);
            try
            {
                if (id != request.Id)
                {
                    return BadRequest(new ApiResponse<string>("El ID de la URL no coincide con el ID del request"));
                }

                var tag = await _context.Tags.FindAsync(id);
                if (tag == null)
                {
                    return NotFound(new ApiResponse<string>("Tag not found"));
                }

                tag.Key = request.Key;
                tag.Value = request.Value;
                tag.Category = request.Category;
                tag.SubCategory = request.SubCategory;
                tag.Type = request.Type;
                tag.Name = request.Name;
                tag.Description = request.Description;
                tag.IconUrl = request.IconUrl;
                tag.ImageUrl = request.ImageUrl;
                tag.Status = request.Status;
                tag.UpdatedBy = request.UpdatedBy ?? "System";
                tag.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var language = HttpContext.Items["Language"] as string;
                var tagDto = _mapper.Map<Tag, TagDTO>(tag, opt =>
                    opt.Items["Language"] = language);

                _logger.LogInformation("TaggingController --> UpdateTag --> End: {Id}", id);
                return Ok(new ApiResponse<TagDTO>(tagDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating tag: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(TagDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchTag(Guid id, [FromBody] PatchTagRequest request)
        {
            _logger.LogInformation("TaggingController --> PatchTag --> Start: {Id}", id);
            try
            {
                if (id != request.Id)
                {
                    return BadRequest(new ApiResponse<string>("El ID de la URL no coincide con el ID del request"));
                }

                var tag = await _context.Tags.FindAsync(id);
                if (tag == null)
                {
                    return NotFound(new ApiResponse<string>("Tag not found"));
                }

                // Actualizar solo los campos proporcionados
                if (request.Key != null) tag.Key = request.Key;
                if (request.Value != null) tag.Value = request.Value;
                if (request.Category != null) tag.Category = request.Category;
                if (request.SubCategory != null) tag.SubCategory = request.SubCategory;
                if (request.Type != null) tag.Type = request.Type;
                if (request.Name != null) tag.Name = request.Name;
                if (request.Description != null) tag.Description = request.Description;
                if (request.IconUrl != null) tag.IconUrl = request.IconUrl;
                if (request.ImageUrl != null) tag.ImageUrl = request.ImageUrl;
                if (request.Status != null) tag.Status = request.Status;

                tag.UpdatedBy = request.CreatedBy ?? "System";
                tag.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var language = HttpContext.Items["Language"] as string;
                var tagDto = _mapper.Map<Tag, TagDTO>(tag, opt =>
                    opt.Items["Language"] = language);

                _logger.LogInformation("TaggingController --> PatchTag --> End: {Id}", id);
                return Ok(new ApiResponse<TagDTO>(tagDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error patching tag: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTag(Guid id)
        {
            _logger.LogInformation("TaggingController --> DeleteTag --> Start: {Id}", id);
            try
            {
                var tag = await _context.Tags.FindAsync(id);
                if (tag == null)
                {
                    return NotFound(new ApiResponse<string>("Tag not found"));
                }

                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();

                _logger.LogInformation("TaggingController --> DeleteTag --> End: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting tag: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(List<TagDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BulkCreateTags([FromBody] BulkCreateTagRequest request)
        {
            _logger.LogInformation("TaggingController --> BulkCreateTags --> Start");
            try
            {
                var createdTags = new List<Tag>();
                var errors = new List<string>();

                foreach (var tagRequest in request.Tags)
                {
                    try
                    {
                        var tag = _mapper.Map<CreateTagRequest, Tag>(tagRequest);
                        
                        // Establecer fechas de auditoría manualmente
                        tag.CreatedAt = DateTime.UtcNow;
                        tag.CreatedBy = tagRequest.CreatedBy ?? "System";
                        
                        await _context.Tags.AddAsync(tag);
                        createdTags.Add(tag);
                    }
                    catch (Exception ex)
                    {
                        var error = $"Error creating tag with key '{tagRequest.Key}': {ex.Message}";
                        errors.Add(error);
                        _logger.LogError(ex, "Error in bulk create for tag: {Key}", tagRequest.Key);
                        
                        if (!request.ContinueOnError)
                        {
                            return BadRequest(new ApiResponse<string>($"Error en creación masiva: {string.Join(", ", errors)}"));
                        }
                    }
                }

                await _context.SaveChangesAsync();

                var language = HttpContext.Items["Language"] as string;
                var tagDtos = _mapper.Map<List<Tag>, List<TagDTO>>(createdTags, opt =>
                    opt.Items["Language"] = language);

                _logger.LogInformation("TaggingController --> BulkCreateTags --> End: {Count} tags created", createdTags.Count);
                return CreatedAtAction(nameof(GetTags), new ApiResponse<List<TagDTO>>(tagDtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in bulk create tags");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpDelete("bulk")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BulkDeleteTags([FromBody] BulkDeleteTagRequest request)
        {
            _logger.LogInformation("TaggingController --> BulkDeleteTags --> Start");
            try
            {
                var deletedCount = 0;
                var errors = new List<string>();

                foreach (var tagId in request.TagIds)
                {
                    try
                    {
                        var tag = await _context.Tags.FindAsync(tagId);
                        if (tag != null)
                        {
                            _context.Tags.Remove(tag);
                            deletedCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        var error = $"Error deleting tag with id '{tagId}': {ex.Message}";
                        errors.Add(error);
                        _logger.LogError(ex, "Error in bulk delete for tag: {Id}", tagId);
                        
                        // Continue processing other tags even if one fails
                    }
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation("TaggingController --> BulkDeleteTags --> End: {Count} tags deleted", deletedCount);
                
                if (errors.Any())
                {
                    _logger.LogWarning("BulkDeleteTags completed with {ErrorCount} errors: {Errors}", errors.Count, string.Join(", ", errors));
                }
                
                return Ok(new ApiResponse<string>($"Se eliminaron {deletedCount} tags exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in bulk delete tags");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpGet("type/{type}")]
        [ProducesResponseType(typeof(List<TagDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTagsByType(string type)
        {
            _logger.LogInformation("TaggingController --> GetTagsByType --> Start: {Type}", type);
            try
            {
                var tags = await _context.Tags.Where(t => t.Type == type).ToListAsync();
                
                var language = HttpContext.Items["Language"] as string;
                var tagDtos = _mapper.Map<List<Tag>, List<TagDTO>>(tags, opt => 
                    opt.Items["Language"] = language);

                _logger.LogInformation("TaggingController --> GetTagsByType --> End: {Type}", type);
                return Ok(new ApiResponse<List<TagDTO>>(tagDtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tags by type: {Type}", type);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpGet("category/{category}")]
        [ProducesResponseType(typeof(List<TagDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTagsByCategory(string category)
        {
            _logger.LogInformation("TaggingController --> GetTagsByCategory --> Start: {Category}", category);
            try
            {
                var tags = await _context.Tags.Where(t => t.Category == category).ToListAsync();
                
                var language = HttpContext.Items["Language"] as string;
                var tagDtos = _mapper.Map<List<Tag>, List<TagDTO>>(tags, opt => 
                    opt.Items["Language"] = language);

                _logger.LogInformation("TaggingController --> GetTagsByCategory --> End: {Category}", category);
                return Ok(new ApiResponse<List<TagDTO>>(tagDtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tags by category: {Category}", category);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        #endregion
    }
}
