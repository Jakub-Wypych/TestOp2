using ProductApp.Models;

namespace BLZR
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(int id);
        Task AddProduct(Product product);
        Task EditProduct(Product product);
        Task DeleteProduct(int id);
    }
}