using Yourttoo.DTOs.Common.Constants;

namespace Yourttoo.DTOs.Common.Extensions
{
    /// <summary>
    /// Extension methods for IdiomaticText operations
    /// </summary>
    public static class IdiomaticTextExtensions
    {
        /// <summary>
        /// Gets the IdiomaticText for the specified language, with fallback logic
        /// </summary>
        /// <param name="texts">List of IdiomaticText items</param>
        /// <param name="language">Preferred language code (e.g., "es", "en")</param>
        /// <returns>IdiomaticText in the preferred language, fallback language, or first available</returns>
        public static MultiLanguageText GetByLanguageOrDefault(this List<MultiLanguageText> texts, string? language)
        {
            if (texts == null || !texts.Any())
                return new MultiLanguageText { Content = "", Language = language ?? Languages.Default };

            var normalizedLanguage = Languages.GetValidLanguageOrDefault(language);

            // Buscar coincidencia exacta (case-insensitive)
            var exactMatch = texts.FirstOrDefault(t => 
                string.Equals(t.Language, normalizedLanguage, StringComparison.OrdinalIgnoreCase));
            
            if (exactMatch != null)
                return exactMatch;

            // Buscar el idioma por defecto si no es el mismo que se buscó
            if (!string.Equals(normalizedLanguage, Languages.Default, StringComparison.OrdinalIgnoreCase))
            {
                var defaultMatch = texts.FirstOrDefault(t => 
                    string.Equals(t.Language, Languages.Default, StringComparison.OrdinalIgnoreCase));
                
                if (defaultMatch != null)
                    return defaultMatch;
            }

            // Último recurso: retornar el primero disponible
            var firstAvailable = texts.FirstOrDefault();
            return firstAvailable ?? new MultiLanguageText { Content = "", Language = normalizedLanguage };
        }

        /// <summary>
        /// Gets the content for the specified language
        /// </summary>
        /// <param name="texts">List of IdiomaticText items</param>
        /// <param name="language">Preferred language code</param>
        /// <returns>Content string in the preferred language</returns>
        public static string GetContentByLanguageOrDefault(this List<MultiLanguageText> texts, string? language)
        {
            return texts.GetByLanguageOrDefault(language).Content;
        }

        /// <summary>
        /// Checks if the list contains text for the specified language
        /// </summary>
        /// <param name="texts">List of IdiomaticText items</param>
        /// <param name="language">Language code to check</param>
        /// <returns>True if language is available</returns>
        public static bool HasLanguage(this List<MultiLanguageText> texts, string? language)
        {
            if (texts == null || !texts.Any() || string.IsNullOrWhiteSpace(language))
                return false;

            var normalizedLanguage = Languages.GetValidLanguageOrDefault(language);
            return texts.Any(t => string.Equals(t.Language, normalizedLanguage, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets all available languages in the list
        /// </summary>
        /// <param name="texts">List of IdiomaticText items</param>
        /// <returns>List of available language codes</returns>
        public static List<string> GetAvailableLanguages(this List<MultiLanguageText> texts)
        {
            if (texts == null || !texts.Any())
                return new List<string>();

            return texts
                .Where(t => !string.IsNullOrWhiteSpace(t.Language))
                .Select(t => t.Language)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }
}
