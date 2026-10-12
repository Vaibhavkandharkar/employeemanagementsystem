using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces;
using EmployeeManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationDbContext _context;

    public DepartmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ParentDepartment>> GetAllParentDepartmentsAsync()
    {
        return await _context.ParentDepartments
            .AsNoTracking()
            .Include(p => p.ChildDepartments)
            .ToListAsync();
    }

    public async Task<ParentDepartment?> GetParentDepartmentByIdAsync(int id)
    {
        return await _context.ParentDepartments
            .AsNoTracking()
            .Include(p => p.ChildDepartments)
            .FirstOrDefaultAsync(p => p.DepartmentId == id);
    }

    public async Task<ParentDepartment> AddParentDepartmentAsync(ParentDepartment department)
    {
        await _context.ParentDepartments.AddAsync(department);
        await _context.SaveChangesAsync();
        return department;
    }

    public async Task<IEnumerable<ChildDepartment>> GetAllChildDepartmentsAsync()
    {
        return await _context.ChildDepartments
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<ChildDepartment>> GetChildDepartmentsByParentIdAsync(int parentId)
    {
        return await _context.ChildDepartments
            .AsNoTracking()
            .Where(c => c.ParentDepartmentId == parentId)
            .ToListAsync();
    }

    public async Task<ChildDepartment?> GetChildDepartmentByIdAsync(int id)
    {
        return await _context.ChildDepartments
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ChildDepartmentId == id);
    }

    public async Task<ChildDepartment> AddChildDepartmentAsync(ChildDepartment department)
    {
        await _context.ChildDepartments.AddAsync(department);
        await _context.SaveChangesAsync();
        return department;
    }

    public async Task DeleteParentDepartmentAsync(int id)
    {
        var dept = await _context.ParentDepartments.FindAsync(id);
        if (dept != null)
        {
            _context.ParentDepartments.Remove(dept);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteChildDepartmentAsync(int id)
    {
        var dept = await _context.ChildDepartments.FindAsync(id);
        if (dept != null)
        {
            _context.ChildDepartments.Remove(dept);
            await _context.SaveChangesAsync();
        }
    }
}
