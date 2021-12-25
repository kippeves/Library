namespace DAL
{
    public interface IRepository<T> : IDisposable
    {
        public Task<T> GetById(int id);
        public void Add(T _object);
        public Task<IEnumerable<T>> GetAll();
        public void Remove(int id);
        public void Update(IEnumerable<T> _ObjectsToUpdate);
    }
}

