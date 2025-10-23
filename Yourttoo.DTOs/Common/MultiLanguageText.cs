using Yourttoo.DTOs.Common.Constants;

namespace Yourttoo.DTOs.Common
{
    /// <summary>
    /// Represents a text in a specific language.
    /// </summary>
    public class LanguageText
    {
        public string Language { get; set; } = Languages.Spanish; // e.g., "en", "es", "fr", etc.
        public string? Text { get; set; }
    }

    /// <summary>
    /// Represents a collection of texts in multiple languages.
    /// </summary>
    public class MultiLanguageText
    {
        public List<LanguageText> Texts { get; set; } = new List<LanguageText>();
        
        /// <summary>
        /// Gets the text for a specific language.
        /// </summary>
        /// <param name="language">The language code to get text for</param>
        /// <returns>The text in the specified language, or null if not found</returns>
        public string? GetText(string language)
        {
            var langText = Texts.FirstOrDefault(t => t.Language.Equals(language, StringComparison.OrdinalIgnoreCase));
            return langText != null ? langText.Text : null;
        }
        
        /// <summary>
        /// Adds or updates text for a specific language.
        /// </summary>
        /// <param name="language">The language code</param>
        /// <param name="text">The text content</param>
        public void AddOrUpdateText(string language, string text)
        {
            var langText = Texts.FirstOrDefault(t => t.Language.Equals(language, StringComparison.OrdinalIgnoreCase));
            if (langText != null)
            {
                langText.Text = text;
            }
            else
            {
                Texts.Add(new LanguageText { Language = language, Text = text });
            }
        }
    }
}
