using ProductApp.Models;

namespace BLZR
{
    public interface IProductService
    {
        Task<ServiceResponse<IEnumerable<Product>>> GetProductsAsync();
        Task<ServiceResponse<Product>> GetProductAsync(int id);
        Task<ServiceResponse<bool>> AddProductAsync(Product product);
        Task<ServiceResponse<bool>> EditProductAsync(Product product);
        Task<ServiceResponse<bool>> DeleteProductAsync(int id);
    }
}