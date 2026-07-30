using System.ComponentModel.DataAnnotations;

namespace Leave_Management_System.Models.Domain
{
    public class LeaveBalance
    {
        [Key]
        public int LeaveBalanceId { get; set; }

        public Guid EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public int LeaveTypeId { get; set; }
        public LeaveType? LeaveType { get; set; }

        public Guid LeaveRequestId { get; set; }
        public LeaveRequest? LeaveRequest { get; set; }

        public int LeaveYear { get; set; }

        public int AllocatedDays { get; set; }

        public int UsedDays { get; set; }
    }
}
