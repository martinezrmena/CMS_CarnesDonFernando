using CarnesDonFernando.EL;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarnesDonFernando.DAL
{
    public class DistritoDAL
    {
        #region Propiedades

        private static readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=appmomentosdf;AccountKey=jxxkCEhAjwvDBda+0sF4cy/Etul9DY1LM3cGMv5Bp8ACrYPhY3yNYpwI77IYVnB4T/7gqbQ3/eFA0049VhPIuw==;EndpointSuffix=core.windows.net";
        static CloudStorageAccount cloudStorageAccount;
        static CloudTableClient tableDistrito;
        static CloudTable DistritoTable;
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
            cloudStorageAccount = CloudStorageAccount.Parse(_connectionString);
            tableDistrito = cloudStorageAccount.CreateCloudTableClient();
            DistritoTable = tableDistrito.GetTableReference("Distritos");
            await DistritoTable.CreateIfNotExistsAsync();
        }

        /// <summary>
        /// Permite obtener una lista total de distritos.
        /// </summary>
        /// <returns>Un modelo de tipo distrito</returns>
        public async Task<List<DistritoEntity>> GetDistritos()
        {
            try
            {
                await ConnectToTable();

                TableContinuationToken continuationToken = null;

                // Create the table query.
                var query = new TableQuery<DistritoEntity>();
                var lst = DistritoTable.ExecuteQuerySegmentedAsync(query, continuationToken);

                List<DistritoEntity> cantones = new List<DistritoEntity>();

                cantones.AddRange(lst.Result);

                return cantones;
            }
            catch (Exception)
            {
            }

            return null;
        }


        /// <summary>
        /// Permite obtener una lista  de cantones especifico por provincia.
        /// </summary>
        /// <param name="provincia">un string con el codigo de la provincia</param>
        /// <returns>Un modelo de tipo UserModel</returns>
        public async Task<List<DistritoEntity>> GetDistritosPorCanton(string canton)
        {
            try
            {
                await ConnectToTable();

                TableContinuationToken continuationToken = null;

                // Create the table query.
                var condition = TableQuery.GenerateFilterCondition("Canton", QueryComparisons.Equal, canton);

                var query = new TableQuery<DistritoEntity>().Where(condition);
                var lst = DistritoTable.ExecuteQuerySegmentedAsync(query, continuationToken);

                List<DistritoEntity> cantones = new List<DistritoEntity>();

                cantones.AddRange(lst.Result);

                return cantones;
            }
            catch (Exception)
            {
            }

            return null;
        }

        /// <summary>
        /// Metodo que permite actualizar o insertar un distrito en el Azure Table
        /// </summary>
        /// <param name="distrito">Modelo de tipo distrito</param>
        /// <returns>Un bool que indica si la operación fue exitosa</returns>
        public async Task<bool> SaveDistrito(DistritoEntity distrito)
        {
            try
            {
                await ConnectToTable();
                distrito.Timestamp = new DateTimeOffset();
                var operation = TableOperation.InsertOrMerge(distrito);
                var upsert = await DistritoTable.ExecuteAsync(operation);

                return (upsert.HttpStatusCode == 204);
            }
            catch (Exception ex)
            {

            }

            return false;
        }

    }
}