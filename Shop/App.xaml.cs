using Microsoft.Extensions.DependencyInjection;
using Shop.Main.ViewModel;
using Shop.Services;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Windows;

namespace Shop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            // Rejestracja serwisów
            services.AddSingleton(new HttpClient { BaseAddress = new Uri("https://localhost:7012") });
            services.AddSingleton<ProductApiService>();
            services.AddSingleton<BasketApiService>();

            // Rejestracja ViewModeli
            services.AddSingleton<MainWindowViewModel>();

            ServiceProvider = services.BuildServiceProvider();

            base.OnStartup(e);
        }
    }

}
