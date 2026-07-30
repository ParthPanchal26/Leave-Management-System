using Leave_Management_System.Models.Domain;

namespace Leave_Management_System.Repository.Employees.IRepositories
{
    public interface ILeaveRequestRepository
    {
        Task<LeaveRequest> CreateLeaveRequest(LeaveRequest leaveRequest);
        Task<LeaveRequest?> CheckExistingLeave(Guid id, DateTime startDate, DateTime endDate);
        Task<LeaveRequest?> GetLeaveRequestById(Guid id);
        Task<Holidays?> CheckHolidayInDates(DateTime startDate, DateTime endDate);
        Task<LeaveRequest> UpdateLeaveRequest(LeaveRequest leaveRequest);

        Task<LeaveType?> GetLeaveTypeInfo(int leaveTypeId);
        Task<IEnumerable<LeaveRequest>> GetLeavesByEmployeeIdLeaveTypeId(Guid employeeId, int leaveTypeId);
        //Task<LeaveBalance?> GetLeaveBalance(Guid employeeId, int leaveTypeId);
        //Task CreateLeaveBalance(LeaveBalance leaveBalance);
    }
}
