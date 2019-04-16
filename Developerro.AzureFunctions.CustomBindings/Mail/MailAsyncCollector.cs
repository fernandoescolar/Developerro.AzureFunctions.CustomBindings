using System;
using System.Collections.Concurrent;
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
        private readonly ConcurrentQueue<MailMessage> _messages = new ConcurrentQueue<MailMessage>();

        public MailAsyncCollector(MailSendAttribute binding)
        {
            _binding = binding;
            _binding.Autofill();
        }

        public Task AddAsync(MailMessage item, CancellationToken cancellationToken = default)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (string.IsNullOrEmpty(item.From) || string.IsNullOrEmpty(item.To))
            {
                throw new InvalidOperationException("A 'From' and 'To' address must be specified for the message.");
            }

            _messages.Enqueue(item);

            return Task.CompletedTask;
        }

        public async Task FlushAsync(CancellationToken cancellationToken = default)
        {
            using (var smtp = CreateSmtpClient(_binding))
            {
                while (_messages.TryDequeue(out var message))
                {
                    var mailMessage = CreateMailMessage(message);
                    await smtp.SendMailAsync(mailMessage);
                }
            }
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
