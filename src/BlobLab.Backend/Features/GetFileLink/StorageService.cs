using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Configuration;
using System;

namespace BlobLab.Backend.Features.GetFileLink
{
    public interface IStorageService
    {
        string GetFileLink(string blobFilePath);
    };

    public class StorageService : IStorageService
    {
        private readonly IConfiguration _configuration;
        public StorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetFileLink(string blobFilePath)
        {
            string privateFilesContainer = _configuration.GetSection("Storage").GetValue<string>("PrivateContainer");
            BlobServiceClient blobServiceClient = new BlobServiceClient(_configuration.GetConnectionString("Storage"));
            var container = blobServiceClient.GetBlobContainerClient(privateFilesContainer);
            BlobClient blobClient = container.GetBlobClient(blobFilePath);

            BlobSasBuilder sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = container.Name,
                BlobName = blobFilePath,
                Resource = "b"
            };

            sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddSeconds(30);
            sasBuilder.SetPermissions(BlobContainerSasPermissions.Read);
            Uri link = blobClient.GenerateSasUri(sasBuilder);
            return link.ToString();
        }

    }
}
