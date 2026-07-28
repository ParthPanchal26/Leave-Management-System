using Leave_Management_System.Utility;
using System.ComponentModel.DataAnnotations;

namespace Leave_Management_System.Models.Domain
{
    public class LeaveRequest
    {
        [Key]
        public Guid LeaveId { get; set; } = Guid.NewGuid();

        public Guid EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public int LeaveTypeId { get; set; }
        public LeaveType? LeaveType { get; set; }

        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required string LeaveReason { get; set; } = string.Empty;

        public required string LeaveStatus { get; set; } = LeaveStatusValues.LeavePending;

        public Guid? ApprovedBy { get; set; }
        public Employee? Approver { get; set; }

        public DateTime? ApproveDate { get; set; }
        public string? RejectReason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
