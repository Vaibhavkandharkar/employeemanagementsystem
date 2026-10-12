using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces;
using EmployeeManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Repositories;

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Employee?> GetByEmailAsync(string email)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.EmailId == email);
    }

    public async Task<Employee?> AuthenticateAsync(string emailOrUsername, string password)
    {
        return await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(e =>
                (e.EmailId == emailOrUsername || e.EmployeeName == emailOrUsername) &&
                e.Password == password);
    }

    public async Task<IEnumerable<Employee>> SearchAsync(string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return await GetAllAsync();
        }

        var term = searchTerm.Trim().ToLower();
        return await _dbSet.AsNoTracking()
            .Where(e => e.EmployeeName.ToLower().Contains(term) ||
                        e.EmailId.ToLower().Contains(term) ||
                        e.ContactNo.Contains(term) ||
                        e.Role.ToLower().Contains(term))
            .ToListAsync();
    }
}
