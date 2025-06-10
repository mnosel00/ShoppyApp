using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shop.Basket.Dto
{
    public class BasketDto
    {
        [JsonPropertyName("basketId")]
        public Guid BasketId { get; set; }

        [JsonPropertyName("items")]
        public List<BasketItemDto> Items { get; set; } = new();

        [JsonPropertyName("totalValue")]
        public decimal TotalValue { get; set; }
    }
}
