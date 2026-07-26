using Leave_Management_System.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leave_Management_System.Repository.Employees.IRepositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetEmployeesAsync();
        Task<Employee?> CreateEmployeeModelAsync(Employee employee);
    }
}
