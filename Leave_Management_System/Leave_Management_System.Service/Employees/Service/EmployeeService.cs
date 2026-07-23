using Leave_Management_System.Models.DTO;
using Leave_Management_System.Service.Employees.IService;

namespace Leave_Management_System.Service.Employees.Service
{
    public class EmployeeService : IEmployeeService
    {
        public Task<IEnumerable<EmployeeResponseDTO>> GetEmployeeResponseDTOsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
