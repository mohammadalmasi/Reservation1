using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using UserManagement.Domain.Dto;

namespace UserManagement.Application.Interfaces
{
    public interface IAccountService
    {
        UserDto Get(Guid id);
        IList<UserDto> GetAll();
        void DeleteAsync(Guid id);
        Task<bool> SignOutAsync();
        Task<bool> CreateAsync(UserDto dto);
        Task<bool> IsEmailInUse(string email);
        Task<bool> IsUserNameInUse(string userName);
        Task<bool> ConfirmEmail(string userName, string token);
        AuthenticationProperties ExternalLogin(string provider, string redirectUrl);
        Task<bool> ExternalLoginCallBack(string returnUrl, string remoteError = null);
        Task<bool> SignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure, ClaimsPrincipal user);
    }
}
