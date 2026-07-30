using System.ComponentModel.DataAnnotations;

namespace Leave_Management_System.Models.DTO
{
    public class EmployeeUpdateDTO
    {

        [Required(ErrorMessage = "Employee firstname is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
        [RegularExpression(@"^[a-zA-Z\s'-]+$", ErrorMessage = "First name can only contain letters, spaces, apostrophes, and hyphens.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Employee lastname is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
        [RegularExpression(@"^[a-zA-Z\s'-]+$", ErrorMessage = "Last name can only contain letters, spaces, apostrophes, and hyphens.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Employee email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Employee phone number is required")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        [RegularExpression(@"^\+?[1-9]\d{9,14}$", ErrorMessage = "Phone number must contain 10 to 15 digits and may start with '+'.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Employee DOB is required")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }


        [Required(ErrorMessage = "Employee salary is required")]
        [Range(1, 100000000, ErrorMessage = "Salary must be greater than 0.")]
        public decimal Salary { get; set; }

        public Guid? ManagerId { get; set; }

        public int? RoleId { get; set; } = 4;

        public int? DepartmentId { get; set; }

    }
}
