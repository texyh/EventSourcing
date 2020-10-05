using EventSourcing.Domain.Account.ReadModel;
using EventSourcing.Domain.DataAccess.EventStore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventSourcing.Infrastructure
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<EventEntity> Events { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventEntity>(entity =>
            {
                entity.HasKey(x => x.AggregateId);
            });

            modelBuilder.Entity<AccountReadModel>(entity =>
            {
                entity.HasKey(x => x.Id);
            });
        }

    }
}
