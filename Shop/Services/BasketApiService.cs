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

        public async Task<Guid> CreateBasketAsync(Guid userId)
        {
            var response = await _httpClient.PostAsync("/api/basket", null);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<CreateBasketResponse>();
            return result.BasketId;
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
            var request = new { ProductId = productId };
            var response = await _httpClient.PostAsJsonAsync($"/api/basket/{basketId}/items", request);
            response.EnsureSuccessStatusCode();
        }

        private class CreateBasketResponse
        {
            public Guid BasketId { get; set; }
        }

    }
}
