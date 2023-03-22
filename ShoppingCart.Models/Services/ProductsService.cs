using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data.DataAccess;
using ShoppingCart.Data.Models;
using ShoppingCart.Logic.Interfaces;
using ShoppingCart.Logic.ViewModels;

namespace ShoppingCart.Logic.Services
{
    public class ProductsService : IProducts
    {

        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductsService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }


        //***************************************************//



        public async Task<OperationResult> CreateProduct(Product product)
        {
            product.Slug = product.Name.ToLower().Replace(" ", "-");

            var slug = await context.Products.FirstOrDefaultAsync(x => x.Slug == product.Slug);

            if (slug != null)
            {
                return OperationResult.NotFound("The Product already exists");
            }

            string imageName = "Noimage.png";

            if (product.ImageUpload != null)
            {
                string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/products");
                imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                string filePath = Path.Combine(uploadsDir, imageName);

                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                await product.ImageUpload.CopyToAsync(fileStream);
                fileStream.Close();
            }

            product.Image = imageName;

            context.Add(product);
            await context.SaveChangesAsync();

            return OperationResult.Succeeded("The Product has been added!");
        }




        public async Task<OperationResult> DeleteProduct(Guid id)
        {
            Product product = await context.Products.FindAsync(id);

            if (product == null)
            {
                return OperationResult.NotFound("The product does not exist");
            }
            else
            {
                if (!string.Equals(product.Image, "Noimage.png"))
                {
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/products");

                    string oldImagePath = Path.Combine(uploadsDir, product.Image);

                    //System.IO.File
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }

                context.Products.Remove(product);
                await context.SaveChangesAsync();

                return OperationResult.Succeeded("The Product has been Deleted!");
            }
        }




        public async Task<OperationResult> EditProduct(Guid id, Product product)
        {
            product.Slug = product.Name.ToLower().Replace(" ", "-");

            var slug = await context.Products.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Slug == product.Slug);

            if (slug != null)
            {
                return OperationResult.NotFound("The Product already exists");
            }

            string imageName = "Noimage.png";

            if (product.ImageUpload != null)
            {
                string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/products");

                if (!string.Equals(product.Image, "Noimage.png"))
                {
                    string oldImagePath = Path.Combine(uploadsDir, product.Image);
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }


                imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                string filePath = Path.Combine(uploadsDir, imageName);

                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                await product.ImageUpload.CopyToAsync(fileStream);
                fileStream.Close();
            }


            context.Update(product);
            await context.SaveChangesAsync();

            return OperationResult.Succeeded("The Product has been edited!");
        }





        public IQueryable<Product> GetAllProducts(int p = 1)
        {
            int pageSize = 6;
            var products = context.Products.OrderByDescending(x => x.Id).Include(x => x.Category)
                                               .Skip((p - 1) * pageSize)
                                               .Take(pageSize);
            return products;
        }




        public async Task<Product> ProductDetails(Guid id)
        {
            Product product = await context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

            return product;
        }
    }
}
