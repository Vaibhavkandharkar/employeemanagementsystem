using System.Text.Json.Serialization;

namespace EmployeeManagement.Application.DTOs.Department;

public class ParentDepartmentDto
{
    [JsonPropertyName("departmentId")]
    public int DepartmentId { get; set; }

    [JsonPropertyName("departmentName")]
    public string DepartmentName { get; set; } = string.Empty;

    [JsonPropertyName("departmentLogo")]
    public string DepartmentLogo { get; set; } = string.Empty;

    [JsonPropertyName("departementLogo")]
    public string DepartementLogo => DepartmentLogo;
}
