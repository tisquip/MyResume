using Resume.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resume.Server.Services
{
    public class ExeptionNotifierSingleton : IExceptionNotifierSingleton
    {
        public Task Notify(Exception ex)
        {
            throw new NotImplementedException();
        }

        public Task Notify(Exception ex, Result result, bool setResultWithRealExceptionMessage = false)
        {
            throw new NotImplementedException();
        }
    }
}
