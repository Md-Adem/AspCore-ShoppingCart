using ShoppingCart.Data.Models;

namespace ShoppingCart.Logic.ViewModels
{
    public class CartViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; }
        public int NumberOfItems { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
