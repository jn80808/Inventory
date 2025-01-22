using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Inventory1.Pages.Inventory_Products
{
    public class Create : PageModel
    {
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

        public string ErrorMessage { get; set; } = "";//Initial Value of ErrorMessage is Empty

        public void OnPost()
        {
            if (!ModelState.IsValid){
                return;
            }

            // Validation
            if (Name == null) Name="";
            if(QuantityInStock == null) QuantityInStock = 0;
            if(Price == null ) Price = 0.0m; 

            //Create new Product 
            //Process of Inserting to our data 
            try {
                
                string connectionString = "Server=DESKTOP-NSLMOTD;Database=Inventory;Trusted_Connection=True;TrustServerCertificate=True";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string sql = "Insert Into Products" +
                            "(Name, QuantityInStock, Price) VALUES" +
                            "(@Name, @QuantityInStock, @Price);";

                            using (SqlCommand command = new SqlCommand(sql, connection)){
                                command.Parameters.AddWithValue ("@Name", Name);
                                command.Parameters.AddWithValue ("@QuantityInStock", QuantityInStock);
                                command.Parameters.AddWithValue ("@Price", Price);

                                command.ExecuteNonQuery();
                            }

                        }
            }
            catch (Exception ex){
                    ErrorMessage = ex.Message;
                    return;

            }

            Response.Redirect("/Inventory_Products/Index");
            
        }

    }
}
