using DB;
using DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace UI.Pages.Book
{
    public class RemoveModel : PageModel
    {
        private readonly LibraryContext _context;

        public RemoveModel(LibraryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Books Books { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Books = await _context.Books.SingleOrDefaultAsync(b=>b.Id == id.Value);

            if (Books == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Books = _context.Books.SingleOrDefault(b => b.Id == id.Value);

            if (Books != null)
            {
                Books bookToRemove = _context.Books.SingleOrDefault(b => b.Id == id.Value);
                _context.Books.Remove(bookToRemove);
                _context.SaveChanges();
            }
            return RedirectToPage("/Book/Index");
        }
    }
}
