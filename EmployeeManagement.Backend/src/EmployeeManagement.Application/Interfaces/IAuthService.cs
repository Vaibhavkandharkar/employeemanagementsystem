using EmployeeManagement.Application.Common;
using EmployeeManagement.Application.DTOs.Auth;
using EmployeeManagement.Application.DTOs.Employee;

namespace EmployeeManagement.Application.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<EmployeeDto>> LoginAsync(LoginRequestDto loginDto);
}
