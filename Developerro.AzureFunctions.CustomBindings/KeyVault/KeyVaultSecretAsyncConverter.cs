using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.WebJobs;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Developerro.AzureFunctions.CustomBindings
{
    public class KeyVaultSecretAsyncConverter : IAsyncConverter<KeyVaultSecretAttribute, string>
    {
        public async Task<string> ConvertAsync(KeyVaultSecretAttribute input, CancellationToken cancellationToken = default)
        {
            input.Validate();
            using (var client = CreateClient(input.ClientId, input.ClientSecret))
            {
                var secret = await client.GetSecretAsync(input.SecretIdentifier, cancellationToken);
                return secret.Value;
            }
        }

        private static KeyVaultClient CreateClient(string clientId, string clientSecret)
        {
            return new KeyVaultClient(async (authority, resource, scope) =>
            {
                var authContext = new AuthenticationContext(authority);
                var clientCredential = new ClientCredential(clientId, clientSecret);
                var result = await authContext.AcquireTokenAsync(resource, clientCredential).ConfigureAwait(false);
                if (result == null)
                {
                    throw new InvalidOperationException("Failed to obtain the JWT token");
                }

                return result.AccessToken;
            });
        }
    }
}
