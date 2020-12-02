using Resume.Domain;
using Resume.Domain.Response;
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
            db.ChangeTracker.Clear(); //<-- DbContext is a singleton so dont want it having tracking info being just kept
            return vtr;
        }
    }
}
