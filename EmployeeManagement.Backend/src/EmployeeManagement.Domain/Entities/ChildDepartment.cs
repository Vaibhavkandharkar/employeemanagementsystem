namespace EmployeeManagement.Domain.Entities;

public class ChildDepartment : BaseEntity
{
    public int ChildDepartmentId { get; set; }
    public string ChildDepartmentName { get; set; } = string.Empty;
    public string ChildDepartmentLogo { get; set; } = string.Empty;

    // Foreign Key
    public int ParentDepartmentId { get; set; }

    // Navigation property
    public ParentDepartment? ParentDepartment { get; set; }
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
