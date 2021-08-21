using CarnesDonFernando.EL;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarnesDonFernando.DAL
{
    public class PromocionesDAL
    {
        #region Propiedades

        private static readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=appmomentosdf;AccountKey=jxxkCEhAjwvDBda+0sF4cy/Etul9DY1LM3cGMv5Bp8ACrYPhY3yNYpwI77IYVnB4T/7gqbQ3/eFA0049VhPIuw==;EndpointSuffix=core.windows.net";
        static CloudStorageAccount _cloudStorageAccount;
        static CloudTableClient _tableClient;
        static CloudTable _PromotionTable;
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
            _PromotionTable = _tableClient.GetTableReference("Promotions");
            await _PromotionTable.CreateIfNotExistsAsync();
        }

        #region Consultas Promociones

        /// <summary>
        /// Permite obtener una lista de todas las promociones
        /// </summary>
        /// <returns></returns>
        public async Task<List<PromocionesEntity>> GetPromotions()
        {
            List<PromocionesEntity> _records = new List<PromocionesEntity>();
            TableContinuationToken token = null;

            try
            {
                await ConnectToTable();

                do
                {
                    var _result = await _PromotionTable.ExecuteQuerySegmentedAsync(new TableQuery<PromocionesEntity>(), token);
                    token = _result.ContinuationToken;

                    if (_result.Results.Count > 0)
                        _records.AddRange(_result.Results);

                } while (token != null);
            }
            catch (Exception ex)
            {

            }
            return _records;
        }

        /// <summary>
        /// Permite obtener una promocion especifica por las llaves de particion y fila.
        /// </summary>
        /// <param name="partitionKey">identificador del modelo</param>
        /// <param name="rowKey">idenificacion del modelo</param>
        /// <returns></returns>
        public async Task<PromocionesEntity> GetPromotion(string partitionKey, string rowKey)
        {
            List<PromocionesEntity> promotions = new List<PromocionesEntity>();
            TableContinuationToken token = null;
            PromocionesEntity promotion = new PromocionesEntity();

            try
            {
                await ConnectToTable();

                //Create the table query
                var condition = TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey));

                var query = new TableQuery<PromocionesEntity>().Where(condition);
                var lst = _PromotionTable.ExecuteQuerySegmentedAsync(query, token);

                promotions.AddRange(lst.Result);

                promotion = promotions.FirstOrDefault();

                return promotion;
            }
            catch
            {

            }
            return null;
        }

        public async Task<PromocionesEntity> ExistPromotion(string pTitulo, string pEnlace, string pFechaIni, string pFechaFin)
        {
            List<PromocionesEntity> promotions = new List<PromocionesEntity>();
            TableContinuationToken token = null;
            PromocionesEntity entity = new PromocionesEntity();

            await ConnectToTable();

            //Create the table query
            var condition1 = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("Fecha_Finalizacion", QueryComparisons.Equal, pFechaFin),
                TableOperators.And,
                TableQuery.GenerateFilterCondition("Fecha_Publicacion", QueryComparisons.Equal, pFechaIni));

            var condition2 = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("Enlace", QueryComparisons.Equal, pEnlace),
                TableOperators.And,
                TableQuery.GenerateFilterCondition("Titulo", QueryComparisons.Equal, pTitulo));

            var condicionFinal = TableQuery.CombineFilters(condition1, TableOperators.And, condition2);

            var query = new TableQuery<PromocionesEntity>().Where(condicionFinal);
            var lst = _PromotionTable.ExecuteQuerySegmentedAsync(query, token);

            promotions.AddRange(lst.Result);

            entity = promotions.FirstOrDefault();

            return entity;
        }

        #endregion

        #region Metodos Insertar-Actualizar Promociones

        /// <summary>
        /// Metodo que permite actualizar o insertar una promocion en el Azure Tab
        /// </summary>
        /// <param name="promotion"></param>
        /// <returns></returns>
        public async Task<bool> SavePromotion(PromocionesEntity promotion)
        {

            try
            {
                await ConnectToTable();
                promotion.Timestamp = new DateTimeOffset();

                var operation = TableOperation.InsertOrMerge(promotion);
                var upsert = await _PromotionTable.ExecuteAsync(operation);

                return (upsert.HttpStatusCode == 204);


            }
            catch (Exception ex)
            {

            }
            return false;
        }

        #endregion

        #region Eliminar Promociones

        /// <summary>
        /// Metodo que permita eliminar una promocion
        /// </summary>
        /// <param name="promotion">Modelo de tipo promocion</param>
        /// <returns>Un bool que indica si la operación fue exitosa</returns>
        public async Task<bool> DeletePromotion(PromocionesEntity promotion)
        {
            AzureBlobStorageDAL blobDAL = new AzureBlobStorageDAL();

            try
            {
                await ConnectToTable();

                var tablePromotion = new TableEntity() { PartitionKey = promotion.PartitionKey, RowKey = promotion.RowKey, ETag = "*" };

                var operation = TableOperation.Delete(tablePromotion);
                var delete = await _PromotionTable.ExecuteAsync(operation);

                if (delete.HttpStatusCode == 204)
                {
                    return await blobDAL.DeleteImage(promotion.ImagenUrl, "promotionimages");
                }


                //return (delete.HttpStatusCode == 204);
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
