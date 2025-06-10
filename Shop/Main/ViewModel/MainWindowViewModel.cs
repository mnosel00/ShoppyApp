using Shop.Basket.View;
using Shop.Basket.ViewModel;
using Shop.Main.Common;
using Shop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Shop.Main.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ICommand OpenBasketCommand { get; }
        public ICommand CreateBasketCommand { get; }
        public bool IsBasketVisible => BasketState.Instance.BasketId != null && BasketState.Instance.BasketId != Guid.Empty;
        public bool IsBasketNotVisible => !IsBasketVisible;

        private string _infoMessage = string.Empty;
        public string InfoMessage
        {
            get => _infoMessage;
            set { _infoMessage = value; OnPropertyChanged(); }
        }
        private readonly BasketApiService _basketApiService;

        public MainWindowViewModel(BasketApiService basketApiService)
        {
            _basketApiService = basketApiService;
            OpenBasketCommand = new RelayCommand(_ => OpenBasket());
            CreateBasketCommand = new RelayCommand(async _ => await CreateBasketAsync());

            BasketState.Instance.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(BasketState.BasketId))
                {
                    OnPropertyChanged(nameof(IsBasketVisible));
                    OnPropertyChanged(nameof(IsBasketNotVisible));
                }
            };
        }


        //To trzeba przenieść do api 
        private async Task CreateBasketAsync()
        {
            try
            {
                var (basketId, userId) = await _basketApiService.CreateBasketAsync();
                BasketState.Instance.BasketId = basketId;
                BasketState.Instance.UserId = userId;
                InfoMessage = "Koszyk został utworzony!";
            }
            catch
            {
                InfoMessage = "Błąd podczas tworzenia koszyka.";
            }
        }

        private async void OpenBasket()
        {
            var basketId = BasketState.Instance.BasketId;
            var userId = BasketState.Instance.UserId;
            if (basketId == null || basketId == Guid.Empty)
                return;

            var basket = await _basketApiService.GetBasketAsync(userId);
            if (basket == null)
            {
                MessageBox.Show("Nie udało się pobrać koszyka.");
                return;
            }

            var vm = new BasketViewModel(basket);
            var window = new BasketWindow(vm);
            window.ShowDialog();
        }
    }
}
