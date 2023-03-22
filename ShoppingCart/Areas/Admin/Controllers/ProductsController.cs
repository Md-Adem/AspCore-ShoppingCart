using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data.Models;
using ShoppingCart.Data.DataAccess;
using System.Data;
using ShoppingCart.Logic.Interfaces;

namespace ShoppingCart.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {

        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IProducts products;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IProducts products)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
            this.products = products;
        }



        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 6;

            var allProduct = products.GetAllProducts(p);

            ViewBag.PageNumber = p;
            ViewBag.PageRange = 6;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Products.Count() / pageSize);


            var result = await allProduct.ToListAsync();

            return View(result);
        }




        [HttpGet]
        public IActionResult Create()
        {
            var CategoryId = context.Categories.OrderBy(x => x.Sorting);

            ViewBag.CategoryId = new SelectList(CategoryId, "Id", "Name");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                await products.CreateProduct(product);

                return RedirectToAction("Index");
            }

            return View(product);
        }






        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            Product product = await products.ProductDetails(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }






        public async Task<IActionResult> Edit(Guid id)
        {
            Product product = await context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name", product.CategoryId);

            return View(product);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Product product)
        {

            var CategoryId = context.Categories.OrderBy(x => x.Sorting);

            ViewBag.CategoryId = new SelectList(CategoryId, "Id", "Name", product.CategoryId);


            if (ModelState.IsValid)
            {
                await products.EditProduct(id, product);

                return RedirectToAction("Index");
            }

            return View(product);
        }




        public async Task<IActionResult> Delete(Guid id)
        {
            await products.DeleteProduct(id);

            return RedirectToAction("Index");
        }
    }
}
