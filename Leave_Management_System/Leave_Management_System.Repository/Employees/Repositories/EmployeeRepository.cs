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
            return await _applicationDbContextEFCore.Employees.Include(e => e.Manager).Include(e => e.Employees).Include(e => e.Role).Include(e => e.Department).ToListAsync();
        }

        public async Task<Employee?> CreateEmployeeModelAsync(Employee employee)
        {
            var employee_db = await _applicationDbContextEFCore.Employees.AddAsync(employee);
            await _applicationDbContextEFCore.SaveChangesAsync();

            var employeeResponse = await _applicationDbContextEFCore.Employees.Include(e => e.Manager).Include(e => e.Employees).Include(e => e.Role).Include(e => e.Department).FirstOrDefaultAsync(e => e.Email == employee.Email);
            return employeeResponse;
        }
    }
}
