# Employee Management System (EMS)

Full-stack Enterprise Employee Management System built with **Angular** frontend and **ASP.NET Core Web API (Clean Architecture)** backend connected to **MySQL**.

## 🚀 Tech Stack
- **Frontend**: Angular 17+, TypeScript, CSS3, HTML5
- **Backend**: ASP.NET Core 8 Web API, Entity Framework Core, Pomelo MySQL Provider
- **Database**: MySQL (phpMyAdmin)
- **Architecture**: Clean Architecture (Domain, Application, Infrastructure, API)

## 📌 Features
- **Employee Directory**: Search, filter, view, and delete employee records.
- **Add Employee**: Full validation form linked to live department data.
- **Executive Dashboard**: Workforce headcount, department metrics, and new hire feed.
- **Department & Team Manager**: Manage parent divisions and child functional teams.
- **Leaves & Attendance**: Apply for leaves, manage entitlements, and approve/reject requests.
- **Reporting & Export**: One-click CSV export and workforce summary analytics.
- **User Profile & Settings**: Personal details overview and password management.

## 🛠️ Getting Started

### 1. Database Setup
1. Import `EmployeeManagement.Backend/schema.sql` into MySQL / phpMyAdmin database named `employeedb`.
2. Update connection string in `EmployeeManagement.Backend/src/EmployeeManagement.API/appsettings.json` if needed.

### 2. Backend
```bash
cd EmployeeManagement.Backend
dotnet run --project src/EmployeeManagement.API
```
Runs at `http://localhost:5126`.

### 3. Frontend
```bash
cd employee_app
npm install
ng serve
```
Runs at `http://localhost:4200`.
