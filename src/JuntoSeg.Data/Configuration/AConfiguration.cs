using JuntoSeg.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuntoSeg.Data.Configuration
{
    public abstract class AConfiguration<T> : IEntityTypeConfiguration<T>
        where T : class, IEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.CreatedAt)
                .IsRequired();
            builder.Property(p => p.CreatedAt)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.UpdatedAt)
                .ValueGeneratedOnUpdate();
        }
    }
}
