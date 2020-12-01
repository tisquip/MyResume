using Resume.Domain.Interfaces;
using Resume.Domain.Interfaces.RepoInterfaces;

namespace Resume.Server.Data.Repositories
{
    public interface IRootAggregateRepositoryScoped<T> : IRootAggregateRepository<T> where T : IRootAggregate
    {
    }
}
