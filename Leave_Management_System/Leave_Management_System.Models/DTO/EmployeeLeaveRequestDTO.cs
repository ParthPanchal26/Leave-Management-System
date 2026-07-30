using System.ComponentModel.DataAnnotations;

namespace Leave_Management_System.Models.DTO
{
    public class EmployeeLeaveRequestDTO
    {
        [Required(ErrorMessage = "Employee is required")]
        public Guid EmployeeId { get; set; }

        [Required(ErrorMessage = "Leave Type is required")]
        public int LeaveTypeId { get; set; }

        [Required(ErrorMessage = "Please enter leave start date")]
        [DataType(DataType.Date)]
        public required DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Please enter leave end date")]
        [DataType(DataType.Date)]
        public required DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Please enter leave reason")]
        [MinLength(50, ErrorMessage = "Leave reason is too short")]
        public required string LeaveReason { get; set; } = string.Empty;
    }
}
