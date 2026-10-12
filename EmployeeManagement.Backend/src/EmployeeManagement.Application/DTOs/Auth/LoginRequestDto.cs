using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmployeeManagement.Application.DTOs.Auth;

public class LoginRequestDto
{
    [Required]
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}
