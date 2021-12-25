#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DB;
using DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace UI.Pages.Book
{
    public class DetailsModel : PageModel
    {
        private readonly LibraryContext _context;

        [BindProperty]
        public Books Books { get; set; }

        public DetailsModel(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Books = await _context.Books
                .Include(a=>a.Attributes)
                .ThenInclude(c=>c.Names)
                .Include(a=>a.Author)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Books == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
