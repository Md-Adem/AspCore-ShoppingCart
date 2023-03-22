using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Data.Models
{
    public class Category
    {
        public Guid Id { get; set; }

        [Required, MinLength(2, ErrorMessage = "Minimum length is 2")]
        [RegularExpression(@"^[a-zA-Z-]+$", ErrorMessage = "Only letters ae allowed")]
        public string Name { get; set; }

        public string? Slug { get; set; }
        public int Sorting { get; set; }

    }
}
