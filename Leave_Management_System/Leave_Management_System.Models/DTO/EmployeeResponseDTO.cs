using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Leave_Management_System.Models.DTO
{
    public class EmployeeResponseDTO
    {
        public Guid EmployeeId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public DateOnly DateOfBirth { get; set; }

        public DateOnly HireDate { get; set; }

        public decimal Salary { get; set; } = decimal.Zero;

        public Guid? ManagerId { get; set; }

        public int RoleId { get; set; }

        public int DepartmentId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

    }
}
