using EmployeeAttendanceTracker.DAL.Context;
using Microsoft.EntityFrameworkCore;
using EmployeeAttendanceTracker.DAL.Repositories;
using EmployeeAttendanceTracker.DAL.Interfaces;
using EmployeeAttendanceTracker.BLL.Services;
using EmployeeAttendanceTracker.BLL.ServiceInterfaces;
using Microsoft.AspNetCore.Builder; // Required for WebApplication
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EmployeeAttendanceTracker.DAL.Data.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("AttendanceDb");
});
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
//builder.Services.AddScoped<>();

var app = builder.Build();


// ✅ SEED DEPARTMENT DATA HERE
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!dbContext.Departments.Any())
    {
        dbContext.Departments.AddRange(
            new Department { DepartmentName = "HR", DepartmentCode = "HRXX", Location = "Cairo" },
            new Department { DepartmentName = "IT", DepartmentCode = "ITTT", Location = "Alexandria" },
            new Department { DepartmentName = "Finance", DepartmentCode = "FINA", Location = "Giza" }
        );
        dbContext.Employees.AddRange(
            new Employee
            {
                FullName = "Shereen Ahmed Ahmed Ahmed",
                DepartmentId = 1,
                Email = "ShereenAhmed@gmail.com"
            },

            new Employee
            {
                FullName = "Farah Ahmed Ahmed Ahmed",
                DepartmentId = 1,
                Email = "FarahAhmed@gmail.com"
            },
            new Employee
            {
                FullName = " Ahmed Ahmed Ahmed",
                DepartmentId = 2,
                Email = "Ahmed@gmail.com"
            }
            );

        dbContext.SaveChanges();
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
