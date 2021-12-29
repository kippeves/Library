using DB;
using DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace UI.Pages.Book
{
    public class IndexModel : PageModel
    {
        private LibraryContext _context { get; set; }
        public List<Books> Books { get; set; }
        [BindProperty]
        public string Term { get; set; }
        public string Sort { get; set; }

        public IndexModel(LibraryContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            Books = await _context.Books
                .AsNoTracking()
                .Include(b=>b.Author)
                .Include(b=>b.Category)
            .ToListAsync();
        }

        public async Task<IActionResult> OnPostSearch() {
            Books = await _context.Books
                .Include("Author")
                .Include("Category")
                .Include("Attributes")
            .Where(b => b.Title.ToLower().Contains(Term.ToLower())).ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnGetSort(string order)
        {
            switch (order.ToLower())
            {
                case "desc":
                    Books = await _context.Books.Include(b => b.Author).Include(b => b.Category).OrderByDescending(b=>b.Title).ToListAsync();
                    Sort = "desc";
                    return Page();
                case "asc":
                    Books = await _context.Books.Include(b => b.Author).Include(b => b.Category).OrderBy(b=>b.Title).ToListAsync();
                    Sort = "asc";
                    return Page();
                default:
                    return RedirectToPage("/Book/Index");
            }
        }
    }
}
