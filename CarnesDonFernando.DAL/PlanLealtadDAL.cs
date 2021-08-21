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
   public class PlanLealtadDAL
    {
        #region Propiedades

        private static readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=appmomentosdf;AccountKey=jxxkCEhAjwvDBda+0sF4cy/Etul9DY1LM3cGMv5Bp8ACrYPhY3yNYpwI77IYVnB4T/7gqbQ3/eFA0049VhPIuw==;EndpointSuffix=core.windows.net";
        static CloudStorageAccount _cloudStorageAccount;
        static CloudTableClient _tableClient;
        static CloudTable _loyaltyPlanTable;
      
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
            _loyaltyPlanTable = _tableClient.GetTableReference("Loyaltyplan");
            await _loyaltyPlanTable.CreateIfNotExistsAsync();
        }

        #region Consultas Planes de Lealtad

        /// <summary>
        /// Permite obtener una lista de todos los planes de lealtad
        /// </summary>
        /// <returns></returns>
        public async Task<List<PlanLealtadEntity>> GetPlans()
        {
            List<PlanLealtadEntity> _records = new List<PlanLealtadEntity>();
            TableContinuationToken token = null;

            try
            {
                await ConnectToTable();

                do
                {
                    var _result = await _loyaltyPlanTable.ExecuteQuerySegmentedAsync(new TableQuery<PlanLealtadEntity>(), token);
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
        /// Permite obtener un plan de lealtad especifico por las llaves de particion y fila.
        /// </summary>
        /// <param name="partitionKey">identificador del modelo</param>
        /// <param name="rowKey">idenificacion del modelo</param>
        /// <returns></returns>
        public async Task<PlanLealtadEntity> GetPlan(string partitionKey, string rowKey)
        {
            List<PlanLealtadEntity> promotions = new List<PlanLealtadEntity>();
            TableContinuationToken token = null;
            PlanLealtadEntity promotion = new PlanLealtadEntity();

            try
            {
                await ConnectToTable();

                //Create the table query
                var condition = TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey));

                var query = new TableQuery<PlanLealtadEntity>().Where(condition);
                var lst = _loyaltyPlanTable.ExecuteQuerySegmentedAsync(query, token);

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

        #region Metodos Insertar-Actualizar Planes de Lealtad

        /// <summary>
        /// Metodo que permite actualizar o insertar un plan de lealtad en el Azure Tab
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task<bool> SavePlan(PlanLealtadEntity plan)
        {
            try
            {
                await ConnectToTable();
                plan.Timestamp = new DateTimeOffset();

                var operation = TableOperation.InsertOrMerge(plan);
                var upsert = await _loyaltyPlanTable.ExecuteAsync(operation);

                return (upsert.HttpStatusCode == 204);
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        #endregion

        #region Eliminar Planes de Lealtad

        /// <summary>
        /// Metodo que permita eliminar un plan de Lealtad
        /// </summary>
        /// <param name="plan">Modelo de tipo plan lealtad</param>
        /// <returns>Un bool que indica si la operación fue exitosa</returns>
        public async Task<bool> DeletePlan(PlanLealtadEntity plan)
        {
            AzureBlobStorageDAL blobDAL = new AzureBlobStorageDAL();

            try
            {
                await ConnectToTable();

                var tablePlan = new TableEntity() { PartitionKey = plan.PartitionKey, RowKey = plan.RowKey, ETag = "*" };

                var operation = TableOperation.Delete(tablePlan);
                var delete = await _loyaltyPlanTable.ExecuteAsync(operation);

                if (delete.HttpStatusCode == 204)
                {
                    return await blobDAL.DeleteImage(plan.Foto, "plansimages");
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
