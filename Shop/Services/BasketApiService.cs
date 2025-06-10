using Shop.Basket.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services
{
    public  class BasketApiService
    {
        private readonly HttpClient _httpClient;

        public BasketApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(Guid basketId, Guid userId)> CreateBasketAsync()
        {
            var response = await _httpClient.PostAsync("/api/basket", null);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<CreateBasketResponse>();
            return (result.BasketId, result.UserId);
        }

        public async Task<BasketDto?> GetBasketAsync(Guid userId)
        {
            var response = await _httpClient.GetAsync($"/api/basket?userId={userId}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<BasketDto>();
        }

        public async Task AddProductToBasketAsync(Guid basketId, Guid productId)
        {
            try
            {
                var request = new { ProductId = productId };
                var response = await _httpClient.PostAsJsonAsync($"/api/basket/{basketId}/items", request);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show(
                    "Produkt znajduje się w innym koszyku lub jest zablokowany",
                    "Błąd dodawania produktu",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning
                );
            }
        }

        private class CreateBasketResponse
        {
            public Guid BasketId { get; set; }
            public Guid UserId { get; set; }
        }

    }
}
