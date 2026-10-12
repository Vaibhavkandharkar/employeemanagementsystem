using EmployeeManagement.Application.Common;
using EmployeeManagement.Application.DTOs.Department;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Interfaces;

namespace EmployeeManagement.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<ApiResponse<IEnumerable<ParentDepartmentDto>>> GetAllParentDepartmentsAsync()
    {
        var departments = await _departmentRepository.GetAllParentDepartmentsAsync();
        var dtos = departments.Select(d => new ParentDepartmentDto
        {
            DepartmentId = d.DepartmentId,
            DepartmentName = d.DepartmentName,
            DepartmentLogo = d.DepartmentLogo
        });

        return ApiResponse<IEnumerable<ParentDepartmentDto>>.Success(dtos, "Parent departments retrieved successfully");
    }

    public async Task<ApiResponse<IEnumerable<ChildDepartmentDto>>> GetAllChildDepartmentsAsync()
    {
        var departments = await _departmentRepository.GetAllChildDepartmentsAsync();
        var dtos = departments.Select(d => new ChildDepartmentDto
        {
            ChildDepartmentId = d.ChildDepartmentId,
            ChildDepartmentName = d.ChildDepartmentName,
            ChildDepartmentLogo = d.ChildDepartmentLogo,
            ParentDepartmentId = d.ParentDepartmentId
        });

        return ApiResponse<IEnumerable<ChildDepartmentDto>>.Success(dtos, "Child departments retrieved successfully");
    }

    public async Task<ApiResponse<IEnumerable<ChildDepartmentDto>>> GetChildDepartmentsByParentIdAsync(int parentId)
    {
        var departments = await _departmentRepository.GetChildDepartmentsByParentIdAsync(parentId);
        var dtos = departments.Select(d => new ChildDepartmentDto
        {
            ChildDepartmentId = d.ChildDepartmentId,
            ChildDepartmentName = d.ChildDepartmentName,
            ChildDepartmentLogo = d.ChildDepartmentLogo,
            ParentDepartmentId = d.ParentDepartmentId
        });

        return ApiResponse<IEnumerable<ChildDepartmentDto>>.Success(dtos, "Child departments for parent retrieved successfully");
    }

    public async Task<ApiResponse<ParentDepartmentDto>> CreateParentDepartmentAsync(ParentDepartmentDto dto)
    {
        var parent = new EmployeeManagement.Domain.Entities.ParentDepartment
        {
            DepartmentName = dto.DepartmentName,
            DepartmentLogo = string.IsNullOrWhiteSpace(dto.DepartmentLogo) 
                ? (dto.DepartementLogo ?? "https://images.unsplash.com/photo-1551836022-d5d88e9218df?w=100&auto=format&fit=crop&q=60") 
                : dto.DepartmentLogo,
            CreatedDate = DateTime.UtcNow
        };

        var created = await _departmentRepository.AddParentDepartmentAsync(parent);
        var createdDto = new ParentDepartmentDto
        {
            DepartmentId = created.DepartmentId,
            DepartmentName = created.DepartmentName,
            DepartmentLogo = created.DepartmentLogo
        };

        return ApiResponse<ParentDepartmentDto>.Success(createdDto, "Parent department created successfully");
    }

    public async Task<ApiResponse<ChildDepartmentDto>> CreateChildDepartmentAsync(ChildDepartmentDto dto)
    {
        var child = new EmployeeManagement.Domain.Entities.ChildDepartment
        {
            ChildDepartmentName = dto.ChildDepartmentName,
            ChildDepartmentLogo = string.IsNullOrWhiteSpace(dto.ChildDepartmentLogo) 
                ? (dto.ChildDepartementLogo ?? "https://images.unsplash.com/photo-1498050108023-c5249f4df085?w=100&auto=format&fit=crop&q=60") 
                : dto.ChildDepartmentLogo,
            ParentDepartmentId = dto.ParentDepartmentId,
            CreatedDate = DateTime.UtcNow
        };

        var created = await _departmentRepository.AddChildDepartmentAsync(child);
        var createdDto = new ChildDepartmentDto
        {
            ChildDepartmentId = created.ChildDepartmentId,
            ChildDepartmentName = created.ChildDepartmentName,
            ChildDepartmentLogo = created.ChildDepartmentLogo,
            ParentDepartmentId = created.ParentDepartmentId
        };

        return ApiResponse<ChildDepartmentDto>.Success(createdDto, "Child department created successfully");
    }

    public async Task<ApiResponse<bool>> DeleteParentDepartmentAsync(int id)
    {
        var parent = await _departmentRepository.GetParentDepartmentByIdAsync(id);
        if (parent == null)
        {
            return ApiResponse<bool>.Failure($"Parent Department with ID {id} not found.", false);
        }
        
        var childDepts = await _departmentRepository.GetChildDepartmentsByParentIdAsync(id);
        if (childDepts != null && childDepts.Any())
        {
            return ApiResponse<bool>.Failure("Cannot delete parent department because it has child departments assigned to it.", false);
        }

        await _departmentRepository.DeleteParentDepartmentAsync(id);
        return ApiResponse<bool>.Success(true, "Parent department removed successfully");
    }

    public async Task<ApiResponse<bool>> DeleteChildDepartmentAsync(int id)
    {
        await _departmentRepository.DeleteChildDepartmentAsync(id);
        return ApiResponse<bool>.Success(true, "Child department removed successfully");
    }
}
