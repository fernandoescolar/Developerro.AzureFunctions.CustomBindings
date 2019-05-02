using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;

[assembly: WebJobsStartup(typeof(Developerro.AzureFunctions.CustomBindings.Startup))]

namespace Developerro.AzureFunctions.CustomBindings
{

    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddExtension<MailExtensionConfigProvider>();
            builder.AddExtension<KeyVaultExtensionConfigProvider>();
            builder.AddExtension<RedisSubExtensionConfigProvider>();
        }
    }
}
