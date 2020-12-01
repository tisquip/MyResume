using Resume.Domain.Interfaces;
using Resume.Domain.Interfaces.RepoInterfaces;

namespace Resume.Server.Data.Repositories
{
    public interface IRootAggregateRepositorySingleton<T> : IRootAggregateRepository<T> where T : IRootAggregate
    {
    }
}
