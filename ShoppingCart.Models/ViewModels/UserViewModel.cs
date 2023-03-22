using ShoppingCart.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Logic.ViewModels
{
    public class UserViewModel
    {
        [MinLength(4, ErrorMessage = "Minimum length is 4")]
        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password), MinLength(4, ErrorMessage = "Minimum length is 4")]
        public string Password { get; set; }


        public UserViewModel()
        {

        }

        public UserViewModel(ApplicationUser appUser)
        {
            UserName = appUser.UserName;
            Email = appUser.Email;
            Password = appUser.PasswordHash;
        }
    }
}
