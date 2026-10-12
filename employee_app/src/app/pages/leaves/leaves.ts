import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LeaveService, ILeaveRequest } from '../../core/service/leave-service';
import { EmployeeService } from '../../core/service/employee-service';
import { IEmployeeList } from '../../core/model/Employee.model';
import { API_response } from '../../core/model/User.model';

@Component({
  imports: [DatePipe, FormsModule],
  selector: 'app-leaves',
  styleUrl: './leaves.css',
  templateUrl: './leaves.html',
})
export class Leaves implements OnInit {
  leaveService = inject(LeaveService);
  employeeService = inject(EmployeeService);

  leaves = signal<ILeaveRequest[]>([]);
  employees = signal<IEmployeeList[]>([]);
  activeFilter = signal<'ALL' | 'Pending' | 'Approved' | 'Rejected'>('ALL');
  isModalOpen = signal<boolean>(false);
  isSubmitting = signal<boolean>(false);

  leaveFormObj = {
    employeeId: 0,
    employeeName: '',
    leaveType: 'Casual Leave',
    startDate: '',
    endDate: '',
    reason: ''
  };

  pendingCount = computed(() => this.leaves().filter(l => l.status === 'Pending').length);
  approvedCount = computed(() => this.leaves().filter(l => l.status === 'Approved').length);
  rejectedCount = computed(() => this.leaves().filter(l => l.status === 'Rejected').length);
  
  totalDaysApproved = computed(() => 
    this.leaves()
      .filter(l => l.status === 'Approved')
      .reduce((sum, l) => sum + (l.daysCount || 1), 0)
  );

  filteredLeaves = computed(() => {
    const filter = this.activeFilter();
    if (filter === 'ALL') return this.leaves();
    return this.leaves().filter(l => l.status === filter);
  });

  ngOnInit(): void {
    this.loadLeaves();
    this.loadEmployees();

    const today = new Date().toISOString().slice(0, 10);
    this.leaveFormObj.startDate = today;
    this.leaveFormObj.endDate = today;
  }

  loadLeaves() {
    this.leaveService.getAllLeaves().subscribe({
      next: (res: API_response) => {
        this.leaves.set(res.data || []);
      },
      error: (err) => console.error('Error fetching leaves', err)
    });
  }

  loadEmployees() {
    this.employeeService.getAllEmployee().subscribe({
      next: (res: API_response) => {
        if (res.data && res.data.length > 0) {
          this.employees.set(res.data);
          this.leaveFormObj.employeeId = res.data[0].employeeId;
          this.leaveFormObj.employeeName = res.data[0].employeeName;
        }
      }
    });
  }

  openModal() {
    this.isModalOpen.set(true);
  }

  closeModal() {
    this.isModalOpen.set(false);
  }

  onEmployeeSelect(empId: any) {
    const id = parseInt(empId, 10);
    const emp = this.employees().find(e => e.employeeId === id);
    if (emp) {
      this.leaveFormObj.employeeId = emp.employeeId;
      this.leaveFormObj.employeeName = emp.employeeName;
    }
  }

  onSubmitLeave() {
    if (!this.leaveFormObj.employeeId || !this.leaveFormObj.startDate || !this.leaveFormObj.endDate) {
      alert('Please fill in employee and leave dates.');
      return;
    }

    this.isSubmitting.set(true);
    this.leaveService.applyLeave(this.leaveFormObj).subscribe({
      next: (res: API_response) => {
        this.isSubmitting.set(false);
        if (res.result) {
          alert('Leave application submitted successfully!');
          this.closeModal();
          this.loadLeaves();
        } else {
          alert(res.message || 'Failed to submit leave.');
        }
      },
      error: (err) => {
        this.isSubmitting.set(false);
        console.error('Leave submission error', err);
        alert('Error submitting leave application.');
      }
    });
  }

  onStatusChange(leaveId: number, status: 'Approved' | 'Rejected') {
    this.leaveService.updateLeaveStatus(leaveId, status).subscribe({
      next: (res: API_response) => {
        if (res.result) {
          this.loadLeaves();
        } else {
          alert(res.message);
        }
      },
      error: (err) => console.error('Status update failed', err)
    });
  }

  onDelete(leaveId: number) {
    if (confirm('Are you sure you want to delete this leave record?')) {
      this.leaveService.deleteLeave(leaveId).subscribe({
        next: (res: API_response) => {
          if (res.result) {
            this.loadLeaves();
          }
        }
      });
    }
  }

  setFilter(f: 'ALL' | 'Pending' | 'Approved' | 'Rejected') {
    this.activeFilter.set(f);
  }
}
