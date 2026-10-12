namespace EmployeeManagement.Domain.Entities;

public class LeaveRequest : BaseEntity
{
    public int LeaveId { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string LeaveType { get; set; } = "Casual Leave"; // Sick Leave, Casual Leave, Paid Leave, Work From Home
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int DaysCount { get; set; } = 1;
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

    // Navigation property
    public Employee? Employee { get; set; }
}
