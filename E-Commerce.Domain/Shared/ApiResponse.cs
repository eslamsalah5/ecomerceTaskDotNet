using System.Text.Json.Serialization;

namespace E_Commerce.Domain.Shared
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? Errors { get; set; }

        public static ApiResponse<T> SuccessResult(T data, string message = "Success", int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                Success = true,
                StatusCode = statusCode,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> ErrorResult(string message, List<string>? errors = null, int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                Success = false,
                StatusCode = statusCode,
                Message = message,
                Errors = errors?.Any() == true ? errors : null
            };
        }
    }

    public class ApiResponse : ApiResponse<object>
    {
        public static ApiResponse SuccessResult(string message = "Success", int statusCode = 200)
        {
            return new ApiResponse
            {
                Success = true,
                StatusCode = statusCode,
                Message = message,
                Data = null
            };
        }

        public static new ApiResponse ErrorResult(string message, List<string>? errors = null, int statusCode = 400)
        {
            return new ApiResponse
            {
                Success = false,
                StatusCode = statusCode,
                Message = message,
                Errors = errors?.Any() == true ? errors : null
            };
        }
    }
}