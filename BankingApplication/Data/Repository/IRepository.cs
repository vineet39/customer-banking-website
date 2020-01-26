using System;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public interface IRepository<T>
    {
        IQueryable<T> GetByID(Expression<Func<T, bool>> expression);
        IQueryable<T> GetAll();
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
