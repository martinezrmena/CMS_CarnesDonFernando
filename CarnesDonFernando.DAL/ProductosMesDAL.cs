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
    public class ProductosMesDAL
    {
        #region Propiedades

        private static readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=appmomentosdf;AccountKey=jxxkCEhAjwvDBda+0sF4cy/Etul9DY1LM3cGMv5Bp8ACrYPhY3yNYpwI77IYVnB4T/7gqbQ3/eFA0049VhPIuw==;EndpointSuffix=core.windows.net";
        
        static CloudStorageAccount _cloudStorageAccount;
        static CloudTableClient _tableClient;
        static CloudTable _ProductsTable;
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
            _ProductsTable = _tableClient.GetTableReference("Produts");
            await _ProductsTable.CreateIfNotExistsAsync();
        }

        #region Consultas Productos del Mes

        /// <summary>
        /// Permite obtener una lista de todos los productos del mes
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductosMesEntity>> GetProducts()
        {
            List<ProductosMesEntity> _records = new List<ProductosMesEntity>();
            TableContinuationToken token = null;

            try
            {
                await ConnectToTable();

                do
                {
                    var _result = await _ProductsTable.ExecuteQuerySegmentedAsync(new TableQuery<ProductosMesEntity>(), token);
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
        /// Permite obtener el producto del mes especifico por las llaves de particion y fila.
        /// </summary>
        /// <param name="partitionKey">identificador del modelo</param>
        /// <param name="rowKey">idenificacion del modelo</param>
        /// <returns></returns>
        public async Task<ProductosMesEntity> GetProduct(string partitionKey, string rowKey)
        {
            List<ProductosMesEntity> promotions = new List<ProductosMesEntity>();
            TableContinuationToken token = null;
            ProductosMesEntity promotion = new ProductosMesEntity();

            try
            {
                await ConnectToTable();

                //Create the table query
                var condition = TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey));

                var query = new TableQuery<ProductosMesEntity>().Where(condition);
                var lst = _ProductsTable.ExecuteQuerySegmentedAsync(query, token);

                promotions.AddRange(lst.Result);

                promotion = promotions.FirstOrDefault();

                return promotion;
            }
            catch
            {

            }
            return null;
        }


        public async Task<ProductosMesEntity> ExistProduct(string titulo, string detalle, string resumen, string preparacion, string fechaInicio, string fechaFin)
        {
            List<ProductosMesEntity> products = new List<ProductosMesEntity>();
            TableContinuationToken token = null;
            ProductosMesEntity entity = new ProductosMesEntity();

            await ConnectToTable();


            //Se crean los filtros para la consulta a la tabla de azure
            string condition = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("Fecha_Finalizacion", QueryComparisons.Equal, fechaFin),
                TableOperators.And,
                TableQuery.GenerateFilterCondition("Fecha_Publicacion", QueryComparisons.Equal, fechaInicio));

            string condition1 = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("Titulo", QueryComparisons.Equal, titulo),
                TableOperators.And,
                TableQuery.GenerateFilterCondition("Preparacion", QueryComparisons.Equal, preparacion));

            string condition2 = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("Descripcion_Detalle", QueryComparisons.Equal, detalle),
                TableOperators.And,
                TableQuery.GenerateFilterCondition("Descripcion_Resumen", QueryComparisons.Equal, resumen));

            string EndCondition = TableQuery.CombineFilters(
                        TableQuery.CombineFilters(
                            condition,
                            TableOperators.And,
                             condition1),
                        TableOperators.And, condition2);

            var query = new TableQuery<ProductosMesEntity>().Where(EndCondition);
            var lst = _ProductsTable.ExecuteQuerySegmentedAsync(query, token);

            products.AddRange(lst.Result);

            entity = products.FirstOrDefault();

            return entity;
        }


        #endregion

        #region Metodos Insertar-Actualizar Productos del mes

        /// <summary>
        /// Metodo que permite actualizar o insertar un producto del mes en el Azure Table
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<bool> SaveProduct(ProductosMesEntity product)
        {
            try
            {
                await ConnectToTable();
                product.Timestamp = new DateTimeOffset();

                var operation = TableOperation.InsertOrMerge(product);
                var upsert = await _ProductsTable.ExecuteAsync(operation);

                return (upsert.HttpStatusCode == 204);
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        #endregion

        #region Eliminar Producto del Mes

        /// <summary>
        /// Metodo que permita eliminar un producto del mes
        /// </summary>
        /// <param name="product">Modelo de tipo producto Mes</param>
        /// <returns>Un bool que indica si la operación fue exitosa</returns>
        public async Task<bool> DeleteProduct(ProductosMesEntity product)
        {
            AzureBlobStorageDAL blobDAL = new AzureBlobStorageDAL();

            try
            {
                await ConnectToTable();

                var tableProducts = new TableEntity() { PartitionKey = product.PartitionKey, RowKey = product.RowKey, ETag = "*" };

                var operation = TableOperation.Delete(tableProducts);
                var delete = await _ProductsTable.ExecuteAsync(operation);

                if (delete.HttpStatusCode == 204)
                {
                    return await blobDAL.DeleteImage(product.ImagenUrl, "productsimages");
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
