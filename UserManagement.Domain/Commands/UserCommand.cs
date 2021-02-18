using Framework.Core.Commands;

namespace UserManagement.Domain.Commands
{
    public class UserCommand: Command
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
    }
}
