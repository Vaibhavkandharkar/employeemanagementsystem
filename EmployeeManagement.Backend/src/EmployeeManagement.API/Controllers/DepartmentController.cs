using EmployeeManagement.Application.Common;
using EmployeeManagement.Application.DTOs.Department;
using EmployeeManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet("parent")]
    [HttpGet("parent-departments")]
    [HttpGet("/GETALLPARENTDEPT")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ParentDepartmentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetParentDepartments()
    {
        var result = await _departmentService.GetAllParentDepartmentsAsync();
        return Ok(result);
    }

    [HttpGet("child")]
    [HttpGet("child-departments")]
    [HttpGet("/GET_CHILD_DEPT")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ChildDepartmentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetChildDepartments()
    {
        var result = await _departmentService.GetAllChildDepartmentsAsync();
        return Ok(result);
    }

    [HttpGet("child-by-parent/{parentId:int}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ChildDepartmentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetChildDepartmentsByParent(int parentId)
    {
        var result = await _departmentService.GetChildDepartmentsByParentIdAsync(parentId);
        return Ok(result);
    }

    [HttpPost("parent")]
    [ProducesResponseType(typeof(ApiResponse<ParentDepartmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateParentDepartment([FromBody] ParentDepartmentDto dto)
    {
        var result = await _departmentService.CreateParentDepartmentAsync(dto);
        return Ok(result);
    }

    [HttpPost("child")]
    [ProducesResponseType(typeof(ApiResponse<ChildDepartmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateChildDepartment([FromBody] ChildDepartmentDto dto)
    {
        var result = await _departmentService.CreateChildDepartmentAsync(dto);
        return Ok(result);
    }

    [HttpDelete("parent/{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteParentDepartment(int id)
    {
        var result = await _departmentService.DeleteParentDepartmentAsync(id);
        return Ok(result);
    }

    [HttpDelete("child/{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteChildDepartment(int id)
    {
        var result = await _departmentService.DeleteChildDepartmentAsync(id);
        return Ok(result);
    }
}
