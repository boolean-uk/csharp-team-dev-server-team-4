using exercise.wwwapi.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace exercise.wwwapi.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DataContext _db;

        public Repository(DataContext db)
        {
            _db = db;
            Table = _db.Set<T>();
        }

        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includeExpressions)
        {
            if (includeExpressions.Length != 0)
            {
                var set = includeExpressions
                    .Aggregate<Expression<Func<T, object>>, IQueryable<T>>
                        (Table, (current, expression) => current.Include(expression));
            }

            return Table.ToList();
        }

        public async Task<IEnumerable<T>> Get()
        {
            return await Table.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await Table.ToListAsync();
        }

        public T? GetById(object id)
        {
            return Table.Find(id);
        }

        public void Insert(T obj)
        {
            Table.Add(obj);
        }

        public void Update(T obj)
        {
            Table.Attach(obj);
            _db.Entry(obj).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            var existing = Table.Find(id);
            if (existing != null)
            {
                Table.Remove(existing);
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Table.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeExpressions)
        {
            if (includeExpressions.Length != 0)
            {
                var set = includeExpressions
                    .Aggregate<Expression<Func<T, object>>, IQueryable<T>>
                        (Table, (current, expression) => current.Include(expression));
            }

            return await Table.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            return await Table.FindAsync(id);
        }

        public async Task InsertAsync(T obj)
        {
            await Table.AddAsync(obj);
        }

        public async Task DeleteAsync(object id)
        {
            var existing = await Table.FindAsync(id);
            if (existing != null)
            {
                Table.Remove(existing);
            }
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public DbSet<T> Table { get; }
    }
}