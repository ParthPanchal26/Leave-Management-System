using System.ComponentModel.DataAnnotations;

namespace Leave_Management_System.Models.DTO
{
    public class EmployeeRequestDTO
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

        [Required(ErrorMessage = "Employee hire date is required")]
        [DataType(DataType.Date)]
        public DateOnly HireDate { get; set; }

        [Required(ErrorMessage = "Employee salary is required")]
        [Range(1, 100000000, ErrorMessage = "Salary must be greater than 0.")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "Employee password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
        [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#^()_\-+=])[A-Za-z\d@$!%*?&#^()_\-+=]{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //public Guid? ManagerId { get; set; }

        //[Required(ErrorMessage = "Employee role is required")]
        //public int? RoleId { get; set; } = 4;

        //[Required(ErrorMessage = "Employee department is required")]
        //public int? DepartmentId { get; set; }
    }
}
