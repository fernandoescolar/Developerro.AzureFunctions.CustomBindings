using System;
using Microsoft.Azure.WebJobs.Host.Config;
using Newtonsoft.Json.Linq;

namespace Developerro.AzureFunctions.CustomBindings
{
    public class MailExtensionConfigProvider : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            // add json to MailMessage mapper
            context.AddConverter<string, MailMessage>(input => JObject.Parse(input).ToObject<MailMessage>());
            context.AddConverter<JObject, MailMessage>(input => input.ToObject<MailMessage>());

            // add output custom binding
            var rule = context.AddBindingRule<MailSendAttribute>();
            rule.AddValidator(ValidateMailSendAttribute);
            rule.BindToCollector(attr => new MailAsyncCollector(attr));
        }

        private static void ValidateMailSendAttribute(MailSendAttribute attribute, Type parameterType)
        {
            attribute.Autofill();
            if (string.IsNullOrEmpty(attribute.Host) || attribute.Port <= 0)
            {
                throw new ArgumentException("You should specify 'Host' and 'Port' SMTP connection parameters.");
            }
        }
    }
}
