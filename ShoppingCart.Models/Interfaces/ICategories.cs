using ShoppingCart.Data.Models;
using ShoppingCart.Logic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Logic.Interfaces
{
    public interface ICategories
    {
        Task<List<Category>> GetAllCategories();

        Task<OperationResult> CreateCategory(Category category);

        Task<OperationResult> EditCategory(Guid id, Category category);

        Task<OperationResult> DeleteCategory(Guid id);
    }
}
