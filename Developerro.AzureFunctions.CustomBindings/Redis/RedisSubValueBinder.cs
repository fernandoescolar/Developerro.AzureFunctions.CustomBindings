using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;

namespace Developerro.AzureFunctions.CustomBindings
{
    public class RedisSubValueBinder : IValueBinder
    {
        private object _value;

        public RedisSubValueBinder(object value)
        {
            _value = value;
        }

        public Type Type => typeof(string);

        public Task<object> GetValueAsync()
        {
            return Task.FromResult(_value);
        }

        public Task SetValueAsync(object value, CancellationToken cancellationToken)
        {
            _value = value;
            return Task.CompletedTask;
        }

        public string ToInvokeString()
        {
            return _value?.ToString();
        }
    }
}
