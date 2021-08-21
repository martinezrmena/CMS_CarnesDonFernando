using CarnesDonFernando.EL;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarnesDonFernando.DAL
{
    public class AcercaDeDAL
    {
        #region Propiedades

        private static readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=appmomentosdf;AccountKey=jxxkCEhAjwvDBda+0sF4cy/Etul9DY1LM3cGMv5Bp8ACrYPhY3yNYpwI77IYVnB4T/7gqbQ3/eFA0049VhPIuw==;EndpointSuffix=core.windows.net";
       
        static CloudStorageAccount _cloudStorageAccount;
        static CloudTableClient _tableClient;
        static CloudTable _AboutTable;
        //private CloudBlobClient cloudBlobClient;
        //private CloudBlobContainer cloudBlobContainer;

        #endregion

        /// <summary>
        /// Metodo incial que permite establecer conexión con el Azure Table
        /// correspondiente al metodo
        /// </summary>
        /// <returns></returns>
        private static async Task ConnectToTable()
        {
            _cloudStorageAccount = CloudStorageAccount.Parse(_connectionString);
            _tableClient = _cloudStorageAccount.CreateCloudTableClient();
            _AboutTable = _tableClient.GetTableReference("About");
            await _AboutTable.CreateIfNotExistsAsync();
        }

        #region Consultas Acerca De

        /// <summary>
        /// Permite obtener el Acerca De.
        /// </summary>
        /// <returns></returns>
        public async Task<AcercaDeEntity> GetAbout()
        {
           
            List<AcercaDeEntity> _records = new List<AcercaDeEntity>();
            AcercaDeEntity _aboutEntity = new AcercaDeEntity();
            TableContinuationToken token = null;

            try
            {
                await ConnectToTable();

                do
                {
                    var _result = await _AboutTable.ExecuteQuerySegmentedAsync(new TableQuery<AcercaDeEntity>(), token);
                    token = _result.ContinuationToken;

                    if (_result.Results.Count > 0)
                        _records.AddRange(_result.Results);

                } while (token != null);

               _aboutEntity = _records.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Error-" + ex);
            }
            return _aboutEntity;
        }

        #endregion

        /// <summary>
        /// Permite obtener el acerca De especifico por las llaves de particion y fila.
        /// </summary>
        /// <param name="partitionKey">identificador del modelo</param>
        /// <param name="rowKey">idenificacion del modelo</param>
        /// <returns></returns>
        public async Task<AcercaDeEntity> GetAboutKeys(string partitionKey, string rowKey)
        {
            List<AcercaDeEntity> promotions = new List<AcercaDeEntity>();
            TableContinuationToken token = null;
            AcercaDeEntity about = new AcercaDeEntity();

            try
            {
                await ConnectToTable();

                //Create the table query
                var condition = TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey));

                var query = new TableQuery<AcercaDeEntity>().Where(condition);
                var lst = _AboutTable.ExecuteQuerySegmentedAsync(query, token);

                promotions.AddRange(lst.Result);

                about = promotions.FirstOrDefault();

                return about;
            }
            catch
            {

            }
            return null;
        }


        #region Metodos Insertar-Actualizar Acerca De

        /// <summary>
        /// Metodo que permite actualizar o insertar el Acerca De en el Azure Tab
        /// </summary>
        /// <param name="about"></param>
        /// <returns></returns>
        public async Task<bool> SaveAbout(AcercaDeEntity about)
        {
            try
            {
                await ConnectToTable();
                about.Timestamp = new DateTimeOffset();

                var operation = TableOperation.InsertOrMerge(about);
                var upsert = await _AboutTable.ExecuteAsync(operation);

                return (upsert.HttpStatusCode == 204);
            }
            catch (Exception ex)
            {
                throw new Exception("Error-" + ex);

            }
            return false;
        }

        #endregion

        #region Eliminar Acerca De

        /// <summary>
        /// Metodo que permita eliminar el Acerca De
        /// </summary>
        /// <param name="user">Modelo de tipo usuario</param>
        /// <returns>Un bool que indica si la operación fue exitosa</returns>
        public async Task<bool> DeleteAbout(AcercaDeEntity about)
        {
            AzureBlobStorageDAL blobDAL = new AzureBlobStorageDAL();

            try
            {
                await ConnectToTable();

                var tablePromotion = new TableEntity() { PartitionKey = about.PartitionKey, RowKey = about.RowKey, ETag = "*" };

                var operation = TableOperation.Delete(tablePromotion);
                var delete = await _AboutTable.ExecuteAsync(operation);

                return (delete.HttpStatusCode == 204);
            }
            catch (Exception ex)
            {
                var mensaje = ex.Message;
            }

            return false;
        }

        #endregion
    }
}
