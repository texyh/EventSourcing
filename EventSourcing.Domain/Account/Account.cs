using System;
using EventSourcing.Domain.Core;

namespace EventSourcing.Domain.Account
{
    public class Account : AggregateBase<Guid>
    {
        private const double InitialBalance = 0.0;

        public int AccountNumber {get; private set;}

        public string AccountOwner {get; private set;}

        public double AcountBalance { get; private set; }

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

        public void Deposit(double amount)
        {
            if(amount == default(double))
            {
                throw new ArgumentException("You can not deposit below $1");
            }

            RaiseEvent(new FundDepositEvent(Id, amount));
        }

        public void Withdraw(double amount)
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