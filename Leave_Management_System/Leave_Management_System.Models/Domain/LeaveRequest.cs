using Leave_Management_System.Models.Enum;
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

        //public enum LeaveStatuses
        //{
        //    APPROVED,
        //    REJECTED,
        //    PENDING
        //}

        public required LeaveStatus LeaveStatus { get; set; } = LeaveStatus.PENDING;

        public Guid? ReviewedBy { get; set; }
        public Employee? Reviewer { get; set; }

        public DateTime? ApproveDate { get; set; }
        public string? RejectReason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
