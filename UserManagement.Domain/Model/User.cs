using Framework.Domain.Model;
using Microsoft.AspNetCore.Identity;
using System;

namespace UserManagement.Domain.Model
{
    public class User : IdentityUser<Guid>, IAggregateRoot
    {
        public string Password { get; set; }
    }
    public class Role : IdentityRole<Guid>
    {
    }
    public class UserRole : IdentityUserRole<Guid>
    {
    }
    public class UserClaim : IdentityUserClaim<Guid>
    {
    }
    public class UserToken : IdentityUserToken<Guid>
    {
    }
    public class UserLogin : IdentityUserLogin<Guid>
    {
    }
    public class RoleClaim : IdentityRoleClaim<Guid>
    {

    }
}
