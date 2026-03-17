using System.ComponentModel.DataAnnotations;

namespace frontend_task_2.Models
{
    //(Model for user login)
    public class LoginRequest
    {
  [Required(ErrorMessage = "Email is required")]
      [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;
      
        
   [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }

    public class User
    {
        public int Id { get; set; }
   public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
       public string Role { get; set; } = string.Empty;
      public int? ClinicId { get; set; }
     public string? ClinicName { get; set; }
     public string? ClinicCode { get; set; }
    }

    public class LoginResponse
    {
    
       
    public string Token { get; set; } = string.Empty;
        public User User { get; set; } = new User();
    }
}
