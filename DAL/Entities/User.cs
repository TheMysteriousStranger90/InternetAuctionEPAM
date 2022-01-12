using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DAL.Entities
{
    /// <summary>
    /// User entity.
    /// </summary>
    public class User : IdentityUser
    {
        [Required, RegularExpression(@"^[a-zA-Zа-яА-я-_ ]+$")]
        [MaxLength(50), MinLength(2)]
        public string FirstName { get; set; }

        [Required, RegularExpression(@"^[a-zA-Zа-яА-я-_ ]+$")]
        [MaxLength(50), MinLength(2)]
        public string LastName { get; set; }

        public string Role { get; set; }

        public virtual ICollection<Lot> PurchasedLots { get; set; }
    }
}
