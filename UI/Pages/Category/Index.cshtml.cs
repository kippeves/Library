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

namespace UI.Pages.Category
{
    public class IndexModel : PageModel
    {
        private readonly LibraryContext _context;

        public IndexModel(LibraryContext context)
        {
            _context = context;
        }

        public IList<Categories> Categories { get;set; }
        public string Sort { get; set; }
        public async Task OnGetAsync()
        {
            Categories = await _context.Categories.AsNoTracking().ToListAsync();
        }

        public async Task<IActionResult> OnGetSort(string order)
        {
            switch (order.ToLower())
            {
                case "desc":
                    Categories = await _context.Categories.OrderByDescending(c=>c.Name).ToListAsync();
                    Sort = "desc";
                    return Page();
                case "asc":
                    Categories = await _context.Categories.OrderBy(c=>c.Name).ToListAsync();
                    Sort = "asc";
                    return Page();
                default:
                    return Redirect("/Categories/Index");
            }
        }
    }
}
