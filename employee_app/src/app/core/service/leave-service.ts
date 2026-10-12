import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_response } from '../model/User.model';
import { environment } from '../../../environments/environment';

export interface ILeaveRequest {
  leaveId: number;
  employeeId: number;
  employeeName: string;
  leaveType: string;
  startDate: string;
  endDate: string;
  daysCount: number;
  reason: string;
  status: 'Pending' | 'Approved' | 'Rejected';
  createdDate: string;
}

@Injectable({
  providedIn: 'root'
})
export class LeaveService {
  http = inject(HttpClient);

  getAllLeaves(): Observable<API_response> {
    return this.http.get<API_response>(environment.API_URL + 'api/Leave');
  }

  applyLeave(leaveObj: any): Observable<API_response> {
    return this.http.post<API_response>(environment.API_URL + 'api/Leave', leaveObj);
  }

  updateLeaveStatus(leaveId: number, status: string): Observable<API_response> {
    return this.http.put<API_response>(environment.API_URL + `api/Leave/${leaveId}/status`, { status });
  }

  deleteLeave(leaveId: number): Observable<API_response> {
    return this.http.delete<API_response>(environment.API_URL + `api/Leave/${leaveId}`);
  }
}
