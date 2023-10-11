using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveAPI.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Email { get; set; }
        public string? Password { get; set; }
        public bool IsAdmin { get; set; }
        public List<Request> Requests { get; set; }
    }
}
