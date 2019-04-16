using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using MMessage = System.Net.Mail.MailMessage;

namespace Developerro.AzureFunctions.CustomBindings
{
    public class MailAsyncCollector : IAsyncCollector<MailMessage>
    {
        private readonly MailSendAttribute _binding;

        public MailAsyncCollector(MailSendAttribute binding)
        {
            _binding = binding;
            _binding.Autofill();
        }

        public async Task AddAsync(MailMessage item, CancellationToken cancellationToken = default)
        {
            using (var smtp = CreateSmtpClient(_binding))
            {
                var message = CreateMailMessage(item);
                await smtp.SendMailAsync(message);
            }
        }

        public Task FlushAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);
        }

        private static SmtpClient CreateSmtpClient(MailSendAttribute binding)
        {
            var smtp = new SmtpClient(binding.Host, binding.Port);
            smtp.EnableSsl = binding.UseSsl;

            if (!string.IsNullOrEmpty(binding.User) && !string.IsNullOrEmpty(binding.Password))
            {
                smtp.Credentials = new NetworkCredential(binding.User, binding.Password);
            }

            return smtp;
        }

        private static MMessage CreateMailMessage(MailMessage mail)
        {
            var from = new MailAddress(mail.From);
            var to = new MailAddress(mail.To);
            var message = new MMessage(from, to);
            message.Subject = mail.Subject;
            message.Body = mail.Body;

            return message;
        }
    }
}
