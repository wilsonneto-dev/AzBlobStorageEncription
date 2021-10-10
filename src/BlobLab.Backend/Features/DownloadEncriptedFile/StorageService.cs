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

namespace BlobLab.Backend.Features.DownloadEncriptedFile
{
    public interface IStorageService
    {
        public Task<DownloadInfoDTO> DownloadEncriptedFile(string blobFilePath);
    }

    public class DownloadInfoDTO
    {
        public string ContentType { get; set; }
        public long Length { get; set; }
        public MemoryStream MemoryStream { get; set; }
    }

    public class StorageService : IStorageService
    {
        private readonly IConfiguration _configuration;
        public StorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<DownloadInfoDTO> DownloadEncriptedFile(string blobFilePath)
        {
            const string keyName = "keyv-key-bloblab-dev";
            var keyVaultName = _configuration.GetSection("KeyVault").GetValue<string>("KeyVaultName");
            var AzureTenantId = _configuration.GetSection("KeyVault").GetValue<string>("AzureTenantId");
            var AzureClientSecret = _configuration.GetSection("KeyVault").GetValue<string>("AzureClientSecret");
            var AzureClientId = _configuration.GetSection("KeyVault").GetValue<string>("AzureClientId");
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
            var memoryStream = new MemoryStream();
            
            var fileinfo = await blobClient.DownloadToAsync(memoryStream);
            
            return new DownloadInfoDTO
            {
                ContentType = fileinfo.Headers.ContentType,
                Length = fileinfo.Headers.ContentLength ?? 0,
                MemoryStream = memoryStream
            };
        }
    }
}
