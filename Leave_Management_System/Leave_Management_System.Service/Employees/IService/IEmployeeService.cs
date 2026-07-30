using Leave_Management_System.Models;
using Leave_Management_System.Models.Domain;
using Leave_Management_System.Models.DTO;

namespace Leave_Management_System.Service.Employees.IService
{
    public interface IEmployeeService
    {
        //Task<IEnumerable<EmployeeResponseDTO>> GetEmployeeResponseDTOsAsync();
        Task<ServiceResult<IEnumerable<EmployeeResponseDTO>>> GetEmployeeResponseDTOsAsync();
        //Task<EmployeeResponseDTO?> CreateEmployeeAsync(EmployeeRequestDTO model);
        Task<ServiceResult<EmployeeResponseDTO>> CreateEmployeeAsync(EmployeeRequestDTO model);
        Task<ServiceResult<LoginResponseDTO>> LoginUser(LoginRequestDTO model);
        Task<ServiceResult<EmployeeResponseDTO>> UpdateEmployeeAsync(Guid id, EmployeeUpdateDTO model);
        Task<ServiceResult<bool?>> DeleteEmployeeByIdAsync(Guid id);
    }
}
