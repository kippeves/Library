using DB;
using DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UI.Pages.Book
{
    public class CreateModel : PageModel
    {
        private readonly LibraryContext _context;
        private readonly IConfiguration _configuration;
        public SelectList AuthorSelect { get; set; }
        public SelectList CategorySelect { get; set; }
        [BindProperty]
        public Books Books { get; set; }
        [BindProperty]
        public int[] AuthorID{ get; set; }
        [BindProperty]
        public int[] CategoryID { get; set; }
        [BindProperty]
        public IFormFile Cover { get; set; }


        public CreateModel(IConfiguration configuration, LibraryContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            AuthorSelect = new SelectList(_context.Authors, "Id", "FullName");
            CategorySelect = new SelectList(_context.Categories, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            long size = Cover.Length;

            if (Cover.Length > 0)
            {
                var file = Path.GetRandomFileName() + Path.GetExtension(Cover.FileName);
                var filePath = Path.Combine(_configuration.GetSection("filepaths").GetSection("img").Value, file);
                using (var stream = System.IO.File.Create(filePath))
                {
                    await Cover.CopyToAsync(stream);
                }
                Books.Cover = file;
            }

            foreach (int i in AuthorID)
            { 
                Books.Author.Add(_context.Authors.Single(a => a.Id == i));
            }

            foreach (int i in CategoryID)
            {
                Books.Category.Add(_context.Categories.Single(c => c.Id == i));
            }

            _context.Books.Add(Books);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Book/Index");
        }
    }
}
