using Gotur.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Gotur.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        //Kayıt ekleme
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        //Sorgudan ilk kaydı alma
        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string includeProperties = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

       
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.FirstOrDefault();
        }

        //Sorgudan tüm kayıtları alma
        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter, string includeProperties = null)
        {
            IQueryable<T> query = _dbSet;

            //Query filtreleme
            if (filter != null)
            {
                query= query.Where(filter);
            }

            //Propertyleri ayırma
            if (includeProperties!=null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query= query.Include(includeProperty);
                }
            }
            return query.ToList();
        }

        //Kayıt güncelleme
        public void Update(T entity)
        {
          _dbSet.Update(entity);
        }

        //Kayıt silme
        public void Remove(T entity)
        {
           _dbSet.Remove(entity);
        }

        //Çoklu kayıt silme
        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}
