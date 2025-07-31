using EmployeeAttendanceTracker.DAL.Context;
using EmployeeAttendanceTracker.DAL.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAttendanceTracker.DAL.Seeding
{
    public static class AppDbInitializer
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Always ensure we have the required departments
            var requiredDepartments = new List<Department>
            {
                new Department { DepartmentName = "Information Technology", DepartmentCode = "TECH", Location = "Building A, Floor 3" },
                new Department { DepartmentName = "Human Resources", DepartmentCode = "HRMG", Location = "Building B, Floor 2" },
                new Department { DepartmentName = "Finance", DepartmentCode = "FINC", Location = "Building A, Floor 4" },
                new Department { DepartmentName = "Marketing", DepartmentCode = "MKTG", Location = "Building C, Floor 1" },
                new Department { DepartmentName = "Operations", DepartmentCode = "OPSR", Location = "Building B, Floor 3" }
            };

            // Check if departments exist, if not add them
            foreach (var dept in requiredDepartments)
            {
                if (!await context.Departments.AnyAsync(d => d.DepartmentCode == dept.DepartmentCode))
                {
                    context.Departments.Add(dept);
                }
            }
            await context.SaveChangesAsync();

            // Get all departments (including existing ones)
            var departments = await context.Departments.ToListAsync();

            // Seed Employees if they don't exist
            if (!await context.Employees.AnyAsync())
            {
                var employees = new List<Employee>
                {
                    new Employee { FullName = "Ahmed Mohamed Ali Hassan", Email = "ahmed.mohamed@company.com", EmployeeCode = 1001, DepartmentId = departments[0].DepartmentId },
                    new Employee { FullName = "Fatima Zahra Omar Khalil", Email = "fatima.zahra@company.com", EmployeeCode = 1002, DepartmentId = departments[0].DepartmentId },
                    new Employee { FullName = "Omar Ibrahim Saleh Ahmed", Email = "omar.ibrahim@company.com", EmployeeCode = 1003, DepartmentId = departments[1].DepartmentId },
                    new Employee { FullName = "Aisha Hassan Ali Mohamed", Email = "aisha.hassan@company.com", EmployeeCode = 1004, DepartmentId = departments[1].DepartmentId },
                    new Employee { FullName = "Khalid Abdullah Omar Hassan", Email = "khalid.abdullah@company.com", EmployeeCode = 1005, DepartmentId = departments[2].DepartmentId },
                    new Employee { FullName = "MAriam Ali Amr Hassan", Email = "MariamAmr@company.com", EmployeeCode = 1006, DepartmentId = departments[2].DepartmentId },
                   
                };

                await context.Employees.AddRangeAsync(employees);
                await context.SaveChangesAsync();
            }

            // Seed Attendance records if they don't exist
            if (!await context.Attendances.AnyAsync())
            {
                var employees = await context.Employees.ToListAsync();
                var today = DateTime.Today;
                var random = new Random();

                var attendances = new List<Attendance>();

                // Create random attendance records (not for every employee every day)
                for (int i = 0; i < 6; i++) // Only 20 random records
                {
                    var randomEmployee = employees[random.Next(employees.Count)];
                    var randomDate = today.AddDays(-random.Next(0, 10)); // Random date in last 10 days
                    var status = random.Next(2) == 0 ? "Present" : "Absent";
                    
                    // Check if attendance already exists for this employee on this date
                    var existingAttendance = attendances.FirstOrDefault(a => 
                        a.EmployeeId == randomEmployee.EmployeeId && 
                        a.Date.Date == randomDate.Date);
                    
                    if (existingAttendance == null)
                    {
                        attendances.Add(new Attendance
                        {
                            EmployeeId = randomEmployee.EmployeeId,
                            Date = randomDate,
                            Status = status
                        });
                    }
                }

                await context.Attendances.AddRangeAsync(attendances);
                await context.SaveChangesAsync();
            }
        }
    }
}
