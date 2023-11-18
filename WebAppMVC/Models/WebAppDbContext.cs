using Microsoft.EntityFrameworkCore;
using WebAppMVC.Models.Entities;

namespace WebAppMVC.Models
{
    public class WebAppDbContext : DbContext
    {
        public WebAppDbContext(DbContextOptions options) : base(options) { }
        
        public DbSet<Product> products { get; set; }
    }
}
