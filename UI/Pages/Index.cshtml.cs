using DB;
using DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UI.Pages
{
    public class IndexModel : PageModel
    {
        public IndexModel()
        {

        }

        public IActionResult OnGet()
        {
             return Page();
        }
    }
}
