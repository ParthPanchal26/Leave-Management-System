using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Leave_Management_System.Models.Domain
{
    public class Employee
    {
        [Key]
        public Guid EmployeeId { get; set; } = Guid.NewGuid();
        public required string FirstName { get; set; } = string.Empty;
        public required string LastName { get; set; } = string.Empty;
        public required string Email { get; set; } = string.Empty;
        public required string PhoneNumber { get; set; } = string.Empty;
        public required DateOnly DateOfBirth { get; set; }
        public DateOnly HireDate { get; set; }
        public decimal Salary { get; set; } = decimal.Zero;
        public required string PasswordHash { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Guid? ManagerId { get; set; }
        [ForeignKey(nameof(ManagerId))]
        public Employee? Manager { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public ICollection<LeaveRequest> LeaveRequests { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;

    }
}
