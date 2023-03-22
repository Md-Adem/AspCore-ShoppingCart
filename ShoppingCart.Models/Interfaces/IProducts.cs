using ShoppingCart.Data.Models;
using ShoppingCart.Logic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Logic.Interfaces
{
    public interface IProducts
    {

        IQueryable<Product> GetAllProducts(int p = 1);

        Task<OperationResult> CreateProduct(Product product);

        Task<Product> ProductDetails(Guid id);

        Task<OperationResult> EditProduct(Guid id, Product product);

        Task<OperationResult> DeleteProduct(Guid id);

    }
}
