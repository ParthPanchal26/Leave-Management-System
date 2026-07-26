using Leave_Management_System.Data.DbContexts;
using Leave_Management_System.Repository.Employees.IRepositories;
using Leave_Management_System.Repository.Employees.Repositories;
using Leave_Management_System.Service.Employees.IService;
using Leave_Management_System.Service.Employees.Service;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// --- DBConetexts ---
builder.Services.AddDbContext<ApplicationDbContextEFCore>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlServerEFcore")
    )
);

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
