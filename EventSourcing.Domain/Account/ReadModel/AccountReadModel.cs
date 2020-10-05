using EventSourcing.Domain.DataAccess.ReadModel;

namespace EventSourcing.Domain.Account.ReadModel
{
    public class AccountReadModel : IReadEntity
    {
        public string Id {get; set;}

        public int Number {get; set;}

        public decimal Balance {get; set;}

        public string Name {get; set;}
    }
}