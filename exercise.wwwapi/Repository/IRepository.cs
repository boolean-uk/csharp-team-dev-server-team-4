using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace exercise.wwwapi.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> Get();
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includeExpressions);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeExpressions);
        T GetById(object id);
        Task<T> GetByIdAsync(object id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(object id);
        void Save();
        Task InsertAsync(T obj);
        Task DeleteAsync(object id);
        Task SaveAsync();
        DbSet<T> Table { get; }

    }
}
