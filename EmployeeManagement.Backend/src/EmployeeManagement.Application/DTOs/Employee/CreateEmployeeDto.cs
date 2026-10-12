using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmployeeManagement.Application.DTOs.Employee;

public class CreateEmployeeDto
{
    [Required(ErrorMessage = "Employee name is required")]
    [JsonPropertyName("employeeName")]
    public string EmployeeName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Contact number is required")]
    [JsonPropertyName("contactNo")]
    public string ContactNo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    [JsonPropertyName("emailId")]
    public string EmailId { get; set; } = string.Empty;

    [JsonPropertyName("emaiId")]
    public string? EmaiId { set => EmailId = value ?? EmailId; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(4)]
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("gender")]
    public string Gender { get; set; } = "Male";

    [JsonPropertyName("role")]
    public string Role { get; set; } = "Employee";

    [JsonPropertyName("deptId")]
    public string? DeptId { get; set; }
}
