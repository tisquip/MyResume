namespace Resume.Server.Services.EmailServices
{
    /// <summary>
    /// The singleton version is required for the hostedService which may need some of the operations
    /// </summary>
    public interface IEmailServiceSingleton : IEmailService
    {
    }
}
