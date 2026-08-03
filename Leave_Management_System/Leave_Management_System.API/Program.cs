using Hangfire;
using Leave_Management_System.API.ExceptionHandler;
using Leave_Management_System.Data.DbContexts;
using Leave_Management_System.Repository.Employees.IRepositories;
using Leave_Management_System.Repository.Employees.Repositories;
using Leave_Management_System.Service.Employees.IService;
using Leave_Management_System.Service.Employees.Service;
using Leave_Management_System.Service.Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/Leave_Management_System_API_Serilogs.txt", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Information()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// cors
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173")
                            .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

// --- DBConetexts ---
builder.Services.AddDbContext<ApplicationDbContextEFCore>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlServerEFcore")
    )
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(
        options => options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:JWT_Secret"]!))
        }
);

// Hangfire integration
builder.Services.AddHangfire((sp, config) =>
{
    var connectionString = builder.Configuration.GetConnectionString("Hangfire_DB");
    config.UseSqlServerStorage(connectionString);

});

builder.Services.AddHangfireServer();



builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ILeaveRequestService, LeaveRequestService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var recurringJobManager = scope.ServiceProvider
        .GetRequiredService<IRecurringJobManager>();

    recurringJobManager.AddOrUpdate<HangfireServices>(
    "leave-request-status-update",
    service => service.UpdateLeaveRequestStatus(),
    Cron.Daily()
);
}

app.UseHangfireDashboard();

app.Run();
