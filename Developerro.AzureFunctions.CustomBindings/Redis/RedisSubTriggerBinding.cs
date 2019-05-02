using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Listeners;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Azure.WebJobs.Host.Triggers;

namespace Developerro.AzureFunctions.CustomBindings
{
    public class RedisSubTriggerBinding : ITriggerBinding
    {
        private readonly ParameterInfo _parameter;

        public RedisSubTriggerBinding(ParameterInfo parameter)
        {
            _parameter = parameter;
        }

        public Type TriggerValueType => typeof(string);

        public IReadOnlyDictionary<string, Type> BindingDataContract => new Dictionary<string, Type>();

        public Task<ITriggerData> BindAsync(object value, ValueBindingContext context)
        {
            var valueProvider = new RedisSubValueBinder(value);
            var bindingData = new Dictionary<string, object>();
            var triggerData = new TriggerData(valueProvider, bindingData);

            return Task.FromResult<ITriggerData>(triggerData);
        }

        public Task<IListener> CreateListenerAsync(ListenerFactoryContext context)
        {
            var executor = context.Executor;
            var attribute = _parameter.GetCustomAttribute<RedisSubTriggerAttribute>();
            var listener = new RedisSubListener(executor, attribute);

            return Task.FromResult<IListener>(listener);
        }

        public ParameterDescriptor ToParameterDescriptor()
        {
            return new TriggerParameterDescriptor
            {
                Name = _parameter.Name,
                DisplayHints = new ParameterDisplayHints
                {
                    Prompt = "RedisSub",
                    Description = "RedisSub message trigger"
                }
            };
        }
    }
}
