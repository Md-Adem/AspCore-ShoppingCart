using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data.DataAccess;
using ShoppingCart.Data.Models;

namespace ShoppingCart.Utility
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext context;

        public CategoriesViewComponent(ApplicationDbContext context)
        {
            this.context = context;
        }



        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await GetCategoriesAsync();
            return View(categories);
        }

        private Task<List<Category>> GetCategoriesAsync()
        {
            return context.Categories.OrderBy(x => x.Sorting).ToListAsync();
        }
    }
}
