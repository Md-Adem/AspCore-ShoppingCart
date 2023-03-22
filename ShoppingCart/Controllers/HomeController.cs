using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Data;
using ShoppingCart.Data.Models;
using ShoppingCart.Data.DataAccess;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ShoppingCart.Utility;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using ShoppingCart.Logic.ViewModels;

namespace ShoppingCart.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            this.context = context;
        }



        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 8;
            var products = context.Products.OrderByDescending(x => x.Id)
                                               .Skip((p - 1) * pageSize)
                                               .Take(pageSize);

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Products.Count() / pageSize);


            var result = await products.ToListAsync();



            var cart = HttpContext.Session.GetJson<List<CartItemViewModel>>("Cart") ?? new List<CartItemViewModel>();

            ViewBag.NumberOfItems = cart.Sum(x => x.Quantity);

            return View(result);
        }



        [Authorize]
        public async Task<IActionResult> ProductsByCategory(string categorySlug, int p = 1)
        {
            Category category = await context.Categories.Where(x => x.Slug == categorySlug).FirstOrDefaultAsync();

            if (category == null) RedirectToAction("Index");

            int pageSize = 8;
            var products = context.Products.OrderByDescending(x => x.Id)
                                               .Where(x => x.CategoryId == category.Id)
                                               .Skip((p - 1) * pageSize)
                                               .Take(pageSize);



            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Products.Where(x => x.CategoryId == category.Id).Count() / pageSize);

            ViewBag.CategoryName = category.Name;

            var result = await products.ToListAsync();


            var cart = HttpContext.Session.GetJson<List<CartItemViewModel>>("Cart") ?? new List<CartItemViewModel>();

            ViewBag.NumberOfItems = cart.Sum(x => x.Quantity);

            return View(result);
        }


        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            Product product = await context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }



            var cart = HttpContext.Session.GetJson<List<CartItemViewModel>>("Cart") ?? new List<CartItemViewModel>();

            ViewBag.NumberOfItems = cart.Sum(x => x.Quantity);


            return View(product);
        }


        //public async Task<IActionResult> Page(string slug)
        //{
        //    if (slug == null)
        //    {
        //        return View(await context.Pages.Where(x => x.Slug == "home").FirstOrDefaultAsync());
        //    }

        //    Page page = await context.Pages.Where(x => x.Slug == slug).FirstOrDefaultAsync();

        //    if (page == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(page);
        //}




        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}