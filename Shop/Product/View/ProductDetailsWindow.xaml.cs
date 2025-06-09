using Microsoft.Extensions.DependencyInjection;
using Shop.Main.Common;
using Shop.Product.ViewModel;
using Shop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Shop.Product.View
{
    /// <summary>
    /// Logika interakcji dla klasy ProductDetailsWindow.xaml
    /// </summary>
    public partial class ProductDetailsWindow : Window
    {
        public ProductDetailsWindow(Guid productId)
        {
            InitializeComponent();

            // Pobierz serwisy przez DI
            var basketApiService = App.ServiceProvider.GetRequiredService<BasketApiService>();
            var productApiService = App.ServiceProvider.GetRequiredService<ProductApiService>();
            var userId = BasketState.Instance.UserId != Guid.Empty ? BasketState.Instance.UserId : Guid.NewGuid();
            BasketState.Instance.UserId = userId;

            var vm = new ProductDetailsViewModel(
                userId,
                productId,
                basketApiService,
                productApiService
            );
            DataContext = vm;

            Loaded += async (s, e) =>
            {
                await vm.LoadProductAsync();
            };

            vm.ProductAddedToBasket += () =>
            {
                MessageBox.Show("Produkt dodany do koszyka!");
                Close();
            };
        }
    }
}
