using JuntoSeg.Domain.Abstract;
using JuntoSeg.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuntoSeg.Domain.Entities
{
    public class User : AEntity, IEntity
    {
        public string Name { get; set; }
        public string Passw { get; set; }
        public string Email { get; set; }
        public virtual IEnumerable<ValidationToken> ValidationTokens { get; set; }
    }
}
