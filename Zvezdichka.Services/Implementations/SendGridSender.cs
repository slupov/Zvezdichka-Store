using System;
using System.Collections.Generic;
using System.Text;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Zvezdichka.Services.Implementations
{
    public class SendGridSender
    {
        public static string SenderEmail { get; set; }
        public static string SenderName { get; set; }

        public static string RecipientEmail { get; set; }
        public static string RecipientName { get; set; }

        public static string Subject { get; set; }

        public static string PlainTextContent { get; set; }
        public static string HtmlContent { get; set; }

        public static async void  Send()
        {
            //TODO Stoyan Lupov 20 June, 2019 Extract to user secret
            var apiKey = "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";

            var client = new SendGridClient(apiKey);

            var msg = MailHelper.CreateSingleEmail(
                new EmailAddress(SenderEmail, SenderName),
                new EmailAddress(RecipientEmail, RecipientName),
                Subject,
                PlainTextContent,
                HtmlContent
            );
            
            var response = await client.SendEmailAsync(msg);
        }
    }
}
