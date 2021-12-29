using DB;
using DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace UI.Pages.Book
{
    public class EditModel : PageModel
    {
        private readonly LibraryContext _context;
        private readonly IConfiguration _configuration;
        [BindProperty]
        public Books FormBook { get; set; }
        public SelectList AuthorSelect { get; set;}
        public SelectList CategorySelect { get; set; }
        [Required]
        [BindProperty]
        public List<int> SelectedAuthor { get; set; }
        [Required]
        [BindProperty]
        public List<int> CategoryID { get; set; }
        public IFormFile Cover { get; set; }

        public EditModel(LibraryContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FormBook = _context.Books.Include("Author")
                                     .Include("Category")
                                     .FirstOrDefault(b => b.Id == id.Value);
            if (FormBook!=null) {
                AuthorSelect = new SelectList(_context.Authors, "Id", "FullName");
                CategorySelect = new SelectList(_context.Categories, "Id", "Name");
                SelectedAuthor = new List<int>();
                CategoryID = new List<int>();
                FormBook.Author.ToList().ForEach(a => SelectedAuthor.Add(a.Id));
                FormBook.Category.ToList().ForEach(c=> CategoryID.Add(c.Id));
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _context.Attach(FormBook).State = EntityState.Added;
            if (!ModelState.IsValid)
            {
                if (SelectedAuthor.Count == 0)
                {
                    TempData["AuthorError"] = "You have to select at least one author";
                }
                if (CategoryID.Count == 0)
                {
                    TempData["CategoryError"] = "You have to select at least one category.";
                }
                if (TempData.Count>0) {
                    return RedirectToPage("/Book/Edit", new { FormBook.Id });
                }
            }
            if (Cover!=null)
            {
                long size = Cover.Length;

                if (Cover.Length > 0)
                {
                    var file = Path.GetRandomFileName() + Path.GetExtension(Cover.FileName);
                    var filePath = Path.Combine(_configuration.GetSection("filepaths").GetSection("img").Value, file);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await Cover.CopyToAsync(stream);
                    }
                    FormBook.Cover = file;
                }
            }
            List<Authors> authors = new List<Authors>();
            SelectedAuthor.ForEach(i =>
                           authors.Add(_context.Authors.Single(a => a.Id == i))
            );
            List<Categories> categories = new List<Categories>();
            CategoryID.ForEach(i =>
                           categories.Add(_context.Categories.Single(c => c.Id == i))
            );
            _context.Entry(FormBook).Collection(c => c.Author).Load();
            _context.Entry(FormBook).Collection(c => c.Category).Load();
            FormBook.Author = authors;
            FormBook.Category = categories;
            _context.Update(FormBook);
            _context.SaveChanges();
            return RedirectToPage("/Book/Index");
        }
    }
}
