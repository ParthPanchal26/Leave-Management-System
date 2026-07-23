using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Leave_Management_System.Models.Domain
{
    public class LeaveType
    {
        [Key]
        public int LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; }
        public int MaxDaysPerYear { get; set; }
        public bool IsPaid { get; set; } = true;
    }
}
