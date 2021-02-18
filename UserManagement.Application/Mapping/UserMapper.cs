using System.Collections.Generic;
using UserManagement.Domain.Dto;
using UserManagement.Domain.Model;
using UserManagement.Presentation;

namespace UserManagement.Application.Mapping
{
    public static class UserMapper
    {
        public static User ToModel(this UserDto dto)
        {
            return new User
            {
                Email = dto.Email,
                UserName = dto.UserName,
                Password = dto.Password,
                PhoneNumber = dto.PhoneNumber,
            };
        }
        public static UserDto ToDto(this UserInVM inVM)
        {
            return new UserDto
            {
                Email=inVM.Email,
                Password = inVM.Password,
                UserName = inVM.UserName,
                PhoneNumber = inVM.PhoneNumber,
            };
        }
        public static UserDto ToViewModel(this User model)
        {
            return new UserDto
            {
                Email = model.Email,
                UserName = model.UserName,
                Password = model.Password,
                PhoneNumber = model.PhoneNumber,
            };
        }
        public static IList<UserDto> ToDto(this IList<User> models)
        {
            var userDtos = new List<UserDto>();
            foreach (var user in models)
            {
                userDtos.Add(new UserDto
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                });
            }
            return userDtos;
        }
        public static IList<UserOutVM> ToViewModel(this IList<User> models)
        {
            var userOutVMs = new List<UserOutVM>();
            foreach (var user in models)
            {
                userOutVMs.Add(new UserOutVM
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    PasswordHash = user.PasswordHash,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                });
            }
            return userOutVMs;
        }
    }
}
