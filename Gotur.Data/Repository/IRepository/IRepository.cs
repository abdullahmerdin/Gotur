using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Gotur.Data.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //CREATE
        void Add(T entity);


        //READ
        T GetFirstOrDefault(Expression<Func<T, bool>> filter,
            string? includeProperties = null);

        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter,
            string? includeProperties = null);


        //UPDATE
        void Update(T entity);


        //DELETE
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
