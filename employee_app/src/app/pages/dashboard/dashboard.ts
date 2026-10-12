import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { EmployeeService } from '../../core/service/employee-service';
import { MasterService } from '../../core/service/master';
import { IEmployeeList } from '../../core/model/Employee.model';
import { IParentDeptModel, IChildDeptModel, API_response } from '../../core/model/User.model';

@Component({
  imports: [DatePipe, RouterLink],
  selector: 'app-dashboard',
  styleUrl: './dashboard.css',
  templateUrl: './dashboard.html',
})
export class Dashboard implements OnInit {
  employeeService = inject(EmployeeService);
  masterService = inject(MasterService);

  employees = signal<IEmployeeList[]>([]);
  parentDepts = signal<IParentDeptModel[]>([]);
  childDepts = signal<IChildDeptModel[]>([]);
  isLoading = signal<boolean>(true);

  // Computed metrics
  totalEmployees = computed(() => this.employees().length);
  totalParentDepts = computed(() => this.parentDepts().length);
  totalChildDepts = computed(() => this.childDepts().length);

  adminCount = computed(() => 
    this.employees().filter(e => (e.role || '').toLowerCase() === 'admin').length
  );

  hrCount = computed(() => 
    this.employees().filter(e => (e.role || '').toLowerCase() === 'hr').length
  );

  staffCount = computed(() => 
    this.employees().filter(e => {
      const r = (e.role || '').toLowerCase();
      return r !== 'admin' && r !== 'hr';
    }).length
  );

  maleCount = computed(() => 
    this.employees().filter(e => (e.gender || '').toLowerCase() === 'male').length
  );

  femaleCount = computed(() => 
    this.employees().filter(e => (e.gender || '').toLowerCase() === 'female').length
  );

  // Recent 5 employees
  recentEmployees = computed(() => {
    return [...this.employees()]
      .reverse()
      .slice(0, 5);
  });

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.isLoading.set(true);

    // Fetch employees
    this.employeeService.getAllEmployee().subscribe({
      next: (res: API_response) => {
        this.employees.set(res.data || []);
        this.isLoading.set(false);
      },
      error: () => this.isLoading.set(false)
    });

    // Fetch parent departments
    this.masterService.getAllParentDept().subscribe({
      next: (res: API_response) => {
        this.parentDepts.set(res.data || []);
      }
    });

    // Fetch child departments
    this.masterService.getChildDept().subscribe({
      next: (res: API_response) => {
        this.childDepts.set(res.data || []);
      }
    });
  }

  getDepartmentName(deptId: any): string {
    if (!deptId) return 'General';
    const dept = this.childDepts().find(d => d.childDepartmentId.toString() === deptId.toString());
    return dept ? dept.childDepartmentName : `Dept #${deptId}`;
  }
}
