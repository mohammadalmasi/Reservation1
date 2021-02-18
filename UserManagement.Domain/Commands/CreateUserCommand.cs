namespace UserManagement.Domain.Commands
{
    public class CreateUserCommand : UserCommand
    {
        public CreateUserCommand(string email, string userName, string phoneNumber,
            bool emailConfirmed, string passwordHash, bool phoneNumberConfirmed)
        {
            Email = email;
            UserName = userName;
            PhoneNumber = phoneNumber;
            EmailConfirmed = emailConfirmed;
            PasswordHash = passwordHash;
            PhoneNumberConfirmed = PhoneNumberConfirmed;
        }
    }
}
