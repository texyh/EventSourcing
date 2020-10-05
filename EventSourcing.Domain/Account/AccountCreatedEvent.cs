using System;
using EventSourcing.Domain.Core;

namespace EventSourcing.Domain.Account
{
    public class AccountCreatedEvent : DomainEventBase<Guid>
    {

        public int AccountNumber {get; private set;}

        public string AccountOwner {get; private set;}


        public AccountCreatedEvent()
        {
            
        }

        public AccountCreatedEvent(Guid aggregateId, int accountNumber, string accountOwner) : base(aggregateId)
        {
            AccountOwner = accountOwner;
            AccountNumber = accountNumber;
        }

        public AccountCreatedEvent(Guid aggregateId, long aggregateVersion, int accountNumber, string accountOwner) : base(aggregateId, aggregateVersion)
        {
            AccountOwner = accountOwner;
            AccountNumber = accountNumber;
        }

        public override IDomainEvent<Guid> CreateWithAggregate(Guid aggregateId, long aggregateVersion) 
        {
            return new AccountCreatedEvent(aggregateId, aggregateVersion, AccountNumber, AccountOwner);
        }
    }
}