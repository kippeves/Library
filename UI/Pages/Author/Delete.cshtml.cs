#nullable disable
using DB;
using DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace UI.Pages.Author
{
    public class DeleteModel : PageModel
    {
        private readonly LibraryContext _context;

        public DeleteModel(LibraryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Authors Authors { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Authors = await _context.Authors.FirstOrDefaultAsync(m => m.Id == id);

            if (Authors == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Authors = await _context.Authors.Include(a=>a.Book).SingleAsync(a=>a.Id == id.Value);

            if (Authors != null)
            {
                foreach(Books b in Authors.Book )
                {
                    Books book = await _context.Books.Include(a => a.Author).SingleAsync(a => a.Id == b.Id);
                    if (book.Author.Count==1) {
                        _context.Remove(book);
                    }
                }
                _context.Authors.Remove(Authors);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
