using System.ComponentModel.DataAnnotations;

namespace WebAppMVC.Models.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; } = 0;
    }
}
