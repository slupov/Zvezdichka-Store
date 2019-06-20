using System.Threading.Tasks;
using Zvezdichka.Services.Contracts;

namespace Zvezdichka.Services.Implementations
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            SendGridSender.SenderEmail = "zvezdichka.online@gmail.com";
            SendGridSender.SenderName  = "Zvezdichka Store";

            SendGridSender.RecipientEmail = "stoyan.lupov@gmail.com";
            SendGridSender.RecipientName  = "Stoyan Lupov";

            SendGridSender.Subject = "Testing Send grid emails";

            SendGridSender.PlainTextContent = "Testing plain text";
            SendGridSender.HtmlContent = "<strong>Testing HTML content of email</strong>";

            SendGridSender.Send();

            return Task.CompletedTask;
        }
    }
}
