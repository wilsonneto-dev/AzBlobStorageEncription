using Azure.Core.Cryptography;
using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BlobLab.Backend.Features.AddFile
{
    public interface IStorageService
    {
        Task SaveFile(string fileName, Stream stream);
        Task SaveFileEncripted(string blobFilePath, Stream stream);
    }

    public class StorageService : IStorageService
    {
        private readonly IConfiguration _configuration;
        public StorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SaveFile(string fileName, Stream stream)
        {
            string privateFilesContainer = _configuration.GetSection("Storage").GetValue<string>("PrivateContainer");
            BlobServiceClient blobServiceClient = new BlobServiceClient(_configuration.GetConnectionString("Storage"));
            BlobClient blobClient = blobServiceClient
                .GetBlobContainerClient(privateFilesContainer)
                .GetBlobClient(fileName);
            await blobClient.UploadAsync(stream, true);
        }

        public async Task SaveFileEncripted(string blobFilePath, Stream stream)
        {
            const string keyName = "keyv-key-bloblab-dev";
            var keyVaultSectionConfiguration = _configuration.GetSection("KeyVault");
            var keyVaultName = keyVaultSectionConfiguration.GetValue<string>("KeyVaultName");
            var AzureTenantId = keyVaultSectionConfiguration.GetValue<string>("AzureTenantId");
            var AzureClientSecret = keyVaultSectionConfiguration.GetValue<string>("AzureClientSecret");
            var AzureClientId = keyVaultSectionConfiguration.GetValue<string>("AzureClientId");
            var kvUri = $"https://{keyVaultName}.vault.azure.net";

            var credentials = new ClientSecretCredential(AzureTenantId, AzureClientId, AzureClientSecret);
            var keyClient = new KeyClient(new Uri(kvUri), credentials);
            var rasKey = (await keyClient.GetKeyAsync(keyName)).Value;
            IKeyEncryptionKey key = new CryptographyClient(rasKey.Id, credentials);
            IKeyEncryptionKeyResolver keyResolver = new KeyResolver(credentials);
            
            ClientSideEncryptionOptions encryptionOptions = new ClientSideEncryptionOptions(ClientSideEncryptionVersion.V1_0)
            {
                KeyEncryptionKey = key,
                KeyResolver = keyResolver,
                KeyWrapAlgorithm = "RSA1_5"
            };

            BlobClientOptions options = new SpecializedBlobClientOptions() { ClientSideEncryption = encryptionOptions };
            BlobServiceClient blobServiceClient = new BlobServiceClient(_configuration.GetConnectionString("Storage"), options);
            string encriptedFilesContainer = _configuration.GetSection("Storage").GetValue<string>("EncriptedFilesContainer");
            var container = blobServiceClient.GetBlobContainerClient(encriptedFilesContainer);
            BlobClient blobClient = container.GetBlobClient(blobFilePath);

            await blobClient.UploadAsync(stream, true);
        }
    }
}
