using System.Linq.Expressions;

namespace exercise.wwwapi.Repository;

public interface IRepository<T>
{
    Task<List<T>> GetWithIncludes(Func<IQueryable<T>, IQueryable<T>> includeQuery);
    Task<T> GetByIdWithIncludes(Func<IQueryable<T>, IQueryable<T>> includeQuery, int id);
    IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includeExpressions);
    Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeExpressions);
    T? GetById(object id, params Expression<Func<T, object>>[] includeExpressions);
    Task<T?> GetByIdAsync(object id, params Expression<Func<T, object>>[] includeExpressions);

    /// <remarks>
    /// There should be no async version of this as AddAsync is advised against.
    /// </remarks>
    void Insert(T obj);

    void Update(T obj);
    void Delete(T obj);
    void Save();
    Task SaveAsync();
}