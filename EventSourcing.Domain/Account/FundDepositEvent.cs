using EventSourcing.Domain.Core;
using System;

namespace EventSourcing.Domain.Account
{
    public class FundDepositEvent : DomainEventBase<Guid>
    {
        public double Amount { get; private set; }

        public FundDepositEvent()
        {

        }

        public FundDepositEvent(Guid aggregateId, double amount) : base(aggregateId)
        {
            Amount = amount;
        }

        public FundDepositEvent(Guid aggregateId, long aggregateVersion, double amount) : base(aggregateId, aggregateVersion)
        {
            Amount = amount;
        }

        public override IDomainEvent<Guid> CreateWithAggregate(Guid aggregateId, long aggregateVersion)
        {
            return new FundDepositEvent(aggregateId, aggregateVersion, Amount);
        }
    }
}