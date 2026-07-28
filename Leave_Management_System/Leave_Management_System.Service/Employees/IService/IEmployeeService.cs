using Leave_Management_System.Models.DTO;

namespace Leave_Management_System.Service.Employees.IService
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeResponseDTO>> GetEmployeeResponseDTOsAsync();
        Task<EmployeeResponseDTO?> CreateEmployeeAsync(EmployeeRequestDTO model);
        Task<LoginResponseDTO?> LoginUser(LoginRequestDTO model);
        Task<EmployeeResponseDTO?> UpdateEmployeeAsync(Guid id, EmployeeUpdateDTO model);
        Task<bool?> DeleteEmployeeByIdAsync(Guid id);
    }
}
