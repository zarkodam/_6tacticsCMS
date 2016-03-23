using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace _DataAccess.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Get
            (
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includes = null,
            int? page = null,
            int? pageSize = null
            );

        IQueryable<T> Include(string path);
        T GetById(object id);
        T GetById(int id);
        T GetById(int? id);
        void Insert(T entity);
        void InsertRange(IEnumerable<T> entity);
        void Update(T entityForUpdate);
        void Delete(object id);
        void Delete(int id);
        void Delete(int? id);
        void Delete(T entityForDelete);
    }
}