using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Yourttoo.Api.DataAccess;
using Yourttoo.DTOs.DTOs.Tagging.TagCategory;
using Yourttoo.DTOs.Requests.Tagging.TagCategory;
using Yourttoo.DTOs.Shared.API;
using Yourttoo.DTOs.Shared.Pagination;
using Yourttoo.DTOs.Models.Tagging;
using Yourttoo.DTOs.DTOs.Tagging.Tag;
using Yourttoo.DTOs.Requests.Tagging.Tag;
using Yourttoo.DTOs.Common;
using Yourttoo.DTOs.Common.Constants;
using Yourttoo.DTOs.Common.Helpers;

namespace Yourttoo.Api.Controllers
{
    [Route("/store/tagging/categories")]
    [ApiController]
    public class TagCategoryController : ControllerBase
    {
        private readonly ILogger<TagCategoryController> _logger;
        private readonly IMapper _mapper;
        private readonly DatabaseContext _context;
        public TagCategoryController(ILogger<TagCategoryController> logger, IMapper mapper, DatabaseContext context)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("{id}/tags")]
        [ProducesResponseType(typeof(PaginatedApiResponse<TagDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTagsByCategoryId(
            Guid id,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? status = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation($"TagCategoryController --> GetTagsByCategoryId --> Start: {id}");
            try
            {
                // Verificar que la categoría existe
                var categoryExists = await _context.TagCategory
                    .AnyAsync(tc => tc.Id == id);

                if (!categoryExists)
                {
                    return NotFound(new ApiResponse<string>("Categoría no encontrada"));
                }

                // Construir query de tags para esta categoría
                var query = _context.Tags
                    .Where(t => t.Categories.Any(c => c.Id == id))
                    .AsNoTracking();

                // Aplicar filtros opcionales
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = query.Where(t => t.Name.Any(n => n.Content.Contains(searchTerm)) ||
                                           t.Description.Any(d => d.Content.Contains(searchTerm)) ||
                                           t.Code.Contains(searchTerm) ||
                                           t.Slug.Contains(searchTerm));
                }

                if (!string.IsNullOrWhiteSpace(status))
                {
                    query = query.Where(t => t.Status == status);
                }

                // Obtener conteo total
                var totalCount = await query.CountAsync();

                // Aplicar paginación
                var tags = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var tagDtos = _mapper.Map<List<TagDTO>>(tags);
                var pagination = new PaginatedParameters(page, pageSize, totalCount);

                _logger.LogInformation($"TagCategoryController --> GetTagsByCategoryId --> End: {id}, Page={page}, PageSize={pageSize}, Total={totalCount}");
                return Ok(new PaginatedApiResponse<TagDTO>(tagDtos, pagination));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los tags de la categoría: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedApiResponse<TagCategoryDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTagCategories(
            [FromQuery] QueryTagCategoryRequest request,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation($"TagCategoryController --> GetTagCategories --> Start at {DateTime.UtcNow}");

            // Extraer idioma del header Accept-Language
            var acceptLanguage = Request.Headers["Accept-Language"].FirstOrDefault();
            var language = LanguageHelper.ExtractLanguageFromAcceptLanguage(acceptLanguage);

            // Log de parámetros de la request
            _logger.LogInformation("Request Parameters: SearchTerm={SearchTerm}, Code={Code}, IsFilterable={IsFilterable}, ExtractedLanguage={Language}, Page={Page}, PageSize={PageSize}",
                request.SearchTerm, request.Code, request.IsFilterable, language, page, pageSize);

            try
            {
                // Usar parámetros de paginación directos

                // Construcción de la consulta base con navegación de Tags
                var query = _context.TagCategory
                    .Include(tc => tc.Tags)
                    .AsNoTracking();

                // Aplicar filtros
                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    if (!string.IsNullOrWhiteSpace(language))
                    {
                        // Filtrar por idioma específico y buscar en ese idioma (SQLite compatible)
                        var searchLanguage = Languages.GetValidLanguageOrDefault(language);
                        query = query.Where(tc => tc.Name.Any(n => n.Language == searchLanguage && n.Content.Contains(request.SearchTerm)) ||
                                                tc.Description.Any(d => d.Language == searchLanguage && d.Content.Contains(request.SearchTerm)));
                    }
                    else
                    {
                        // Buscar en todos los idiomas
                        query = query.Where(tc => tc.Name.Any(n => n.Content.Contains(request.SearchTerm)) ||
                                                tc.Description.Any(d => d.Content.Contains(request.SearchTerm)));
                    }
                }

                if (!string.IsNullOrWhiteSpace(request.Code))
                {
                    query = query.Where(tc => tc.Code == request.Code);
                }

                if (request.IsFilterable.HasValue)
                {
                    query = query.Where(tc => tc.Filtering == request.IsFilterable.Value);
                }

                // Obtener conteo total
                var totalCount = await query.CountAsync();

                // Obtener datos con paginación (SQLite compatible)
                var categories = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                // Mapeo con selección de idioma usando helper reutilizable
                var categoryDtos = MultiLanguageMappingHelper.MapToTagCategoryDtos(categories, language);

                var pagination = new PaginatedParameters(page, pageSize, totalCount);

                _logger.LogInformation("TagCategoryController --> GetTagCategories --> End at {EndTime}", DateTime.UtcNow);
                return Ok(new PaginatedApiResponse<TagCategoryDTO>(categoryDtos, pagination));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tag categories");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TagCategoryDetailDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTagCategoryById(Guid id)
        {
            _logger.LogInformation("TagCategoryController --> GetTagCategoryById --> Start: {Id}", id);

            try
            {
                var category = await _context.TagCategory
                    .Include(tc => tc.Tags)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(tc => tc.Id == id);

                if (category == null)
                {
                    return NotFound(new ApiResponse<string>("Categoría no encontrada"));
                }

                var categoryDto = _mapper.Map<TagCategoryDetailDTO>(category);
                _logger.LogInformation("TagCategoryController --> GetTagCategoryById --> End: {Id}", id);
                return Ok(new ApiResponse<TagCategoryDetailDTO>(categoryDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tag category by id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(TagCategoryDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTagCategory([FromBody] CreateTagCategoryRequest request)
        {
            _logger.LogInformation("TagCategoryController --> CreateTagCategory --> Start");
            try
            {
                // Verificar si ya existe una categoría con el mismo código
                var existingCategory = await _context.TagCategory
                    .AsNoTracking()
                    .FirstOrDefaultAsync(tc => tc.Code == request.Code);

                if (existingCategory != null)
                {
                    return BadRequest(new ApiResponse<string>("Ya existe una categoría con este código"));
                }

                var category = new TagCategory
                {
                    Id = Guid.NewGuid(),
                    Code = request.Code,
                    Name = request.Name,
                    Description = request.Description,
                    Filtering = request.Filtering,
                    CreatedBy = "System", // TODO: Obtener del contexto de usuario
                    CreatedAt = DateTime.UtcNow
                };

                _context.TagCategory.Add(category);
                await _context.SaveChangesAsync();

                var categoryDto = _mapper.Map<TagCategoryDTO>(category);
                _logger.LogInformation("TagCategoryController --> CreateTagCategory --> End: {Id}", category.Id);

                return CreatedAtAction(nameof(GetTagCategoryById),
                    new { id = category.Id },
                    new ApiResponse<TagCategoryDTO>(categoryDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating tag category");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TagCategoryDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTagCategory(Guid id, [FromBody] UpdateTagCategoryRequest request)
        {
            _logger.LogInformation($"TagCategoryController --> UpdateTagCategory --> Start: {id}");
            try
            {
                if (id != request.Id)
                {
                    return BadRequest(new ApiResponse<string>("El ID de la URL no coincide con el ID del request"));
                }

                var category = await _context.TagCategory.FindAsync(id);
                if (category == null)
                {
                    return NotFound(new ApiResponse<string>("Categoría no encontrada"));
                }

                // Verificar si el nuevo código ya existe en otra categoría
                if (category.Code != request.Code)
                {
                    var existingCategory = await _context.TagCategory
                        .AsNoTracking()
                        .FirstOrDefaultAsync(tc => tc.Code == request.Code && tc.Id != id);

                    if (existingCategory != null)
                    {
                        return BadRequest(new ApiResponse<string>("Ya existe otra categoría con este código"));
                    }
                }

                // Actualizar propiedades
                category.Code = request.Code;
                category.Name = request.Name;
                category.Description = request.Description;
                category.Filtering = request.Filtering;
                category.UpdatedBy = "System"; // TODO: Obtener del contexto de usuario
                category.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var categoryDto = _mapper.Map<TagCategoryDTO>(category);
                _logger.LogInformation($"TagCategoryController --> UpdateTagCategory --> End: {id}");
                return Ok(new ApiResponse<TagCategoryDTO>(categoryDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating tag category: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTagCategory(Guid id)
        {
            _logger.LogInformation($"TagCategoryController --> DeleteTagCategory --> Start: {id}");
            try
            {
                var category = await _context.TagCategory.FindAsync(id);
                if (category == null)
                {
                    return NotFound(new ApiResponse<string>("Categoría no encontrada"));
                }

                // Verificar si hay tags asociados a esta categoría
                var hasAssociatedTags = await _context.Tags
                    .AsNoTracking()
                    .AnyAsync(t => t.Categories.Any(c => c.Id == id));

                if (hasAssociatedTags)
                {
                    return BadRequest(new ApiResponse<string>("No se puede eliminar la categoría porque tiene tags asociados"));
                }

                _context.TagCategory.Remove(category);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"TagCategoryController --> DeleteTagCategory --> End: {id}");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting tag category: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(ApiResponse<List<TagCategoryDTO>>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BulkCreateTagCategories([FromBody] BulkCreateTagCategoryRequest request)
        {
            _logger.LogInformation($"TagCategoryController --> BulkCreateTagCategories --> Start: {request.Categories.Count} categories");

            if (!request.Categories.Any())
            {
                return BadRequest(new ApiResponse<string>("La lista de categorías no puede estar vacía"));
            }

            try
            {
                var results = new List<TagCategoryDTO>();
                var errors = new List<string>();

                // Validar códigos únicos dentro del request
                var duplicateCodes = request.Categories
                    .GroupBy(c => c.Code)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                if (duplicateCodes.Any())
                {
                    return BadRequest(new ApiResponse<string>($"Códigos duplicados en el request: {string.Join(", ", duplicateCodes)}"));
                }

                // Verificar códigos existentes en la base de datos
                var requestCodes = request.Categories.Select(c => c.Code).ToList();
                var existingCategories = await _context.TagCategory
                    .AsNoTracking()
                    .Where(tc => requestCodes.Contains(tc.Code))
                    .Select(tc => tc.Code)
                    .ToListAsync();

                if (existingCategories.Any())
                {
                    return BadRequest(new ApiResponse<string>($"Los siguientes códigos ya existen: {string.Join(", ", existingCategories)}"));
                }

                // Crear todas las categorías
                var categoriesToCreate = new List<TagCategory>();
                var currentTime = DateTime.UtcNow;

                foreach (var categoryRequest in request.Categories)
                {
                    var category = new TagCategory
                    {
                        Id = Guid.NewGuid(),
                        Code = categoryRequest.Code,
                        Name = categoryRequest.Name,
                        Description = categoryRequest.Description,
                        Filtering = categoryRequest.Filtering,
                        CreatedBy = "System", // TODO: Obtener del contexto de usuario
                        CreatedAt = currentTime
                    };

                    categoriesToCreate.Add(category);
                }

                // Insertar en lote
                _context.TagCategory.AddRange(categoriesToCreate);
                await _context.SaveChangesAsync();

                // Mapear a DTOs
                var categoryDtos = _mapper.Map<List<TagCategoryDTO>>(categoriesToCreate);

                _logger.LogInformation($"TagCategoryController --> BulkCreateTagCategories --> End: {categoryDtos.Count} categories created");
                return CreatedAtAction(nameof(GetTagCategories),
                    new ApiResponse<List<TagCategoryDTO>>(categoryDtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error bulk creating tag categories");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor durante la creación en lote"));
            }
        }


    }

    [Route("/store/tagging/tags")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ILogger<TagController> _logger;
        private readonly IMapper _mapper;
        private readonly DatabaseContext _context;

        public TagController(ILogger<TagController> logger, IMapper mapper, DatabaseContext context)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedApiResponse<TagDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTags([FromQuery] QueryTagsRequest request, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation($"TagController --> GetTags --> Start at {DateTime.UtcNow}");

            // Extraer idioma del header Accept-Language
            var acceptLanguage = Request.Headers["Accept-Language"].FirstOrDefault();
            var language = LanguageHelper.ExtractLanguageFromAcceptLanguage(acceptLanguage);

            // Log de parámetros de la request
            _logger.LogInformation("Request Parameters: SearchTerm={SearchTerm}, Code={Code}, Status={Status}, Slug={Slug}, CategoryId={CategoryId}, ExtractedLanguage={Language}, Page={Page}, PageSize={PageSize}",
                request.SearchTerm, request.Code, request.Status, request.Slug, request.CategoryId, language, page, pageSize);

            try
            {
                // Construcción de la consulta base con navegación de Categorías
                var query = _context.Tags
                    .Include(t => t.Categories)
                    .AsNoTracking();

                // Aplicar filtros
                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    if (!string.IsNullOrWhiteSpace(language))
                    {
                        // Filtrar por idioma específico y buscar en ese idioma (SQLite compatible)
                        var searchLanguage = Languages.GetValidLanguageOrDefault(language);
                        query = query.Where(t => t.Name.Any(n => n.Language == searchLanguage && n.Content.Contains(request.SearchTerm)) ||
                                                t.Description.Any(d => d.Language == searchLanguage && d.Content.Contains(request.SearchTerm)));
                    }
                    else
                    {
                        // Buscar en todos los idiomas
                        query = query.Where(t => t.Name.Any(n => n.Content.Contains(request.SearchTerm)) ||
                                                t.Description.Any(d => d.Content.Contains(request.SearchTerm)));
                    }
                }

                if (!string.IsNullOrWhiteSpace(request.Code))
                {
                    query = query.Where(t => t.Code == request.Code);
                }

                if (!string.IsNullOrWhiteSpace(request.Status))
                {
                    query = query.Where(t => t.Status == request.Status);
                }

                if (!string.IsNullOrWhiteSpace(request.Slug))
                {
                    query = query.Where(t => t.Slug == request.Slug);
                }

                if (request.CategoryId.HasValue)
                {
                    query = query.Where(t => t.Categories.Any(c => c.Id == request.CategoryId.Value));
                }

                if (request.CategoryIds != null && request.CategoryIds.Any())
                {
                    query = query.Where(t => t.Categories.Any(c => request.CategoryIds.Contains(c.Id)));
                }

                // Obtener conteo total
                var totalCount = await query.CountAsync();

                // Obtener datos con paginación (SQLite compatible)
                var tags = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                // Mapeo con selección de idioma usando helper reutilizable
                var tagDtos = MultiLanguageMappingHelper.MapToTagDtos(tags, language);

                var pagination = new PaginatedParameters(page, pageSize, totalCount);

                _logger.LogInformation("TagController --> GetTags --> End at {EndTime}", DateTime.UtcNow);
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
            _logger.LogInformation("TagController --> GetTagById --> Start: {Id}", id);
            try
            {
                var tag = await _context.Tags
                    .Include(t => t.Categories)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (tag == null)
                {
                    return NotFound(new ApiResponse<string>("Tag no encontrada"));
                }

                var tagDto = _mapper.Map<TagDetailDTO>(tag);
                _logger.LogInformation("TagController --> GetTagById --> End: {Id}", id);
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
            _logger.LogInformation("TagController --> CreateTag --> Start");
            try
            {
                // Verificar que las categorías existen
                var existingCategories = await _context.TagCategory
                    .Where(c => request.Categories.Contains(c.Id))
                    .ToListAsync();

                if (existingCategories.Count != request.Categories.Count)
                {
                    return BadRequest(new ApiResponse<string>("Una o más categorías no existen"));
                }

                var tag = new Tag
                {
                    Id = Guid.NewGuid(),
                    Code = request.Code,
                    Name = request.Name,
                    Description = request.Description,
                    Label = request.Label,
                    Slug = request.Slug,
                    Status = request.Status,
                    Icon = request.Icon,
                    Categories = existingCategories,
                    CreatedBy = "System", // TODO: Obtener del contexto de usuario
                    CreatedAt = DateTime.UtcNow
                };

                _context.Tags.Add(tag);
                await _context.SaveChangesAsync();

                var tagDto = _mapper.Map<TagDTO>(tag);
                _logger.LogInformation($"TagController --> CreateTag --> End: {tag.Id}");
                return CreatedAtAction(nameof(GetTagById),
                    new { id = tag.Id },
                    new ApiResponse<TagDTO>(tagDto));
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
            _logger.LogInformation($"TagController --> UpdateTag --> Start: {id}");
            try
            {
                var tag = await _context.Tags
                    .Include(t => t.Categories)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (tag == null)
                {
                    return NotFound(new ApiResponse<string>("Tag no encontrada"));
                }

                // Verificar que las categorías existen
                var existingCategories = await _context.TagCategory
                    .Where(c => request.Categories.Contains(c.Id))
                    .ToListAsync();

                if (existingCategories.Count != request.Categories.Count)
                {
                    return BadRequest(new ApiResponse<string>("Una o más categorías no existen"));
                }

                tag.Code = request.Code;
                tag.Name = request.Name;
                tag.Description = request.Description;
                tag.Label = request.Label;
                tag.Slug = request.Slug;
                tag.Status = request.Status;
                tag.Icon = request.Icon;
                tag.Categories = existingCategories;
                tag.UpdatedBy = "System"; // TODO: Obtener del contexto de usuario
                tag.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var tagDto = _mapper.Map<TagDTO>(tag);
                _logger.LogInformation($"TagController --> UpdateTag --> End: {id}");
                return Ok(new ApiResponse<TagDTO>(tagDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating tag: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTag(Guid id)
        {
            _logger.LogInformation($"TagController --> DeleteTag --> Start: {id}");
            try
            {
                var tag = await _context.Tags.FindAsync(id);
                if (tag == null)
                {
                    return NotFound(new ApiResponse<string>("Tag no encontrada"));
                }

                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"TagController --> DeleteTag --> End: {id}");
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
        [ProducesResponseType(typeof(ApiResponse<List<TagDTO>>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BulkCreateTags([FromBody] BulkCreateTagRequest request)
        {
            _logger.LogInformation($"TagController --> BulkCreateTags --> Start: {request.Tags.Count} tags");

            if (!request.Tags.Any())
            {
                return BadRequest(new ApiResponse<string>("La lista de tags no puede estar vacía"));
            }

            try
            {
                // Obtener todas las categorías únicas que se necesitan
                var allCategoryIds = request.Tags
                    .SelectMany(t => t.Categories)
                    .Distinct()
                    .ToList();

                var existingCategories = await _context.TagCategory
                    .Where(c => allCategoryIds.Contains(c.Id))
                    .ToListAsync();

                if (existingCategories.Count != allCategoryIds.Count)
                {
                    var missingIds = allCategoryIds.Except(existingCategories.Select(c => c.Id));
                    return BadRequest(new ApiResponse<string>($"Las siguientes categorías no existen: {string.Join(", ", missingIds)}"));
                }

                var tagsToCreate = new List<Tag>();
                var currentTime = DateTime.UtcNow;

                foreach (var tagRequest in request.Tags)
                {
                    // Filtrar las categorías para este tag específico
                    var tagCategories = existingCategories
                        .Where(c => tagRequest.Categories.Contains(c.Id))
                        .ToList();

                    var tag = new Tag
                    {
                        Id = Guid.NewGuid(),
                        Code = tagRequest.Code,
                        Name = tagRequest.Name,
                        Description = tagRequest.Description,
                        Label = tagRequest.Label,
                        Slug = tagRequest.Slug,
                        Status = tagRequest.Status,
                        Icon = tagRequest.Icon,
                        Categories = tagCategories,
                        CreatedBy = "System", // TODO: Obtener del contexto de usuario
                        CreatedAt = currentTime
                    };

                    tagsToCreate.Add(tag);
                }

                _context.Tags.AddRange(tagsToCreate);
                await _context.SaveChangesAsync();

                var tagDtos = _mapper.Map<List<TagDTO>>(tagsToCreate);
                _logger.LogInformation($"TagController --> BulkCreateTags --> End: {tagDtos.Count} tags created");
                return CreatedAtAction(nameof(GetTags),
                    new ApiResponse<List<TagDTO>>(tagDtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error bulk creating tags");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor durante la creación en lote"));
            }
        }
    }
}
