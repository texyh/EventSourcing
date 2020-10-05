using System;
using EventSourcing.Domain.Core;

namespace EventSourcing.Domain.Account
{
    public class Account : AggregateBase<Guid>
    {
        private const decimal InitialBalance = 0.0M;

        public int AccountNumber {get; private set;}

        public string AccountOwner {get; private set;}

        public decimal AcountBalance { get; private set; }

        private Account() { }

        public Account(Guid id, int accountNumber, string accountOwner)
        {
            if(accountNumber == default(int)) 
            {
                throw new ArgumentNullException(nameof(accountNumber));
            }

            if(string.IsNullOrEmpty(accountOwner)) 
            {
                throw new ArgumentNullException(nameof(accountOwner));
            }

            if(id == default(Guid)) 
            {
                throw new ArgumentNullException(nameof(id));
            }

            RaiseEvent(new AccountCreatedEvent(id, accountNumber, accountOwner));
        }

        public void Deposit(decimal amount)
        {
            if(amount <= default(decimal))
            {
                throw new ArgumentException("You can not deposit below $1");
            }

            RaiseEvent(new FundDepositEvent(Id, amount));
        }

        public void Withdraw(decimal amount)
        {
            if(amount > AcountBalance)
            {
                throw new InsufficientFundsException("you do not have sufficient funds to make this withdrawl");
            }

            RaiseEvent(new WithdrawalEvent(Id, amount));
        }

        internal void Apply(AccountCreatedEvent @event) 
        {
            Id = @event.AggregateId;
            AccountNumber = @event.AccountNumber;
            AccountOwner = @event.AccountOwner;
            AcountBalance = InitialBalance;
        }

        internal void Apply(WithdrawalEvent @event)
        {
            AcountBalance -= @event.Amount;
        }

        internal void Apply(FundDepositEvent @event)
        {
            AcountBalance += @event.Amount;
        }

        protected override void RegisterAppliers()
        {
            this.RegisterApplier<AccountCreatedEvent>(this.Apply);
            this.RegisterApplier<FundDepositEvent>(this.Apply);
            this.RegisterApplier<WithdrawalEvent>(this.Apply);
        }
    }
}