using Leave_Management_System.Models.Domain;

namespace Leave_Management_System.Repository.Employees.IRepositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetEmployeesAsync();
        Task<Employee?> CreateEmployeeModelAsync(Employee employee);
        Task<Employee?> GetEmployeeByEmailAsync(string email);
        Task<Employee?> GetEmployeeByIdAsync(Guid id);
        Task<Employee> UpdateEmployeeModelAsync(Employee employee);
        Task DeleteEmployeeAsync(Employee employee);
        Task<LeaveRequest> CreateLeaveRequest(LeaveRequest leaveRequest);
        Task<LeaveRequest?> CheckExistingLeave(Guid id, DateTime startDate, DateTime endDate);
        Task<Holidays?> CheckHolidayInDates(DateTime startDate, DateTime endDate);
    }
}
