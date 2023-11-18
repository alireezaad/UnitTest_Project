using System.Collections;
using WebAppMVC.Models.Entities;

namespace WebAppMVC.TestData
{
    public class ProductMemberData
    {


        public List<Product> GetData()
        {
            List<Product> data = new List<Product>()
            {
                new Product{ Id = 1,Name = "iPhone 12 mini", Description = "Bad battery", Price = 20000},
                new Product{ Id = 2,Name = "Marker", Description = "nothing usefull", Price = 5000},
                new Product{ Id = 3,Name = "Nintendo Switch", Description = "Good handling", Price = 15000},
            };

            return data;
        }

    }
}
