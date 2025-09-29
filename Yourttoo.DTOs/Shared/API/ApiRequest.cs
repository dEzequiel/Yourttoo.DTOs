using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Shared.API
{
    public class ApiRequest<TPayload> where TPayload : RequestPayloadBase
    {
        /// <summary>
        /// Endpoint route.
        /// </summary>
        [Required]
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// HTTP Method (GET, POST, PUT, DELETE, etc.)
        /// </summary>
        [Required]
        public string Method { get; set; } = string.Empty;

        /// <summary>
        /// Request payload.
        /// </summary>
        public TPayload? Payload { get; set; }

        /// <summary>
        /// Additional headers.
        /// </summary>
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Query string parameters.
        /// </summary>
        public Dictionary<string, string> QueryParameters { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Validate request./
        /// </summary>
        public virtual bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(Path) || string.IsNullOrWhiteSpace(Method))
                return false;


            return true;
        }
    }
}
