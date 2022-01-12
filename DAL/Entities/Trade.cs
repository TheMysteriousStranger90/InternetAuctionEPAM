using System;
using System.ComponentModel.DataAnnotations;
using NJsonSchema.Annotations;

namespace DAL.Entities
{
    /// <summary>
    /// Trade entity.
    /// </summary>
    public class Trade
    {
        [Key]
        public int Id { get; set; }

        [JsonSchemaDate]
        public DateTime StartDateTime { get; set; }

        public double Sum { get; set; }

        public string BuyerUserId { get; set; }

        public int LotId { get; set; }

        public User User { get; set; }

        public Lot Lot { get; set; }
    }
}
