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
    public class AttributeModel : PageModel
    {
        private readonly LibraryContext _context;

        public AttributeModel(LibraryContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Attributes Attributes { get; set; }

        [BindProperty]
        public Books Books { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id.HasValue) {
                return RedirectToPage("/Book/Index", new { id });
            } else return RedirectToPage("/Book/Index"); }

        public async Task<IActionResult> OnGetAdd(int? id)
        {
            if (id.HasValue)
            {
                    Attributes = new();
                    Books = await _context.Books
                        .Include(b => b.Attributes)
                            .ThenInclude(a => a.Names)
                        .Include(b => b.Author)
                        .Include(b => b.Category)
                        .AsNoTracking()
                        .SingleAsync(b => b.Id == id.Value);
                    Attributes.BookId = Books.Id;
                    return Page();
            }
            else return RedirectToPage("/Book/Index");
        }

        public IActionResult OnPostAddAsync() {
            if (Attributes.Names.Name.Length < 20)
            {
                AttributesNames name = _context.AttributesNames.SingleOrDefault(a => a.Name.Trim().ToLower() == Attributes.Names.Name.Trim().ToLower());
                if (name == null)
                {
                    name = Attributes.Names;
                    _context.AttributesNames.Add(name);
                    _context.SaveChanges();
                }

                Attributes UpdateAttribute = _context.Attributes.SingleOrDefault(a => a.BookId == Attributes.BookId && a.AttributeId == name.Id);
                if (UpdateAttribute == null)
                {
                    UpdateAttribute = new();
                    UpdateAttribute.AttributeId = name.Id;
                    UpdateAttribute.BookId = Attributes.BookId;
                    UpdateAttribute.Value = Attributes.Value;
                    _context.Attributes.Add(UpdateAttribute);
                }
                else
                {
                    UpdateAttribute.Value = Attributes.Value;
                    _context.Update(UpdateAttribute);
                }

                _context.SaveChanges();
                return RedirectToPage("/Book/Details", new { id = UpdateAttribute.BookId });
            }
            else {
                TempData["Name"] = Attributes.Names.Name;
                TempData["Value"] = Attributes.Value;
                TempData["CategoryAmountError"] = "Your attributename is too long. Maximum characters allowed is 20.";
                return RedirectToPage("/Book/Attribute","Add", new { id = Attributes.BookId });
            }
        }
        

        public async Task<IActionResult> OnGetRemoveAsync(int? book, int? attr)
        {
            if (book.HasValue &&attr.HasValue)
            {
                Attributes temp = await _context.Attributes.SingleOrDefaultAsync(a=>a.BookId == book.Value && a.AttributeId == attr.Value);
                _context.Attributes.Remove(temp);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("/Book/Details", new { id = book.Value });
        }
    }
}
