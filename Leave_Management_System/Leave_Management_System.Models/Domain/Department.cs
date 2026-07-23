using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Leave_Management_System.Models.Domain
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
