using EventSourcing.Domain.Account;
using EventSourcing.Domain.Account.ReadModel;
using EventSourcing.Domain.Core;
using EventSourcing.Domain.DataAccess;
using EventSourcing.Domain.DataAccess.EventStore;
using EventSourcing.Domain.DataAccess.ReadModel;
using EventSourcing.Domain.EventProcessor;
using EventSourcing.Infrastructure;
using EventSourcing.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace EventSourcing
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddDbContextPool<DataContext>(options => options.UseInMemoryDatabase("EventSourcing"));
            
            services.AddScoped<IRepository<Account, Guid>, EventSourcingRepository<Account, Guid>>();
            services.AddScoped<IEventStore, EventStore>();
            services.AddScoped<IRepository<AccountReadModel>, ReadRepository<AccountReadModel>>();
            services.AddScoped<IDomainEventHandler<AccountCreatedEvent>, AccountEventHandler>();
            services.AddScoped<IDomainEventHandler<FundDepositEvent>, AccountEventHandler>();
            services.AddScoped<IDomainEventHandler<WithdrawalEvent>, AccountEventHandler>();

            services.AddSingleton<AccountEventProcessor>(_ =>
            {
                var handlers = new Dictionary<string, IDomainEventHandler<object>>();
                //handlers.Add(typeof(AccountCreatedEvent).Name, _.GetService<IDomainEventHandler<AccountCreatedEvent>>());
                //handlers.Add(typeof(FundDepositEvent).Name, (IDomainEventHandler<object>)_.GetService<IDomainEventHandler<FundDepositEvent>>());
                //handlers.Add(typeof(WithdrawalEvent).Name, (IDomainEventHandler<object>)_.GetService<IDomainEventHandler<WithdrawalEvent>>());

                return new AccountEventProcessor(handlers);
            });

            services.AddSingleton<IProcessor>(_ => 
            {
                return _.GetService<AccountEventProcessor>();
            });

            services.AddScoped<IAcountCommandHandler, AccountCommandHandler>();

            var serviceProvider = services.BuildServiceProvider();

            var accountHandler = serviceProvider.GetService<IAcountCommandHandler>();
            var readRepo = serviceProvider.GetService<IRepository<AccountReadModel>>();

            var accountNumber =  accountHandler.CreateAsync("John doe").GetAwaiter().GetResult();
            Console.WriteLine("============== " + (readRepo.Get(x => x.Number == accountNumber).GetAwaiter().GetResult()).Balance);

            accountHandler.Deposit(400, accountNumber);
            Console.WriteLine("============== " + (readRepo.Get(x => x.Number == accountNumber).GetAwaiter().GetResult()).Balance);

            accountHandler.Withdraw(100, accountNumber);
            Console.WriteLine("============== " + (readRepo.Get(x => x.Number == accountNumber).GetAwaiter().GetResult()).Balance);

        }

        private void LogBalance(int accountNumber)
        {

        }
    }
}
