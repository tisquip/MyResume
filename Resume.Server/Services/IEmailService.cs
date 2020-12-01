using Resume.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resume.Server.Services
{
    public interface IEmailService
    {
        Task<Result> Send(string emailAddress, string emailBody);
    }
}
