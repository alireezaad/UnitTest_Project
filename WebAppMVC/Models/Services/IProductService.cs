using WebAppMVC.Models.Entities;

namespace WebAppMVC.Models.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetProducts();
        Product GetProductById(int id);
        Product Add(Product product);
        Product Update(Product product);
        void Delete(int id);
    }
}
