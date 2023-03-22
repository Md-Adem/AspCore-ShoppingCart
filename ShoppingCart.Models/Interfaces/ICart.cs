using ShoppingCart.Logic.ViewModels;

namespace ShoppingCart.Logic.Interfaces
{
    public interface ICart
    {

        //Task<CartViewModel> GetAll();

        Task AddToCart(Guid id);

        void DecreaseFromCart(Guid id);

        void RemoveFromCart(Guid id);

        void ClearCart();
    }
}
