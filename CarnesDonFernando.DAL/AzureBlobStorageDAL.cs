using CarnesDonFernando.EL;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CarnesDonFernando.DAL
{
    public class AzureBlobStorageDAL
    {

        private static readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=appmomentosdf;AccountKey=jxxkCEhAjwvDBda+0sF4cy/Etul9DY1LM3cGMv5Bp8ACrYPhY3yNYpwI77IYVnB4T/7gqbQ3/eFA0049VhPIuw==;EndpointSuffix=core.windows.net";

        public async Task<string> UploadFileToStorage(Stream fileStream, string fileName, string nameContainer)
        {
           try
            {
                // Create cloudstorage account 
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_connectionString);
                // Create the blob client
                CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
                // Get reference to the blob container by passing the name 
                CloudBlobContainer container = blobClient.GetContainerReference(nameContainer);

                if (await container.CreateIfNotExistsAsync())
                {
                    await container.SetPermissionsAsync(
                        new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        }
                        );
                }

                // Get the reference to the block blob from the container
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

                string blobUrl = string.Empty;

                // Upload the file
                await blockBlob.UploadFromStreamAsync(fileStream);

                blobUrl = blockBlob.Uri.AbsoluteUri;

                return blobUrl;
            }
            catch
            {
                return null;
            }
    
        }


        public async Task<string> UploadImageBlob(AzureBlobEntity entity)
        {
            try
            {
                string fileName = Path.GetFileName(entity.UrlLocal);

                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_connectionString);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(entity.NameContainer);

                await cloudBlobContainer.CreateIfNotExistsAsync();
                
                await cloudBlobContainer.SetPermissionsAsync(
                        new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        }
                        );
                

                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);

                string blobUrl = string.Empty;

                await cloudBlockBlob.UploadFromFileAsync(entity.UrlLocal);
                blobUrl = cloudBlockBlob.Uri.AbsoluteUri;

                return blobUrl;

            }
              catch (Exception ex) //modificar por null
            {
                throw new Exception(ex.Message.ToString());

            }


        }

        public async Task<string> UploadImageAsync(string sFilePath, string sNameContainer)
        {
            try
            {
                string fileName = Path.GetFileName(sFilePath);

                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_connectionString);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(sNameContainer);

                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(
                        new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        }
                        );
                }

                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);

                string blobUrl = string.Empty;

                await cloudBlockBlob.UploadFromFileAsync(sFilePath);
                blobUrl = cloudBlockBlob.Uri.AbsoluteUri;



                //using (var fileStream = File.OpenRead(sFilePath))
                //{
                //    await cloudBlockBlob.UploadFromStreamAsync(fileStream);
                //    blobUrl = cloudBlockBlob.Uri.AbsoluteUri;
                //}

                return blobUrl;

            }
            catch (Exception ex) //modificar por null
            {
                throw new Exception(ex.Message.ToString());

            }


        }

        public async Task<string> UploadImageBaseAsync(string Code, string fileName, string sNameContainer) 
        {
            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_connectionString);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(sNameContainer);

                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {
                    await cloudBlobContainer.SetPermissionsAsync(
                        new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        }
                        );
                }

                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);

                string blobUrl = string.Empty;

                using (var fileStream = new MemoryStream(Convert.FromBase64String(Code)))
                {
                    await cloudBlockBlob.UploadFromStreamAsync(fileStream);
                    blobUrl = cloudBlockBlob.Uri.AbsoluteUri;
                }

                return blobUrl;

            }
            catch (Exception ex) //modificar por null
            {
                throw new Exception(ex.Message.ToString());

            }
        }

        public async Task<bool> DeleteImage(string sFileURL, string sNameContainer)
        {
            bool response = false;
            try
            {
                Uri uriObj = new Uri(sFileURL);
                string BlobName = Path.GetFileName(uriObj.LocalPath);

                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_connectionString);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(sNameContainer);

                CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(BlobName);

                if (blockBlob.ExistsAsync().Result)
                {
                    //Delete blob from container
                    await blockBlob.DeleteAsync();
                    response = true;
                }

                return response;

            }
            catch
            {
                return response;
            }        
        }
    }

}
