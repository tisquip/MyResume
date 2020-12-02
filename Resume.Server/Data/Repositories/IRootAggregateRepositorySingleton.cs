using Resume.Domain.Interfaces;
using Resume.Domain.Interfaces.RepoInterfaces;
using Resume.Domain.Response;
using System.Threading.Tasks;

namespace Resume.Server.Data.Repositories
{
    public interface IRootAggregateRepositorySingleton<T> : IRootAggregateRepository<T> where T : IRootAggregate
    {
    }
}
