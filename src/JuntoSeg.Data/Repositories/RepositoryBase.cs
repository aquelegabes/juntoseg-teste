using JuntoSeg.Data.Context;
using JuntoSeg.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuntoSeg.Data.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T>
        where T: class, IEntity
    {
        private readonly JuntoSegContext _context;

        public RepositoryBase(JuntoSegContext context)
        {
            _context = context;
        }

        public IQueryable<T> Query => _context.Set<T>();

        public void Add(T entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            _context.Add(entity);
        }

        public async Task<T> FindAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Set<T>().Update(entity);
        }

        public void Dispose()
        {
            _context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
