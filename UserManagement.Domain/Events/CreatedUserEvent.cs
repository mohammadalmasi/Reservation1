using Framework.Core.Events;

namespace UserManagement.Domain.Events
{
    public class CreatedUserEvent: Event
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public bool PhoneNumberConfirmed { get; set; }

        public CreatedUserEvent(string email, string userName, string phoneNumber,
            bool emailConfirmed, string passwordHash, bool phoneNumberConfirmed)
        {
            Email = email;
            UserName = userName;
            PhoneNumber = phoneNumber;
            PasswordHash = passwordHash;
            EmailConfirmed = emailConfirmed;
            PhoneNumberConfirmed = phoneNumberConfirmed;
        }
    }
}
