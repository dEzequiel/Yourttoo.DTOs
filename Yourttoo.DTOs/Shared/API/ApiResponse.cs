namespace Yourttoo.DTOs.Shared.API
{
    /// <summary>
    /// Class to build and represent API responses.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponse<T>
    {
        public ApiResponse() { }

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiResponse(bool succeeded, string? message, List<string>? errors, T? data)
        {
            Succeeded = succeeded;
            Message = message;
            Errors = errors;
            Data = data;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiResponse(T data, string message = "")
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiResponse(string message)
        {
            Succeeded = false;
            Message = message;
        }

        /// <summary>
        /// Succeeded
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Error list
        /// </summary>
        public List<string>? Errors { get; set; }

        /// <summary>
        /// Data
        /// </susmmary>
        public T? Data { get; set; }
    }
}
