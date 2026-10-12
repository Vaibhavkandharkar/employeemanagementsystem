namespace EmployeeManagement.Domain.Entities;

public class ParentDepartment : BaseEntity
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string DepartmentLogo { get; set; } = string.Empty;

    public ICollection<ChildDepartment> ChildDepartments { get; set; } = new List<ChildDepartment>();
}
