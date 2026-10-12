using EmployeeManagement.Application.Common;
using EmployeeManagement.Application.DTOs.Department;

namespace EmployeeManagement.Application.Interfaces;

public interface IDepartmentService
{
    Task<ApiResponse<IEnumerable<ParentDepartmentDto>>> GetAllParentDepartmentsAsync();
    Task<ApiResponse<IEnumerable<ChildDepartmentDto>>> GetAllChildDepartmentsAsync();
    Task<ApiResponse<IEnumerable<ChildDepartmentDto>>> GetChildDepartmentsByParentIdAsync(int parentId);
    Task<ApiResponse<ParentDepartmentDto>> CreateParentDepartmentAsync(ParentDepartmentDto dto);
    Task<ApiResponse<ChildDepartmentDto>> CreateChildDepartmentAsync(ChildDepartmentDto dto);
    Task<ApiResponse<bool>> DeleteParentDepartmentAsync(int id);
    Task<ApiResponse<bool>> DeleteChildDepartmentAsync(int id);
}
