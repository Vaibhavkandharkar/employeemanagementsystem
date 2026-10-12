using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Domain.Interfaces;

public interface IDepartmentRepository
{
    Task<IEnumerable<ParentDepartment>> GetAllParentDepartmentsAsync();
    Task<ParentDepartment?> GetParentDepartmentByIdAsync(int id);
    Task<ParentDepartment> AddParentDepartmentAsync(ParentDepartment department);

    Task<IEnumerable<ChildDepartment>> GetAllChildDepartmentsAsync();
    Task<IEnumerable<ChildDepartment>> GetChildDepartmentsByParentIdAsync(int parentId);
    Task<ChildDepartment?> GetChildDepartmentByIdAsync(int id);
    Task<ChildDepartment> AddChildDepartmentAsync(ChildDepartment department);

    Task DeleteParentDepartmentAsync(int id);
    Task DeleteChildDepartmentAsync(int id);
}
