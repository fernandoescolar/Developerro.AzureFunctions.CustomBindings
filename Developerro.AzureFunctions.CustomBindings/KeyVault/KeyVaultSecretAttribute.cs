using System;
using System.Linq;
using Microsoft.Azure.WebJobs.Description;

namespace Developerro.AzureFunctions.CustomBindings
{
    [AttributeUsage(AttributeTargets.Parameter)]
    [Binding]
    public class KeyVaultSecretAttribute : Attribute
    {
        public KeyVaultSecretAttribute()
        { 
        }

        public KeyVaultSecretAttribute(string key)
        {
            Key = key;
        }

        [AppSetting]
        public string Connection { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Key { get; set; }

        internal void Autofill()
        {
            if (string.IsNullOrEmpty(Connection)) return;

            var values = Connection.Split(';').ToDictionary(x => x.Split('=')[0].Trim().ToLowerInvariant(), x => x.Split('=')[1].Trim());

            if (values.ContainsKey("client_id"))
            {
                ClientId = values["client_id"];
            }

            if (values.ContainsKey("client_secret"))
            {
                ClientSecret = values["client_secret"];
            }
        }
    }
}
