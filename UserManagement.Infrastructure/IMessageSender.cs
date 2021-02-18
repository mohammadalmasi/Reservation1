using System.Threading.Tasks;

namespace UserManagement.Infrastructure
{
    public interface IMessageSender
    {
        public Task SendEmailAsync(string toEmail, string subject, bool isMessageHtml , string userName, string token);
    }
}
