using System.ComponentModel.DataAnnotations;

namespace BLL.Models
{
    /// <summary>
    /// SignupModel
    /// </summary>
    public class SignupModel
    {
        [Required]
        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password do not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
