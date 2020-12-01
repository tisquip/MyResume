using Resume.Domain.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Resume.Domain.Interfaces
{
    public interface IExceptionNotifier
    {
        Task Notify(Exception ex);
        Task Notify(Exception ex, Result result, bool setResultWithRealExceptionMessage = false);
    }
}
