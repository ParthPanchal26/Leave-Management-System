using Leave_Management_System.Models.Domain;

namespace Leave_Management_System.Models.DTO
{
    public class Manager
    {
        public string? ManagerName { get; set; } = string.Empty;
        public Guid? ManagerId { get; set; } = Guid.Empty;
        public string? Department { get; set; } = string.Empty;
        public string? Role { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
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

        public Manager Manager { get; set; }
        public Role Role { get; set; }

        public Department Department { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

    }
}
