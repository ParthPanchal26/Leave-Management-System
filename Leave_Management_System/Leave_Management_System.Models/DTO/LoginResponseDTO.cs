namespace Leave_Management_System.Models.DTO
{
    public class LoginResponseDTO
    {
        public string Email { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
