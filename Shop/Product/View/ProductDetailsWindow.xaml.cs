using Shop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ProductApiService _api = new();

        public ProductDetailsWindow(Guid productId)
        {
            InitializeComponent();
            Loaded += async (s, e) =>
            {
                var product = await _api.GetProductAsync(productId);
                if (product != null)
                {
                    NameText.Text = product.Name;
                    PriceText.Text = $"Price: {product.Price:C}";
                }
            };
        }
    }
}
