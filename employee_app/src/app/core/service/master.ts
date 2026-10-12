import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_response } from '../model/User.model';
import { Constant } from '../constant/Constant';
import { environment } from '../../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class MasterService {

    http = inject(HttpClient);
    
    getAllParentDept(): Observable<API_response> {
        return this.http.get<API_response>(environment.API_URL + Constant.APP_METHODS.GET_PARENT_DEPT);
    }

    getAllParenDept(): Observable<API_response> {
        return this.getAllParentDept();
    }

    getChildDept(): Observable<API_response> {
        return this.http.get<API_response>(environment.API_URL + Constant.APP_METHODS.GET_CHILD_DEPT);
    }

    createParentDept(deptObj: any): Observable<API_response> {
        return this.http.post<API_response>(environment.API_URL + 'api/Department/parent', deptObj);
    }

    createChildDept(deptObj: any): Observable<API_response> {
        return this.http.post<API_response>(environment.API_URL + 'api/Department/child', deptObj);
    }

    deleteParentDept(id: number): Observable<API_response> {
        return this.http.delete<API_response>(environment.API_URL + `api/Department/parent/${id}`);
    }

    deleteChildDept(id: number): Observable<API_response> {
        return this.http.delete<API_response>(environment.API_URL + `api/Department/child/${id}`);
    }
}

export { MasterService as Master };
