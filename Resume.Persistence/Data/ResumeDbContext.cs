using Microsoft.EntityFrameworkCore;
using Resume.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resume.Persistence.Data
{
    public class ResumeDbContext : DbContext
    {
        public DbSet<PaymentReceipt> PaymentReceipt { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PaymentReceipt>().OwnsOne(p => p.Amount);
        }
    }
}
