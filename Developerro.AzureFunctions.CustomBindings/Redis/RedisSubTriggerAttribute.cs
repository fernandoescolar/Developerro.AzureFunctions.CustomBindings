using System;
using Microsoft.Azure.WebJobs.Description;

namespace Developerro.AzureFunctions.CustomBindings
{
    [AttributeUsage(AttributeTargets.Parameter)]
    [Binding]
    public class RedisSubTriggerAttribute : Attribute
    {
        public string Connection { get; set; }

        public string Channel { get; set; }

        internal string GetConnectionString()
        {
            return Environment.GetEnvironmentVariable(Connection);
        }
    }
}
