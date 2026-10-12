using System.Text.Json.Serialization;

namespace EmployeeManagement.Application.DTOs.Department;

public class ChildDepartmentDto
{
    [JsonPropertyName("childDepartmentId")]
    public int ChildDepartmentId { get; set; }

    [JsonPropertyName("childDepartmentName")]
    public string ChildDepartmentName { get; set; } = string.Empty;

    [JsonPropertyName("childDepartmentLogo")]
    public string ChildDepartmentLogo { get; set; } = string.Empty;

    // Backward-compatibility alias for Angular spelling "childDepartementLogo"
    [JsonPropertyName("childDepartementLogo")]
    public string ChildDepartementLogo => ChildDepartmentLogo;

    [JsonPropertyName("parentDepartmentId")]
    public int ParentDepartmentId { get; set; }
}
