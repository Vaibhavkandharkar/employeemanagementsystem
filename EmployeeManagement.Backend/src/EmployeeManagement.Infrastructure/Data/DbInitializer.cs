using EmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context, ILogger logger)
    {
        try
        {
            await context.Database.EnsureCreatedAsync();

            if (!await context.ParentDepartments.AnyAsync())
            {
                logger.LogInformation("Seeding parent and child departments...");

                var hr = new ParentDepartment
                {
                    DepartmentName = "Human Resources",
                    DepartmentLogo = "https://images.unsplash.com/photo-1551836022-d5d88e9218df?w=100&auto=format&fit=crop&q=60"
                };

                var eng = new ParentDepartment
                {
                    DepartmentName = "Engineering & Technology",
                    DepartmentLogo = "https://images.unsplash.com/photo-1581091226825-a6a2a5aee158?w=100&auto=format&fit=crop&q=60"
                };

                var finance = new ParentDepartment
                {
                    DepartmentName = "Finance & Accounting",
                    DepartmentLogo = "https://images.unsplash.com/photo-1554224155-8d04cb21cd6c?w=100&auto=format&fit=crop&q=60"
                };

                await context.ParentDepartments.AddRangeAsync(hr, eng, finance);
                await context.SaveChangesAsync();

                var childDepts = new List<ChildDepartment>
                {
                    new()
                    {
                        ChildDepartmentName = "Talent Acquisition",
                        ChildDepartmentLogo = "https://images.unsplash.com/photo-1521737604893-d14cc237f11d?w=100&auto=format&fit=crop&q=60",
                        ParentDepartmentId = hr.DepartmentId
                    },
                    new()
                    {
                        ChildDepartmentName = "Employee Relations",
                        ChildDepartmentLogo = "https://images.unsplash.com/photo-1573496359142-b8d87734a5a2?w=100&auto=format&fit=crop&q=60",
                        ParentDepartmentId = hr.DepartmentId
                    },
                    new()
                    {
                        ChildDepartmentName = "Software Development",
                        ChildDepartmentLogo = "https://images.unsplash.com/photo-1498050108023-c5249f4df085?w=100&auto=format&fit=crop&q=60",
                        ParentDepartmentId = eng.DepartmentId
                    },
                    new()
                    {
                        ChildDepartmentName = "Quality Assurance & DevOps",
                        ChildDepartmentLogo = "https://images.unsplash.com/photo-1518770660439-4636190af475?w=100&auto=format&fit=crop&q=60",
                        ParentDepartmentId = eng.DepartmentId
                    },
                    new()
                    {
                        ChildDepartmentName = "Payroll & Taxation",
                        ChildDepartmentLogo = "https://images.unsplash.com/photo-1450133064473-71024230f91b?w=100&auto=format&fit=crop&q=60",
                        ParentDepartmentId = finance.DepartmentId
                    }
                };

                await context.ChildDepartments.AddRangeAsync(childDepts);
                await context.SaveChangesAsync();
            }

            if (!await context.Employees.AnyAsync())
            {
                logger.LogInformation("Seeding default employees...");

                var defaultChildDept = await context.ChildDepartments.FirstOrDefaultAsync();

                var employees = new List<Employee>
                {
                    new()
                    {
                        EmployeeName = "Admin User",
                        EmailId = "admin@company.com",
                        ContactNo = "9876543210",
                        Password = "admin",
                        Gender = "Male",
                        Role = "Admin",
                        DeptId = defaultChildDept?.ChildDepartmentId,
                        CreatedDate = DateTime.UtcNow
                    },
                    new()
                    {
                        EmployeeName = "Raj Bosh",
                        EmailId = "raj.bosh@company.com",
                        ContactNo = "9123456789",
                        Password = "password123",
                        Gender = "Male",
                        Role = "Employee",
                        DeptId = defaultChildDept?.ChildDepartmentId,
                        CreatedDate = DateTime.UtcNow
                    }
                };

                await context.Employees.AddRangeAsync(employees);
                await context.SaveChangesAsync();
            }

            if (!await context.LeaveRequests.AnyAsync())
            {
                logger.LogInformation("Seeding sample leave requests...");

                var admin = await context.Employees.FirstOrDefaultAsync(e => e.EmailId == "admin@company.com");
                var raj = await context.Employees.FirstOrDefaultAsync(e => e.EmailId == "raj.bosh@company.com");

                var sampleLeaves = new List<LeaveRequest>
                {
                    new()
                    {
                        EmployeeId = raj?.EmployeeId ?? 2,
                        EmployeeName = raj?.EmployeeName ?? "Raj Bosh",
                        LeaveType = "Casual Leave",
                        StartDate = DateTime.UtcNow.AddDays(2),
                        EndDate = DateTime.UtcNow.AddDays(3),
                        DaysCount = 2,
                        Reason = "Family function in hometown",
                        Status = "Pending",
                        CreatedDate = DateTime.UtcNow
                    },
                    new()
                    {
                        EmployeeId = admin?.EmployeeId ?? 1,
                        EmployeeName = admin?.EmployeeName ?? "Admin User",
                        LeaveType = "Work From Home",
                        StartDate = DateTime.UtcNow.AddDays(-5),
                        EndDate = DateTime.UtcNow.AddDays(-4),
                        DaysCount = 2,
                        Reason = "Internet installation at home",
                        Status = "Approved",
                        CreatedDate = DateTime.UtcNow.AddDays(-6)
                    }
                };

                await context.LeaveRequests.AddRangeAsync(sampleLeaves);
                await context.SaveChangesAsync();
            }

            logger.LogInformation("Database initialization completed.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database: {Message}", ex.Message);
        }
    }
}
