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

        public async Task<ServiceResponse<IEnumerable<Product>>> GetProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(BaseUrl);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var products = JsonSerializer.Deserialize<IEnumerable<Product>>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return new ServiceResponse<IEnumerable<Product>>
                    {
                        Data = products ?? Enumerable.Empty<Product>(),
                        Success = true,
                        Message = "Products retrieved successfully."
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
                string url = $"{BaseUrl}/{id}";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var product = JsonSerializer.Deserialize<Product>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return new ServiceResponse<Product>
                    {
                        Data = product,
                        Success = product != null,
                        Message = product != null ? "Product retrieved successfully." : "Product data is null."
                    };
                }

                return new ServiceResponse<Product>
                {
                    Data = null,
                    Success = false,
                    Message = $"Failed to retrieve product. Server returned status code: {response.StatusCode}"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Product>
                {
                    Data = null,
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
                return new ServiceResponse<bool>
                {
                    Success = true,
                    Message = $"Product '{product.Name}' added successfully"
                };
            }

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
                var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return new ServiceResponse<bool>
                    {
                        Data = false,
                        Success = true,
                        Message = "Product successfully deleted."
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
