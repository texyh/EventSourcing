using EventSourcing.Domain.Account;
using EventSourcing.Domain.Account.ReadModel;
using EventSourcing.Domain.DataAccess.ReadModel;
using EventSourcing.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EventSourcing.Tests
{
    public class When_withdrawing_funds_from_an_account
    {
        [Fact]
        public void The_account_must_have_sufficient_balance()
        {
            var account = new Account(Guid.NewGuid(), 123548, "John Doe");
            account.Deposit(500);
            Assert.Equal(500, account.AcountBalance);
            Assert.Throws<InsufficientFundsException>(() => account.Withdraw(600));
        }


        [Fact]
        public async Task The_accounts_list_should_reflect_the_new_balance()
        {
            var serviceProvider = Bootstrap.BootstrapApp("testdb");
            var accountHandler = serviceProvider.GetService<IAcountCommandHandler>();
            var readModelRepository = serviceProvider.GetService<IRepository<AccountReadModel>>();

            var accountNumber = accountHandler.CreateAsync("John doe").GetAwaiter().GetResult();
            await accountHandler.Deposit(400, accountNumber);

            Assert.Equal(400, (await readModelRepository.Get(x => x.Number == accountNumber)).Balance);

            await accountHandler.Withdraw(150, accountNumber);
            Assert.Equal(250, (await readModelRepository.Get(x => x.Number == accountNumber)).Balance);
        }
    }
}
