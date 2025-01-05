using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProductApp.Models;
using BLZR;

namespace ProductApp.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "/api/products"; 

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Get all products
        public async Task<IEnumerable<Product>> GetProducts()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<Product>>(BaseUrl);
            return response;
        }

        // Get a specific product by ID
        public async Task<Product> GetProduct(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<Product>($"{BaseUrl}/{id}");
            return response;
        }

        // Add a new product
        public async Task AddProduct(Product product)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, product);
            response.EnsureSuccessStatusCode();
        }

        // Edit an existing product
        public async Task EditProduct(Product product)
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{product.Id}", product);
            response.EnsureSuccessStatusCode();
        }

        // Delete a product by ID
        public async Task DeleteProduct(int id)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
        }

        public Task<ServiceResponse<IEnumerable<Product>>> GetProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<Product>> GetProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<bool>> AddProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<bool>> EditProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<bool>> DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
