using Resume.Domain.BaseClasses;
using Resume.Domain.Response;
using Resume.Domain.ValueObjects;
using System;

namespace Resume.Domain
{
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
