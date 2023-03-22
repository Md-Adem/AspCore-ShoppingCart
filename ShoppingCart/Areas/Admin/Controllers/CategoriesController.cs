using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data.Models;
using ShoppingCart.Data.DataAccess;
using ShoppingCart.Logic.Interfaces;

namespace ShoppingCart.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {

        private readonly ApplicationDbContext context;
        private readonly ICategories categories;

        public CategoriesController(ApplicationDbContext context, ICategories categories)
        {
            this.context = context;
            this.categories = categories;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await categories.GetAllCategories();

            return View(result);
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                await categories.CreateCategory(category);

                return RedirectToAction("Index");
            }

            return View(category);
        }



        public async Task<IActionResult> Edit(Guid id)
        {
            Category category = await context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Category category)
        {
            if (ModelState.IsValid)
            {
                await categories.EditCategory(id, category);

                return RedirectToAction("Index");
            }

            return View(category);
        }


        public async Task<IActionResult> Delete(Guid id)
        {

            await categories.DeleteCategory(id);

            return RedirectToAction("Index");
        }
    }
}
