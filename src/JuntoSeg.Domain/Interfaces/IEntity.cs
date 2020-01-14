using System;
using System.Collections.Generic;
using System.Text;

namespace JuntoSeg.Domain.Interfaces
{
    public interface IEntity
    {
        int Id { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}
