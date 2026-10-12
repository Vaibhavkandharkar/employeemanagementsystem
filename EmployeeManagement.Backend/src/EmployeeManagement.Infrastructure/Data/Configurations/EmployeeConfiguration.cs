using EmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagement.Infrastructure.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(e => e.EmployeeId);
        builder.Property(e => e.EmployeeId).ValueGeneratedOnAdd();

        builder.Property(e => e.EmployeeName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(e => e.ContactNo)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.EmailId)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasIndex(e => e.EmailId).IsUnique();

        builder.Property(e => e.Password)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.Gender)
            .HasMaxLength(20);

        builder.Property(e => e.Role)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DeptId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
