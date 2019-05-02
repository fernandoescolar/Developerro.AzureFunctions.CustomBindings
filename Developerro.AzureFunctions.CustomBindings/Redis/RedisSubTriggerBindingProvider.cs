using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.WebJobs.Host.Triggers;

namespace Developerro.AzureFunctions.CustomBindings
{
    public class RedisSubTriggerBindingProvider : ITriggerBindingProvider
    {
        public Task<ITriggerBinding> TryCreateAsync(TriggerBindingProviderContext context)
        {
            var parameter = context.Parameter;
            var attribute = parameter.GetCustomAttribute<RedisSubTriggerAttribute>(false);

            if (attribute == null) return Task.FromResult<ITriggerBinding>(null);
            if (parameter.ParameterType != typeof(string)) throw new InvalidOperationException("Invalid parameter type");

            var triggerBinding = new RedisSubTriggerBinding(parameter);

            return Task.FromResult<ITriggerBinding>(triggerBinding);
        }
    }
}
