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
    public class CantonesDAL
    {

        #region Propiedades

        private static readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=appmomentosdf;AccountKey=jxxkCEhAjwvDBda+0sF4cy/Etul9DY1LM3cGMv5Bp8ACrYPhY3yNYpwI77IYVnB4T/7gqbQ3/eFA0049VhPIuw==;EndpointSuffix=core.windows.net";
        static CloudStorageAccount cloudStorageAccount;
        static CloudTableClient tableClient;
        static CloudTable CantonTable;
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
            tableClient = cloudStorageAccount.CreateCloudTableClient();
            CantonTable = tableClient.GetTableReference("Cantones");
            await CantonTable.CreateIfNotExistsAsync();
        }

        /// <summary>
        /// Permite obtener una lista total de cantones.
        /// </summary>
        /// <param name="provincia">un string con el codigo de la provincia</param>
        /// <returns>Un modelo de tipo UserModel</returns>
        public async Task<List<CantonesEntity>> GetCantones()
        {
            try
            {
                await ConnectToTable();

                TableContinuationToken continuationToken = null;

                // Create the table query.
                var query = new TableQuery<CantonesEntity>();
                var lst = CantonTable.ExecuteQuerySegmentedAsync(query, continuationToken);

                List<CantonesEntity> cantones = new List<CantonesEntity>();

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
        public async Task<List<CantonesEntity>> GetCantonesPorProvincia(string provincia)
        {
            try
            {
                await ConnectToTable();

                TableContinuationToken continuationToken = null;

                // Create the table query.
                var condition = TableQuery.GenerateFilterCondition("Provincia", QueryComparisons.Equal, provincia);

                var query = new TableQuery<CantonesEntity>().Where(condition);
                var lst = CantonTable.ExecuteQuerySegmentedAsync(query, continuationToken);

                List<CantonesEntity> cantones = new List<CantonesEntity>();

                cantones.AddRange(lst.Result);

                return cantones;
            }
            catch (Exception)
            {
            }

            return null;
        }

    }
}
