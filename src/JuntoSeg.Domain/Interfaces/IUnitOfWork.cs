using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JuntoSeg.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepositoryUser Users { get; }
        IRepositoryToken Tokens { get; }
        void Commit();
    }
}
