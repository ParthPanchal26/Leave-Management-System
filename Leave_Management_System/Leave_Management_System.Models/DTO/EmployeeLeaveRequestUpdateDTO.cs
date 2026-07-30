using Leave_Management_System.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace Leave_Management_System.Models.DTO
{
    public class EmployeeLeaveRequestUpdateDTO
    {
        [EnumDataType(typeof(LeaveStatus), ErrorMessage = "Invalid leave status.")]
        [Range(1, int.MaxValue, ErrorMessage = "Leave status is required.")]
        public LeaveStatus LeaveStatus { get; set; }

        [Required(ErrorMessage = "Reviewer required")]
        public Guid ReviewedBy { get; set; }

        public string? RejectReason { get; set; }

    }
}
