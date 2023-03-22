using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Data.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        [Required, MinLength(2, ErrorMessage = "Minimum length is 2")]
        public string Name { get; set; }
        public string? Slug { get; set; }

        [Required, MinLength(5, ErrorMessage = "Minimum length is 5")]
        public string Description { get; set; }

        public decimal Price { get; set; }

        [Display(Name = "Category")]
        //[Range(1,2, ErrorMessage = "You must choose a category")]
        public Guid CategoryId { get; set; }

        [Display(Name = "Product Image")]
        public string? Image { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category? Category { get; set; }

        [NotMapped]
        public IFormFile? ImageUpload { get; set; }


    }
}
