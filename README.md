# Employee Attendance Tracker

A comprehensive Employee Attendance Management System built with ASP.NET MVC, Entity Framework Core, and In-Memory Database.

## 🏗️ Architecture

This project follows **N-Tier Architecture**:

- **Presentation Layer**: ASP.NET MVC (Views and Controllers)
- **Business Layer**: Services (All business logic and validation)
- **Data Layer**: Entity Framework Core with In-Memory Database

## 🚀 Features

### 1. Department Management
- ✅ Add, edit, delete, and list departments
- ✅ Department validation (Name: 3-50 chars, Code: 4 uppercase letters)
- ✅ Prevent duplicate department names and codes
- ✅ Display employee count per department

### 2. Employee Management
- ✅ Add, edit, delete, and list employees
- ✅ Employee validation (4 names, unique email, department selection)
- ✅ System-generated unique employee codes
- ✅ Display current month's attendance summary
- ✅ Pagination support

### 3. Attendance Management
- ✅ Record attendance (Present/Absent) for specific dates
- ✅ Prevent duplicate attendance per employee per day
- ✅ Prevent future date attendance marking
- ✅ Edit and delete attendance records
- ✅ Filter by department, employee, or date range
- ✅ Live filtering with jQuery

### 4. Dynamic UI Features
- ✅ Calendar widget for date selection
- ✅ Real-time attendance status updates
- ✅ Future date prevention in calendar
- ✅ Live status updates without page reload
- ✅ Quick attendance entry form

### 5. Bonus Features
- ✅ Pagination for employee list
- ✅ Live filtering for attendance list
- ✅ Partial views for employee details and attendance history
- ✅ Responsive Bootstrap UI

## 🛠️ Technical Stack

- **Framework**: ASP.NET Core MVC 8.0
- **Database**: Entity Framework Core In-Memory Database
- **Frontend**: Bootstrap 5, jQuery
- **Architecture**: N-Tier with Dependency Injection
- **Validation**: Data Annotations and Custom Business Rules

## 📋 Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or VS Code
- Any modern web browser

## 🚀 Setup Instructions

1. **Clone the repository**
   ```bash
   git clone [repository-url]
   cd EmployeeAttendanceTracker.API
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Access the application**
   - Open your browser and navigate to `https://localhost:5001` or `http://localhost:5000`
   - The application will automatically seed sample data

## 📊 Sample Data

The application comes with pre-seeded data:
- **Departments**: SWE, ITI, Finance
- **Employees**: 3 sample employees with different departments
- **Attendance**: Sample attendance records

## 🏛️ Project Structure

```
EmployeeAttendanceTracker.API/
├── EmployeeAttendanceTracker.API/          # Presentation Layer
│   ├── Controllers/                        # MVC Controllers
│   ├── Views/                             # Razor Views
│   └── wwwroot/                           # Static Files
├── EmployeeAttendanceTracker.BLL/          # Business Layer
│   ├── DTOs/                              # Data Transfer Objects
│   ├── Services/                          # Business Logic Services
│   ├── ServiceInterfaces/                 # Service Contracts
│   └── Enums/                             # Enumerations
└── EmployeeAttendanceTracker.DAL/          # Data Layer
    ├── Context/                           # DbContext
    ├── Data/Entities/                     # Entity Models
    ├── Interfaces/                        # Repository Contracts
    ├── Repositories/                      # Data Access Implementation
    └── Seeding/                           # Database Seeding
```

## 🔧 Key Components

### Controllers
- `DepartmentsController` - Department CRUD operations
- `EmployeesController` - Employee CRUD operations  
- `AttendancesController` - Attendance management with AJAX support

### Services
- `DepartmentService` - Department business logic
- `EmployeeService` - Employee business logic with attendance summary
- `AttendanceService` - Attendance business logic and validation

### Repositories
- `DepartmentRepository` - Department data access
- `EmployeeRepository` - Employee data access with unique code generation
- `AttendanceRepository` - Attendance data access with filtering

## 🎯 Business Rules

### Department Validation
- Name: Required, 3-50 characters, unique
- Code: Required, exactly 4 uppercase letters, unique
- Location: Required, max 100 characters

### Employee Validation
- Full Name: Required, exactly 4 names, each 2+ characters
- Email: Required, valid format, unique
- Department: Required, must exist
- Employee Code: Auto-generated, unique

### Attendance Validation
- One attendance per employee per day
- Cannot mark attendance for future dates
- Status must be Present or Absent

## 🎨 UI/UX Features

- **Responsive Design**: Bootstrap 5 for mobile-friendly interface
- **Dynamic Updates**: jQuery for real-time status updates
- **Live Filtering**: Instant filtering without page reload
- **Calendar Integration**: Date picker with future date prevention
- **Pagination**: Efficient data display for large datasets

## 🔒 Security Features

- CSRF protection on all forms
- Input validation and sanitization
- Business rule enforcement in service layer
- Dependency injection for loose coupling

## 🧪 Testing

The application is designed with testability in mind:
- Service layer separation for unit testing
- Interface-based design for mocking
- Dependency injection for test isolation

## 📝 API Endpoints

### Departments
- `GET /Departments` - List departments
- `GET /Departments/Create` - Create form
- `POST /Departments/Create` - Create department
- `GET /Departments/Edit/{id}` - Edit form
- `POST /Departments/Edit/{id}` - Update department
- `GET /Departments/Delete/{id}` - Delete confirmation
- `POST /Departments/Delete/{id}` - Delete department
- `GET /Departments/GetForDropdown` - AJAX dropdown data

### Employees
- `GET /Employees` - List employees (with pagination)
- `GET /Employees/Create` - Create form
- `POST /Employees/Create` - Create employee
- `GET /Employees/Edit/{id}` - Edit form
- `POST /Employees/Edit/{id}` - Update employee
- `GET /Employees/Delete/{id}` - Delete confirmation
- `POST /Employees/Delete/{id}` - Delete employee
- `GET /Employees/GetForDropdown` - AJAX dropdown data

### Attendances
- `GET /Attendances` - List attendances (with filtering)
- `GET /Attendances/Create` - Create form
- `POST /Attendances/Create` - Create attendance
- `POST /Attendances/CreateAjax` - AJAX create attendance
- `GET /Attendances/GetStatus` - Get attendance status
- `GET /Attendances/Edit/{id}` - Edit form
- `POST /Attendances/Edit/{id}` - Update attendance
- `GET /Attendances/Delete/{id}` - Delete confirmation
- `POST /Attendances/Delete/{id}` - Delete attendance

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

This project is created for CodeZone LLC task submission.

## 📞 Contact

For any inquiries, please contact: career@codezone-eg.com

---

**Developed with ❤️ using ASP.NET Core MVC**