using Microsoft.Extensions.Configuration;
using Resume.Domain.Response;
using System.Threading.Tasks;

namespace Resume.Server.Services.EmailServices
{
    /// <summary>
    /// The singleton version is required for the hostedService which may need some of the operations
    /// </summary>
    public class SendGridEmailServiceSingleton : IEmailServiceSingleton
    {
        string apiKey;
        public SendGridEmailServiceSingleton(IConfiguration configuration)
        {
            apiKey = configuration.GetSection("SendGrid").GetValue<string>("ApiKey");
        }
        public async Task<Result> Send(string emailAddress, string emailSubject, string emailBody) => await SendGridEmailStrategy.Send(apiKey, emailAddress, emailSubject, emailBody);
    }
}
