using Resume.Domain.BaseClasses;
using Resume.Domain.Response;
using Resume.Domain.ValueObjects;
using System;

namespace Resume.Domain
{
    /// <summary>
    /// This is the only Domain class that tries to adhere to DDD principals, all other domain classes were produced fast because this is only a sample project and to invest time on the other classes is costly - also remember that when prototypes are needed fast, development has to be fast, then it can always be refactored later on.
    /// </summary>
    public class PaymentReceipt : RootAggregateBase
    {
        public DateTime Created { get; private set; } = DateTime.UtcNow;
        public Money Amount { get; private set; }
        public bool NotificationWasSent { get; private set; }
        public string ExternalRef { get; private set; }
        public string InternalRef { get; private set; } = Guid.NewGuid().ToString();
        protected PaymentReceipt()
        {
        }

        public PaymentReceipt(Money amount, string externalRef)
        {
            if (string.IsNullOrWhiteSpace(externalRef))
            {
                throw new ArgumentException($"'{nameof(externalRef)}' cannot be null or whitespace", nameof(externalRef));
            }

            Amount = amount ?? throw new ArgumentNullException(nameof(amount));
            ExternalRef = externalRef;
        }

        public Result SetNotificationWasSent(bool notificationWasSent)
        {
            NotificationWasSent = notificationWasSent;
            return Result.Ok();
        }
    }
}
