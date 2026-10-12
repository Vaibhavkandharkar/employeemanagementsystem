import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { EmployeeService } from '../../core/service/employee-service';
import { MasterService } from '../../core/service/master';
import { API_response, IChildDeptModel } from '../../core/model/User.model';

@Component({
  imports: [FormsModule],
  selector: 'app-profile',
  styleUrl: './profile.css',
  templateUrl: './profile.html',
})
export class Profile implements OnInit {
  employeeService = inject(EmployeeService);
  masterService = inject(MasterService);
  router = inject(Router);

  activeTab = signal<'personal' | 'security' | 'preferences'>('personal');
  isSaving = signal<boolean>(false);
  saveSuccess = signal<boolean>(false);

  userProfile = {
    employeeId: 1,
    employeeName: 'Admin User',
    contactNo: '9876543210',
    emailId: 'admin@company.com',
    deptId: '1',
    departmentName: 'General',
    role: 'Admin',
    gender: 'Male',
    createdDate: new Date().toISOString()
  };

  passwordForm = {
    currentPassword: '',
    newPassword: '',
    confirmPassword: ''
  };

  childDepts = signal<IChildDeptModel[]>([]);

  userInitials = computed(() => {
    const name = this.userProfile.employeeName || 'User';
    const parts = name.trim().split(/\s+/);
    if (parts.length > 1) {
      return `${parts[0][0]}${parts[parts.length - 1][0]}`.toUpperCase();
    }
    return name.slice(0, 2).toUpperCase();
  });

  ngOnInit(): void {
    this.loadUserProfile();
    this.loadDepartments();
  }

  loadUserProfile(): void {
    const saved = localStorage.getItem('empUser');
    if (saved) {
      try {
        const u = JSON.parse(saved);
        this.userProfile = {
          employeeId: u.employeeId ?? 1,
          employeeName: u.employeeName ?? u.name ?? 'Admin User',
          contactNo: u.contactNo ?? '9876543210',
          emailId: u.emailId ?? u.email ?? 'admin@company.com',
          deptId: u.deptId?.toString() ?? '1',
          departmentName: 'General',
          role: u.role ?? 'Admin',
          gender: u.gender ?? 'Male',
          createdDate: u.createdDate ?? new Date().toISOString()
        };
      } catch (e) {
        console.error('Error parsing user profile from local storage', e);
      }
    }
  }

  loadDepartments(): void {
    this.masterService.getChildDept().subscribe({
      next: (res: API_response) => {
        if (res.data) {
          this.childDepts.set(res.data);
          const dept = res.data.find((d: any) => d.childDepartmentId?.toString() === this.userProfile.deptId);
          if (dept) {
            this.userProfile.departmentName = dept.childDepartmentName;
          }
        }
      }
    });
  }

  onSaveProfile(): void {
    this.isSaving.set(true);
    this.saveSuccess.set(false);

    const updatePayload = {
      employeeId: this.userProfile.employeeId,
      employeeName: this.userProfile.employeeName,
      contactNo: this.userProfile.contactNo,
      emailId: this.userProfile.emailId,
      gender: this.userProfile.gender,
      role: this.userProfile.role,
      deptId: this.userProfile.deptId
    };

    this.employeeService.updateEmployee(this.userProfile.employeeId, updatePayload).subscribe({
      next: (res: API_response) => {
        this.isSaving.set(false);
        if (res.result) {
          this.saveSuccess.set(true);
          localStorage.setItem('empUser', JSON.stringify({ ...this.userProfile, ...res.data }));
          alert('Profile information updated successfully!');
        } else {
          alert(res.message || 'Update failed');
        }
      },
      error: (err) => {
        this.isSaving.set(false);
        console.error('Update profile error', err);
        alert('Could not update profile. Verify backend connection.');
      }
    });
  }

  onUpdatePassword(): void {
    if (!this.passwordForm.newPassword || this.passwordForm.newPassword.length < 4) {
      alert('New password must be at least 4 characters long.');
      return;
    }

    if (this.passwordForm.newPassword !== this.passwordForm.confirmPassword) {
      alert('New password and confirm password do not match.');
      return;
    }

    this.isSaving.set(true);
    const updatePayload = {
      employeeId: this.userProfile.employeeId,
      employeeName: this.userProfile.employeeName,
      contactNo: this.userProfile.contactNo,
      emailId: this.userProfile.emailId,
      password: this.passwordForm.newPassword,
      gender: this.userProfile.gender,
      role: this.userProfile.role,
      deptId: this.userProfile.deptId
    };

    this.employeeService.updateEmployee(this.userProfile.employeeId, updatePayload).subscribe({
      next: (res: API_response) => {
        this.isSaving.set(false);
        if (res.result) {
          alert('Password successfully updated! Please remember your new password.');
          this.passwordForm = { currentPassword: '', newPassword: '', confirmPassword: '' };
        } else {
          alert(res.message);
        }
      },
      error: (err) => {
        this.isSaving.set(false);
        console.error('Password change error', err);
        alert('Failed to update password.');
      }
    });
  }

  setTab(tab: 'personal' | 'security' | 'preferences'): void {
    this.activeTab.set(tab);
  }
}
