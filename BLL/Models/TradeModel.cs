using System;
using NJsonSchema.Annotations;

namespace BLL.Models
{
    /// <summary>
    /// TradeModel
    /// </summary>
    public class TradeModel
    {
        public int Id { get; set; }

        [JsonSchemaDate]
        public DateTime? StartDateTime { get; set; }

        public double Sum { get; set; }

        public string BuyerUserId { get; set; }
        public UserModel User { get; set; }

        public int LotId { get; set; }
        public LotModel Lot { get; set; }
    }
}
