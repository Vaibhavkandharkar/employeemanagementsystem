using EmployeeManagement.Application.Common;
using EmployeeManagement.Application.DTOs.Employee;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces;

namespace EmployeeManagement.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<ApiResponse<IEnumerable<EmployeeDto>>> GetAllEmployeesAsync()
    {
        var employees = await _employeeRepository.GetAllAsync();
        var dtos = employees.Select(MapToDto);
        return ApiResponse<IEnumerable<EmployeeDto>>.Success(dtos, "Employees retrieved successfully");
    }

    public async Task<ApiResponse<EmployeeDto>> GetEmployeeByIdAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            return ApiResponse<EmployeeDto>.Failure($"Employee with ID {id} not found");
        }

        return ApiResponse<EmployeeDto>.Success(MapToDto(employee), "Employee retrieved successfully");
    }

    public async Task<ApiResponse<EmployeeDto>> CreateEmployeeAsync(CreateEmployeeDto dto)
    {
        var existing = await _employeeRepository.GetByEmailAsync(dto.EmailId);
        if (existing != null)
        {
            return ApiResponse<EmployeeDto>.Failure("An employee with this email already exists");
        }

        int? parsedDeptId = null;
        if (int.TryParse(dto.DeptId, out var deptIdVal))
        {
            parsedDeptId = deptIdVal;
        }

        var employee = new Employee
        {
            EmployeeName = dto.EmployeeName,
            ContactNo = dto.ContactNo,
            EmailId = dto.EmailId,
            Password = dto.Password,
            Gender = dto.Gender,
            Role = string.IsNullOrWhiteSpace(dto.Role) ? "Employee" : dto.Role,
            DeptId = parsedDeptId,
            CreatedDate = DateTime.UtcNow
        };

        var created = await _employeeRepository.AddAsync(employee);
        return ApiResponse<EmployeeDto>.Success(MapToDto(created), "Employee created successfully");
    }

    public async Task<ApiResponse<EmployeeDto>> UpdateEmployeeAsync(UpdateEmployeeDto dto)
    {
        var employee = await _employeeRepository.GetByIdAsync(dto.EmployeeId);
        if (employee == null)
        {
            return ApiResponse<EmployeeDto>.Failure($"Employee with ID {dto.EmployeeId} not found");
        }

        int? parsedDeptId = null;
        if (int.TryParse(dto.DeptId, out var deptIdVal))
        {
            parsedDeptId = deptIdVal;
        }

        employee.EmployeeName = dto.EmployeeName;
        employee.ContactNo = dto.ContactNo;
        employee.EmailId = dto.EmailId;
        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            employee.Password = dto.Password;
        }
        employee.Gender = dto.Gender;
        employee.Role = dto.Role;
        employee.DeptId = parsedDeptId;
        employee.UpdatedDate = DateTime.UtcNow;

        await _employeeRepository.UpdateAsync(employee);
        return ApiResponse<EmployeeDto>.Success(MapToDto(employee), "Employee updated successfully");
    }

    public async Task<ApiResponse<bool>> DeleteEmployeeAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            return ApiResponse<bool>.Failure($"Employee with ID {id} not found", false);
        }

        await _employeeRepository.DeleteAsync(employee);
        return ApiResponse<bool>.Success(true, "Employee deleted successfully");
    }

    public async Task<ApiResponse<IEnumerable<EmployeeDto>>> SearchEmployeesAsync(string? searchTerm)
    {
        var employees = await _employeeRepository.SearchAsync(searchTerm);
        var dtos = employees.Select(MapToDto);
        return ApiResponse<IEnumerable<EmployeeDto>>.Success(dtos, "Search completed successfully");
    }

    private static EmployeeDto MapToDto(Employee employee)
    {
        return new EmployeeDto
        {
            EmployeeId = employee.EmployeeId,
            EmployeeName = employee.EmployeeName,
            ContactNo = employee.ContactNo,
            EmailId = employee.EmailId,
            DeptId = employee.DeptId?.ToString() ?? string.Empty,
            Password = employee.Password,
            Gender = employee.Gender,
            Role = employee.Role,
            CreatedDate = employee.CreatedDate
        };
    }
}
