using Shop.Basket.Dto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Basket.ViewModel
{
    public class BasketViewModel
    {
        public ObservableCollection<BasketItemDto> Items { get; }
        public decimal TotalValue { get; }

        public BasketViewModel(BasketDto basket)
        {
            Items = new ObservableCollection<BasketItemDto>(basket.Items);
            TotalValue = basket.TotalValue;
        }
    }
}
