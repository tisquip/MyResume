using Microsoft.Extensions.Configuration;
using Resume.Domain.Response;
using System.Threading.Tasks;

namespace Resume.Server.Services.EmailServices
{
    public class SendGridEmailServiceScoped : IEmailServiceScoped
    {
        string apiKey;
        string apiId;
        public SendGridEmailServiceScoped(IConfiguration configuration)
        {
            apiId = configuration.GetSection("SendGrid").GetValue<string>("ApiId");
            apiKey = configuration.GetSection("SendGrid").GetValue<string>("ApiKey");
        }
        public async Task<Result> Send(string emailAddress, string emailBody) => await SendGridEmailStrategy.Send(apiKey, apiId, emailAddress, emailBody);
    }
}
