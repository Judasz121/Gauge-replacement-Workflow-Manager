using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace WorkflowManager.EFCoreLibrary.DataAccess
{
    public class EFRepository<T> where T : class
    {
        protected DbSet<T> DbSet;

        public DbContext _context;

        internal EFRepository(DbContext context)
        {
            _context = context;
            DbSet = _context.Set<T>();
        }

        public EntityEntry<T> Insert(T entity)
        {
           return DbSet.Add(entity);
        }

        public EntityEntry<T> Delete(T entity)
        {
           return DbSet.Remove(entity);
        }

        public EntityState Update(T entity)
        {
            return _context.Entry(entity).State = EntityState.Modified;
        }

        public IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public IQueryable<T> GetAll()
        {
            return DbSet;
        }

        public T GetById<PKType>(PKType id)
        {
            return DbSet.Find(id);
        }

        public EntityEntry<T> Attach(T entity)
        {
            return DbSet.Attach(entity);
        }

    }
}
