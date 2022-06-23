using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;

namespace CalendarApp.Web.Services.EmailSendingService
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;

        public EmailSender(IOptions<SenderOptions> optionsAccessor,
                           ILogger<EmailSender> logger)
        {
            Options = optionsAccessor.Value;
            _logger = logger;
        }

        public SenderOptions Options { get; } //Set with Secret Manager.

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            if (string.IsNullOrEmpty(Options.SendGridKey))
            {
                throw new Exception("Null SendGridKey");
            }
            await Execute(Options.SendGridKey, subject, message, toEmail);
        }

        public async Task Execute(string apiKey, string subject, string message, string toEmail)
        {
            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) =>
                {
                    Console.WriteLine(e);
                    return true;
                };
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Connect("smtp.sendgrid.net", 465, true);

                client.Authenticate("apikey", "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");

                var body = new MimeKit.BodyBuilder();
                body.HtmlBody = message;
                body.TextBody = message;

                var message2 = new MimeKit.MimeMessage();
                message2.From.Add(new MimeKit.MailboxAddress("Demo", "mimozashopkoska@gmail.com"));
                message2.To.Add(new MimeKit.MailboxAddress("Demo", toEmail));
                message2.Subject = subject;
                message2.Body = body.ToMessageBody();
                var response = client.Send(message2);

                client.Disconnect(true);
            }
        }
    }
}
