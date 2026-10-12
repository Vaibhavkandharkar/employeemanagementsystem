import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { EmployeeService } from '../../core/service/employee-service';
import { MasterService } from '../../core/service/master';
import { IChildDeptModel, API_response } from '../../core/model/User.model';

@Component({
  imports: [FormsModule, RouterLink],
  selector: 'app-employee-form',
  styleUrl: './employee-form.css',
  templateUrl: './employee-form.html',
})
export class EmployeeForm implements OnInit {
  employeeService = inject(EmployeeService);
  masterService = inject(MasterService);
  router = inject(Router);

  childDeptList = signal<IChildDeptModel[]>([]);
  isSubmitting = signal<boolean>(false);

  employeeObj = {
    employeeName: '',
    contactNo: '',
    emailId: '',
    password: '',
    gender: 'Male',
    role: 'Employee',
    deptId: ''
  };

  ngOnInit(): void {
    this.loadDepartments();
  }

  loadDepartments() {
    this.masterService.getChildDept().subscribe({
      next: (res: API_response) => {
        if (res.data) {
          this.childDeptList.set(res.data);
          if (res.data.length > 0 && !this.employeeObj.deptId) {
            this.employeeObj.deptId = res.data[0].childDepartmentId.toString();
          }
        }
      },
      error: (err) => {
        console.error('Error loading departments', err);
      }
    });
  }

  onSave() {
    if (!this.employeeObj.employeeName || !this.employeeObj.emailId || !this.employeeObj.contactNo || !this.employeeObj.password) {
      alert('Please fill in all required fields (Name, Email, Contact No, Password).');
      return;
    }

    this.isSubmitting.set(true);
    this.employeeService.createEmployee(this.employeeObj).subscribe({
      next: (res: API_response) => {
        this.isSubmitting.set(false);
        if (res.result) {
          alert(res.message || 'Employee created successfully!');
          this.router.navigateByUrl('/emp-list');
        } else {
          alert(res.message || 'Failed to create employee');
        }
      },
      error: (err: any) => {
        this.isSubmitting.set(false);
        console.error('Create employee error:', err);
        alert(err.error?.message || 'Error occurred while saving employee. Check backend connection.');
      }
    });
  }

  onReset() {
    this.employeeObj = {
      employeeName: '',
      contactNo: '',
      emailId: '',
      password: '',
      gender: 'Male',
      role: 'Employee',
      deptId: this.childDeptList()[0]?.childDepartmentId?.toString() || ''
    };
  }
}
