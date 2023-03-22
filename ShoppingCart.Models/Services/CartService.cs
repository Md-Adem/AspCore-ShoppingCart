using ShoppingCart.Data.DataAccess;
using ShoppingCart.Logic.Interfaces;

namespace ShoppingCart.Logic.Services
{
    public class CartService : ICart
    {

        private readonly ApplicationDbContext context;

        public CartService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Task AddToCart(Guid id)
        {
            throw new NotImplementedException();
        }

        public void ClearCart()
        {
            throw new NotImplementedException();
        }

        public void DecreaseFromCart(Guid id)
        {
            throw new NotImplementedException();
        }

        public void RemoveFromCart(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
