using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcing.Services
{
    public interface IAcountCommandHandler
    {
        Task<int> CreateAsync(string accountName);
        Task Deposit(decimal amount, int accoutNumber);
        Task Withdraw(decimal amount, int accoutNumber);
    }
}
