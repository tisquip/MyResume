using Resume.Domain.Response;
using System;
using System.Threading.Tasks;

namespace Resume.Server.Services
{
    public class SendGridEmailServiceScoped : IEmailServiceScoped
    {
        public async Task<Result> Send(string emailAddress, string emailBody)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
    }
}
