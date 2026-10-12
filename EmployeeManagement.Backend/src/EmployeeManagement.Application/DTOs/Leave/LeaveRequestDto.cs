using System.Text.Json.Serialization;

namespace EmployeeManagement.Application.DTOs.Leave;

public class LeaveRequestDto
{
    [JsonPropertyName("leaveId")]
    public int LeaveId { get; set; }

    [JsonPropertyName("employeeId")]
    public int EmployeeId { get; set; }

    [JsonPropertyName("employeeName")]
    public string EmployeeName { get; set; } = string.Empty;

    [JsonPropertyName("leaveType")]
    public string LeaveType { get; set; } = string.Empty;

    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("endDate")]
    public DateTime EndDate { get; set; }

    [JsonPropertyName("daysCount")]
    public int DaysCount { get; set; }

    [JsonPropertyName("reason")]
    public string Reason { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = "Pending";

    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }
}

public class CreateLeaveDto
{
    [JsonPropertyName("employeeId")]
    public int EmployeeId { get; set; }

    [JsonPropertyName("employeeName")]
    public string? EmployeeName { get; set; }

    [JsonPropertyName("leaveType")]
    public string LeaveType { get; set; } = "Casual Leave";

    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("endDate")]
    public DateTime EndDate { get; set; }

    [JsonPropertyName("reason")]
    public string Reason { get; set; } = string.Empty;
}

public class UpdateLeaveStatusDto
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = "Approved"; // Approved or Rejected
}
