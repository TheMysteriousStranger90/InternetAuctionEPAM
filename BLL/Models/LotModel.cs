using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace BLL.Models
{
    public class LotModel
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Image")]
        public byte[] Image { get; set; }

        [Display(Name = "Active")]
        public bool Status { get; set; } = true;

        [Display(Name = "Start Price")]
        [Required]
        public double StartPrice { get; set; }

        public int CategoryId { get; set; }
        public CategoryModel Category { get; set; }

        public string UserId { get; set; }
        public UserModel User { get; set; }

        public TradeModel Trade { get; set; }
    }
}
