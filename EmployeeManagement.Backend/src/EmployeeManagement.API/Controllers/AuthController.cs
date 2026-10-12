using EmployeeManagement.Application.Common;
using EmployeeManagement.Application.DTOs.Auth;
using EmployeeManagement.Application.DTOs.Employee;
using EmployeeManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<EmployeeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);
        return Ok(result);
    }
}
