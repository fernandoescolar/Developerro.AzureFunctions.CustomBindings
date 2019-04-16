using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.WebJobs;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Developerro.AzureFunctions.CustomBindings
{
    public class KeyVaultSecretConverter : IAsyncConverter<KeyVaultSecretAttribute, string>
    {
        public async Task<string> ConvertAsync(KeyVaultSecretAttribute input, CancellationToken cancellationToken = default)
        {
            using (var client = CreateClient(input.ClientId, input.ClientSecret))
            {
                var secret = await client.GetSecretAsync(input.Key);
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
