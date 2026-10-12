import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { EmployeeService } from '../../core/service/employee-service';
import { IEmployeeList } from '../../core/model/Employee.model';
import { API_response } from '../../core/model/User.model';

@Component({
  imports: [DatePipe, RouterLink, FormsModule],
  selector: 'app-employee-list',
  styleUrl: './employee-list.css',
  templateUrl: './employee-list.html',
})
export class EmployeeList implements OnInit {

  empSrv = inject(EmployeeService);
  empList = signal<IEmployeeList[]>([]);
  searchTerm = signal<string>('');

  filteredEmpList = computed(() => {
    const term = this.searchTerm().toLowerCase().trim();
    const all = this.empList();
    if (!term) return all;
    return all.filter(e =>
      (e.employeeName && e.employeeName.toLowerCase().includes(term)) ||
      (e.emailId && e.emailId.toLowerCase().includes(term)) ||
      (e.emaiId && e.emaiId.toLowerCase().includes(term)) ||
      (e.contactNo && e.contactNo.toString().includes(term)) ||
      (e.role && e.role.toLowerCase().includes(term))
    );
  });

  ngOnInit(): void {
    this.loadEmployee();
  }

  loadEmployee() {
    this.empSrv.getAllEmployee().subscribe({
      next: (res: API_response) => {
        this.empList.set(res.data || []);
      },
      error: (err: any) => {
        console.error('Failed to load employees', err);
      }
    });
  }

  onSearchChange(val: string) {
    this.searchTerm.set(val);
  }

  exportToCsv() {
    const data = this.filteredEmpList();
    if (!data || data.length === 0) {
      alert('No employee data available to export.');
      return;
    }

    const headers = ['Employee ID', 'Employee Name', 'Email', 'Contact No', 'Department ID', 'Role', 'Gender', 'Created Date'];
    const rows = data.map(e => [
      e.employeeId,
      `"${(e.employeeName || '').replace(/"/g, '""')}"`,
      `"${e.emailId || e.emaiId || ''}"`,
      `"${e.contactNo || ''}"`,
      `"${e.deptId || ''}"`,
      `"${e.role || ''}"`,
      `"${e.gender || ''}"`,
      `"${e.createdDate || ''}"`
    ]);

    const csvContent = 'data:text/csv;charset=utf-8,\uFEFF' + [headers.join(','), ...rows.map(r => r.join(','))].join('\n');
    const encodedUri = encodeURI(csvContent);
    const link = document.createElement('a');
    link.setAttribute('href', encodedUri);
    link.setAttribute('download', `employees_report_${new Date().toISOString().slice(0, 10)}.csv`);
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }

  onDelete(id: number, name: string) {
    if (confirm(`Are you sure you want to delete employee "${name}"?`)) {
      this.empSrv.deleteEmployee(id).subscribe({
        next: (res: API_response) => {
          if (res.result) {
            alert('Employee deleted successfully.');
            this.loadEmployee();
          } else {
            alert(res.message || 'Failed to delete employee');
          }
        },
        error: (err) => {
          console.error('Delete error', err);
          alert('Error deleting employee');
        }
      });
    }
  }
} 
