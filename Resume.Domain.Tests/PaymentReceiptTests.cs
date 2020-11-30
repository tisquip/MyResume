using System;
using Xunit;

namespace Resume.Domain.Tests
{
    public class PaymentReceiptTests
    {
        
        [Theory]
        [InlineData(false, true)]
        [InlineData(true, true)] 
        public void SetNotificationWasSentTest_AssesReturnedResult(bool notificationWasSet, bool expectedResultSucceeded)
        {
            //Arrange
            PaymentReceipt paymentReceipt = new PaymentReceipt(new ValueObjects.Money(5));

            //Act
            var result = paymentReceipt.SetNotificationWasSent(notificationWasSet);
            bool actualResultSucceeded = result.Succeeded;

            //Assert
            Assert.Equal(expectedResultSucceeded, actualResultSucceeded);
        }

        [Theory]
        [InlineData(false, true)] // <-- Leaving one failing test just for demo purposes 
        [InlineData(true, true)]
        public void SetNotificationWasSentTest_AssessPropertyHasChanged(bool notificationWasSet, bool expectedNotificationPropertyValue)
        {
            //Arrange
            PaymentReceipt paymentReceipt = new PaymentReceipt(new ValueObjects.Money(8));

            //Act
            var result = paymentReceipt.SetNotificationWasSent(notificationWasSet);
            bool actualNotificationPropertyValue = paymentReceipt.NotificationWasSent;

            //Assert
            Assert.Equal(expectedNotificationPropertyValue, actualNotificationPropertyValue);
        }
    }
}
