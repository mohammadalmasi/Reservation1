using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using UserManagement.Application.Interfaces;
using UserManagement.Application.Sevices;
using UserManagement.Domain.Model;
using UserManagement.Domain.Repositories;
using UserManagement.Infrastructure;
using UserManagement.Persistance.EF;
using UserManagement.Persistance.EF.Repositories;

namespace UserManagement.Config
{
    public static class Bootstrapper
    {
        public static void WireUp(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication().AddGoogle(option =>
            { 
                option.ClientId = configuration.GetSection("GoogleConfig:ClientId").Value;
                option.ClientSecret = configuration.GetSection("GoogleConfig:ClientSecret").Value;
            });

            services.AddDbContextPool<UserManagmentDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SecurityConnectionString"));
            });

            services.AddIdentity<User, Role>(options =>
            {
                //Email must be uniq
                options.User.RequireUniqueEmail = true;
                //options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-";
                //چهار بار رمز رو اشتباه وترد کند اکانتش قفل میشود 
                options.Lockout.MaxFailedAccessAttempts = 4;
                //به مدت دو دقیقه اکانت کاربر قفل میشود
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
            })
            .AddEntityFrameworkStores<UserManagmentDbContext>()
            .AddDefaultTokenProviders()
            .AddErrorDescriber<PersianIdentityErrorDescriber>(); ;

            //Registration UserManagement.Persistance.EF
            services.AddScoped<IAccountRepository, AccountRepository>();

            //Registration Application Services
            services.AddScoped<IAccountService, AccountService>();
            //services.AddScoped<IUserService, UserService>();

            //Registration UserManagement.Infrastructure
            services.AddScoped<IMessageSender, MessageSender>();

        }
    }
}
