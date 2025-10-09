using Yourttoo.DTOs.Shared.Pagination;

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
        /// Constructor
        /// </summary>
        public ApiResponse(string message, bool succeeded)
        {
            Succeeded = succeeded;
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

    /// <summary>
    /// API Response with pagination support
    /// </summary>
    public class PaginatedApiResponse<T> : ApiResponse<List<T>>
    {
        /// <summary>
        /// Pagination information
        /// </summary>
        public PaginatedParameters? Pagination { get; set; }

        public PaginatedApiResponse() { }

        public PaginatedApiResponse(List<T> data, PaginatedParameters pagination, string message = "")
            : base(data, message)
        {
            Pagination = pagination;
        }

        public PaginatedApiResponse(string errorMessage) : base(errorMessage)
        {
        }
    }
}
