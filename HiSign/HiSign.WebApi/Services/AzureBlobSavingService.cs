using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace HiSign.WebApi.Services
{
    public class AzureBlobSavingService
    {
        public async Task<string> SavingFileToAzureBlobAndReturnUrl(byte[] bytes, string name, string contentType, CloudBlobContainer cloudBlobContainer)
        {
            //save to azure
            // Retrieve a reference to a container. 
            var container = cloudBlobContainer;

            // Create the container if it doesn't already exist.
            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(
                new BlobContainerPermissions
                {
                    PublicAccess =
                        BlobContainerPublicAccessType.Blob
                });

            //upload file and return file detail
            var blob = container.GetBlockBlobReference(name);
            blob.Properties.ContentType = contentType;
            await blob.UploadFromByteArrayAsync(bytes, 0, bytes.Length);
            return blob.Uri.AbsoluteUri;
        }

        public async Task<string> SavingFileToAzureBlobAsync(byte[] bytes, string name, string contentType, CloudBlobContainer cloudBlobContainer)
        {
            //save to azure
            // Retrieve a reference to a container. 
            var container = cloudBlobContainer;

            // Create the container if it doesn't already exist.
            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(
                new BlobContainerPermissions
                {
                    PublicAccess =
                        BlobContainerPublicAccessType.Blob
                });

            //upload file
            var blob = container.GetBlockBlobReference(name);
            blob.Properties.ContentType = contentType;
            await blob.UploadFromByteArrayAsync(bytes, 0, bytes.Length);

            //return file detail
            return name;

        }

        public async Task<string> RenameAzureUrlAsync(CloudBlobContainer cloudBlobContainer, string oldFilename, string newFilename)
        {
            var container = cloudBlobContainer;
            var oldBlob = container.GetBlockBlobReference(oldFilename);
            var newBlob = container.GetBlockBlobReference(newFilename);

            using (var stream = new MemoryStream())
            {
                await oldBlob.DownloadToStreamAsync(stream);
                stream.Seek(0, SeekOrigin.Begin);
                newBlob.Properties.ContentType = oldBlob.Properties.ContentType;
                await newBlob.UploadFromStreamAsync(stream);
                await oldBlob.DeleteAsync();
            }
            return await Task.FromResult(newFilename);
        }

        public async Task DeleteFileAsync(CloudBlobContainer cloudBlobContainer, string filename)
        {
            var container = cloudBlobContainer;
            var blob = container.GetBlockBlobReference(filename);
            await blob.DeleteAsync();
        }

        public async Task DeleteFilesAsync(CloudBlobContainer cloudBlobContainer, IEnumerable<string> filenames)
        {
            var container = cloudBlobContainer;
            foreach (var filename in filenames)
            {
                var blob = container.GetBlockBlobReference(filename);
                await blob.DeleteAsync();
            }
        }
    }

    public class AzureBlobHelper
    {
        private readonly BlobStorageConnectionString _blobStorageConnection;
        public AzureBlobHelper(
            BlobStorageConnectionString blobStorageConnection)
        {
            _blobStorageConnection = blobStorageConnection;
        }
        #region blob client

        private CloudBlobClient CloudBlobClient
        {
            get
            {
                var blobStorageConnectionString = _blobStorageConnection.ConnectionString;
                // Create blob client and return reference to the container
                var blobStorageAccount = CloudStorageAccount.Parse(blobStorageConnectionString);
                var cloudBlobClient = blobStorageAccount.CreateCloudBlobClient();
                return cloudBlobClient;
            }
        }

        #endregion

        public async Task<CloudBlobContainer> GetBlobContainer(string containerName)
        {
            var container = CloudBlobClient.GetContainerReference(containerName);
            // Create the container if it doesn't already exist.
            await container.CreateIfNotExistsAsync();
            return container;
        }
    }

    public class BlobStorageConnectionString
    {
        public string ConnectionString { get; set; }
    }
}
