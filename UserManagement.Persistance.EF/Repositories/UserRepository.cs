using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UserManagement.Domain.Model;
using UserManagement.Domain.Repositories;

namespace UserManagement.Persistance.EF.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManagmentDbContext _context;

        public UserRepository(UserManagmentDbContext context)
        {
            _context = context;
        }

        public User Get(Guid id)
        {
          var identityUser= _context.Users.FirstOrDefault();
            return new User
            {
                Email = identityUser.Email,
                UserName = identityUser.UserName,
                PhoneNumber = identityUser.PhoneNumber,
                PasswordHash = identityUser.PasswordHash,
                EmailConfirmed = identityUser.EmailConfirmed,
                PhoneNumberConfirmed = identityUser.PhoneNumberConfirmed,
            };
        }
        public IList<User> GetAll()
        {
            var identityUsers= new List<User>();
           
            foreach (var identityUser in _context.Users.ToList())
            {
                identityUsers.Add(new User {
                    Email = identityUser.Email,
                    UserName = identityUser.UserName,
                    PhoneNumber = identityUser.PhoneNumber,
                    PasswordHash = identityUser.PasswordHash,
                    EmailConfirmed = identityUser.EmailConfirmed,
                    PhoneNumberConfirmed = identityUser.PhoneNumberConfirmed,
                });
            }
            return identityUsers;
        }
        public void DeleteAsync(User aggregate)
        {
            var user = new User
            {
                Email = aggregate.Email,
                UserName = aggregate.UserName,
                PhoneNumber = aggregate.PhoneNumber,
                PasswordHash = aggregate.PasswordHash,
                EmailConfirmed = aggregate.EmailConfirmed,
                PhoneNumberConfirmed = aggregate.PhoneNumberConfirmed,
            };
            _context.Users.Remove(user);
        }
        public Task<Guid> CreateAsync(User aggregate)
        {
            var user = new User
            {
                Email = aggregate.Email,
                UserName=aggregate.UserName,
                PhoneNumber = aggregate.PhoneNumber,
                PasswordHash = aggregate.PasswordHash,
                EmailConfirmed = aggregate.EmailConfirmed,
                PhoneNumberConfirmed = aggregate.PhoneNumberConfirmed,
            };
            _context.Users.Add(user);
            return null;
        }
        public IList<User> Get(Expression<Func<User, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
