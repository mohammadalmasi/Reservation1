using Framework.Core.Events;
using System.Threading.Tasks;

namespace Framework.Core.Bus
{
    public interface IEventHandler
    { }
    public interface IEventHandler<in TEvent> : IEventHandler where TEvent : Event
    {
        Task Handle(TEvent @event);
    }
}
