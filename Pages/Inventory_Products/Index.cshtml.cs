using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Inventory1.Pages.Inventory_Products
{
    public class Index : PageModel
    {
        public List<ProductInfo> ProductInfoList { get; set; } = new List<ProductInfo>(); 
        public decimal TotalInventoryValue { get; private set; }  // Property to hold total value

        public void OnGet()
        {
            try
            {
                string connectionString = "Server=DESKTOP-NSLMOTD;Database=Inventory;Trusted_Connection=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlSelect = "SELECT * FROM Products ORDER BY ProductId";
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

                            ProductInfoList.Add(productInfo); 
                        }
                    }
                }

                // Calculate total inventory value
                TotalInventoryValue = GetTotalValue();
            }
            catch (Exception ex)
            {
                Console.WriteLine("We Have an Error: " + ex.Message);
            }
        }

        private decimal GetTotalValue()
        {
            decimal total = 0;
            foreach (var product in ProductInfoList)
            {
                total += product.QuantityInStock * product.Price; // Total = quantity * price
            }
            return total;
        }
    }

    public class ProductInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } = ""; 
        public int QuantityInStock { get; set; } = 0;
        public decimal Price { get; set; } = 0.0m;
    }
}
