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
    public class PoliticaPrivacidadDAL
    {

        #region Propiedades

        private static readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=appmomentosdf;AccountKey=jxxkCEhAjwvDBda+0sF4cy/Etul9DY1LM3cGMv5Bp8ACrYPhY3yNYpwI77IYVnB4T/7gqbQ3/eFA0049VhPIuw==;EndpointSuffix=core.windows.net";
        static CloudStorageAccount _cloudStorageAccount;
        static CloudTableClient _tableClient;
        static CloudTable _privacypolicyTable;

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
            _privacypolicyTable = _tableClient.GetTableReference("PrivacyPolicy");
            await _privacypolicyTable.CreateIfNotExistsAsync();
        }

        #region Consultas Politica de Privacidad

        /// <summary>
        /// Permite obtener la Politica de Privacidad
        /// </summary>
        /// <returns></returns>
        public async Task<PoliticaPrivacidadEntity> GetPrivacyPolicy()
        {
            List<PoliticaPrivacidadEntity> _records = new List<PoliticaPrivacidadEntity>();
            TableContinuationToken token = null;
            PoliticaPrivacidadEntity _record = new PoliticaPrivacidadEntity();

            try
            {
                await ConnectToTable();

                do
                {
                    var _result = await _privacypolicyTable.ExecuteQuerySegmentedAsync(new TableQuery<PoliticaPrivacidadEntity>(), token);
                    token = _result.ContinuationToken;

                    if (_result.Results.Count > 0)
                        _records.AddRange(_result.Results);

                } while (token != null);

                _record = _records.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
            return _record;
        }

        #endregion


    }
}
