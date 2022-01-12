using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NJsonSchema.Annotations;

namespace DAL.Entities
{
    /// <summary>
    /// Lot entity.
    /// </summary>
    public class Lot
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50), MinLength(5)]
        public string Name { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public double StartPrice { get; set; }

        public byte[]? Image { get; set; }

        public bool Status { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public virtual Category? Category { get; set; }

        public virtual Trade? Trade { get; set; }
    }
}
