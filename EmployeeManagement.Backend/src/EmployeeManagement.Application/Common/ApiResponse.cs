using System.Text.Json.Serialization;

namespace EmployeeManagement.Application.Common;

public class ApiResponse<T>
{
    [JsonPropertyName("result")]
    public bool Result { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public T? Data { get; set; }

    public static ApiResponse<T> Success(T data, string message = "Operation completed successfully")
    {
        return new ApiResponse<T>
        {
            Result = true,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> Failure(string message, T? data = default)
    {
        return new ApiResponse<T>
        {
            Result = false,
            Message = message,
            Data = data
        };
    }
}

public class ApiResponse : ApiResponse<object>
{
    public static ApiResponse Success(string message = "Operation completed successfully")
    {
        return new ApiResponse
        {
            Result = true,
            Message = message,
            Data = null
        };
    }

    public static ApiResponse Failure(string message)
    {
        return new ApiResponse
        {
            Result = false,
            Message = message,
            Data = null
        };
    }
}
