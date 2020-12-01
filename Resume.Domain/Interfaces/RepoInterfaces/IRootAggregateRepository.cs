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
    }
}
