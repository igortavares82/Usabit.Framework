using Standard.Framework.Seedworks.Concrete.Events;
using System;
using System.Threading.Tasks;

namespace Usabit.Framework.Seedworks.Abstraction.Events
{
    public interface IIntegrationEventHandler<TRequestEvent> : IDisposable where TRequestEvent : IntegrationEvent
    {
        Task Handle(TRequestEvent @event);
    }

    public interface IIntegrationEventHandler<TRequestEvent, TResponseEvent> : IDisposable where TRequestEvent : IntegrationEvent
                                                                                           where TResponseEvent : IntegrationEvent
    {
        Task<TResponseEvent> Handle(TRequestEvent @event);
    }
}
