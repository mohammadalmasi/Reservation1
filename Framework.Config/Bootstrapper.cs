using Framework.Core.Bus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Config
{
    public static class Bootstrapper
    {
        public static void WireUp(IServiceCollection services)
        {
            //Domain Bus
            services.AddSingleton<IEventBus, RabbitMQBus>(sp =>
            {
                var scopFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitMQBus(sp.GetService<IMediator>(), scopFactory);
            });
        }
    }
}
