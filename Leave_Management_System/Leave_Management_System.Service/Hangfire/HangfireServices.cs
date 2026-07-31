//using Hangfire;
using Leave_Management_System.Repository.Employees.IRepositories;

namespace Leave_Management_System.Service.Hangfire
{
    public class HangfireServices
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        public HangfireServices(ILeaveRequestRepository leaveRequestRepository)
        {
            _leaveRequestRepository = leaveRequestRepository;
        }

        // ------------------------------------
        // === Moved to application startup ===
        // ------------------------------------
        //public static void ScheduleRecurringLeaveStatusUpdate()
        //{
        //    Task.Run(
        //        async () =>
        //        {
        //            RecurringJob.AddOrUpdate("leave-request-status-update", async () => await UpdateLeaveRequestStatus(), Cron.Daily());
        //        });
        //}

        public async Task UpdateLeaveRequestStatus()
        {
            var rejectableLeaveRequests = await _leaveRequestRepository.GetRejectableLeaveRequests();

            foreach(var leaveRequest in rejectableLeaveRequests)
            {
                leaveRequest.LeaveStatus = Models.Enum.LeaveStatus.CANCELLED;
                leaveRequest.RejectReason = "Leave has been rejected automatically as it was pending and not approved";

                await _leaveRequestRepository.UpdateLeaveRequest(leaveRequest);
            }

        }

    }
}
