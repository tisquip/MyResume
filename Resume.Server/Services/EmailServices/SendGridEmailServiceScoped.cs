using Microsoft.Extensions.Configuration;
using Resume.Domain.Response;
using System.Threading.Tasks;

namespace Resume.Server.Services.EmailServices
{
    public class SendGridEmailServiceScoped : IEmailServiceScoped
    {
        string apiKey;
        public SendGridEmailServiceScoped(IConfiguration configuration)
        {
            apiKey = configuration.GetSection("SendGrid").GetValue<string>("ApiKey");
        }
        public async Task<Result> Send(string emailAddress, string emailSubject, string emailBody) => await SendGridEmailStrategy.Send(apiKey, emailAddress, emailSubject,  emailBody);
    }
}
