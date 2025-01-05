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

        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(int id);
        Task AddProduct(Product product);
        Task EditProduct(Product product);
        Task DeleteProduct(int id);
    }
}