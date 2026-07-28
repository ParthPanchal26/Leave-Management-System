namespace Leave_Management_System.Models
{
    public class ServiceResult<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public int Statuscode { get; set; }
    }
}
