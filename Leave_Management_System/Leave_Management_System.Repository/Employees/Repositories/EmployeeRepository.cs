using Leave_Management_System.Data.DbContexts;
using Leave_Management_System.Models.Domain;
using Leave_Management_System.Repository.Employees.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace Leave_Management_System.Repository.Employees.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContextEFCore _applicationDbContextEFCore;

        public EmployeeRepository(ApplicationDbContextEFCore applicationDbContextEFCore)
        {
            _applicationDbContextEFCore = applicationDbContextEFCore;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            return await _applicationDbContextEFCore.Employees.Include(e => e.Manager).Include(e => e.Employees).Include(e => e.Role).Include(e => e.Department).Where(e => e.IsActive == true).ToListAsync();
        }

        public async Task<Employee?> CreateEmployeeModelAsync(Employee employee)
        {
            var employee_db = await _applicationDbContextEFCore.Employees.AddAsync(employee);
            await _applicationDbContextEFCore.SaveChangesAsync();

            var employeeResponse = await _applicationDbContextEFCore.Employees.Include(e => e.Manager).Include(e => e.Employees).Include(e => e.Role).Include(e => e.Department).FirstOrDefaultAsync(e => e.Email == employee.Email);
            return employeeResponse;
        }

        public async Task<LeaveRequest> CreateLeaveRequest(LeaveRequest leaveRequest)
        {
            var leaveReq = await _applicationDbContextEFCore.LeaveRequests.AddAsync(leaveRequest);
            await _applicationDbContextEFCore.SaveChangesAsync();

            var leave = await GetLeaveRequestById(leaveReq.Entity.LeaveId);
            return leave;
        }

        public async Task<LeaveRequest?> GetLeaveRequestById(Guid id)
        {
            return await _applicationDbContextEFCore.LeaveRequests.Include("Employee").Include("Approver").Include("LeaveType").FirstOrDefaultAsync(e => e.LeaveId == id);
        }

        public async Task<LeaveRequest?> CheckExistingLeave(Guid id, DateTime startDate, DateTime endDate)
        {
            return await _applicationDbContextEFCore.LeaveRequests.Include("Employee").Include("Approver").Include("LeaveType").FirstOrDefaultAsync(e => e.EmployeeId == id && e.StartDate <= endDate && e.EndDate >= startDate);
        }

        public async Task<Holidays?> CheckHolidayInDates(DateTime startDate, DateTime endDate)
        {
            return await _applicationDbContextEFCore.Holidays.FirstOrDefaultAsync(e => e.HolidayDate >= startDate && e.HolidayDate <= endDate);
        }

        public async Task<Employee?> GetEmployeeByEmailAsync(string email)
        {
            var employee = await _applicationDbContextEFCore.Employees.Include(e => e.Manager).Include(e => e.Employees).Include(e => e.Role).Include(e => e.Department).FirstOrDefaultAsync(e => e.Email.ToLower() == email.ToLower());
            return employee;
        }

        public async Task<Employee?> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _applicationDbContextEFCore.Employees.Include(e => e.Manager).Include(e => e.Employees).Include(e => e.Role).Include(e => e.Department).FirstOrDefaultAsync(e => e.EmployeeId == id);
            return employee;
        }

        public async Task<Employee> UpdateEmployeeModelAsync(Employee employee)
        {
            _applicationDbContextEFCore.Employees.Update(employee);
            await _applicationDbContextEFCore.SaveChangesAsync();

            return await GetEmployeeByEmailAsync(employee.Email);
        }

        public async Task DeleteEmployeeAsync(Employee employee)
        {
            _applicationDbContextEFCore.Employees.Update(employee);
            await _applicationDbContextEFCore.SaveChangesAsync();
        }
    }
}
