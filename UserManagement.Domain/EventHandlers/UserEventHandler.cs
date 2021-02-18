using Framework.Core.Bus;
using System.Threading.Tasks;
using UserManagement.Domain.Events;
using UserManagement.Domain.Model;
using UserManagement.Domain.Repositories;

namespace UserManagement.Domain.EventHandlers
{
    public class UserEventHandler : IEventHandler<CreatedUserEvent>
    {
        private readonly IUserRepository _userRepository;
        public UserEventHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task Handle(CreatedUserEvent @event)
        {
            _userRepository.CreateAsync(new User
            {
                Email = @event.Email,
                UserName = @event.UserName,
                PhoneNumber = @event.PhoneNumber,
                PasswordHash = @event.PasswordHash,
                EmailConfirmed = @event.EmailConfirmed,
                PhoneNumberConfirmed = @event.PhoneNumberConfirmed,
            });
            return Task.CompletedTask;
        }
    }
}
