using AutoMapper;
using Yourttoo.DTOs.DTOs.Tagging.Tag;
using Yourttoo.DTOs.Models.Tagging;
using Yourttoo.DTOs.Common;
using Yourttoo.DTOs.Common.Constants;
using Yourttoo.DTOs.Requests.Tagging.Tag;

namespace Yourttoo.Api.Mappings
{
    public class TaggingProfile : Profile
    {
        public TaggingProfile()
        {
            #region Tag Mappings

            CreateMap<Tag, TagDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom((src, _, _, context) =>
                    src.Name?.GetText(context.Items["Language"] as string ?? Languages.Default) ?? string.Empty))
                .ForMember(dest => dest.Description, opt => opt.MapFrom((src, _, _, context) =>
                    src.Description?.GetText(context.Items["Language"] as string ?? Languages.Default) ?? string.Empty))
                .ForMember(dest => dest.Language, opt => opt.MapFrom((_, _, _, context) =>
                    context.Items["Language"] as string ?? string.Empty));

            CreateMap<TagDTO, Tag>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom((src, _, _, context) =>
                    new MultiLanguageText
                    {
                        Texts = new List<LanguageText>
                        {
                            new LanguageText
                            {
                                Language = context.Items["Language"] as string ?? Languages.Default,
                                Text = src.Name ?? string.Empty
                            }
                        }
                    }))
                .ForMember(dest => dest.Description, opt => opt.MapFrom((src, _, _, context) =>
                    new MultiLanguageText
                    {
                        Texts = new List<LanguageText>
                        {
                            new LanguageText
                            {
                                Language = context.Items["Language"] as string ?? Languages.Default,
                                Text = src.Description ?? string.Empty
                            }
                        }
                    }));

            CreateMap<Tag, TagDetailDTO>()
                .ForMember(dest => dest.Language, opt => opt.MapFrom((_, _, _, context) =>
                    context.Items["Language"] as string ?? Languages.Default));


            CreateMap<CreateTagRequest, Tag>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Se establece manualmente en el controlador
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Se establece manualmente en el controlador
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());

            CreateMap<UpdateTagRequest, Tag>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<PatchTagRequest, Tag>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((_, _, srcMember) => srcMember != null));

            CreateMap<BulkCreateTagRequest, List<Tag>>()
                .ConvertUsing(src => src.Tags.Select(tagRequest => new Tag
                {
                    Key = tagRequest.Key,
                    Value = tagRequest.Value,
                    Category = tagRequest.Category,
                    SubCategory = tagRequest.SubCategory,
                    Type = tagRequest.Type,
                    Name = tagRequest.Name,
                    Description = tagRequest.Description,
                    IconUrl = tagRequest.IconUrl,
                    ImageUrl = tagRequest.ImageUrl,
                    Status = tagRequest.Status
                    // CreatedAt y CreatedBy se establecen manualmente en el controlador
                }).ToList());

            #endregion
        }
    }
}
