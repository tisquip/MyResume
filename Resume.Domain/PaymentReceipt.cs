using Resume.Domain.BaseClasses;
using Resume.Domain.Response;
using Resume.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resume.Domain
{
    public class PaymentReceipt : RootAggregateBase
    {
        public DateTime Created { get; private set; } = DateTime.UtcNow;
        public Money Amount { get; private set; }
        public bool NotificationWasSent { get; private set; }
        protected PaymentReceipt()
        {
        }

        public PaymentReceipt(Money amount)
        {
            Amount = amount ?? throw new ArgumentNullException(nameof(amount));
        }

        public Result SetNotificationWasSent(bool notificationWasSent)
        {
            NotificationWasSent = notificationWasSent;
            return Result.Ok();
        }
    }
}
