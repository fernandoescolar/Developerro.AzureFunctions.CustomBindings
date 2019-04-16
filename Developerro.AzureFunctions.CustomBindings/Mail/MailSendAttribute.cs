using System;
using System.Linq;
using Microsoft.Azure.WebJobs.Description;

namespace Developerro.AzureFunctions.CustomBindings
{
    [AttributeUsage(AttributeTargets.ReturnValue | AttributeTargets.Parameter)]
    [Binding]
    public class MailSendAttribute : Attribute
    {
        [AppSetting]
        public string Connection { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public bool UseSsl { get; set; }

        internal void Autofill()
        {
            if (string.IsNullOrEmpty(Connection)) return;

            var values = Connection.Split(';').ToDictionary(x => x.Split('=')[0].Trim().ToLowerInvariant(), x => x.Split('=')[1].Trim());

            if (values.ContainsKey("host")) 
            {
                Host = values["host"]; 
            }

            if (values.ContainsKey("port") && int.TryParse(values["port"], out int port))
            {
                Port = port;
            }

            if (values.ContainsKey("user"))
            {
                User = values["user"];
            }

            if (values.ContainsKey("password"))
            {
                Password = values["password"];
            }

            if (values.ContainsKey("usessl") && bool.TryParse(values["usessl"], out bool useSsl))
            {
                UseSsl = useSsl;
            }

            if (string.IsNullOrEmpty(Host) || Port <= 0)
            {
                throw new ArgumentException("You should specify 'Host' and 'Port' SMTP connection parameters.");
            }
        }
    }
}
