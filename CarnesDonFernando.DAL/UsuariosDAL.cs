using CarnesDonFernando.EL;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarnesDonFernando.DAL
{
   public class UsuariosDAL
    {
        #region Propiedades

        private static readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=appmomentosdf;AccountKey=jxxkCEhAjwvDBda+0sF4cy/Etul9DY1LM3cGMv5Bp8ACrYPhY3yNYpwI77IYVnB4T/7gqbQ3/eFA0049VhPIuw==;EndpointSuffix=core.windows.net";
        static CloudStorageAccount cloudStorageAccount;
        static CloudTableClient tableClient;
        static CloudTable UsersTable;

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
            UsersTable = tableClient.GetTableReference("Users");
            await UsersTable.CreateIfNotExistsAsync();
        }

        #region Consultas

        /// <summary>
        /// Permite obtener una lista de todos los usuarios
        /// </summary>
        /// <returns></returns>
        public async Task<List<UsuariosEntity>> GetUsers()
        {
            await ConnectToTable();
            TableContinuationToken continuationToken = null;
            var users = new List<UsuariosEntity>();

            try
            {
                do
                {
                    var result = await UsersTable.ExecuteQuerySegmentedAsync(new TableQuery<UsuariosEntity>(), continuationToken);
                    continuationToken = result.ContinuationToken;

                    if (result.Results.Count > 0)
                        users.AddRange(result.Results);
                } while (continuationToken != null);
            }
            catch (Exception ex)
            {

            }
            return users;
        }

        /// <summary>
        /// Permite obtener un usuario especifico por email y contraseña
        /// </summary>
        /// <param name="email">Correo electronico del usuario</param>
        /// <param name="password">un string cifrado</param>
        /// <returns>Un modelo de tipo UserModel</returns>
        public async Task<UsuariosEntity> GetUser(string email, string password)
        {
            try
            {
                await ConnectToTable();

                // Definir el token de cancelación.
                TableContinuationToken continuationToken = null;
                var users = new List<UsuariosEntity>();

                // Create the table query.
                var condition = TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, email),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("Password", QueryComparisons.Equal, password));

                var query = new TableQuery<UsuariosEntity>().Where(condition);


                do
                {
                    var result = await UsersTable.ExecuteQuerySegmentedAsync(query, continuationToken);
                    continuationToken = result.ContinuationToken;

                    if (result.Results.Count > 0)
                        users.AddRange(result.Results);
                } while (continuationToken != null && users.Count == 0);

                return users.FirstOrDefault();
            }
            catch (Exception)
            {
            }

            return null;
        }

        /// <summary>
        /// Permite obtener un usuario especifico por cedula
        /// </summary>
        /// <param name="cedula">El identificador del modelo</param>
        /// <returns>Un modelo de tipo UserModel</returns>
        public async Task<UsuariosEntity> GetUser(string cedula)
        {
            try
            {
                await ConnectToTable();

                TableContinuationToken continuationToken = null;
                var users = new List<UsuariosEntity>();

                // Create the table query.
                var condition = TableQuery.GenerateFilterCondition("Cedula", QueryComparisons.Equal, cedula);
                var query = new TableQuery<UsuariosEntity>().Where(condition);

                do
                {
                    var result = await UsersTable.ExecuteQuerySegmentedAsync(query, continuationToken);
                    continuationToken = result.ContinuationToken;

                    if (result.Results.Count > 0)
                        users.AddRange(result.Results);
                } while (continuationToken != null && users.Count == 0);

                return users.FirstOrDefault();
            }
            catch (Exception)
            {
            }

            return null;
        }


        /// <summary>
        /// Permite obtener usuarios especificos pendientes de actualizar en servicios del arreo
        /// </summary>
        /// <param name="sincronizar">El identificador del grupo a actualziar</param>
        /// <returns>Un modelo de tipo UserModel</returns>
        public async Task<List<UsuariosEntity>> GetUsersSync(bool sincronizar)
        {
            try
            {
                await ConnectToTable();

                TableContinuationToken continuationToken = null;
                var users = new List<UsuariosEntity>();

                // Create the table query.
                var condition = TableQuery.GenerateFilterConditionForBool("Sincronizado", QueryComparisons.Equal, sincronizar);
                var query = new TableQuery<UsuariosEntity>().Where(condition);

                do
                {
                    var result = await UsersTable.ExecuteQuerySegmentedAsync(query, continuationToken);
                    continuationToken = result.ContinuationToken;

                    if (result.Results.Count > 0)
                        users.AddRange(result.Results);
                } while (continuationToken != null);

                return users;
            }
            catch (Exception)
            {
            }

            return null;
        }


        /// <summary>
        /// Permite obtener un usuario especifico por correo
        /// </summary>
        /// <param name="correo">El identificador del modelo</param>
        /// <returns>Un modelo de tipo UserModel</returns>
        public async Task<UsuariosEntity> GetUserMail(string correo)
        {
            try
            {
                await ConnectToTable();

                // Definir el token de cancelación.
                TableContinuationToken continuationToken = null;
                var users = new List<UsuariosEntity>();

                // Create the table query.
                var condition = TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, correo);
                var query = new TableQuery<UsuariosEntity>().Where(condition);

                do
                {
                    var result = await UsersTable.ExecuteQuerySegmentedAsync(query, continuationToken);
                    continuationToken = result.ContinuationToken;

                    if (result.Results.Count > 0)
                        users.AddRange(result.Results);
                } while (continuationToken != null && users.Count == 0);

                return users.FirstOrDefault();
            }
            catch (Exception)
            {
            }

            return null;
        }

        #endregion

        #region Insertar-Actualizar

        /// <summary>
        /// Metodo que permite actualizar o insertar un usuario en el Azure Table
        /// </summary>
        /// <param name="user">Modelo de tipo usuario</param>
        /// <returns>Un bool que indica si la operación fue exitosa</returns>
        public async Task<bool> SaveUser(UsuariosEntity user)
        {
            try
            {
                await ConnectToTable();
                user.Timestamp = new DateTimeOffset();
                var operation = TableOperation.InsertOrMerge(user);
                var upsert = await UsersTable.ExecuteAsync(operation);

                return (upsert.HttpStatusCode == 204);
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        #endregion

        #region Eliminar

        /// <summary>
        /// Metodo que permita eliminar un usuario
        /// </summary>
        /// <param name="user">Modelo de tipo usaurio</param>
        /// <returns>Un bool que indica si la operación fue exitosa</returns>
        public async Task<bool> DeleteUser(UsuariosEntity user)
        {
            try
            {
                await ConnectToTable();
                var operation = TableOperation.Delete(user);
                var delete = await UsersTable.ExecuteAsync(operation);
                return (delete.HttpStatusCode == 204);
            }
            catch (Exception ex)
            {

            }

            return false;
        }


        #endregion
    }
}
