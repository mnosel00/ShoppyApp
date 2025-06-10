using Shop.Basket.Dto;
using Shop.Main.Common;
using Shop.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Shop.Basket.ViewModel
{
    public class BasketViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<BasketItemDto> Items { get; }
        private decimal _totalValue;
        public decimal TotalValue
        {
            get => _totalValue;
            private set
            {
                if (_totalValue != value)
                {
                    _totalValue = value;
                    OnPropertyChanged(nameof(TotalValue));
                }
            }
        }
        public ICommand RemoveItemCommand { get; }

        private readonly BasketApiService _basketApiService;
        private readonly Guid _basketId;

        public BasketViewModel(BasketDto basket)
        {
            Items = new ObservableCollection<BasketItemDto>(basket.Items);
            TotalValue = basket.TotalValue;
            _basketId = basket.BasketId;

            _basketApiService = App.ServiceProvider.GetService(typeof(BasketApiService)) as BasketApiService
                ?? throw new InvalidOperationException("BasketApiService not found");

            RemoveItemCommand = new RelayCommand(async item => await RemoveItemAsync(item as BasketItemDto));
        }

        private async Task RemoveItemAsync(BasketItemDto? item)
        {
            if (item == null) return;

            try
            {
                await _basketApiService.RemoveProductFromBasketAsync(_basketId, item.ProductId);
                Items.Remove(item);
                RefreshTotalValue();
            }
            catch (Exception)
            {
                MessageBox.Show("Nie udało się usunąć produktu z koszyka.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshTotalValue()
        {
            TotalValue = Items.Sum(i => i.Price);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
