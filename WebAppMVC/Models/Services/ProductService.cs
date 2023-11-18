using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebAppMVC.Models.Entities;

namespace WebAppMVC.Models.Services
{
    public class ProductService : IProductService
    {
        private readonly WebAppDbContext _context;
        public ProductService(WebAppDbContext context) { _context = context; }

        public Product Add(Product product)
        {
            _context.products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public void Delete(int id)
        {
            var product = _context.products.FirstOrDefault(x => x.Id == id);
            _context.products.Remove(product);
            _context.SaveChanges();
        }

        public Product GetProductById(int id)
        {
            var product = _context.products.FirstOrDefault(p => p.Id == id );
            return product;
        }

        public IEnumerable<Product> GetProducts()
        {
            return _context.products.ToList();
        }

        public Product Update(Product product)
        {
            _context.products.Update(product);
            _context.SaveChanges();
            return product;
        }
    }
}
