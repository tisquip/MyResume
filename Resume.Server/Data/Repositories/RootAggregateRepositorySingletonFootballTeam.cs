using Resume.Domain;
using Resume.Domain.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Resume.Server.Data.Repositories
{
    public class RootAggregateRepositorySingletonFootballTeam : IRootAggregateRepositorySingleton<FootballTeam>
    {
        private readonly ResumeBackgroundServiceDbContext db;

        public RootAggregateRepositorySingletonFootballTeam(ResumeBackgroundServiceDbContext db)
        {
            this.db = db;
        }
        public Task<Result> Update(FootballTeam rootAggregate)
        {
            var vtr = RootAggregateRepositoryFootballTeamStategy.Update(db, rootAggregate);
            //db.ChangeTracker.Clear(); //<-- DbContext is a singleton so dont want it having tracking info being just kept

            db.Entry(rootAggregate).State = Microsoft.EntityFrameworkCore.EntityState.Detached; //Previous was throwing an error when I wanted to attach and update
            return vtr;
        }

        public Result<List<FootballTeam>> GetDetachedFromDatabase(Predicate<FootballTeam> predicate = null)
        {
            return RootAggregateRepositoryFootballTeamStategy.GetDetachedFromDatabase(db, predicate);
        }

        public async Task<Result> Insert(FootballTeam rootAggregate)
        {
            var vtr = await RootAggregateRepositoryFootballTeamStategy.Insert(db, rootAggregate);

            //db.ChangeTracker.Clear(); //<-- DbContext is a singleton so dont want it having tracking info being just kept weird wierd 

            db.Entry(rootAggregate).State = Microsoft.EntityFrameworkCore.EntityState.Detached; //Previous was throwing an error when I wanted to attach and update
            return vtr;
        }
    }
}
