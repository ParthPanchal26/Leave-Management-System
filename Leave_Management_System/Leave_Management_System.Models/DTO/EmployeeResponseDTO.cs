using Leave_Management_System.Models.Domain;

namespace Leave_Management_System.Models.DTO
{
    public class Manager
    {
        public string? ManagerName { get; set; }
        public Guid? ManagerId { get; set; }
        public string? Department { get; set; }
        public string? Role { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class EmployeeRole
    {
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
    }

    public class EmployeeDepartment
    {
        public int? DepartmentId { get; set; }

        public string? DepartmentName { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;
    }

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

        public Manager? Manager { get; set; }
        public EmployeeRole? Role { get; set; }

        public EmployeeDepartment? Department { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

    }
}
