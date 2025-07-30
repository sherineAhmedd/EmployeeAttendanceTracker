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
using EmployeeAttendanceTracker.DAL.Seeding;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("AttendanceDb");
});
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await AppDbInitializer.SeedAsync(dbContext);
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
