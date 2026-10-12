using EmployeeManagement.Application.Common;
using EmployeeManagement.Application.DTOs.Employee;

namespace EmployeeManagement.Application.Interfaces;

public interface IEmployeeService
{
    Task<ApiResponse<IEnumerable<EmployeeDto>>> GetAllEmployeesAsync();
    Task<ApiResponse<EmployeeDto>> GetEmployeeByIdAsync(int id);
    Task<ApiResponse<EmployeeDto>> CreateEmployeeAsync(CreateEmployeeDto dto);
    Task<ApiResponse<EmployeeDto>> UpdateEmployeeAsync(UpdateEmployeeDto dto);
    Task<ApiResponse<bool>> DeleteEmployeeAsync(int id);
    Task<ApiResponse<IEnumerable<EmployeeDto>>> SearchEmployeesAsync(string? searchTerm);
}
