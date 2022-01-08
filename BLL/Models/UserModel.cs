using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BLL.Models
{
    public class UserModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [StringLength(70, MinimumLength = 5, ErrorMessage = "The Username value cannot exceed 70 characters. ")]
        public string Username { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [StringLength(70, MinimumLength = 2, ErrorMessage = "The First Name value cannot exceed 70 characters. ")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [StringLength(70, MinimumLength = 2, ErrorMessage = "The Last Name value cannot exceed 70 characters. ")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(40, ErrorMessage = "Must be between 5 and 40 characters", MinimumLength = 5)]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "Must be a valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(30, ErrorMessage = "Must be between 6 and 30 characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Role { get; set; }

        [Display(Name = "Purchased Lots")]
        public ICollection<LotModel> PurchasedLots { get; set; }
    }
}
