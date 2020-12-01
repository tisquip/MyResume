using Resume.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resume.Server.Services
{
    public class SendGridEmailService : IEmailService
    {
        public async Task<Result> Send(string emailAddress, string emailBody)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
    }
}
