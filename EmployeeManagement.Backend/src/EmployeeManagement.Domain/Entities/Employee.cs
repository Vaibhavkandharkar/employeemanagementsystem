namespace EmployeeManagement.Domain.Entities;

public class Employee : BaseEntity
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string ContactNo { get; set; } = string.Empty;
    public string EmailId { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string Role { get; set; } = "Employee"; // Admin, HR, Employee
    public int? DeptId { get; set; }

    public ChildDepartment? Department { get; set; }
}
