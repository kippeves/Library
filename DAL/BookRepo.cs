using DAL;
using DB.Models;
using Microsoft.EntityFrameworkCore;

namespace DB
{
    public class BookRepo : IRepository<Books>, IDisposable
    {
        protected LibraryContext _context;
        private bool disposed = false;

        public BookRepo(LibraryContext context)
        {
            _context = context;
        }
        public void Add(Books _object)
        {
            _context.Books.Add(_object);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public async Task<IEnumerable<Books>> GetAll()
        {
            return await _context.Books
                .Include(a => a.Category)
                .Include(a => a.Author)
                .ToListAsync();
        }

        public async Task<Books> GetById(int id)
        {
            return await _context.Books
                .Include(a => a.Category)
                .Include(a => a.Author)
                .SingleAsync(a => a.Id == id);
        }

        public async void Remove(int id)
        {
            Books b = await GetById(id);
            _context.Books.Remove(b);

        }
        public void Update(IEnumerable<Books> _ObjectsToUpdate)
        {
            _context.Entry(_ObjectsToUpdate).State = EntityState.Modified;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
