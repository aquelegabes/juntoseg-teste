using JuntoSeg.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuntoSeg.Data.Configuration
{
    public class UserConfiguration : AConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.HasIndex(p => p.Email)
                .IsUnique();
            builder.Property(p => p.Email);
            builder.Property(p => p.Name)
                .IsRequired();
            builder.Property(p => p.Passw)
                .IsRequired();
        }
    }
}
