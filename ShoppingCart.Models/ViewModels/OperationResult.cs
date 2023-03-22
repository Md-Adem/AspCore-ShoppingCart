using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Logic.ViewModels
{
    public class OperationResult
    {

        public bool Success { get; set; }
        public string Message { get; set; }


        public static OperationResult NotFound(string message = "Item Not Found")
        {
            return new OperationResult { Success = false, Message = message };
        }

        public static OperationResult Succeeded(string message = "Operation Completed Successfully")
        {
            return new OperationResult { Success = true, Message = message };
        }

    }
}
