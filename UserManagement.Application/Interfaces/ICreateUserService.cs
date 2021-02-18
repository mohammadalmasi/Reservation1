using System.Collections.Generic;
using UserManagement.Presentation;

namespace UserManagement.Application.Interfaces
{
    public interface ICreateUserService
    {
        IList<UserOutVM> GetAll();
        void Create(CreateUserInVM inVM);
    }
}
