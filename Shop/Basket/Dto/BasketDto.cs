using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Basket.Dto
{
    public class BasketDto
    {
        public Guid BasketId { get; set; }
        public List<BasketItemDto> basketItem { get; set; } = new();
        public decimal TotalValue { get; set; }
    }
}
