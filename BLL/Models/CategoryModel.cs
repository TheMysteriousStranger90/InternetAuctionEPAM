using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BLL.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }

        [Required, Display(Name = "Category Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The Name value cannot exceed 50 characters. ")]
        public string Name { get; set; }

        public ICollection<int> LotsId { get; set; }
    }
}
