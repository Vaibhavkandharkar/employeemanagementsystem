using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmployeeManagement.Application.DTOs.Employee;

public class UpdateEmployeeDto
{
    [Required]
    [JsonPropertyName("employeeId")]
    public int EmployeeId { get; set; }

    [Required]
    [JsonPropertyName("employeeName")]
    public string EmployeeName { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("contactNo")]
    public string ContactNo { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [JsonPropertyName("emailId")]
    public string EmailId { get; set; } = string.Empty;

    [JsonPropertyName("emaiId")]
    public string? EmaiId { set => EmailId = value ?? EmailId; }

    [JsonPropertyName("password")]
    public string? Password { get; set; }

    [JsonPropertyName("gender")]
    public string Gender { get; set; } = "Male";

    [JsonPropertyName("role")]
    public string Role { get; set; } = "Employee";

    [JsonPropertyName("deptId")]
    public string? DeptId { get; set; }
}
