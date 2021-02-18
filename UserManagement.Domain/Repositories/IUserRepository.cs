using Framework.Domain.Model;
using System;
using UserManagement.Domain.Model;

namespace UserManagement.Domain.Repositories
{
    public interface IUserRepository : IRepository<Guid,User>
    {
    }
}
