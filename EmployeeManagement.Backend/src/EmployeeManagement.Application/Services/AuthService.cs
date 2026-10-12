using EmployeeManagement.Application.Common;
using EmployeeManagement.Application.DTOs.Auth;
using EmployeeManagement.Application.DTOs.Employee;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Interfaces;

namespace EmployeeManagement.Application.Services;

public class AuthService : IAuthService
{
    private readonly IEmployeeRepository _employeeRepository;

    public AuthService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<ApiResponse<EmployeeDto>> LoginAsync(LoginRequestDto loginDto)
    {
        if (string.IsNullOrWhiteSpace(loginDto.Username) || string.IsNullOrWhiteSpace(loginDto.Password))
        {
            return ApiResponse<EmployeeDto>.Failure("Username and password are required");
        }

        var employee = await _employeeRepository.AuthenticateAsync(loginDto.Username, loginDto.Password);
        if (employee == null)
        {
            return ApiResponse<EmployeeDto>.Failure("Invalid username or password");
        }

        var dto = new EmployeeDto
        {
            EmployeeId = employee.EmployeeId,
            EmployeeName = employee.EmployeeName,
            ContactNo = employee.ContactNo,
            EmailId = employee.EmailId,
            DeptId = employee.DeptId?.ToString() ?? string.Empty,
            Password = string.Empty, // Mask password in response for security
            Gender = employee.Gender,
            Role = employee.Role,
            CreatedDate = employee.CreatedDate
        };

        return ApiResponse<EmployeeDto>.Success(dto, "Login successful");
    }
}
