using Framework.Core.Bus;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using UserManagement.Application.Interfaces;
using UserManagement.Application.Mapping;
using UserManagement.Domain.Dto;
using UserManagement.Domain.Repositories;
using UserManagement.Infrastructure;

namespace UserManagement.Application.Sevices
{
    public class AccountService : IAccountService
    {
        readonly IEventBus _bus;
        IMessageSender _messageSender;
        IAccountRepository _accountRepository;

        public AccountService(IEventBus bus, IMessageSender messageSender, IAccountRepository accountRepository)
        {
            _bus = bus;
            _messageSender = messageSender;
            _accountRepository = accountRepository;
        }

        public UserDto Get(Guid id)
        {
            var user = _accountRepository.Get(id);
            return user.ToViewModel();
        }
        public IList<UserDto> GetAll()
        {
            var users = _accountRepository.GetAll();
            return users.ToDto();
        }
        public void DeleteAsync(Guid id)
        {
            var user = _accountRepository.Get(id);
            _accountRepository.DeleteAsync(user);
        }
        public async Task<bool> SignOutAsync()
        {
            await _accountRepository.SignOutAsync();
            return true;
        }
        public async Task<bool> CreateAsync(UserDto dto)
        {
            var user = dto.ToModel();
            Guid userId = await _accountRepository.CreateAsync(user);

            if (userId != null)
            {
                string emailConfirmationToken = await _accountRepository.GenerateEmailConfirmationTokenAsync(user);

                string token = HttpUtility.UrlEncode(emailConfirmationToken);

                await _messageSender.SendEmailAsync(user.Email, "Email confirmation", true, user.UserName, token);

                return true;
            }

            //foreach (var error in identityResult.Errors)
            //{
            //    return false;
            //}

            return false;
        }
        public async Task<bool> IsEmailInUse(string email)
        {
            var user = await _accountRepository.FindByEmailAsync(email);
            if (user == null)
                return true;

            var tt = "ایمیل وارد شده از قبل موجود است";
            return false;
        }
        public async Task<bool> IsUserNameInUse(string userName)
        {
            var user = await _accountRepository.FindByNameAsync(userName);
            if (user == null)
                return true;

            var yy = "نام کاربری وارد شده از قبل موجود است";
            return false;
        }
        public async Task<bool> ConfirmEmail(string userName, string token)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(token))
                return false;

            var user = await _accountRepository.FindByNameAsync(userName);

            if (user == null)
                return false;

            var result = await _accountRepository.ConfirmEmailAsync(userName, token);

            //return Content(result.Succeeded ? "Email Confirmed" : "Email Not Confirmed");

            return result;
        }
        public AuthenticationProperties ExternalLogin(string provider, string redirectUrl)
        {
            return _accountRepository.ExternalLogin(provider, redirectUrl);
        }
        public async Task<bool> ExternalLoginCallBack(string returnUrl, string remoteError = null)
        {
            var loginDto = new LoginDto()
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _accountRepository.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                //ModelState.AddModelError("", $"Error : {remoteError}");
                //return View("Login", loginDto);
                return false;
            }

            var externalLoginInfo = await _accountRepository.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                //ModelState.AddModelError("ErrorLoadingExternalLoginInfo", $"مشکلی پیش آمد");
                //return View("Login", loginDto);
                return false;
            }

            var signInResult = await _accountRepository.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider,
                externalLoginInfo.ProviderKey, false, true);

            if (signInResult.Succeeded)
            {
                //return Redirect(returnUrl);
                return true;
            }

            var email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);

            if (email != null)
            {
                var user = await _accountRepository.FindByEmailAsync(email);
              
                var userDto = user.ToViewModel();
                if (user == null)
                {
                    var userName = email.Split('@')[0];

                    userDto.Email = email;
                    userDto.EmailConfirmed = true;
                    userDto.UserName = (userName.Length <= 10 ? userName : userName.Substring(0, 10));
 
                    await _accountRepository.CreateAsync(userDto.ToModel());
                }

                await _accountRepository.AddLoginAsync(userDto.ToModel(), externalLoginInfo);
                 _accountRepository.SignInAsync(userDto.ToModel(), false);

                //return Redirect(returnUrl);
                return true;
            }

            //ViewBag.ErrorTitle = "لطفا با بخش پشتیبانی تماس بگیرید";
            //ViewBag.ErrorMessage = $"دریافت کرد {externalLoginInfo.LoginProvider} نمیتوان اطلاعاتی از";
            return false;
        }
        public async Task<bool> SignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure, ClaimsPrincipal user)
        {
            if (_accountRepository.IsSignedIn(user))
                return false;

            var result = await _accountRepository.PasswordSignInAsync(userName, password, isPersistent, true, user);

            if (result)
            {
                return true;
            }

            if (!result)
            {
                var tt = "اکانت شما به دلیل پنج بار ورود ناموفق به مدت پنج دقیق قفل شده است";
                return false;
            }


            var yyy = "رمزعبور یا نام کاربری اشتباه است";
            return false;

        }
    }
}
