using CMS_CarnesDonFernando.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CMS_CarnesDonFernando.Helpers
{
    public class AzureBlobStorageHelper
    {

        private readonly string BaseUrl;

        private static readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=appmomentosdf;AccountKey=jxxkCEhAjwvDBda+0sF4cy/Etul9DY1LM3cGMv5Bp8ACrYPhY3yNYpwI77IYVnB4T/7gqbQ3/eFA0049VhPIuw==;EndpointSuffix=core.windows.net";


        public AzureBlobStorageHelper()
        {
            var AppSetting = new ConfigurationBuilder()
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile("appsettings.json")
                           .Build();
            BaseUrl = AppSetting.GetSection("ApiUrl").Value;
        }


        public async Task<string> SubirImagenBlob(string sPathImagen, string sContainerName)
        {
            try
            {
                JObject jsonData = new JObject
                {
                    { "PathImage", sPathImagen },
                    { "ContainerName", sContainerName }
                };

                


                HttpClient client = new HttpClient();
                string uridos = BaseUrl + "api/AzureBlobStorage/SubirImagen";
                //string uridos = "http://localhost:53997/api/AzureBlobStorage/SubirImagen";
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
               
                HttpResponseMessage response = client.PostAsJsonAsync(uridos, jsonData).Result;

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<string>();
                    
                }

                return string.Empty;
                
            }
            catch(Exception info)
            {
                throw new Exception("Error-" + info); 
            }
        }

        public async Task<string> SubirImagenBlob(AzureBlobModel model)
        {
            try
            {
                

                HttpClient client = new HttpClient();
                string uridos = BaseUrl + "api/AzureBlobStorage/SubirImagenBlob";
               // string uridos = "http://localhost:53997/api/AzureBlobStorage/SubirImagenBlob";
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PostAsJsonAsync(uridos, model).Result;

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<string>();

                }

                return string.Empty;

            }
            catch (Exception info)
            {
                throw new Exception("Error-" + info);
            }
        }

        public async Task<string> SubirImagenCMS(AzureBlobModel model)
        {
            try
            {
                //string fileName = GenerateFileName(Path.GetFileName(model.UrlLocal));
                string fileName = Path.GetFileName(model.UrlLocal);

                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_connectionString);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(model.NameContainer);

                await cloudBlobContainer.CreateIfNotExistsAsync();

                await cloudBlobContainer.SetPermissionsAsync(
                        new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        }
                        );


                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);

                string blobUrl = string.Empty;

                await cloudBlockBlob.UploadFromFileAsync(model.UrlLocal);
                blobUrl = cloudBlockBlob.Uri.AbsoluteUri;

                return blobUrl;

            }
            catch (Exception ex) //modificar por null
            {
                throw new Exception(ex.Message.ToString());

            }
        }

        private string GenerateFileName(string fileName)
        {
            string strFileName = string.Empty;
            string[] strName = fileName.Split('.');
            strFileName = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd") + "/" + DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmssfff") + "." + strName[strName.Length - 1];
            return strFileName;
        }
    }
}
