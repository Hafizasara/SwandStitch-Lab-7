
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace SewStitchLab7.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public DeleteModel(IConfiguration configuration)
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
            Product.Price = (decimal)r["Price"];

            return Page();
        }

        public IActionResult OnPost()
        {
            string cs = _configuration.GetConnectionString("DefaultConnection")!;

            using SqlConnection con = new SqlConnection(cs);
            con.Open();

            using SqlCommand cmd = new SqlCommand(
                "DELETE FROM dbo.Products WHERE ProductId=@Id", con);
            cmd.Parameters.AddWithValue("@Id", Product.ProductId);
            cmd.ExecuteNonQuery();

            return RedirectToPage("Index");
        }
    }
}
