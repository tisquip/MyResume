using Resume.Domain;
using Resume.Domain.Response;
using Resume.Server.Services.EmailServices;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Resume.Server.Services.ExceptionNotifierServices
{
    public static class ExeptionNotifierStrategy
    {
        public static async Task Notify(Exception ex, IEmailService emailService, string adminEmailAddress)
        {
            Log.Logger.Error("{@exception}", ex);
            try
            {
                await emailService.Send(adminEmailAddress, "An exception has occured on your Resume Project", $"{ex.Message} : {ex.SerializeToJson()}");
            }
            catch (Exception sendEx)
            {
                Log.Logger.Error("{@exception}", sendEx);

            }
        }

        public static async Task Notify(Exception ex, IEmailService emailService, string adminEmailAddress, Result result, bool setResultWithRealExceptionMessage = false)
        {
            string errorMessage = setResultWithRealExceptionMessage ? ex.Message : "An exception occurred";
            result.SetError(errorMessage);
            await Notify(ex, emailService, adminEmailAddress);
        }
    }
}
