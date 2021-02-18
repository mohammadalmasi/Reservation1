using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace UserManagement.Infrastructure
{
    public class MessageSender : IMessageSender
    {
        public Task SendEmailAsync(string toEmail, string subject, bool isMessageHtml ,string userName,string token)
        {
            using (var client = new SmtpClient())
            {
                var credentials = new NetworkCredential()
                {
                    Password = "sijhlfyizqqwqofj",
                    UserName = "mohammadalmasi.info", // without @gmail.com
                };

                client.Port = 587;
                client.EnableSsl = true;
                client.Host = "smtp.gmail.com";
                client.Credentials = credentials;

                string FilePath = @"E:\Projects\Exampels\Reservation\UserManagement.Infrastructure\EmailTemplates\ConfirmUserName.html";
                StreamReader str = new StreamReader(FilePath);
                string body = str.ReadToEnd();
                str.Close();

                body = body.Replace("{1}", token);
                body = body.Replace("{0}", userName);

                using var emailMessage = new MailMessage()
                {
                    Body = body,
                    Subject = subject,
                    IsBodyHtml = true,
                    To = { new MailAddress(toEmail) },
                    From = new MailAddress("mohammadalmasi.info@gmail.com"), // with @gmail.com
                };

                client.Send(emailMessage);
            }

            return Task.CompletedTask;
        }
    }
}
