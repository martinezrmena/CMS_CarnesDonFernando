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
    public class PuntosDAL
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
            _AboutTable = _tableClient.GetTableReference("PuntosClientes");
            await _AboutTable.CreateIfNotExistsAsync();
        }

        #region Consultas Sucursales

        /// <summary>
        /// Permite obtener una lista de puntos por cedula.
        /// </summary>
        /// <param name="Cedula">Identificación de la persona que posee los datos</param>
        /// <returns>Una lista de puntos</returns>
        public async Task<List<PuntosEntity>> GetPuntosUsuario(string Cedula)
        {
            await ConnectToTable();
            TableContinuationToken continuationToken = null;
            var puntos = new List<PuntosEntity>();
            var condition = TableQuery.GenerateFilterCondition("Cedula", QueryComparisons.Equal, Cedula);
            var query = new TableQuery<PuntosEntity>().Where(condition);

            try
            {
                do
                {

                    var result = await _AboutTable.ExecuteQuerySegmentedAsync(query, continuationToken);
                    continuationToken = result.ContinuationToken;

                    if (result.Results.Count > 0)
                        puntos.AddRange(result.Results);
                } while (continuationToken != null);
            }
            catch (Exception)
            {

            }
            return puntos;
        }

        /// <summary>
        /// Permite obtener la lista de todos los puntos de los usuarios.
        /// </summary>
        /// <returns>Una lista de puntos</returns>
        public async Task<List<PuntosEntity>> GetPuntosGlobal()
        {
            await ConnectToTable();
            TableContinuationToken continuationToken = null;
            var puntos = new List<PuntosEntity>();
            try
            {
                do
                {

                    var result = await _AboutTable.ExecuteQuerySegmentedAsync(new TableQuery<PuntosEntity>(), continuationToken);
                    continuationToken = result.ContinuationToken;

                    if (result.Results.Count > 0)
                        puntos.AddRange(result.Results);
                } while (continuationToken != null);
            }
            catch (Exception)
            {

            }
            return puntos;
        }
        #endregion

        #region Insertar/Actualizar Sucursales
        /// <summary>
        /// Metodo que permite actualizar o insertar puntos en el Azure Tab
        /// </summary>
        /// <param name="Puntos"></param>
        /// <returns></returns>
        public async Task<bool> SavePuntos(PuntosEntity Puntos)
        {
            try
            {
                await ConnectToTable();
                Puntos.Timestamp = new DateTimeOffset();

                var operation = TableOperation.InsertOrMerge(Puntos);
                var upsert = await _AboutTable.ExecuteAsync(operation);

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
        /// Metodo que permita eliminar una puntos
        /// </summary>
        /// <param name="Puntos">Modelo de tipo puntos</param>
        /// <returns>Un bool que indica si la operación fue exitosa</returns>
        public async Task<bool> DeletePuntos(PuntosEntity Puntos)
        {
            AzureBlobStorageDAL blobDAL = new AzureBlobStorageDAL();

            try
            {
                await ConnectToTable();

                var tablePuntos = new TableEntity() { PartitionKey = Puntos.PartitionKey, RowKey = Puntos.RowKey, ETag = "*" };

                var operation = TableOperation.Delete(tablePuntos);
                var delete = await _AboutTable.ExecuteAsync(operation);

                if (delete.HttpStatusCode == 204)
                {
                    return true;
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
