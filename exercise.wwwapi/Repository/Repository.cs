using exercise.wwwapi.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace exercise.wwwapi.Repository;

public class Repository<T> : IRepository<T>  where T : class, IEntity
{
    private readonly DataContext _db;
    private readonly DbSet<T> _table;

    public Repository(DataContext db)
    {
        _db = db;
        _table = _db.Set<T>();
    }

    public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includeExpressions)
    {
        if (includeExpressions.Length != 0)
        {
            var set = includeExpressions
                .Aggregate<Expression<Func<T, object>>, IQueryable<T>>
                    (_table, (current, expression) => current.Include(expression));
            return set;
        }

        return _table.ToList();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _table.ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeExpressions)
    {
        if (includeExpressions.Length != 0)
        {
            var set = includeExpressions
                .Aggregate<Expression<Func<T, object>>, IQueryable<T>>
                    (_table, (current, expression) => current.Include(expression));
            return set;
        }

        return await _table.ToListAsync();
    }
    
    public T? GetById(object id, params Expression<Func<T, object>>[] includeExpressions)
    {
        if (includeExpressions.Length != 0)
        {
            IQueryable<T> query = _table;

            // Apply includes if any
            foreach (var include in includeExpressions)
            {
                query = query.Include(include);
            }

            // Use EF.Property to dynamically access the "Id" property
            return query.FirstOrDefault(e => EF.Property<object>(e, "Id").Equals(id));
        }

        return _table.Find(id);
    }

    public async Task<T?> GetByIdAsync(object id, params Expression<Func<T, object>>[] includeExpressions)
    {
        if (includeExpressions.Length != 0)
        {
            IQueryable<T> query = _table;

            // Apply includes if any
            foreach (var include in includeExpressions)
            {
                query = query.Include(include);
            }

            // Use EF.Property to dynamically access the "Id" property
            return await query.FirstOrDefaultAsync(e => EF.Property<object>(e, "Id").Equals(id));
        }

        return await _table.FindAsync(id);    }

    public void Insert(T obj)
    {
        _table.Add(obj);
    }

    public void Update(T obj)
    {
        _table.Attach(obj);
        _db.Entry(obj).State = EntityState.Modified;
    }

    public void Delete(object id)
    {
        throw new NotImplementedException();
    }

    public void Delete(T obj)
    {
        _table.Remove(obj);
    }

    public void Save()
    {
        _db.SaveChanges();
    }

    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }

    public async Task<List<T>> GetWithIncludes(Func<IQueryable<T>, IQueryable<T>>? includeQuery)
    {

        IQueryable<T> query = includeQuery != null ? includeQuery(_table) : _table;
        return await query.ToListAsync();
    }

    public async Task<T> GetByIdWithIncludes(Func<IQueryable<T>, IQueryable<T>>? includeQuery, int id)
    {
        IQueryable<T> query = includeQuery != null ? includeQuery(_table) : _table;
        var res = await query.Where(a => a.Id == id).FirstOrDefaultAsync();
        return res;
    }
}