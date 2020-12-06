using Resume.Domain.Response;
using SendGrid;
using SendGrid.Helpers.Mail;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Resume.Server.Services.EmailServices
{
    public static class SendGridEmailStrategy
    {
        public static async Task<Result> Send(string apiKey, string emailAddress, string emailSubject, string emailBody)
        {
            Result vtr = new Result(true);
            try
            {
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("tisquip6@gmail.com", "Dev");
                var subject = emailSubject;
                var to = new EmailAddress(emailAddress);
                var plainTextContent = emailBody;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, "");
                var response = await client.SendEmailAsync(msg);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    vtr.SetSuccess();
                }
                
            }
            catch (Exception ex)
            {
                vtr.SetError("An exception occured whilst trying to send an email");
                Log.Logger.Error("In SendGridEmailStategy: {@ex}", ex);
            }
            return vtr;
        }
    }
}
