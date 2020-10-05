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
            var serviceProvider = Bootstrap.BootstrapApp();
        }
    }
}
