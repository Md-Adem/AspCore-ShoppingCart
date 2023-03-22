using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data.DataAccess;
using ShoppingCart.Data.Models;
using ShoppingCart.Logic.Interfaces;
using ShoppingCart.Logic.ViewModels;

namespace ShoppingCart.Logic.Services
{
    public class CategoriesService : ICategories
    {

        private readonly ApplicationDbContext context;

        public CategoriesService(ApplicationDbContext context)
        {
            this.context = context;
        }


        //***************************************************//


        public async Task<OperationResult> CreateCategory(Category category)
        {

            category.Slug = category.Name.ToLower().Replace(" ", "-");
            category.Sorting = 100;

            var slug = await context.Categories.FirstOrDefaultAsync(x => x.Slug == category.Slug);

            if (slug != null)
            {
                return OperationResult.NotFound("The category already exists");
            }

            context.Add(category);
            await context.SaveChangesAsync();

            return OperationResult.Succeeded("The category has been added!");
        }





        public async Task<OperationResult> DeleteCategory(Guid id)
        {
            Category category = await context.Categories.FindAsync(id);

            if (category == null)
            {
                return OperationResult.NotFound("The category does not exist");
            }
            else
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                return OperationResult.Succeeded("The category has been Deleted!");
            }
        }




        public async Task<OperationResult> EditCategory(Guid id, Category category)
        {
            category.Slug = category.Name.ToLower().Replace(" ", "-");

            var slug = await context.Categories.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Slug == category.Slug);

            if (slug != null)
            {
                return OperationResult.NotFound("The category already exists");
            }

            context.Update(category);
            await context.SaveChangesAsync();

            return OperationResult.Succeeded("The category has been Updated!");
        }



        public async Task<List<Category>> GetAllCategories()
        {
            var result = await context.Categories.OrderBy(x => x.Sorting).ToListAsync();

            return result;
        }
    }
}
