using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Inventory1.Pages.Inventory_Products
{
    public class Update : PageModel
    {
        [BindProperty] //added Error Message if the fields is Empty
        public int ProductId { get; set; }

        [BindProperty, Required(ErrorMessage ="The Name is Required")] //added Error Message if the fields is Empty
        public string Name { get; set; } = ""; // Initializing the value

        [BindProperty, Required(ErrorMessage ="The QuantityInStock is Required")]
        public int QuantityInStock { get; set; } = 0;

        [BindProperty, Required(ErrorMessage = "The Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "The Price must be a positive number.")]
        public decimal Price { get; set; } = 0.0m; // Use decimal for price

        public void OnGet()
        {
        }
    }
}