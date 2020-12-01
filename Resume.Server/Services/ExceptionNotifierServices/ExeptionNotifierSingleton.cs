using Microsoft.Extensions.Configuration;
using Resume.Domain.Response;
using Resume.Server.Services.EmailServices;
using System;
using System.Threading.Tasks;

namespace Resume.Server.Services.ExceptionNotifierServices
{
    /// <summary>
    /// The singleton version is required for the hostedService which may need some of the operations
    /// </summary>
    public class ExeptionNotifierSingleton : IExceptionNotifierSingleton
    {
        private readonly IEmailServiceSingleton emailService;
        string adminEmail;

        public ExeptionNotifierSingleton(IEmailServiceSingleton emailService, IConfiguration configuration)
        {
            //TODO: Magic string
            adminEmail = configuration.GetValue<string>("AdminEmail");
            this.emailService = emailService;
        }
        public async Task Notify(Exception ex) => await ExeptionNotifierStrategy.Notify(ex, emailService, adminEmail);

        public async Task Notify(Exception ex, Result result, bool setResultWithRealExceptionMessage = false) => await ExeptionNotifierStrategy.Notify(ex, emailService, adminEmail);
    }
}
