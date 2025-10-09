namespace Yourttoo.DTOs.Common.Constants
{
    /// <summary>
    /// Language codes constants
    /// </summary>
    public static class Languages
    {
        /// <summary>
        /// Spanish language code
        /// </summary>
        public const string Spanish = "es";

        /// <summary>
        /// English language code
        /// </summary>
        public const string English = "en";

        /// <summary>
        /// French language code
        /// </summary>
        public const string French = "fr";

        /// <summary>
        /// Portuguese language code
        /// </summary>
        public const string Portuguese = "pt";

        /// <summary>
        /// German language code
        /// </summary>
        public const string German = "de";

        /// <summary>
        /// Italian language code
        /// </summary>
        public const string Italian = "it";

        /// <summary>
        /// Default language for the application
        /// </summary>
        public const string Default = Spanish;

        /// <summary>
        /// All supported languages
        /// </summary>
        public static readonly string[] Supported = 
        {
            Spanish,
            English,
            French,
            Portuguese,
            German,
            Italian
        };

        /// <summary>
        /// Checks if a language code is supported
        /// </summary>
        /// <param name="languageCode">Language code to check</param>
        /// <returns>True if the language is supported</returns>
        public static bool IsSupported(string? languageCode)
        {
            if (string.IsNullOrWhiteSpace(languageCode))
                return false;

            return Supported.Contains(languageCode, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the language code or returns the default if not supported
        /// Normalizes to lowercase for consistency
        /// </summary>
        /// <param name="languageCode">Language code to validate</param>
        /// <returns>Valid language code in lowercase or default</returns>
        public static string GetValidLanguageOrDefault(string? languageCode)
        {
            if (IsSupported(languageCode))
                return languageCode!.ToLowerInvariant();
            
            return Default;
        }

        /// <summary>
        /// Normalizes language code to lowercase
        /// </summary>
        /// <param name="languageCode">Language code to normalize</param>
        /// <returns>Normalized language code or null if input is null/empty</returns>
        public static string? Normalize(string? languageCode)
        {
            return string.IsNullOrWhiteSpace(languageCode) ? null : languageCode.ToLowerInvariant();
        }
    }
}
