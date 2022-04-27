using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRegistration.Services
{
    public class AzureBlobStorageService
    {
        private readonly string _azureBlobStorageConnectionString;

        public AzureBlobStorageService()
        {
            _azureBlobStorageConnectionString = AppConfig.GetConfiguration("AzureBlobStorageConnection");
        }

        public string UploadToAzureBlobStorage(string fileName, byte[] fileData, string fileType)
        {
            var task = Task.Run(() => this.UploadToAzureBlobStorageAsync(fileName, fileData, fileType));
            task.Wait();

            string fileUrl = task.Result;

            return fileUrl;
        }

        public async void DeleteFromAzureBlobStorage(string fileUrl)
        {
            Uri uriObject = new Uri(fileUrl);

            string blobName = Path.GetFileName(uriObject.LocalPath);

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_azureBlobStorageConnectionString);

            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            string containerName = "studentUploads";

            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);

            string pathPrefix = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd") + "/";
            
            CloudBlobDirectory cloudBlobDirectory = cloudBlobContainer.GetDirectoryReference(pathPrefix);
            
            // get block blob refarence    
            CloudBlockBlob blockBlob = cloudBlobDirectory.GetBlockBlobReference(blobName);

            // delete blob from container        
            await blockBlob.DeleteAsync();
        }

        private string GenerateFileName(string fileName)
        {
            string _fileName = string.Empty;
            string[] name = fileName.Split('.');

            _fileName = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd")
                + "/" + DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmssfff")
                + "." + name[name.Length - 1];

            return _fileName;                
        }

        private async Task<string> UploadToAzureBlobStorageAsync(string fileName, byte[] fileData, string fileType)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_azureBlobStorageConnectionString);
            
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            
            string strContainerName = "studentuploads";
            
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);
            
            string _fileName = this.GenerateFileName(fileName);

            if (await cloudBlobContainer.CreateIfNotExistsAsync())
            {
                await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }

            if (_fileName != null && fileData != null)
            {
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(_fileName);
                
                cloudBlockBlob.Properties.ContentType = fileType;
                
                await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
                
                return cloudBlockBlob.Uri.AbsoluteUri;
            }

            return "";
        }


    }
}
