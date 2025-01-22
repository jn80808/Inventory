using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Inventory1.Pages.Inventory_Products
{
    public class Delete : PageModel
    {
        public string ErrorMessage { get; set; } = ""; // Initial value of ErrorMessage is Empty

        public void OnGet()
        {
        }


        public IActionResult OnPost(int ProductId)
        {
            if (ProductId == 0)
            {
                ErrorMessage = "Invalid Product ID.";
                return Page();
            }

            bool isDeleted = DeleteProduct(ProductId);
            
            if (isDeleted)
            {
                return RedirectToPage("/Inventory_Products/Index"); // Redirect to index page on success
            }
            else
            {
                ModelState.AddModelError("", ErrorMessage); // Display error message
                return Page();
            }
        }

        private bool DeleteProduct(int ProductId)
        {
            try
            {
                string connectionString = "Server=DESKTOP-NSLMOTD;Database=Inventory;Trusted_Connection=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM Products WHERE ProductId = @ProductId"; // Check correct table name

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ProductId", ProductId);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return true; // Deletion successful
                        }
                        else
                        {
                            ErrorMessage = "Failed to delete the product.";
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                return false;
            }
        }
    }
}