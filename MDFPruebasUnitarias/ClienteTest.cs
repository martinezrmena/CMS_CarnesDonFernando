using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MDFPruebasUnitarias
{
    [TestClass]
    public class ClienteTest
    {
        [TestMethod]
        public async Task GetClienteCIISATest()
        {
            string Password = "qdexHG++5DG5EJ/ZujVwcQ==";
            string Cedula = "110680588";
            string Url = MomentosDFConst.ApiUrl + MomentosDFConst.UserConsulta;

            JObject jsonData = new JObject
            {
                    { "Cedula", Cedula},
                    { "Password", Password }
            };

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //Esperamos la respuesta
            var response = await client.PostAsJsonAsync(Url, jsonData);
            //Recibimos el modelo
            var model = await response.Content.ReadAsAsync<object>();
            //Obtenemos el contenido de la respuesta
            var stream = await response.Content.ReadAsStreamAsync();
        }

        [TestMethod]
        public async Task ConsultarClientesAzureTest() 
        {
            try
            {
                string Url = MomentosDFConst.ApiUrl + MomentosDFConst.UsersConsulta;

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = new TimeSpan(0,3,0);

                //Esperamos la respuesta
                var response = await client.GetAsync(Url);
                //Recibimos el modelo
                var model = await response.Content.ReadAsAsync<object>();

                if (model == null)
                {
                    throw new Exception();
                }

                //Obtenemos el contenido de la respuesta
                var stream = await response.Content.ReadAsStreamAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [TestMethod]
        public async Task ConsultarClientesCIISATest()
        {
            string Url = MomentosDFConst.ApiUrl + MomentosDFConst.UsersConsultaCIISA;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //Esperamos la respuesta
            var response = await client.GetAsync(Url);
            //Recibimos el modelo
            var model = await response.Content.ReadAsAsync<object>();
            //Obtenemos el contenido de la respuesta
            var stream = await response.Content.ReadAsStreamAsync();

        }
    }
}
