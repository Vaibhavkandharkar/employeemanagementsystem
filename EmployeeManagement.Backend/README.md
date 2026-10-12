# Employee Management System - .NET Backend

Enterprise-level Clean Architecture (Onion Architecture) REST API built with **.NET 8**, **Entity Framework Core**, and **MySQL**. Designed to integrate directly with the Angular `employee_app`.

---

## 🏛️ Architecture Breakdown

The solution follows Clean Architecture separation of concerns:

```
EmployeeManagement.Backend/
├── EmployeeManagement.slnx
├── schema.sql                               # Raw MySQL DDL + seed data script
└── src/
    ├── EmployeeManagement.Domain/           # 1. Core Domain Layer (Entities & Interfaces)
    │   ├── Entities/                        # BaseEntity, Employee, ParentDepartment, ChildDepartment
    │   └── Interfaces/                      # IRepository, IEmployeeRepository, IDepartmentRepository
    │
    ├── EmployeeManagement.Application/      # 2. Application Layer (Business Logic & Use Cases)
    │   ├── Common/                          # ApiResponse<T> (Angular API_response format)
    │   ├── DTOs/                            # Auth, Employee, Department DTOs
    │   ├── Interfaces/                      # IEmployeeService, IDepartmentService, IAuthService
    │   └── Services/                        # Business logic implementations
    │
    ├── EmployeeManagement.Infrastructure/   # 3. Infrastructure Layer (EF Core & MySQL)
    │   ├── Data/                            # ApplicationDbContext, EF Configurations, DbInitializer
    │   └── Repositories/                    # Repository<T>, EmployeeRepository, DepartmentRepository
    │
    └── EmployeeManagement.API/              # 4. Presentation Web API Layer
        ├── Controllers/                     # AuthController, EmployeeController, DepartmentController
        ├── Middlewares/                     # GlobalExceptionHandlerMiddleware
        ├── Properties/                      # launchSettings.json
        ├── appsettings.json                 # MySQL connection string & CORS config
        └── Program.cs                       # Swagger, CORS policy, DI composition
```

---

## 🔌 API Endpoints

### 1. Authentication (`/api/auth`)
| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/auth/login` | Authenticate user (also supports `/login` and `/api/employee/login`) |

### 2. Employees (`/api/employee`)
| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/employee` | Get all employees (also mapped to `/GETALLEMPLOYEE`) |
| `GET` | `/api/employee/{id}` | Get employee by ID |
| `GET` | `/api/employee/search?term=...` | Search employees by name, email, contact, or role |
| `POST` | `/api/employee` | Create a new employee |
| `PUT` | `/api/employee/{id}` | Update existing employee |
| `DELETE` | `/api/employee/{id}` | Delete employee |

### 3. Departments (`/api/department`)
| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/department/parent` | Get all parent departments (also `/GETALLPARENTDEPT`) |
| `GET` | `/api/department/child` | Get all child departments (also `/GET_CHILD_DEPT`) |
| `GET` | `/api/department/child-by-parent/{parentId}` | Get child departments for a specific parent |

---

## ⚙️ Configuration & Running

### 1. MySQL Connection String
In [appsettings.json](file:///c:/Users/Dell/OneDrive/Desktop/empmansys/EmployeeManagement.Backend/src/EmployeeManagement.API/appsettings.json):
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=employeedb;User=root;Password=root;TreatTinyAsBoolean=true;"
}
```
*Update `User` and `Password` to match your local MySQL credentials.*

### 2. Database Creation
You can initialize the database in either of two ways:
1. **Automatic**: When you run the API, `DbInitializer.SeedAsync` automatically creates tables and seeds sample data (`admin@company.com` / `admin`).
2. **Manual SQL**: Execute [schema.sql](file:///c:/Users/Dell/OneDrive/Desktop/empmansys/EmployeeManagement.Backend/schema.sql) in MySQL Workbench, phpMyAdmin, or MySQL CLI.

### 3. Start the Backend
```bash
dotnet run --project src/EmployeeManagement.API
```
- Swagger UI will be available at: **`http://localhost:5126/`** (or `https://localhost:7054/`)

---

## 🅰️ Connecting Angular (`employee_app`)
In your Angular `src/environments/environment.development.ts`:
```ts
export const environment = {
  API_URL: "http://localhost:5126/"
};
```
CORS is pre-configured to allow requests from `http://localhost:4200`.
