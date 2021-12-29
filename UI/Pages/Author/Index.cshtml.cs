#nullable disable
using DB;
using DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace UI.Pages.Author
{
    public class IndexModel : PageModel
    {
        private readonly LibraryContext _context;
        public IList<Authors> Authors { get; set; }
        public string Sort { get; set; }

        public IndexModel(LibraryContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            Authors = await _context.Authors.AsNoTracking().Include(a=>a.Book).ToListAsync();
        }

        public async Task<IActionResult> OnGetSort(string order)
        {
            switch (order.ToLower())
            {
                case "desc":
                    Authors = await _context.Authors.Include(a => a.Book).OrderByDescending(a=>a.LastName).ToListAsync();
                    Sort = "desc";
                    return Page();
                case "asc":
                    Authors = await _context.Authors.Include(a => a.Book).OrderBy(a=>a.LastName).ToListAsync();
                    Sort = "asc";
                    return Page();
                default:
                    return Redirect("/Author/Index");
            }
        }
    }
}
