using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProductApp.Models;
using BLZR;
using System.Text.Json.Serialization;
using System.Text;
using System.Text.Json;

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

        public async Task<ServiceResponse<IEnumerable<Product>>> GetProductsAsync()
        {
            try
            {
                // Make an HTTP GET request to the API endpoint
                var response = await _httpClient.GetAsync(BaseUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response body into ServiceResponse<IEnumerable<Product>>
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var serviceResponse = JsonSerializer.Deserialize<ServiceResponse<IEnumerable<Product>>>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return serviceResponse ?? new ServiceResponse<IEnumerable<Product>>
                    {
                        Data = null,
                        Success = false,
                        Message = "Empty response from server."
                    };
                }

                return new ServiceResponse<IEnumerable<Product>>
                {
                    Data = null,
                    Success = false,
                    Message = $"Failed to retrieve products. Server returned status code: {response.StatusCode}"
                };
            }
            catch (Exception ex)
            {
                // Handle exceptions and return a failed response
                return new ServiceResponse<IEnumerable<Product>>
                {
                    Data = null,
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<ServiceResponse<Product>> GetProductAsync(int id)
        {
            try
            {
                // Tworzymy adres URL dla zapytania GET
                string url = $"{BaseUrl}/{id}";

                // Wysyłamy zapytanie GET do API
                var response = await _httpClient.GetAsync(url);

                // Sprawdzamy, czy odpowiedź była pozytywna
                if (response.IsSuccessStatusCode)
                {
                    // Jeśli odpowiedź jest pozytywna, deserializujemy dane produktu
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var serviceResponse = JsonSerializer.Deserialize<ServiceResponse<Product>>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    // Jeśli odpowiedź jest poprawna, zwracamy wynik
                    return serviceResponse ?? new ServiceResponse<Product>
                    {
                        Success = false,
                        Message = "Empty response from server."
                    };
                }

                // Jeśli serwer zwrócił błąd, zwrócimy odpowiednią wiadomość
                return new ServiceResponse<Product>
                {
                    Success = false,
                    Message = $"Failed to retrieve product. Server returned status code: {response.StatusCode}"
                };
            }
            catch (Exception ex)
            {
                // Jeśli wystąpił błąd, zwracamy odpowiednią wiadomość
                return new ServiceResponse<Product>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }


        public async Task<ServiceResponse<bool>> AddProductAsync(Product product)
        {

            var validateResponse = Validate(product);

            if (validateResponse.Success == false)
            {
                return validateResponse;
            }

            var response = await _httpClient.PostAsJsonAsync(BaseUrl, product);

            if (response.IsSuccessStatusCode)
            {
                // Jeśli odpowiedź jest pozytywna, zwrócimy sukces
                return new ServiceResponse<bool>
                {
                    Success = true,
                    Message = $"Product '{product.Name}' added successfully"
                };
            }

            // Jeśli wystąpił błąd, zwrócimy odpowiednią wiadomość
            return new ServiceResponse<bool>
            {
                Success = false,
                Message = "Failed to add product"
            };
        }

        public async Task<ServiceResponse<bool>> EditProductAsync(Product product)
        {
            string url = $"{BaseUrl}/{product.Id}";
            var content = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var validateResponse = Validate(product);

                    if (validateResponse.Success == false)
                    {
                        return validateResponse;
                    }

                    return new ServiceResponse<bool>
                    {
                        Success = true,
                        Message = $"Product {product.Name} with price {product.Price} saved successfully"
                    };
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new ServiceResponse<bool>
                    {
                        Success = false,
                        Message = "Product does not exist"
                    };
                }
                else
                {
                    return new ServiceResponse<bool>
                    {
                        Success = false,
                        Message = "An error occurred while editing the product"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        private ServiceResponse<bool> Validate(Product product)
        {

            if (string.IsNullOrWhiteSpace(product.Name))
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "Name is required"
                };
            }

            if (product.Price <= 0)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "Price must be greater than 0"
                };
            }

            if (product.Quantity < 0)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "Quantity cannot be negative"
                };
            }

  

            return new ServiceResponse<bool>
            {
                Success = true
            };
        }

        public async Task<ServiceResponse<bool>> DeleteProductAsync(int id)
        {
            try
            {
                // Make an HTTP DELETE request to the API endpoint
                var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response body into ServiceResponse<bool>
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var serviceResponse = JsonSerializer.Deserialize<ServiceResponse<bool>>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return serviceResponse ?? new ServiceResponse<bool>
                    {
                        Data = false,
                        Success = false,
                        Message = "Empty response from server."
                    };
                }

                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = $"Failed to delete product. Server returned status code: {response.StatusCode}"
                };
            }
            catch (Exception ex)
            {
                // Handle exceptions and return a failed response
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
    }
}
