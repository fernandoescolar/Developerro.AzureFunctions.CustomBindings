using System;
using Microsoft.Azure.WebJobs.Host.Config;

namespace Developerro.AzureFunctions.CustomBindings
{
    public class KeyVaultExtensionConfigProvider : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            var rule = context.AddBindingRule<KeyVaultSecretAttribute>();
            rule.AddValidator(ValidateKeyVaultSecretAttribute);
            rule.BindToInput(new KeyVaultSecretAsyncConverter());
        }

        private static void ValidateKeyVaultSecretAttribute(KeyVaultSecretAttribute attribute, Type parameterType)
        {
            attribute.Validate();
        }
    }
}
