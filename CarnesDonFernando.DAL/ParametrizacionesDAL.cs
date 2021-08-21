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
    public class ParametrizacionesDAL
    {
        #region Propiedades

        private static readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=appmomentosdf;AccountKey=jxxkCEhAjwvDBda+0sF4cy/Etul9DY1LM3cGMv5Bp8ACrYPhY3yNYpwI77IYVnB4T/7gqbQ3/eFA0049VhPIuw==;EndpointSuffix=core.windows.net";

        static CloudStorageAccount _cloudStorageAccount;
        static CloudTableClient _tableClient;
        static CloudTable _SettingsTable;

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
            _SettingsTable = _tableClient.GetTableReference("Settings");
            await _SettingsTable.CreateIfNotExistsAsync();
        }

        #region Consultas Acerca De

        /// <summary>
        /// Permite obtener las parametrizaciones de la tabla de azure.
        /// </summary>
        /// <returns></returns>
        public async Task<ParametrizacionesEntity> GetSettings()
        {

            List<ParametrizacionesEntity> _records = new List<ParametrizacionesEntity>();
            ParametrizacionesEntity _Entity = new ParametrizacionesEntity();
            TableContinuationToken token = null;

            try
            {
                await ConnectToTable();

                do
                {
                    var _result = await _SettingsTable.ExecuteQuerySegmentedAsync(new TableQuery<ParametrizacionesEntity>(), token);
                    token = _result.ContinuationToken;

                    if (_result.Results.Count > 0)
                        _records.AddRange(_result.Results);

                } while (token != null);

                _Entity = _records.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Error-" + ex);
            }
            return _Entity;
        }

        #endregion

        #region Metodos Insertar-Actualizar Acerca De

        /// <summary>
        /// Metodo que permite actualizar o insertar las parametrizaciones en la Azure Table
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public async Task<bool> SaveSettings(ParametrizacionesEntity settings)
        {
            try
            {
                await ConnectToTable();
                settings.Timestamp = new DateTimeOffset();

                var operation = TableOperation.InsertOrMerge(settings);
                var upsert = await _SettingsTable.ExecuteAsync(operation);

                return (upsert.HttpStatusCode == 204);
            }
            catch (Exception ex)
            {
                throw new Exception("Error-" + ex);

            }
            return false;
        }

        #endregion

    }
}
