using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
namespace frontend_task_2.Models
{
    public class ClinicInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int UserCount { get; set; }
        public int AppointmentCount { get; set; }
        public int QueueCount { get; set; }
    }

    public class ClinicUser
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    // (For making new user)
    public class AdminCreateUserRequest
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;



        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;



        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } = string.Empty; // receptionist, patient, doctor

        [Required(ErrorMessage = "Phone is required")]
        public string Phone { get; set; } = string.Empty;
    }
}
