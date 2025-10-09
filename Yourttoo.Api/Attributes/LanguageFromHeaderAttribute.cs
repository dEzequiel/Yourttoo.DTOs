using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Yourttoo.DTOs.Common.Helpers;

namespace Yourttoo.Api.Attributes
{
    /// <summary>
    /// Attribute that automatically extracts language from Accept-Language header
    /// and makes it available via HttpContext.Items["Language"]
    /// </summary>
    public class LanguageFromHeaderAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var acceptLanguage = context.HttpContext.Request.Headers["Accept-Language"].FirstOrDefault();
            var language = LanguageHelper.ExtractLanguageFromAcceptLanguage(acceptLanguage);
            
            // Store language in HttpContext.Items for easy access
            context.HttpContext.Items["Language"] = language;
            
            base.OnActionExecuting(context);
        }
    }
}
