using Leave_Management_System.Data.DbContexts;
using Leave_Management_System.Models.Domain;
using Leave_Management_System.Models.Enum;
using Leave_Management_System.Repository.Employees.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Leave_Management_System.Repository.Employees.Repositories
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly ApplicationDbContextEFCore _applicationDbContextEFCore;

        public LeaveRequestRepository(ApplicationDbContextEFCore applicationDbContextEFCore)
        {
            _applicationDbContextEFCore = applicationDbContextEFCore;
        }

        public async Task<LeaveRequest> CreateLeaveRequest(LeaveRequest leaveRequest)
        {
            var leaveReq = await _applicationDbContextEFCore.LeaveRequests.AddAsync(leaveRequest);
            await _applicationDbContextEFCore.SaveChangesAsync();

            //return leaveReq.Entity;
            var leave = await GetLeaveRequestById(leaveReq.Entity.LeaveId);
            return leave!;
        }

        public async Task<LeaveRequest?> GetLeaveRequestById(Guid id)
        {

            return await _applicationDbContextEFCore.LeaveRequests.Include("Employee").Include("Reviewer").Include("LeaveType").FirstOrDefaultAsync(e => e.LeaveId.ToString() == id.ToString()); ;
        }

        public async Task<LeaveRequest?> CheckExistingLeave(Guid id, DateTime startDate, DateTime endDate)
        {
            return await _applicationDbContextEFCore.LeaveRequests.Include("Employee").Include("Reviewer").Include("LeaveType").FirstOrDefaultAsync(e => e.EmployeeId == id && e.StartDate <= endDate && e.EndDate >= startDate && e.LeaveStatus != Models.Enum.LeaveStatus.REJECTED);
        }

        public async Task<Holidays?> CheckHolidayInDates(DateTime startDate, DateTime endDate)
        {
            return await _applicationDbContextEFCore.Holidays.FirstOrDefaultAsync(e => e.HolidayDate >= startDate && e.HolidayDate <= endDate && e.IsOptional == false);
        }

        public async Task<LeaveRequest> UpdateLeaveRequest(LeaveRequest leaveRequest)
        {
            var result = _applicationDbContextEFCore.LeaveRequests.Update(leaveRequest);
            await _applicationDbContextEFCore.SaveChangesAsync();

            var leave = await GetLeaveRequestById(result.Entity.LeaveId);
            return leave!;
        }

        public async Task<LeaveType?> GetLeaveTypeInfo(int leaveTypeId)
        {
            return await _applicationDbContextEFCore.LeaveTypes.FindAsync(leaveTypeId);
        }

        public async Task<IEnumerable<LeaveRequest>> GetLeavesByEmployeeIdLeaveTypeId(Guid employeeId, int leaveTypeId)
        {
            return await _applicationDbContextEFCore.LeaveRequests.Where(l => l.EmployeeId == employeeId && l.LeaveTypeId == leaveTypeId && l.LeaveStatus == LeaveStatus.APPROVED || l.LeaveStatus == LeaveStatus.PENDING).ToListAsync();
        }

        //public async Task<LeaveBalance?> GetLeaveBalance(Guid employeeId, int leaveTypeId)
        //{
        //    return await _applicationDbContextEFCore.LeaveBalances.FirstOrDefaultAsync(b => b.EmployeeId == employeeId && b.LeaveTypeId == leaveTypeId);
        //}

        //public async Task CreateLeaveBalance(LeaveBalance leaveBalance)
        //{
        //    await _applicationDbContextEFCore.LeaveBalances.AddAsync(leaveBalance);
        //    await _applicationDbContextEFCore.SaveChangesAsync();
        //}

        public async Task<IEnumerable<LeaveRequest>> GetRejectableLeaveRequests()
        {
            return await _applicationDbContextEFCore.LeaveRequests.Where(l => l.LeaveStatus == LeaveStatus.PENDING && l.EndDate < DateTime.Now).ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetLeaveRequests()
        {
            return await _applicationDbContextEFCore.LeaveRequests.Include(lr => lr.Employee).Include(lr => lr.LeaveType).Include(lr => lr.Reviewer).ToListAsync();
        }
    }
}
