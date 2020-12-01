using Microsoft.Extensions.Configuration;
using Resume.Domain.Response;
using Resume.Server.Services.EmailServices;
using System;
using System.Threading.Tasks;

namespace Resume.Server.Services.ExceptionNotifierServices
{
    public class ExceptionNotifierScoped : IExceptionNotifierScoped
    {
        private readonly IEmailServiceScoped emailService;
        string adminEmail;

        public ExceptionNotifierScoped(IEmailServiceScoped emailService, IConfiguration configuration)
        {
            //TODO: Magic string
            adminEmail = configuration.GetValue<string>("AdminEmail");
            this.emailService = emailService;
        }
        public async Task Notify(Exception ex) => await ExeptionNotifierStrategy.Notify(ex, emailService, adminEmail);

        public async Task Notify(Exception ex, Result result, bool setResultWithRealExceptionMessage = false) => await ExeptionNotifierStrategy.Notify(ex, emailService, adminEmail);
    }
}
