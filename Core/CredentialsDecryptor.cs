using System.Configuration;

namespace Filian.Core
{
    public static class CredentialsDecryptor
    {
        public static string DecryptCredentials()
        {
            string encryptedCredentials = ConfigurationManager.AppSettings.Get("encryptedCredentials");

            return CryptoService.CredentialCryptor.Decrypt(encryptedCredentials);
        }
    }
}