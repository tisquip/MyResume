using Data;
using Resume.Domain;
using Resume.Domain.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Resume.Server.Data.Repositories
{
    public class RootAggregateRepositoryScopedFootballTeam : IRootAggregateRepositoryScoped<FootballTeam>
    {
        private readonly ResumeDbContext db;

        public RootAggregateRepositoryScopedFootballTeam(ResumeDbContext db)
        {
            this.db = db;
        }

        public Result<List<FootballTeam>> GetDetachedFromDatabase(Predicate<FootballTeam> predicate = null)
        {
            return RootAggregateRepositoryFootballTeamStategy.GetDetachedFromDatabase(db, predicate);
        }

        public async Task<Result> Insert(FootballTeam rootAggregate)
        {
            //Will fail with error result because inserting of teams is only from hosted service, a singleton
            return await RootAggregateRepositoryFootballTeamStategy.Insert(db, rootAggregate);
        }

        public Task<Result> Update(FootballTeam rootAggregate)
        {
            return RootAggregateRepositoryFootballTeamStategy.Update(db, rootAggregate);
        }
    }
}
