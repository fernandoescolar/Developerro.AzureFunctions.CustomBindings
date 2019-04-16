using Newtonsoft.Json;

namespace Developerro.AzureFunctions.CustomBindings
{
    public class MailMessage
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }
    }
}
