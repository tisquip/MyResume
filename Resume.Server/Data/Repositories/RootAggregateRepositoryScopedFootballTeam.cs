using Data;
using Resume.Domain;
using Resume.Domain.Response;
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
        public Task<Result> Update(FootballTeam rootAggregate)
        {
            return RootAggregateRepositoryFootballTeamStategy.Update(db, rootAggregate);
        }
    }
}
