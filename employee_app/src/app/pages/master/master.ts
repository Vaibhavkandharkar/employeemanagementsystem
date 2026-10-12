import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { API_response, IChildDeptModel, IParentDeptModel } from '../../core/model/User.model';
import { MasterService } from '../../core/service/master';

@Component({
  imports: [FormsModule],
  selector: 'app-master',
  styleUrl: './master.css',
  templateUrl: './master.html',
})
export class Master implements OnInit {
  masterSr = inject(MasterService);
  parentDeptList = signal<IParentDeptModel[]>([]);
  originalChildList: IChildDeptModel[] = [];
  childDeptList = signal<IChildDeptModel[]>([]);
  selectedParentDeptId = signal<number | null>(null);

  isAddParentOpen = signal<boolean>(false);
  isAddChildOpen = signal<boolean>(false);
  isSaving = signal<boolean>(false);

  parentForm = {
    departmentName: '',
    departmentLogo: ''
  };

  childForm = {
    childDepartmentName: '',
    childDepartmentLogo: '',
    parentDepartmentId: 0
  };

  ngOnInit(): void {
    this.getParentDept();
    this.getChildDept();
  }

  getParentDept() {
    this.masterSr.getAllParentDept().subscribe({
      next: (res: API_response) => {
        this.parentDeptList.set(res.data || []);
        if (res.data && res.data.length > 0 && !this.childForm.parentDepartmentId) {
          this.childForm.parentDepartmentId = res.data[0].departmentId;
        }
      },
      error: (err: any) => {
        console.error('Failed to get parent departments', err);
      }
    });
  }

  getChildDept() {
    this.masterSr.getChildDept().subscribe({
      next: (res: API_response) => {
        this.originalChildList = res.data || [];
        if (this.selectedParentDeptId()) {
          this.filterSelectedDept(this.selectedParentDeptId());
        } else {
          this.childDeptList.set(res.data || []);
        }
      },
      error: (err: any) => {
        console.error('Failed to get child departments', err);
      }
    });
  }

  filterSelectedDept(pdId: number | null) {
    if (this.selectedParentDeptId() === pdId && pdId !== null) {
      // Toggle off filter if clicked again
      this.selectedParentDeptId.set(null);
      this.childDeptList.set(this.originalChildList);
      return;
    }

    this.selectedParentDeptId.set(pdId);
    if (pdId === null) {
      this.childDeptList.set(this.originalChildList);
    } else {
      const childDept = this.originalChildList.filter(m => m.parentDepartmentId === pdId);
      this.childDeptList.set(childDept);
    }
  }

  openAddParent() {
    this.parentForm = { departmentName: '', departmentLogo: '' };
    this.isAddParentOpen.set(true);
  }

  closeAddParent() {
    this.isAddParentOpen.set(false);
  }

  openAddChild(parentId?: number) {
    this.childForm = {
      childDepartmentName: '',
      childDepartmentLogo: '',
      parentDepartmentId: parentId || this.selectedParentDeptId() || (this.parentDeptList()[0]?.departmentId ?? 1)
    };
    this.isAddChildOpen.set(true);
  }

  closeAddChild() {
    this.isAddChildOpen.set(false);
  }

  onSaveParentDept() {
    if (!this.parentForm.departmentName.trim()) {
      alert('Department name is required');
      return;
    }

    this.isSaving.set(true);
    this.masterSr.createParentDept(this.parentForm).subscribe({
      next: (res: API_response) => {
        this.isSaving.set(false);
        if (res.result) {
          alert('Parent department created successfully!');
          this.closeAddParent();
          this.getParentDept();
        } else {
          alert(res.message);
        }
      },
      error: (err) => {
        this.isSaving.set(false);
        console.error('Failed to create parent department', err);
        alert('Error creating parent department.');
      }
    });
  }

  onSaveChildDept() {
    if (!this.childForm.childDepartmentName.trim()) {
      alert('Child department name is required');
      return;
    }

    this.isSaving.set(true);
    this.masterSr.createChildDept(this.childForm).subscribe({
      next: (res: API_response) => {
        this.isSaving.set(false);
        if (res.result) {
          alert('Child department created successfully!');
          this.closeAddChild();
          this.getChildDept();
        } else {
          alert(res.message);
        }
      },
      error: (err) => {
        this.isSaving.set(false);
        console.error('Failed to create child department', err);
        alert('Error creating child department.');
      }
    });
  }

  onDeleteParent(id: number, name: string) {
    if (confirm(`Are you sure you want to delete "${name}"? Child departments under this will also be deleted.`)) {
      this.masterSr.deleteParentDept(id).subscribe({
        next: (res: API_response) => {
          if (res.result) {
            this.getParentDept();
            this.getChildDept();
          }
        }
      });
    }
  }

  onDeleteChild(id: number, name: string) {
    if (confirm(`Are you sure you want to delete child department "${name}"?`)) {
      this.masterSr.deleteChildDept(id).subscribe({
        next: (res: API_response) => {
          if (res.result) {
            this.getChildDept();
          }
        }
      });
    }
  }
}
