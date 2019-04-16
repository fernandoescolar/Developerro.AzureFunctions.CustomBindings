using Microsoft.Azure.WebJobs.Host.Config;
using Newtonsoft.Json.Linq;

namespace Developerro.AzureFunctions.CustomBindings
{
    public class MailExtensionConfigProvider : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            // add json to MailMessage mapper
            context.AddConverter<JObject, MailMessage>(input => input.ToObject<MailMessage>());

            // add output custom binding
            context
                .AddBindingRule<MailSendAttribute>()
                .BindToCollector(attr => new MailAsyncCollector(attr));
        }
    }
}
