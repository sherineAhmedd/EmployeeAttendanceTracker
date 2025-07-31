# Employee Attendance Tracker

A comprehensive Employee Attendance Management System built with ASP.NET MVC, Entity Framework Core, and an in-memory database.

## 🏗️ Architecture

This project follows **N-Tier Architecture** principles:

### **Presentation Layer**
- **ASP.NET MVC**: Views and Controllers
- **Controllers**: Handle HTTP requests/responses only
- **Views**: Razor views with jQuery for dynamic interactions

### **Business Layer**
- **Services**: All business logic and validation
- **Interfaces**: IAttendanceService, IEmployeeService, IDepartmentService
- **DTOs**: Data Transfer Objects for clean data flow

### **Data Layer**
- **Entity Framework Core**: Code-First approach
- **In-Memory Database**: For development and testing
- **Repositories**: Data access abstraction

## 🚀 Features

### **Department Management**
- ✅ Add, edit, delete, and list departments
- ✅ Validation: Name (3-50 chars, unique), Code (4 uppercase letters, unique), Location (max 100 chars)
- ✅ Display employee count per department

### **Employee Management**
- ✅ Add, edit, delete, and list employees
- ✅ Employee Code: Auto-generated, unique, non-editable
- ✅ Full Name: Four names, each ≥2 characters, letters/spaces only
- ✅ Email: Required, unique, valid format
- ✅ Department: Dropdown selection
- ✅ Current month attendance summary with percentage

### **Attendance Management**
- ✅ Record attendance (Present/Absent) for specific dates
- ✅ One attendance per employee per day
- ✅ No future date attendance
- ✅ Edit and delete attendance records
- ✅ Filter by department, employee, or date range

### **Dynamic UI Features**
- ✅ Interactive calendar widget for date selection
- ✅ Live status updates without page reload
- ✅ Future dates disabled in calendar
- ✅ Color-coded status (green for Present, red for Absent)
- ✅ jQuery-powered dynamic interactions

### **Bonus Features**
- ✅ Pagination for employee and attendance lists
- ✅ Live filtering with jQuery
- ✅ Partial views for attendance history
- ✅ Modern, responsive UI design

## 🛠️ Setup Instructions

### **Prerequisites**
- .NET 6.0 or later
- Visual Studio 2022 or VS Code

### **Installation**
1. **Clone the repository**
   ```bash
   git clone [your-repository-url]
   cd EmployeeAttendanceTracker.API
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

4. **Access the application**
   - Open browser and navigate to `https://localhost:7000` or `http://localhost:5000`
   - The application will automatically seed sample data

### **Database**
- **Type**: In-Memory Database (Entity Framework Core)
- **Seeding**: Automatic sample data on startup
- **Sample Data**: 5 departments, 10 employees, 20 attendance records

## 📁 Project Structure

```
EmployeeAttendanceTracker.API/
├── Controllers/          # MVC Controllers (HTTP handling only)
├── Views/               # Razor Views
├── wwwroot/             # Static files (CSS, JS, images)
├── Models/              # View Models
└── Program.cs           # Application startup and DI configuration

EmployeeAttendanceTracker.BLL/
├── Services/            # Business logic implementation
├── ServiceInterfaces/   # Service contracts
├── DTOs/               # Data Transfer Objects
└── Enums/              # Enumerations

EmployeeAttendanceTracker.DAL/
├── Context/            # Entity Framework DbContext
├── Data/Entities/      # Entity models
├── Interfaces/         # Repository contracts
├── Repositories/       # Data access implementation
└── Seeding/           # Database seeding logic
```

## 🎯 Key Features

### **Home Dashboard**
- Navigation cards for quick access to all modules
- Quick action buttons for common tasks

### **Quick Attendance Entry**
- Real-time status checking
- Interactive calendar widget
- Color-coded status indicators

### **Advanced Filtering**
- Live filtering with jQuery
- Date range selection
- Department and employee filters

### **Responsive Design**
- Bootstrap-based UI
- Mobile-friendly interface
- Modern animations and transitions

## 🔧 Technical Implementation

### **Validation Rules**
- All validation implemented in service layer
- No business logic in controllers
- Comprehensive error handling

### **Security**
- Anti-forgery tokens for forms
- Input validation and sanitization
- Proper model binding

### **Performance**
- Efficient database queries
- Pagination for large datasets
- Optimized AJAX calls

## 📝 Development Notes

- **Architecture**: Strict N-Tier separation
- **Testing**: Ready for unit testing implementation
- **Scalability**: Easy to switch to SQL Server or other databases
- **Maintainability**: Clean code with proper separation of concerns

## 🤝 Contributing

This project follows clean coding practices and n-tier architecture principles. All business logic must be implemented in the service layer.

## 📧 Contact

For any inquiries about this project, please contact the development team.

---

**Built with ❤️ using ASP.NET Core MVC and Entity Framework Core**