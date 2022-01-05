#nullable disable
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

        public IActionResult OnGetAdd(int? id)
        {
            if (id.HasValue)
            {
                    Attributes = new();
                    _context.Attach(Attributes).State = EntityState.Added;
                    Attributes.BookId = id.Value;
                    _context.Entry(Attributes).Reference(a => a.Book).Load();

                return Page();
            }
            else return RedirectToPage("/Book/Index");
        }

        public IActionResult OnPostAddAsync() {

            if (Attributes.AttributeName.Name.Length < 20)
            {
                AttributesNames name = _context.AttributesNames.SingleOrDefault(a => a.Name.Trim().ToLower() == Attributes.AttributeName.Name.Trim().ToLower());
                if (name == null)
                {
                    name = Attributes.AttributeName;
                    _context.AttributesNames.Add(name);
                }
                Attributes.AttributeName = name;
                Attributes UpdateAttribute = _context.Attributes.SingleOrDefault(a => a.BookId == Attributes.BookId && a.AttributeId == name.Id);
                if (UpdateAttribute == null)
                {
                    UpdateAttribute = Attributes;
                    _context.Attributes.Add(UpdateAttribute);
                }
                else
                {
                    UpdateAttribute.Value = Attributes.Value;
                    _context.Update(UpdateAttribute);
                }
                _context.SaveChanges();
                return RedirectToPage("/Book/Details", new { id = Attributes.BookId });
            } else {
                TempData["Name"] = Attributes.Name;
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
