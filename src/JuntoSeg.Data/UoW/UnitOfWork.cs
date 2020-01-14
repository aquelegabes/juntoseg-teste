using JuntoSeg.Data.Context;
using JuntoSeg.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JuntoSeg.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly JuntoSegContext _context;

        public IRepositoryUser Users { get; }
        public IRepositoryToken Tokens { get; }

        public UnitOfWork(JuntoSegContext context,
            IRepositoryUser users,
            IRepositoryToken tokens)
        {
            _context = context;
            Users = users;
            Tokens = tokens;
        }

        public void Commit()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.SaveChanges();
                    transaction.Commit();
                }catch (Exception ex) { transaction.Rollback(); }
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
