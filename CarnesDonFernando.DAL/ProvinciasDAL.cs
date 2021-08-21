using CarnesDonFernando.EL;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarnesDonFernando.DAL
{
    public class ProvinciasDAL
    {
        #region Propiedades

        private static readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=appmomentosdf;AccountKey=jxxkCEhAjwvDBda+0sF4cy/Etul9DY1LM3cGMv5Bp8ACrYPhY3yNYpwI77IYVnB4T/7gqbQ3/eFA0049VhPIuw==;EndpointSuffix=core.windows.net";
        static CloudStorageAccount cloudStorageAccount;
        static CloudTableClient tableClient;
        static CloudTable ProvinciaTable;
     
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
            ProvinciaTable = tableClient.GetTableReference("Provincias");
            await ProvinciaTable.CreateIfNotExistsAsync();
        }


        /// <summary>
        /// Permite obtener una lista de provincias.
        /// </summary>
        /// <returns>Una lista de provincias</returns>
        public async Task<List<ProvinciasEntity>> GetProvincias()
        {
            TableContinuationToken continuationToken = null;
            List<ProvinciasEntity> provincias = new List<ProvinciasEntity>();
            try
            {
                await ConnectToTable();

                var query = new TableQuery<ProvinciasEntity>();
                var lst = ProvinciaTable.ExecuteQuerySegmentedAsync(query, continuationToken);

                provincias.AddRange(lst.Result);

                return provincias;
            }
            catch (Exception)
            {
            }

            return null;
        }

    }
}
