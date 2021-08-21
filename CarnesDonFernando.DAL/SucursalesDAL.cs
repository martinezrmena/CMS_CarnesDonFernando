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
    public class SucursalesDAL
    {
        #region Propiedades

        private static readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=appmomentosdf;AccountKey=jxxkCEhAjwvDBda+0sF4cy/Etul9DY1LM3cGMv5Bp8ACrYPhY3yNYpwI77IYVnB4T/7gqbQ3/eFA0049VhPIuw==;EndpointSuffix=core.windows.net";
        static CloudStorageAccount cloudStorageAccount;
        static CloudTableClient tableClient;
        static CloudTable SucursalTable;
        static CloudTable DataSucursalTable;

        #endregion

        #region Conexion tablas azure

        /// <summary>
        /// Metodo incial que permite establecer conexión con el Azure Table
        /// correspondiente al metodo
        /// </summary>
        /// <returns></returns>
        private static async Task ConnectToTable()
        {
            cloudStorageAccount = CloudStorageAccount.Parse(_connectionString);
            tableClient = cloudStorageAccount.CreateCloudTableClient();
            SucursalTable = tableClient.GetTableReference("Sucursal"); //Informacion sucursales desde la BD de Carnes Don Fernando.
            DataSucursalTable = tableClient.GetTableReference("DatosSucursal"); //datos de la sucursal desde el CMS. 
            await SucursalTable.CreateIfNotExistsAsync();
            await DataSucursalTable.CreateIfNotExistsAsync();
        }
        #endregion

        #region Consultas Sucursales

        /// <summary>
        /// Permite obtener una lista de sucursales (codigo y nombre).
        /// </summary>
        /// <returns>Una lista de sucursales</returns>
        public async Task<List<SucursalEntity>> GetSucursal()
        {
            TableContinuationToken continuationToken = null;
            List<SucursalEntity> sucursales = new List<SucursalEntity>();
            try
            {
                await ConnectToTable();

                var query = new TableQuery<SucursalEntity>();
                var lst = SucursalTable.ExecuteQuerySegmentedAsync(query, continuationToken);

                sucursales.AddRange(lst.Result);

                return sucursales;
            }
            catch (Exception)
            {
            }

            return null;
        }


        /// <summary>
        /// Permite obtener una lista con la informacion completa de las sucursales.
        /// </summary>
        /// <returns>Una lista de provincias</returns>
        public async Task<List<SucursalesEntity>> GetSucursalesData()
        {
            TableContinuationToken continuationToken = null;
            List<SucursalesEntity> sucursales = new List<SucursalesEntity>();
            try
            {
                await ConnectToTable();

                var query = new TableQuery<SucursalesEntity>();
                var lst = DataSucursalTable.ExecuteQuerySegmentedAsync(query, continuationToken);

                sucursales.AddRange(lst.Result);

                return sucursales;
            }
            catch (Exception)
            {
            }

            return null;
        }

        /// <summary>
        /// Permite obtener la sucursal especifica por las llaves de particion y fila.
        /// </summary>
        /// <param name="partitionKey">identificador del modelo</param>
        /// <param name="rowKey">idenificacion del modelo</param>
        /// <returns></returns>
        public async Task<SucursalesEntity> GetSucursalData(string partitionKey, string rowKey)
        {
            List<SucursalesEntity> promotions = new List<SucursalesEntity>();
            TableContinuationToken token = null;
            SucursalesEntity promotion = new SucursalesEntity();

            try
            {
                await ConnectToTable();

                //Create the table query
                var condition = TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey));

                var query = new TableQuery<SucursalesEntity>().Where(condition);
                var lst = DataSucursalTable.ExecuteQuerySegmentedAsync(query, token);

                promotions.AddRange(lst.Result);

                promotion = promotions.FirstOrDefault();

                return promotion;
            }
            catch
            {

            }
            return null;
        }

        #endregion

        #region Insertar/Actualizar Sucursales

        /// <summary>
        /// Metodo que permite actualizar o insertar una sucursal en el Azure Tab
        /// </summary>
        /// <param name="sucursal"></param>
        /// <returns></returns>
        public async Task<bool> SaveSucursal(SucursalesEntity sucursal)
        {
            try
            {
                await ConnectToTable();
                sucursal.Timestamp = new DateTimeOffset();

                var operation = TableOperation.InsertOrMerge(sucursal);
                var upsert = await DataSucursalTable.ExecuteAsync(operation);

                return (upsert.HttpStatusCode == 204);
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        /// <summary>
        /// Metodo que permite actualizar o insertar una sucursal en el Azure Tab
        /// </summary>
        /// <param name="sucursal"></param>
        /// <returns></returns>
        public async Task<bool> SaveSucursalEntity(SucursalEntity sucursal)
        {
            try
            {
                await ConnectToTable();
                sucursal.Timestamp = new DateTimeOffset();

                var operation = TableOperation.InsertOrMerge(sucursal);
                var upsert = await SucursalTable.ExecuteAsync(operation);

                return (upsert.HttpStatusCode == 204);
            }
            catch (Exception ex)
            {

            }
            return false;
        }


        #endregion

        #region Eliminar Sucursal

        /// <summary>
        /// Metodo que permita eliminar una sucursal
        /// </summary>
        /// <param name="sucursal">Modelo de tipo promocion</param>
        /// <returns>Un bool que indica si la operación fue exitosa</returns>
        public async Task<bool> DeleteSucursal(SucursalesEntity sucursal)
        {
            AzureBlobStorageDAL blobDAL = new AzureBlobStorageDAL();

            try
            {
                await ConnectToTable();

                var tablePromotion = new TableEntity() { PartitionKey = sucursal.PartitionKey, RowKey = sucursal.RowKey, ETag = "*" };

                var operation = TableOperation.Delete(tablePromotion);
                var delete = await DataSucursalTable.ExecuteAsync(operation);

                if (delete.HttpStatusCode == 204)
                {
                    //Borrar la imagen de la sucursal
                    return await blobDAL.DeleteImage(sucursal.FotoUrl, "storeimages");
                }

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
