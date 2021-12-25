#nullable disable
using DB;
using DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace UI.Pages.Author
{
    public class DetailsModel : PageModel
    {
        private readonly LibraryContext _context;

        public DetailsModel(LibraryContext context)
        {
            _context = context;
        }

        public Authors Authors { get; set; }
        public List<Books> AuthorBooks { get; set; }
        public string Sort { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Authors = await _context.Authors.Include(b=>b.Book).FirstOrDefaultAsync(m => m.Id == id);
            AuthorBooks = Authors.Book.OrderBy(b => b.Title).ToList();

            if (Authors == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnGetSort(int? id, string order)
        {
            if (id.HasValue)
            {
                switch (order.ToLower())
                {
                    case "desc":
                        Authors = await _context.Authors.Include(b => b.Book).FirstOrDefaultAsync(m => m.Id == id);
                        AuthorBooks = Authors.Book.OrderByDescending(b => b.Title).ToList();
                        Sort = "desc";
                        return Page();
                    case "asc":
                        Authors = await _context.Authors.Include(b => b.Book).FirstOrDefaultAsync(m => m.Id == id);
                        AuthorBooks = Authors.Book.OrderBy(b => b.Title).ToList();
                        Sort = "asc";
                        return Page();
                    default:
                        return Redirect("/Category/Index");
                }
            }
            else return Redirect("/Category/Index");
        }
    }
}
