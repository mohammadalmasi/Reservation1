using Framework.Core.Bus;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using UserManagement.Domain.Commands;
using UserManagement.Domain.Events;

namespace UserManagement.Domain.CommandHandlers
{
    public class UserCreateCommandHandler : IRequestHandler<CreateUserCommand, bool>
    {
        readonly IEventBus _bus;

        public UserCreateCommandHandler(IEventBus bus)
        {
            _bus = bus;
        }

        public Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            //Publish Event to RabbitMQ
            _bus.Publish(new CreatedUserEvent(request.Email, request.UserName, request.PhoneNumber,
             request.EmailConfirmed, request.PasswordHash, request.PhoneNumberConfirmed));

            return Task.FromResult(true);
        }
    }
}
