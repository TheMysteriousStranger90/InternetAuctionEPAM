using System.ComponentModel.DataAnnotations;

namespace BLL.Models
{
    /// <summary>
    /// LoginModel
    /// </summary>
    public class LoginModel
    {
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Please, enter valid email address")]
        [StringLength(40, ErrorMessage = "Must be between 5 and 40 characters", MinimumLength = 5)]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "Must be a valid email")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(30, ErrorMessage = "Must be between 6 and 30 characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
