export interface UserModel {
     employeeId: number;   
     employeeName: string;
     contactNo: number;
     emaiId: string;
     deptId: string;
     password: string;
     gender: string;
     role: string;
     createdDate: Date;
}

export interface API_response {
     message: string;
     result: boolean;
     data: any;
}

export interface IParentDeptModel {
     departmentId: number;
     departmentName: string;
     departementName?: string;
     departmentLogo?: string;
     departementLogo?: string;
}

export interface IChildDeptModel {
     childDepartmentId: number;
     childDepartmentName: string;
     childDepartmentLogo?: string;
     childDepartementLogo?: string;
     parentDepartmentId: number;
}