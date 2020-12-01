using Microsoft.EntityFrameworkCore;
using Resume.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resume.Server.Data
{
    public class ResumeBackgroundServiceDbContext : DbContext
    {
        public DbSet<FootballTeam> FootballTeam { get; set; }
        public ResumeBackgroundServiceDbContext(DbContextOptions<ResumeBackgroundServiceDbContext> options)
            : base(options)
        {
        }

    }
}
