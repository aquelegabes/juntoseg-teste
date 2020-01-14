using JuntoSeg.Data.Context;
using JuntoSeg.Domain.Entities;
using JuntoSeg.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuntoSeg.Data.Repositories
{
    public class RepositoryToken : RepositoryBase<ValidationToken>, IRepositoryToken
    {
        public RepositoryToken(JuntoSegContext context) : base (context) { }
    }
}
