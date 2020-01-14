using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuntoSeg.Domain.Interfaces
{
    public interface IRepositoryBase<T> : IDisposable
        where T: class, IEntity
    {
        IQueryable<T> Query { get; }
        Task<T> FindAsync(int id);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
