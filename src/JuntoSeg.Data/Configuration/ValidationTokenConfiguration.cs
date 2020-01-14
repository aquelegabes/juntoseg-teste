using JuntoSeg.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuntoSeg.Data.Configuration
{
    public class ValidationTokenConfiguration : AConfiguration<ValidationToken>
    {
        public override void Configure(EntityTypeBuilder<ValidationToken> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.IsUsed)
                .HasDefaultValue(false);

            builder.Property(p => p.Token)
                .IsRequired();

            builder.HasIndex(p => p.Token)
                .IsUnique();

            builder.Property(p => p.ExpirationSpan);

            builder.HasOne(token => token.User)
                .WithMany(users => users.ValidationTokens)
                .HasForeignKey(fk => fk.UserId);
        }
    }
}
