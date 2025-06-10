using Shop.Product.Cache;
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
        private readonly HttpClient _httpClient;
        private readonly ProductCacheService _cacheService;

        public ProductApiService(HttpClient httpClient, ProductCacheService cacheService)
        {
            _httpClient = httpClient;
            _cacheService = cacheService;
        }

        public async Task<List<ProductDto>> GetProductsAsync()
        {
            try
            {
                var products = await _httpClient.GetFromJsonAsync<List<ProductDto>>("/api/product");
                if (products != null && products.Count > 0)
                {
                    try
                    {
                        await _cacheService.SaveProductsAsync(products);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Błąd zapisu cache produktów: {ex.Message}");
                    }
                    return products;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Błąd pobierania produktów z API: {ex.Message}");
            }

            // Jeśli nie udało się pobrać z API, pobierz z cache
            try
            {
                return await _cacheService.LoadProductsAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Błąd odczytu cache produktów: {ex.Message}");
                return new List<ProductDto>();
            }
        }

        public async Task<ProductDto?> GetProductAsync(Guid id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<ProductDto>($"/api/product/{id}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Błąd pobierania produktu z API: {ex.Message}");
                return null;
            }
        }
    }
}
