using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data.DataAccess;
using ShoppingCart.Data.Models;

namespace ShoppingCart.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PagesController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        public PagesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }


        public async Task<IActionResult> Dashboard()
        {
            ViewBag.TotalCategories = await context.Categories.CountAsync();

            ViewBag.TotalProducts = await context.Products.CountAsync();

            ViewBag.TotalUsers = await userManager.Users.CountAsync();

            return View();
        }




        public async Task<IActionResult> Index()
        {
            var pages = await context.Pages.OrderBy(p => p.Sorting).ToListAsync();

            return View(pages);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Page pages = await context.Pages.FirstOrDefaultAsync(p => p.Id == id);

            if (pages == null)
            {
                return NotFound();
            }

            return View(pages);
        }


        // Http is Get by Default
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Page page)
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.Title.ToLower().Replace(" ", "-");
                page.Sorting = 100;

                var slug = await context.Pages.FirstOrDefaultAsync(x => x.Slug == page.Slug);

                if (slug != null)
                {
                    ModelState.AddModelError("", "The Page already exists");
                    return View(page);
                }

                context.Add(page);
                await context.SaveChangesAsync();

                TempData["Success"] = "The Page has been added!";

                return RedirectToAction("Index");

            }

            return View(page);
        }




        public async Task<IActionResult> Edit(int id)
        {
            Page pages = await context.Pages.FindAsync(id);

            if (pages == null)
            {
                return NotFound();
            }

            return View(pages);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Page page)
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.Id == 1 ? "home" : page.Title.ToLower().Replace(" ", "-");

                var slug = await context.Pages.Where(x => x.Id != page.Id).FirstOrDefaultAsync(x => x.Slug == page.Slug);

                if (slug != null)
                {
                    ModelState.AddModelError("", "The Page already exists");
                    return View(page);
                }

                context.Update(page);
                await context.SaveChangesAsync();

                TempData["Success"] = "The Page has been Updated!";

                return RedirectToAction("Index");

            }

            return View(page);
        }



        public async Task<IActionResult> Delete(int id)
        {
            Page pages = await context.Pages.FindAsync(id);

            if (pages == null)
            {
                TempData["Error"] = "The Page does not exist";
            }
            else
            {
                context.Pages.Remove(pages);
                await context.SaveChangesAsync();

                TempData["Success"] = "The Page has been Deleted!";
            }


            return RedirectToAction("Index");
        }
    }
}
