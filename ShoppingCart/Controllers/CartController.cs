using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Data;
using ShoppingCart.Utility;
using ShoppingCart.Data.Models;
using ShoppingCart.Data.DataAccess;
using ShoppingCart.Logic.ViewModels;

namespace ShoppingCart.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext context;

        public CartController(ApplicationDbContext context)
        {
            this.context = context;
        }



        public IActionResult Index()
        {
            List<CartItemViewModel> cart = HttpContext.Session.GetJson<List<CartItemViewModel>>("Cart") ?? new List<CartItemViewModel>();

            CartViewModel CartVm = new CartViewModel
            {
                CartItems = cart,
                NumberOfItems = cart.Sum(x => x.Quantity),
                GrandTotal = cart.Sum(x => x.Price * x.Quantity)
            };

            ViewBag.NumberOfItems = CartVm.NumberOfItems;

            return View(CartVm);
        }


        public async Task<IActionResult> Add(Guid id)
        {
            Product product = await context.Products.FindAsync(id);

            List<CartItemViewModel> cart = HttpContext.Session.GetJson<List<CartItemViewModel>>("Cart") ?? new List<CartItemViewModel>();

            CartItemViewModel cartItem = cart.Where(x => x.ProductId == id).FirstOrDefault();

            if (cartItem == null)
            {
                cart.Add(new CartItemViewModel(product));
            }
            else
            {
                cartItem.Quantity += 1;
            }

            HttpContext.Session.SetJson("Cart", cart);


            return Redirect(Request.Headers["Referer"].ToString());
        }


        public IActionResult Decrease(Guid id)
        {

            List<CartItemViewModel> cart = HttpContext.Session.GetJson<List<CartItemViewModel>>("Cart");

            CartItemViewModel cartItem = cart.Where(x => x.ProductId == id).FirstOrDefault();

            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity -= 1;
            }
            else
            {
                cart.RemoveAll(x => x.ProductId == id);
            }

            HttpContext.Session.SetJson("Cart", cart);

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }



        public IActionResult Remove(Guid id)
        {

            List<CartItemViewModel> cart = HttpContext.Session.GetJson<List<CartItemViewModel>>("Cart");


            cart.RemoveAll(x => x.ProductId == id);


            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }


        public IActionResult Clear()
        {

            HttpContext.Session.Remove("Cart");

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
