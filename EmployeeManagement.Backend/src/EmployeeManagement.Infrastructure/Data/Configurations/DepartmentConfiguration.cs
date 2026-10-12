using EmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagement.Infrastructure.Data.Configurations;

public class ParentDepartmentConfiguration : IEntityTypeConfiguration<ParentDepartment>
{
    public void Configure(EntityTypeBuilder<ParentDepartment> builder)
    {
        builder.ToTable("ParentDepartments");

        builder.HasKey(p => p.DepartmentId);
        builder.Property(p => p.DepartmentId).ValueGeneratedOnAdd();

        builder.Property(p => p.DepartmentName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.DepartmentLogo)
            .HasMaxLength(500);

        builder.HasMany(p => p.ChildDepartments)
            .WithOne(c => c.ParentDepartment)
            .HasForeignKey(c => c.ParentDepartmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ChildDepartmentConfiguration : IEntityTypeConfiguration<ChildDepartment>
{
    public void Configure(EntityTypeBuilder<ChildDepartment> builder)
    {
        builder.ToTable("ChildDepartments");

        builder.HasKey(c => c.ChildDepartmentId);
        builder.Property(c => c.ChildDepartmentId).ValueGeneratedOnAdd();

        builder.Property(c => c.ChildDepartmentName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(c => c.ChildDepartmentLogo)
            .HasMaxLength(500);
    }
}
