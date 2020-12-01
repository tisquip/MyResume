using Microsoft.Extensions.Configuration;
using Resume.Domain;
using Resume.Domain.Interfaces;
using Resume.Domain.Response;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resume.Server.Services
{
    public class ExceptionNotifier : IExceptionNotifier
    {
        private readonly IEmailService emailService;
        string adminEmail;

        public ExceptionNotifier(IEmailService emailService, IConfiguration configuration)
        {
            //TODO: Magic string
            adminEmail = configuration.GetValue<string>("AdminEmail"); 
            this.emailService = emailService;
        }
        public async Task Notify(Exception ex)
        {
            Log.Logger.Error("{@exception}", ex);
            try
            {
                await emailService.Send(adminEmail, $"An exception has occured on your Resume Project. {ex.Message} : {ex.SerializeToJson()}");
            }
            catch (Exception sendEx)
            {
                Log.Logger.Error("{@exception}", sendEx);

            }
        }

        public async Task Notify(Exception ex, Result result, bool setResultWithRealExceptionMessage = false)
        {
            string errorMessage = setResultWithRealExceptionMessage ? ex.Message : "An exception occured";
            result.SetError(errorMessage);
            await Notify(ex);
        }
    }
}
