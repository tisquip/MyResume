using Resume.Domain.Response;
using System;
using System.Threading.Tasks;

namespace Resume.Server.Services.EmailServices
{
    public static class SendGridEmailStrategy
    {
        public static Task<Result> Send(string apiKey, string apiId, string emailAddress, string emailBody)
        {
            //TODO: Implement
            throw new NotImplementedException();
        }
    }
}
