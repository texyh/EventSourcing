using EventSourcing.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcing.Domain.EventProcessor
{
    public interface IProcessor<TId>
    {
        void ProcessAsync(object @event, Type type);
    }
}
