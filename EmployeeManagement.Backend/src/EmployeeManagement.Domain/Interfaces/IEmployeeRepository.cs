using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Domain.Interfaces;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<Employee?> GetByEmailAsync(string email);
    Task<Employee?> AuthenticateAsync(string emailOrUsername, string password);
    Task<IEnumerable<Employee>> SearchAsync(string? searchTerm);
}
