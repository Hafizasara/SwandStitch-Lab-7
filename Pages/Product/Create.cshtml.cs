using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SewStitchLab7.Models;

namespace SewStitchLab7.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public Models.Product Product { get; set; } = new();

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            string cs = _configuration.GetConnectionString("DefaultConnection")!;

            using SqlConnection con = new SqlConnection(cs);
            con.Open();

            string query = @"INSERT INTO dbo.Products
        (ProductName, Category, Material, Color, Size, Price, Stock)
        VALUES
        (@Name, @Category, @Material, @Color, @Size, @Price, @Stock)";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Name", Product.ProductName);
            cmd.Parameters.AddWithValue("@Category", Product.Category);
            cmd.Parameters.AddWithValue("@Material", (object?)Product.Material ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Color", (object?)Product.Color ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Size", (object?)Product.Size ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Price", Product.Price);
            cmd.Parameters.AddWithValue("@Stock", Product.Stock);

            cmd.ExecuteNonQuery();

            return RedirectToPage("/Index");
        }
    }
}
