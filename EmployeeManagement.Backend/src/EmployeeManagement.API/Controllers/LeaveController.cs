using EmployeeManagement.Application.Common;
using EmployeeManagement.Application.DTOs.Leave;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaveController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public LeaveController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<LeaveRequestDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var list = await _context.LeaveRequests
            .AsNoTracking()
            .OrderByDescending(l => l.CreatedDate)
            .Select(l => MapToDto(l))
            .ToListAsync();

        return Ok(ApiResponse<IEnumerable<LeaveRequestDto>>.Success(list, "Leave requests retrieved successfully"));
    }

    [HttpGet("employee/{empId:int}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<LeaveRequestDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmployee(int empId)
    {
        var list = await _context.LeaveRequests
            .AsNoTracking()
            .Where(l => l.EmployeeId == empId)
            .OrderByDescending(l => l.CreatedDate)
            .Select(l => MapToDto(l))
            .ToListAsync();

        return Ok(ApiResponse<IEnumerable<LeaveRequestDto>>.Success(list, "Employee leaves retrieved successfully"));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<LeaveRequestDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ApplyLeave([FromBody] CreateLeaveDto dto)
    {
        string empName = dto.EmployeeName ?? string.Empty;
        if (string.IsNullOrWhiteSpace(empName))
        {
            var emp = await _context.Employees.FindAsync(dto.EmployeeId);
            empName = emp?.EmployeeName ?? $"Employee #{dto.EmployeeId}";
        }

        var days = Math.Max(1, (int)(dto.EndDate.Date - dto.StartDate.Date).TotalDays + 1);

        var leave = new LeaveRequest
        {
            EmployeeId = dto.EmployeeId,
            EmployeeName = empName,
            LeaveType = string.IsNullOrWhiteSpace(dto.LeaveType) ? "Casual Leave" : dto.LeaveType,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            DaysCount = days,
            Reason = dto.Reason,
            Status = "Pending",
            CreatedDate = DateTime.UtcNow
        };

        await _context.LeaveRequests.AddAsync(leave);
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<LeaveRequestDto>.Success(MapToDto(leave), "Leave request submitted successfully"));
    }

    [HttpPut("{id:int}/status")]
    [ProducesResponseType(typeof(ApiResponse<LeaveRequestDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateLeaveStatusDto dto)
    {
        var leave = await _context.LeaveRequests.FindAsync(id);
        if (leave == null)
        {
            return NotFound(ApiResponse.Failure($"Leave request #{id} not found"));
        }

        leave.Status = dto.Status;
        leave.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(ApiResponse<LeaveRequestDto>.Success(MapToDto(leave), $"Leave request {dto.Status.ToLower()} successfully"));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(int id)
    {
        var leave = await _context.LeaveRequests.FindAsync(id);
        if (leave == null)
        {
            return NotFound(ApiResponse.Failure($"Leave request #{id} not found"));
        }

        _context.LeaveRequests.Remove(leave);
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.Success(true, "Leave request removed successfully"));
    }

    private static LeaveRequestDto MapToDto(LeaveRequest l)
    {
        return new LeaveRequestDto
        {
            LeaveId = l.LeaveId,
            EmployeeId = l.EmployeeId,
            EmployeeName = l.EmployeeName,
            LeaveType = l.LeaveType,
            StartDate = l.StartDate,
            EndDate = l.EndDate,
            DaysCount = l.DaysCount,
            Reason = l.Reason,
            Status = l.Status,
            CreatedDate = l.CreatedDate
        };
    }
}
