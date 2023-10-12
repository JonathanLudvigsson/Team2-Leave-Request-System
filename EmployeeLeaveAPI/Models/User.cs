using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        public string? PasswordHash { get; set; }
        public bool IsAdmin { get; set; }
        [JsonIgnore]
        public List<Request> Requests { get; set; }
    }
}
