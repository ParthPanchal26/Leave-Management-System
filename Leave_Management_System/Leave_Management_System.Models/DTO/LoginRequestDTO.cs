using System.ComponentModel.DataAnnotations;

namespace Leave_Management_System.Models.DTO
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Employee email is required")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Employee password is required")]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
    }
}
