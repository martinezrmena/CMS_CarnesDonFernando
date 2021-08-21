using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CarnesDonFernando.DAL;
using CarnesDonFernando.EL;

namespace CarnesDonFernando.BL
{
   public class AzureBlobStorageBL
    {

        private readonly AzureBlobStorageDAL _azureBlob = new AzureBlobStorageDAL();

        public async Task<string> SubirImagen(string sPath, string sNameContainer)
        {
            try
            {
                return await _azureBlob.UploadImageAsync(sPath, sNameContainer);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());

            }
        }

        public async Task<string> SubirImagenBase(string Code, string FileName, string sNameContainer)
        {
            try
            {
                return await _azureBlob.UploadImageBaseAsync(Code, FileName, sNameContainer);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());

            }
        }

        public async Task<string> SubirImagenBlob(AzureBlobEntity data)
        {
            try
            {
                return await _azureBlob.UploadImageBlob(data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());

            }
        }

        public async Task<bool> BorrarImagen(string sUrlFile, string sNameContainer)
        {
            try
            {
                return await _azureBlob.DeleteImage(sUrlFile, sNameContainer);
            }
            catch
            {
                throw new Exception("");
            }
        }

        public async Task<string> SubirBlobAzure(Stream fileStream, string fileName, string nameContainer)
        {
            try
            {
                return await _azureBlob.UploadFileToStorage(fileStream, fileName, nameContainer);
            }
            catch
            {
                throw new Exception();
            }
        }

    }
}
