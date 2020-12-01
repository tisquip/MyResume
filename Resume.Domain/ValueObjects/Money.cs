using Resume.Domain.BaseClasses;
using System;

namespace Resume.Domain.ValueObjects
{
    public class Money : ValueObjectBase<Money>
    {
        //Currency not there since its a simple fast project
        public decimal Amount { get; }
        protected Money()
        {
        }
        public Money(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            Amount = amount;
        }

        protected override bool EqualsCore(Money other)
        {
            return Amount == other.Amount;
        }

        protected override int GetHashCodeCore()
        {
            return HashCode.Combine(Amount);
        }


    }
}
