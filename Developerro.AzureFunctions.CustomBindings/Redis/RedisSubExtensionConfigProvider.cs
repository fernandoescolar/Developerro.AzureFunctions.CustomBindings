using Microsoft.Azure.WebJobs.Host.Config;

namespace Developerro.AzureFunctions.CustomBindings
{
    public class RedisSubExtensionConfigProvider : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            var rule = context.AddBindingRule<RedisSubTriggerAttribute>();
            rule.BindToTrigger(new RedisSubTriggerBindingProvider());
        }
    }
}
