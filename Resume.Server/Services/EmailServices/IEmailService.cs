using Resume.Domain.Response;
using System.Threading.Tasks;

namespace Resume.Server.Services.EmailServices
{
    public interface IEmailService
    {
        Task<Result> Send(string emailAddress, string emailSubject, string emailBody);
    }
}
