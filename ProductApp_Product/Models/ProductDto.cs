using System.ComponentModel.DataAnnotations;

namespace ProductApp_Product.Models
{
    public class ProductDto
    {
        [Required]
        public string Name { get; set; } = "";

        [Required]
        public string Brand { get; set; } = "";

        [Required]
        public string Category { get; set; } = "";

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; } = "";


    }
}
