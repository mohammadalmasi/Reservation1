using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using UserManagement.Domain.Model;
using UserManagement.Domain.Repositories;

namespace UserManagement.Persistance.EF.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        UserManager<User> _userManager;
        SignInManager<User> _signInManager;

        public AccountRepository(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public User Get(Guid id)
        {
            var user = _userManager.Users.Where(x => x.Id == id).FirstOrDefault();
            return user;
        }
        public IList<User> GetAll()
        {
            var users = _userManager.Users.ToList();
            return users;
        }
        public async void DeleteAsync(User aggregate)
        {
            var result = await _userManager.DeleteAsync(aggregate);

            //using (var transaction = context.Database.BeginTransaction())
            //{
            //    IdentityResult result = IdentityResult.Success;
            //    foreach (var login in logins)
            //    {
            //        result = await _userManager.RemoveLoginAsync(aggregate, login.LoginProvider, login.ProviderKey);
            //        if (result != IdentityResult.Success)
            //            break;
            //    }
            //    if (result == IdentityResult.Success)
            //    {
            //        foreach (var item in rolesForUser)
            //        {
            //            result = await _userManager.RemoveFromRoleAsync(aggregate, item);
            //            if (result != IdentityResult.Success)
            //                break;
            //        }
            //    }
            //    if (result == IdentityResult.Success)
            //    {
            //        result = await _userManager.DeleteAsync(aggregate);
            //        if (result == IdentityResult.Success)
            //            transaction.Commit(); //only commit if user and all his logins/roles have been deleted  
            //    }
            //}
        }
        public async Task<Guid> CreateAsync(User aggregate)
        {
            IdentityResult identityResult = await _userManager.CreateAsync(aggregate, aggregate.Password);

            if (identityResult.Succeeded)
            {
                return aggregate.Id;
            }

            foreach (var error in identityResult.Errors)
            {

            }

            return aggregate.Id;
        }
        public IList<User> Get(Expression<Func<User, bool>> predicate)
        {
            throw new NotImplementedException();
        }


        public async Task<bool> SignOutAsync()
        {
            await _signInManager.SignOutAsync();
            return true;
        }
        public bool IsSignedIn(ClaimsPrincipal user)
        {
            if (_signInManager.IsSignedIn(user))
                return false;

            return true;
        }
        public async Task<User> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                //ایمیل وارد شده از قبل موجود است
                return user;
            }

            return user;
        }
        public async Task<bool> FindByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                //نام کاربری وارد شده از قبل موجود است
                return true;
            }

            return false;
        }
        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            return await _signInManager.GetExternalLoginInfoAsync();
        }
        public async Task<bool> ConfirmEmailAsync(string userName, string token)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(token))
                return false;

            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
                return false;

            var result = await _userManager.ConfirmEmailAsync(user, token);

            //return Content(result.Succeeded ? "Email Confirmed" : "Email Not Confirmed");

            return true;
        }
        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            string emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return emailConfirmationToken;
        }
        public async Task<IdentityResult> AddLoginAsync(User user, UserLoginInfo login)
        {
            return await _userManager.AddLoginAsync(user, login);
        }
        public AuthenticationProperties ExternalLogin(string provider, string redirectUrl)
        {
            return _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        }
        public void SignInAsync(User user, bool isPersistent, string authenticationMethod = null)
        {
            _signInManager.SignInAsync(user, isPersistent, authenticationMethod = null);
        }
        public async Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync()
        {
            return await _signInManager.GetExternalAuthenticationSchemesAsync();
        }
        public async Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor)
        {
            return await _signInManager.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent, bypassTwoFactor);
        }
        public async Task<bool> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure, ClaimsPrincipal user)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(userName, password, isPersistent, true);

            if (result.Succeeded)
            {
                return true;
            }

            if (result.IsLockedOut)
            {
                // اکانت شما به دلیل پنج بار ورود ناموفق به مدت پنج دقیق قفل شده است
                return false;
            }


            // رمزعبور یا نام کاربری اشتباه است
            return false;
        }
    }
}
