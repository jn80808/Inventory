using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Inventory1.Pages.Inventory_Products
{
    public class Update : PageModel
    {
        [BindProperty] // Added Error Message if the fields are Empty
        public int ProductId { get; set; }

        [BindProperty, Required(ErrorMessage = "The Name is Required")] // Added Error Message if the fields are Empty
        public string Name { get; set; } = ""; // Initializing the value

        [BindProperty, Required(ErrorMessage = "The QuantityInStock is Required")]
        public int QuantityInStock { get; set; } = 0;

        [BindProperty, Required(ErrorMessage = "The Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "The Price must be a positive number.")]
        public decimal Price { get; set; } = 0.0m; // Use decimal for price

        public string ErrorMessage { get; set; } = ""; // Initial value of ErrorMessage is Empty

        //--------------------
        //--- Get -----------
        //--------------------
        public void OnGet(int ProductId)
        {
            try
            {
                string connectionString = "Server=DESKTOP-NSLMOTD;Database=Inventory;Trusted_Connection=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Products WHERE ProductId = @ProductId";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ProductId", ProductId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                this.ProductId = reader.GetInt32(0);
                                Name = reader.GetString(1);
                                QuantityInStock = reader.GetInt32(2);
                                Price = reader.GetDecimal(3);
                            }
                            else
                            {
                                Response.Redirect("/Inventory_Products/Index");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        //--------------------
        //--- Post -----------
        //--------------------
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();// Return the current page
            }

            try
            {
                string connectionString = "Server=DESKTOP-NSLMOTD;Database=Inventory;Trusted_Connection=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE Products SET Name = @Name, QuantityInStock = @QuantityInStock, Price = @Price WHERE ProductId = @ProductId";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Name", Name);
                        command.Parameters.AddWithValue("@QuantityInStock", QuantityInStock);
                        command.Parameters.AddWithValue("@Price", Price);
                        command.Parameters.AddWithValue("@ProductId", ProductId);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return RedirectToPage("/Inventory_Products/Index"); // Redirect back to the index page on success
                        }
                        else
                        {
                            ErrorMessage = "Failed to update the product.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return Page(); // Return the current page if there's an error
        }
    }
}
