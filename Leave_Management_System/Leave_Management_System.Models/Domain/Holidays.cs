using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Leave_Management_System.Models.Domain
{
    public class Holidays
    {
        [Key]
        public int HolidayId { get; set; }

        public string HolidayName { get; set; } = string.Empty;
        public DateTime HolidayDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsOptional { get; set; } = true;
    }
}
