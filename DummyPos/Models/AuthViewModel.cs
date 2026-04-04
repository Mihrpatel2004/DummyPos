using System.ComponentModel.DataAnnotations;

namespace DummyPos.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class LoggedInUser
    {
        public int Staff_Id { get; set; }
        public string Staff_Name { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; } // Will hold "Admin" or "Staff"
    }
}