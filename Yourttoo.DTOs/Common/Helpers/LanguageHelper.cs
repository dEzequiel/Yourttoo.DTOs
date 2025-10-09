using Yourttoo.DTOs.Common.Constants;

namespace Yourttoo.DTOs.Common.Helpers
{
    /// <summary>
    /// Helper class for language-related operations
    /// </summary>
    public static class LanguageHelper
    {
        /// <summary>
        /// Extrae el código de idioma del header Accept-Language
        /// </summary>
        /// <param name="acceptLanguageHeader">Valor del header Accept-Language</param>
        /// <returns>Código de idioma válido o idioma por defecto</returns>
        public static string ExtractLanguageFromAcceptLanguage(string? acceptLanguageHeader)
        {
            if (string.IsNullOrWhiteSpace(acceptLanguageHeader))
                return Languages.Default;

            // Accept-Language puede ser: "es-ES,es;q=0.9,en;q=0.8,fr;q=0.7"
            // Tomamos el primer idioma soportado en orden de preferencia
            var languages = acceptLanguageHeader.Split(',');
            
            foreach (var lang in languages)
            {
                // Extraer código de idioma (ej: "es-ES" -> "es", "en;q=0.8" -> "en")
                var languageCode = lang.Trim().Split(';')[0].Split('-')[0].ToLowerInvariant();
                
                // Verificar si es un idioma soportado
                if (Languages.IsSupported(languageCode))
                {
                    return languageCode;
                }
            }

            return Languages.Default;
        }

        /// <summary>
        /// Extrae el código de idioma del header Accept-Language con calidad (q-values)
        /// </summary>
        /// <param name="acceptLanguageHeader">Valor del header Accept-Language</param>
        /// <returns>Código de idioma válido con mayor prioridad o idioma por defecto</returns>
        public static string ExtractLanguageFromAcceptLanguageWithQuality(string? acceptLanguageHeader)
        {
            if (string.IsNullOrWhiteSpace(acceptLanguageHeader))
                return Languages.Default;

            var languagePreferences = new List<(string language, float quality)>();

            // Parsear cada idioma con su calidad
            var languages = acceptLanguageHeader.Split(',');
            
            foreach (var lang in languages)
            {
                var parts = lang.Trim().Split(';');
                var languageCode = parts[0].Split('-')[0].ToLowerInvariant();
                
                // Obtener calidad (q-value), por defecto es 1.0
                float quality = 1.0f;
                if (parts.Length > 1)
                {
                    var qPart = parts[1].Trim();
                    if (qPart.StartsWith("q=") && float.TryParse(qPart.Substring(2), out var q))
                    {
                        quality = q;
                    }
                }

                // Solo agregar idiomas soportados
                if (Languages.IsSupported(languageCode))
                {
                    languagePreferences.Add((languageCode, quality));
                }
            }

            // Ordenar por calidad descendente y retornar el primero
            var bestLanguage = languagePreferences
                .OrderByDescending(x => x.quality)
                .FirstOrDefault();

            return !string.IsNullOrEmpty(bestLanguage.language) ? bestLanguage.language : Languages.Default;
        }

        /// <summary>
        /// Obtiene todos los idiomas preferidos del header Accept-Language ordenados por prioridad
        /// </summary>
        /// <param name="acceptLanguageHeader">Valor del header Accept-Language</param>
        /// <returns>Lista de idiomas ordenados por preferencia</returns>
        public static List<string> GetPreferredLanguages(string? acceptLanguageHeader)
        {
            if (string.IsNullOrWhiteSpace(acceptLanguageHeader))
                return new List<string> { Languages.Default };

            var languagePreferences = new List<(string language, float quality)>();
            var languages = acceptLanguageHeader.Split(',');
            
            foreach (var lang in languages)
            {
                var parts = lang.Trim().Split(';');
                var languageCode = parts[0].Split('-')[0].ToLowerInvariant();
                
                float quality = 1.0f;
                if (parts.Length > 1)
                {
                    var qPart = parts[1].Trim();
                    if (qPart.StartsWith("q=") && float.TryParse(qPart.Substring(2), out var q))
                    {
                        quality = q;
                    }
                }

                if (Languages.IsSupported(languageCode))
                {
                    languagePreferences.Add((languageCode, quality));
                }
            }

            var result = languagePreferences
                .OrderByDescending(x => x.quality)
                .Select(x => x.language)
                .Distinct()
                .ToList();

            return result.Any() ? result : new List<string> { Languages.Default };
        }

        /// <summary>
        /// Verifica si un header Accept-Language contiene un idioma específico
        /// </summary>
        /// <param name="acceptLanguageHeader">Valor del header Accept-Language</param>
        /// <param name="targetLanguage">Idioma a buscar</param>
        /// <returns>True si el idioma está presente en el header</returns>
        public static bool ContainsLanguage(string? acceptLanguageHeader, string targetLanguage)
        {
            if (string.IsNullOrWhiteSpace(acceptLanguageHeader) || string.IsNullOrWhiteSpace(targetLanguage))
                return false;

            var preferredLanguages = GetPreferredLanguages(acceptLanguageHeader);
            return preferredLanguages.Contains(targetLanguage.ToLowerInvariant());
        }
    }
}
