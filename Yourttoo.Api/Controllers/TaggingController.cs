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

                if (!string.IsNullOrWhiteSpace(request.Value))
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

                // Mapeo con selección de idioma usando helper reutilizable
                var tagDtos = MultiLanguageMappingHelper.MapToTagDtos(tags, language);

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
            _logger.LogInformation($"TagController --> UpdateTag --> Start: {id}");
            try
            {
                var tag = await _context.Tags
                    .Include(t => t.Categories)
                    .FirstOrDefaultAsync(t => t.Id == id);

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
