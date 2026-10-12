export interface IEmployeeList {
    employeeId: number;
    employeeName: string;
    contactNo: string | number;
    emailId?: string;
    emaiId?: string;
    deptId?: string | number;
    password?: string;
    gender?: string;
    role?: string;
    createdDate?: string | Date;
}