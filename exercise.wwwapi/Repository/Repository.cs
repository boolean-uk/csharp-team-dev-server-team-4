﻿using exercise.wwwapi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace exercise.wwwapi.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {


        private DataContext _db;
        private DbSet<T> _table = null;
       
        public Repository(DataContext db)
        {
            _db = db;
            _table = _db.Set<T>();
        }

        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includeExpressions)
        {
            if (includeExpressions.Any())
            {
                var set = includeExpressions
                    .Aggregate<Expression<Func<T, object>>, IQueryable<T>>
                     (_table, (current, expression) => current.Include(expression));
            }
            return _table.ToList();
        }
        public async Task<IEnumerable<T>> Get()
        {
            return _table.ToList();
        }
        public IEnumerable<T> GetAll()
        {
            return _table.ToList();
        }
        public T GetById(object id)
        {
            return _table.Find(id);
        }

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
            T existing = _table.Find(id);
            _table.Remove(existing);
        }


        public void Save()
        {
            _db.SaveChanges();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _table.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeExpressions)
        {
            if (includeExpressions.Any())
            {
                var set = includeExpressions
                    .Aggregate<Expression<Func<T, object>>, IQueryable<T>>
                     (_table, (current, expression) => current.Include(expression));
            }
            return await _table.ToListAsync();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _table.FindAsync(id);
        }

        public async Task InsertAsync(T obj)
        {
            await _table.AddAsync(obj);
        }

        public async Task DeleteAsync(object id)
        {
            T existing = await _table.FindAsync(id);
            _table.Remove(existing);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public DbSet<T> Table { get { return _table; } }

    }
}
