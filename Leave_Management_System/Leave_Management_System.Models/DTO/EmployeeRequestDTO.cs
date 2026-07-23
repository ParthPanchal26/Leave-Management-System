using System.ComponentModel.DataAnnotations;

namespace Leave_Management_System.Models.DTO
{
    public class EmployeeRequestDTO
    {
        [Required(ErrorMessage = "Employee firstname is required")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Employee lastname is required")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Employee email is required")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Employee phone number is required")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Employee DOB is required")]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Employee hire date is required")]
        public DateOnly HireDate { get; set; }

        [Required(ErrorMessage = "Employee salary is required")]
        public decimal Salary { get; set; } = decimal.Zero;

        [Required(ErrorMessage = "Employee password is required")]
        public string Password { get; set; }

        public Guid? ManagerId { get; set; }

        [Required(ErrorMessage = "Employee role is required")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Employee department is required")]
        public int DepartmentId { get; set; }
    }
}
