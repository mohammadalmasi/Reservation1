using Framework.Core.Bus;
using System.Collections.Generic;
using UserManagement.Application.Interfaces;
using UserManagement.Application.Mapping;
using UserManagement.Domain.Commands;
using UserManagement.Domain.Repositories;
using UserManagement.Presentation;

namespace UserManagement.Application.Sevices
{
    public class CreateUserService : ICreateUserService
    {
        readonly IEventBus _bus;
        readonly IUserRepository _userRepository;

        public CreateUserService(IEventBus bus, IUserRepository userRepository)
        {
            _bus = bus;
            _userRepository = userRepository;
        }
        public void Create(CreateUserInVM inVM)
        {
            var createUserCommand = new CreateUserCommand(inVM.Email, inVM.UserName, inVM.PhoneNumber, inVM.EmailConfirmed,
                inVM.PasswordHash, inVM.PhoneNumberConfirmed);

            _bus.SendCommand(createUserCommand);
        }

        public IList<UserOutVM> GetAll()
        {
            var users = _userRepository.GetAll();
            return users.ToViewModel();
        }
    }
}
