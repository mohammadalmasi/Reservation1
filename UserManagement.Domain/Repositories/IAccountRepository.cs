using Framework.Domain.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using UserManagement.Domain.Model;

namespace UserManagement.Domain.Repositories
{
    public interface IAccountRepository : IRepository<Guid, User>
    {
        Task<bool> SignOutAsync();
        bool IsSignedIn(ClaimsPrincipal user);
        Task<User> FindByEmailAsync(string email);
        Task<bool> FindByNameAsync(string userName);
        Task<ExternalLoginInfo> GetExternalLoginInfoAsync();
        Task<bool> ConfirmEmailAsync(string userName, string token);
        Task<string> GenerateEmailConfirmationTokenAsync(User user);
        Task<IdentityResult> AddLoginAsync(User user, UserLoginInfo login);
        AuthenticationProperties ExternalLogin(string provider, string redirectUrl);
        Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync();
        void SignInAsync(User user, bool isPersistent, string authenticationMethod = null);
        Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor);
        Task<bool> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure, ClaimsPrincipal user);
    }
}
