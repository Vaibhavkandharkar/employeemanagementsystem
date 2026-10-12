using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
