using JuntoSeg.Data.Configuration;
using JuntoSeg.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuntoSeg.Data.Context
{
    public class JuntoSegContext : DbContext
    {
        public JuntoSegContext(string connectionString) : 
            base(new DbContextOptionsBuilder().UseSqlite(connectionString).Options) { }

        public JuntoSegContext(DbContextOptions opts) : base(opts) { }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
