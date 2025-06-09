using Shop.Main.Common;
using Shop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Shop.Product.ViewModel
{
    public class ProductDetailsViewModel : BaseViewModel
    {
        private readonly BasketApiService _basketApiService;
        private readonly ProductApiService _productApiService;
        private readonly BasketState _basketState;
        private readonly Guid _userId;
        private readonly Guid _productId;

        public string Name { get; private set; }
        public decimal Price { get; private set; }

        public ICommand AddToBasketCommand { get; }
        public event Action? ProductAddedToBasket;

        public ProductDetailsViewModel(
            Guid userId,
            Guid productId,
            BasketApiService basketApiService,
            ProductApiService productApiService)
        {
            _userId = userId;
            _productId = productId;
            _basketApiService = basketApiService;
            _productApiService = productApiService;
            _basketState = BasketState.Instance;
            _basketState.UserId = userId;

            AddToBasketCommand = new RelayCommand(async _ => await AddToBasketAsync());
        }

        public async Task LoadProductAsync()
        {
            var product = await _productApiService.GetProductAsync(_productId);
            if (product != null)
            {
                Name = product.Name;
                Price = product.Price;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Price));
            }
        }

        private async Task AddToBasketAsync()
        {
            if (_basketState.BasketId == null || _basketState.BasketId == Guid.Empty)
            {
                var basketId = await _basketApiService.CreateBasketAsync(_userId);
                _basketState.BasketId = basketId;
            }
            await _basketApiService.AddProductToBasketAsync(_basketState.BasketId.Value, _productId);
            ProductAddedToBasket?.Invoke();
        }
    }
}
