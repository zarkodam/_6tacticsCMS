using _DataAccess.Contexts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace _DataAccess.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> Get
        (
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null
        )
        {
            IQueryable<TEntity> query = _dbSet;

            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            if (orderBy != null)
                query = orderBy(query);
            if (filter != null)
                query = query.Where(filter);
            if (page != null && pageSize != null)
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return query;
        }

        public virtual IQueryable<TEntity> Include(string path)
        {
            return _dbSet.Include(path);
        }

        public virtual TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual TEntity GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public virtual TEntity GetById(int? id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void InsertRange(IEnumerable<TEntity> entity)
        {
            _dbSet.AddRange(entity);
        }

        public virtual void Update(TEntity entityForUpdate)
        {
            _dbSet.Attach(entityForUpdate);
            _context.Entry(entityForUpdate).State = EntityState.Modified;
        }

        public virtual void Delete(int id)
        {
            var entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(int? id)
        {
            var entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(object id)
        {
            var entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityForDelete)
        {
            if (_context.Entry(entityForDelete).State == EntityState.Detached)
                _dbSet.Attach(entityForDelete);

            _dbSet.Remove(entityForDelete);
        }



    }
}