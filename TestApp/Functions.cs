using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Developerro.AzureFunctions.CustomBindings;

namespace TestApp
{
    public static class Functions
    {
        [FunctionName("TestMailSend")]
        public static IActionResult TestMailSend(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [MailSend(Connection = "SmtpConnectionString")] out MailMessage message,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            message = new MailMessage();
            message.From = "noreply@tokiota.com";
            message.To = "fernando.escolar@tokiota.com";
            message.Subject = "Binding Demo";
            message.Body = $"Deal with it {name}!";


            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }

        [FunctionName("TestKeyVaultSecret")]
        public static IActionResult TestKeyVaultSecret(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
           [KeyVaultSecret("https://my.vault.azure.net/secrets/MyKey", Connection = "KeyVaultConnectionString")] string secret,
           ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult($"Your secret: {secret}");
        }
    }
}
