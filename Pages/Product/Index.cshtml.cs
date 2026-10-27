using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SewStitchLab7.Models;

namespace SewStitchLab7.Pages.Product
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Models.Product>
        Products
        { get; set; } = new();

        public void OnGet()
        {
            string cs = _configuration.GetConnectionString("DefaultConnection")!;

            using SqlConnection con = new SqlConnection(cs);
            con.Open();

            string query = "SELECT * FROM dbo.Products ORDER BY ProductId DESC";
            using SqlCommand cmd = new SqlCommand(query, con);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Products.Add(new Models.Product
                {
                    ProductId = (int)reader["ProductId"],
                    ProductName = (string)reader["ProductName"],
                    Category = (string)reader["Category"],
                    Material = reader["Material"] as string,
                    Color = reader["Color"] as string,
                    Size = reader["Size"] as string,
                    Price = (decimal)reader["Price"],
                    Stock = (int)reader["Stock"]
                });
            }
        }
    }
}