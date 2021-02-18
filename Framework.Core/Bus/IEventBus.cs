using Framework.Core.Commands;
using Framework.Core.Events;
using System.Threading.Tasks;

namespace Framework.Core.Bus
{
    public interface IEventBus
    {
        void Publish<T>(T @event) where T : Event;
        Task SendCommand<T>(T command) where T : Command;
        void Subscribe<T, TH>() where T : Event where TH : IEventHandler<T>;
    }
}
