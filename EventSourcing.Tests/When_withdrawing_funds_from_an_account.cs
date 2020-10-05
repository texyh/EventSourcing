using EventSourcing.Domain.Account;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EventSourcing.Tests
{
    public class When_withdrawing_funds_from_an_account
    {
        [Fact]
        public void The_account_must_have_sufficient_balance()
        {
            var account = new Account(Guid.NewGuid(), 123548,  "John Doe");
            account.Deposit(500);
            Assert.Equal(500, account.AcountBalance);
            Assert.Throws<InsufficientFundsException>(() => account.Withdraw(600));
        }
    }
}
