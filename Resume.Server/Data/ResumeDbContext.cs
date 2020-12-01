using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Resume.Domain;

namespace Data
{
    public class ResumeDbContext : DbContext
    {
        public DbSet<PaymentReceipt> PaymentReceipt { get; set; }
        public DbSet<FootballTeam> FootballTeam { get; set; }
        public ResumeDbContext (DbContextOptions<ResumeDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PaymentReceipt>().OwnsOne(e => e.Amount);
        }
    }
}
