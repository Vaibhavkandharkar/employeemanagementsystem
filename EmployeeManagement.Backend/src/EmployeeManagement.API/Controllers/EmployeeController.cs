using EmployeeManagement.Application.Common;
using EmployeeManagement.Application.DTOs.Auth;
using EmployeeManagement.Application.DTOs.Employee;
using EmployeeManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IAuthService _authService;

    public EmployeeController(IEmployeeService employeeService, IAuthService authService)
    {
        _employeeService = employeeService;
        _authService = authService;
    }

    [HttpGet]
    [HttpGet("GETALLEMPLOYEE")]
    [HttpGet("/GETALLEMPLOYEE")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmployeeDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _employeeService.GetAllEmployeesAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<EmployeeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _employeeService.GetEmployeeByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmployeeDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] string? term)
    {
        var result = await _employeeService.SearchEmployeesAsync(term);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<EmployeeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse.Failure("Invalid employee data submitted"));
        }

        var result = await _employeeService.CreateEmployeeAsync(dto);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<EmployeeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeDto dto)
    {
        if (id != dto.EmployeeId)
        {
            return BadRequest(ApiResponse.Failure("Employee ID mismatch"));
        }

        var result = await _employeeService.UpdateEmployeeAsync(dto);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _employeeService.DeleteEmployeeAsync(id);
        return Ok(result);
    }

    [HttpPost("login")]
    [HttpPost("/login")]
    [ProducesResponseType(typeof(ApiResponse<EmployeeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);
        return Ok(result);
    }

    [HttpGet("export/csv")]
    public async Task<IActionResult> ExportCsv()
    {
        var result = await _employeeService.GetAllEmployeesAsync();
        var employees = result.Data ?? Enumerable.Empty<EmployeeDto>();

        var builder = new System.Text.StringBuilder();
        // Header
        builder.AppendLine("Employee ID,Employee Name,Email Address,Contact Number,Department ID,Role,Gender,Created Date");

        foreach (var emp in employees)
        {
            builder.AppendLine($"\"{emp.EmployeeId}\",\"{emp.EmployeeName.Replace("\"", "\"\"")}\",\"{emp.EmailId}\",\"{emp.ContactNo}\",\"{emp.DeptId}\",\"{emp.Role}\",\"{emp.Gender}\",\"{emp.CreatedDate:yyyy-MM-dd HH:mm:ss}\"");
        }

        var bytes = System.Text.Encoding.UTF8.GetPreamble().Concat(System.Text.Encoding.UTF8.GetBytes(builder.ToString())).ToArray();
        var fileName = $"employees_report_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";

        return File(bytes, "text/csv", fileName);
    }

    [HttpGet("report/summary")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReportSummary()
    {
        var result = await _employeeService.GetAllEmployeesAsync();
        var employees = (result.Data ?? Enumerable.Empty<EmployeeDto>()).ToList();

        var summary = new
        {
            totalEmployees = employees.Count,
            roleDistribution = employees.GroupBy(e => string.IsNullOrWhiteSpace(e.Role) ? "Unassigned" : e.Role)
                                        .ToDictionary(g => g.Key, g => g.Count()),
            genderDistribution = employees.GroupBy(e => string.IsNullOrWhiteSpace(e.Gender) ? "Other" : e.Gender)
                                          .ToDictionary(g => g.Key, g => g.Count()),
            generatedAt = DateTime.UtcNow
        };

        return Ok(ApiResponse<object>.Success(summary, "Report summary generated successfully"));
    }
}
