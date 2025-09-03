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
        T GetById(int id);
        Task<T> GetByIdAsync(int id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(int id);
        void Save();
        Task InsertAsync(T obj);
        Task<T> DeleteAsync(int id);
        Task SaveAsync();
        DbSet<T> Table { get; }

    }
}
