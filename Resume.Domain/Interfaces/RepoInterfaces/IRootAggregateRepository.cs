using Resume.Domain.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Resume.Domain.Interfaces.RepoInterfaces
{
    /// <summary>
    /// Although domain should be not concerned with persistence, in this case, the compromise I am making is because I dont want to remember to Update an entity when it changes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRootAggregateRepository<T> where T: IRootAggregate
    {
        Task<Result> Update(T rootAggregate);

        //Added these so that I just totally avoid using the dbcontext directly, e.g in the IHosted service found in the web server. Though these methods are not called in the domain project itself.
        Task<Result> Insert(T rootAggregate);
        Result<List<T>> GetDetachedFromDatabase(Predicate<T> predicate = null);
    }
}
