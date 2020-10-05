using EventSourcing.Domain.Account;
using EventSourcing.Domain.Core;
using EventSourcing.Domain.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IReadOnlyRepository = EventSourcing.Domain.DataAccess.ReadModel.IRepository<EventSourcing.Domain.Account.ReadModel.AccountReadModel>;

namespace EventSourcing.Services
{
    public class AccountCommandHandler : IAcountCommandHandler
    {
        private readonly IRepository<Account, Guid> _accountReposiotry;

        private readonly IReadOnlyRepository _accountReadonlyRepository;

        private readonly IDomainEventHandler<AccountCreatedEvent> _accountCreatedHandler;

        private readonly IDomainEventHandler<FundDepositEvent> _fundsDepositHandler;

        private readonly IDomainEventHandler<WithdrawalEvent> _withdrawalEvent;

        public AccountCommandHandler(
            IRepository<Account, Guid> accountReposiotry,
            IDomainEventHandler<AccountCreatedEvent> accountCreatedHandlers,
            IReadOnlyRepository accountReadonlyRepository, 
            IDomainEventHandler<FundDepositEvent> fundsDepositHandler, 
            IDomainEventHandler<WithdrawalEvent> withdrawalEvent)
        {
            _accountReposiotry = accountReposiotry;
            _accountCreatedHandler = accountCreatedHandlers;
            _accountReadonlyRepository = accountReadonlyRepository;
            _fundsDepositHandler = fundsDepositHandler;
            _withdrawalEvent = withdrawalEvent;
        }

        public async Task<int> CreateAsync(string accountName)
        {
            var account = new Account(Guid.NewGuid(), GenerateAccountNumber(), accountName);

            await _accountReposiotry.SaveAsync(account);

            return account.AccountNumber;
        }

        public async Task Deposit(decimal amount, int accoutNumber)
        {
            var accountReadModel = await _accountReadonlyRepository.Get(x => x.Number == accoutNumber);

            if(accountReadModel == null)
            {
                throw new InvalidOperationException("The account number is invalid");
            }

            var account = await _accountReposiotry.GetByIdAsync(Guid.Parse(accountReadModel.Id));
            account.Deposit(amount);

            await _accountReposiotry.SaveAsync(account);
        }

        public async Task Withdraw(decimal amount, int accoutNumber)
        {
            var accountReadModel = await _accountReadonlyRepository.Get(x => x.Number == accoutNumber);

            if (accountReadModel == null)
            {
                throw new InvalidOperationException("The account number is invalid");
            }

            var account = await _accountReposiotry.GetByIdAsync(Guid.Parse(accountReadModel.Id));
            account.Withdraw(amount);

            await _accountReposiotry.SaveAsync(account);
        }

        private int GenerateAccountNumber()
        {
            Random random = new Random();
            string number = "";
            int i;
            for (i = 1; i < 5; i++)
            {
                number += random.Next(0, 9);
            }

            return int.Parse(number);
        }
    }
}
