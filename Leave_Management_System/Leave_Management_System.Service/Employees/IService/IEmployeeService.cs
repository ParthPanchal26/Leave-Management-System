using Leave_Management_System.Models.DTO;

namespace Leave_Management_System.Service.Employees.IService
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeResponseDTO>> GetEmployeeResponseDTOsAsync();
    }
}
