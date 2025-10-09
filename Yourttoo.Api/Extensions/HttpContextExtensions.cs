namespace Yourttoo.Api.Extensions
{
    /// <summary>
    /// Extension methods for HttpContext to provide easy access to common request data
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Gets the extracted language from Accept-Language header
        /// </summary>
        /// <param name="context">HttpContext instance</param>
        /// <returns>Language code or null if not available</returns>
        public static string? GetLanguage(this HttpContext context)
        {
            return context.Items["Language"] as string;
        }
    }
}
