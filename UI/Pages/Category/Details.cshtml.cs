#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DB;
using DB.Models;

namespace UI.Pages.Category
{
    public class DetailsModel : PageModel
    {
        private readonly LibraryContext _context;
        [BindProperty]
        public List<Books> Books { get; set; }

        public DetailsModel(LibraryContext context)
        {
            _context = context;
        }

        public Categories Categories { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Categories = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
            Books = _context.Books.Where(b => b.Category.Contains(Categories)).ToList();

            if (Categories == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
