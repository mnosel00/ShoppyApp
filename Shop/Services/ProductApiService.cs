using Shop.Product.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services
{
    public class ProductApiService
    {
        private readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7012") };

        public async Task<List<ProductDto>> GetProductsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ProductDto>>("/api/product") ?? new();
        }

        public async Task<ProductDto?> GetProductAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<ProductDto>($"/api/product/{id}");
        }
    }
}
