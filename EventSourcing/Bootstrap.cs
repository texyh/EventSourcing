using EventSourcing.Domain.Account;
using EventSourcing.Domain.Account.ReadModel;
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
using System.Text;

namespace EventSourcing
{
    public static class Bootstrap
    {
        public static IServiceProvider BootstrapApp(string dbName = "EventSourcing")
        {
            var services = new ServiceCollection();
            services.AddDbContextPool<DataContext>(options => options.UseInMemoryDatabase(dbName));

            services.AddScoped<IRepository<Account, Guid>, EventSourcingRepository<Account, Guid>>();
            services.AddScoped<IEventStore, EventStore>();
            services.AddScoped<IRepository<AccountReadModel>, ReadRepository<AccountReadModel>>();
            services.AddScoped<IDomainEventHandler<AccountCreatedEvent>, AccountEventHandler>();
            services.AddScoped<IDomainEventHandler<FundDepositEvent>, AccountEventHandler>();
            services.AddScoped<IDomainEventHandler<WithdrawalEvent>, AccountEventHandler>();

            services.AddSingleton<AccountEventProcessor<Guid>>(_ =>
            {
                var accountHandler = _.GetService<IDomainEventHandler<AccountCreatedEvent>>();
                var fundsHandler = _.GetService<IDomainEventHandler<FundDepositEvent>>();
                var withdrawlHandler = _.GetService<IDomainEventHandler<WithdrawalEvent>>();

                var processor = new AccountEventProcessor<Guid>();
                processor.RegisterApplier<AccountCreatedEvent>(async @event => await accountHandler.HandleAsync(@event));
                processor.RegisterApplier<FundDepositEvent>(async @event => await fundsHandler.HandleAsync(@event));
                processor.RegisterApplier<WithdrawalEvent>(async @event => await withdrawlHandler.HandleAsync(@event));

                return processor;
            });

            services.AddSingleton<IProcessor<Guid>>(_ =>
            {
                return _.GetService<AccountEventProcessor<Guid>>();
            });

            services.AddScoped<IAcountCommandHandler, AccountCommandHandler>();

            return services.BuildServiceProvider();
        }
    }
}
