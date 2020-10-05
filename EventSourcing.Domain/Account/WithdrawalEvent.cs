using EventSourcing.Domain.Core;
using System;

namespace EventSourcing.Domain.Account
{
    public class WithdrawalEvent : DomainEventBase<Guid>
    {
        public double Amount { get; private set; }

        public WithdrawalEvent()
        {

        }

        public WithdrawalEvent(Guid aggregateId, double amount) : base(aggregateId)
        {
            Amount = amount;
        }

        public WithdrawalEvent(Guid aggregateId, long aggregateVersion, double amount) : base(aggregateId, aggregateVersion)
        {
            Amount = amount;
        }

        public override IDomainEvent<Guid> CreateWithAggregate(Guid aggregateId, long aggregateVersion)
        {
            return new WithdrawalEvent(aggregateId, aggregateVersion, Amount);
        }
    }
}