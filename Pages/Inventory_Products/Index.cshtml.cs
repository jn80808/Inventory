using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;

namespace Inventory1.Pages.Inventory_Products
{
    public class Index : PageModel
    {
        public List<ProductInfo> ProductInfoList { get; set; } = new List<ProductInfo>(); 

        public void OnGet()
        {
            try
            {
                string connectionString = "Server=DESKTOP-NSLMOTD;Database=Inventory;Trusted_Connection=True;TrustServerCertificate=True";

                // Attempting to connect to the database to test the connection string
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open(); // Open the connection
                    Console.WriteLine("Connection successful!");

                    string sqlSelect = "SELECT * FROM Products ORDER BY ProductId DESC"; // Updated query

                    using (SqlCommand command = new SqlCommand(sqlSelect, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductInfo productInfo = new ProductInfo(); 
                            
                            productInfo.Id = reader.GetInt32(0); // Mapping ProductId
                            productInfo.Name = reader.GetString(1); // Mapping Name
                            productInfo.QuantityInStock = reader.GetInt32(2); // Mapping QuantityInStock
                            productInfo.Price = reader.GetDecimal(3); // Mapping Price

                            ProductInfoList.Add(productInfo); // Add productInfo to the list
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("We Have an Error: " + ex.Message);
            }
        }
    }

    public class ProductInfo // custom data type
    {
        public int Id { get; set; }
        public string Name { get; set; } = ""; // Initializing the value
        public int QuantityInStock { get; set; } = 0;
        public decimal Price { get; set; } = 0.0m; // Use decimal for price
    }
}
