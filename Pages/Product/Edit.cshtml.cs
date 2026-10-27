
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace SewStitchLab7.Pages
{
    public class EditModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public EditModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public global::SewStitchLab7.Models.Product Product { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            string cs = _configuration.GetConnectionString("DefaultConnection")!;

            using SqlConnection con = new SqlConnection(cs);
            con.Open();

            using SqlCommand cmd = new SqlCommand(
                "SELECT * FROM dbo.Products WHERE ProductId=@Id", con);
            cmd.Parameters.AddWithValue("@Id", id);

            using SqlDataReader r = cmd.ExecuteReader();

            if (!r.Read())
                return NotFound();

            Product.ProductId = (int)r["ProductId"];
            Product.ProductName = (string)r["ProductName"];
            Product.Category = (string)r["Category"];
            Product.Material = r["Material"] as string;
            Product.Color = r["Color"] as string;
            Product.Size = r["Size"] as string;
            Product.Price = (decimal)r["Price"];
            Product.Stock = (int)r["Stock"];

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            string cs = _configuration.GetConnectionString("DefaultConnection")!;

            using SqlConnection con = new SqlConnection(cs);
            con.Open();

            string query = @"UPDATE dbo.Products SET
                ProductName=@Name, Category=@Category, Material=@Material,
                Color=@Color, Size=@Size, Price=@Price, Stock=@Stock
                WHERE ProductId=@Id";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Name", Product.ProductName);
            cmd.Parameters.AddWithValue("@Category", Product.Category);
            cmd.Parameters.AddWithValue("@Material", (object?)Product.Material ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Color", (object?)Product.Color ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Size", (object?)Product.Size ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Price", Product.Price);
            cmd.Parameters.AddWithValue("@Stock", Product.Stock);
            cmd.Parameters.AddWithValue("@Id", Product.ProductId);

            cmd.ExecuteNonQuery();

            return RedirectToPage("Index");
        }
    }
}
