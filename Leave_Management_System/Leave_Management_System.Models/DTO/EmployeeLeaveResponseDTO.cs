using Leave_Management_System.Models.Domain;

namespace Leave_Management_System.Models.DTO
{
    public class Approver
    {
        public Guid? ApproverId { get; set; }
        public string? ApproverName { get; set; }
        public string? ApproverEmail { get; set; }
        public string? ApproverPhoneNumber { get; set; }
    }

    public class LeaveIssuer
    {
        public Guid? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeEmail { get; set; }
        public string? EmployeePhoneNumber { get; set; }
    }

    public class IssuedLeaveType
    {
        public int? LeaveTypeId { get; set; }
        public string? LeaveTypeName { get; set; }
        public bool? IsPaid { get; set; } = true;
    }

    public class EmployeeLeaveResponseDTO
    {
        public Guid LeaveId { get; set; }

        public Guid EmployeeId { get; set; }
        public LeaveIssuer? Employee { get; set; }

        public int LeaveTypeId { get; set; }
        public IssuedLeaveType? LeaveType { get; set; }

        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required string LeaveReason { get; set; }

        public required string LeaveStatus { get; set; }

        public Guid? ApprovedBy { get; set; }
        public Approver? Approver { get; set; }

        public DateTime? ApproveDate { get; set; }
        public string? RejectReason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
