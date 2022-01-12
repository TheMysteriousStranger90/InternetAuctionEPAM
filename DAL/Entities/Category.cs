using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    /// <summary>
    /// Category entity.
    /// </summary>
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required, MinLength(3), MaxLength(30)]
        public string Name { get; set; }

        public virtual ICollection<Lot> Lots { get; set; }
    }
}
