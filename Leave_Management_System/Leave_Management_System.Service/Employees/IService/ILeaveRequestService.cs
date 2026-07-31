using Leave_Management_System.Models;
using Leave_Management_System.Models.Domain;
using Leave_Management_System.Models.DTO;

namespace Leave_Management_System.Service.Employees.IService
{
    public interface ILeaveRequestService
    {
        Task<ServiceResult<EmployeeLeaveResponseDTO>> CreateLeaveRequestAsync(EmployeeLeaveRequestDTO model);
        Task<ServiceResult<EmployeeLeaveResponseDTO>> UpdateLeaveRequestAsync(Guid leaveId, EmployeeLeaveRequestUpdateDTO model);

        Task<ServiceResult<IEnumerable<LeaveRequest>>> GetAllLeaveRequestsService();
    }
}
