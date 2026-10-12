import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_response } from '../model/User.model';
import { Constant } from '../constant/Constant';
import { environment } from '../../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class EmployeeService {

    http = inject(HttpClient);

    getAllEmployee(): Observable<API_response> {
        return this.http.get<API_response>(environment.API_URL + Constant.APP_METHODS.GET_ALL_EMPLOYEE);
    }

    createEmployee(employeeObj: any): Observable<API_response> {
        return this.http.post<API_response>(environment.API_URL + 'api/Employee', employeeObj);
    }

    updateEmployee(id: number, employeeObj: any): Observable<API_response> {
        return this.http.put<API_response>(environment.API_URL + `api/Employee/${id}`, employeeObj);
    }

    deleteEmployee(id: number): Observable<API_response> {
        return this.http.delete<API_response>(environment.API_URL + `api/Employee/${id}`);
    }
}
