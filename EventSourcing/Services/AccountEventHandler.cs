using EventSourcing.Domain.Account;
using EventSourcing.Domain.Account.ReadModel;
using EventSourcing.Domain.DataAccess.ReadModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcing.Services
{
    public class AccountEventHandler : IDomainEventHandler<AccountCreatedEvent>, 
                                       IDomainEventHandler<FundDepositEvent>,
                                       IDomainEventHandler<WithdrawalEvent>

    {
        private readonly IRepository<AccountReadModel> _accountRepository;

        public AccountEventHandler(IRepository<AccountReadModel> accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task HandleAsync(AccountCreatedEvent @event)
        {
            await _accountRepository.InsertAsync(new AccountReadModel
            {
                Balance = 0.0,
                Id = @event.AggregateId.ToString(),
                Name = @event.AccountOwner,
                Number = @event.AccountNumber
            });
        }

        public async Task HandleAsync(FundDepositEvent @event)
        {
            var account = await _accountRepository.GetByIdAsync(@event.AggregateId.ToString());
            account.Balance += @event.Amount;

            await _accountRepository.UpdateAsync(account);
        }

        public async Task HandleAsync(WithdrawalEvent @event)
        {
            var account = await _accountRepository.GetByIdAsync(@event.AggregateId.ToString());
            account.Balance -= @event.Amount;

            await _accountRepository.UpdateAsync(account);
        }
    }
}
