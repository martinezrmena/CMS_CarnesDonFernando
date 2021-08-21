using CMS_CarnesDonFernando.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CMS_CarnesDonFernando.Helpers
{
    public class PlanLealtadHelper
    {
        #region URL

        private readonly string BaseUrl;

        public PlanLealtadHelper()
        {
            var AppSetting = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            BaseUrl = AppSetting.GetSection("ApiUrl").Value;
        }

        #endregion

        #region Consulta Plan de lealtad
        public List<PlanLealtadModel> ConsultarPlanLealtad()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var ruta = BaseUrl + "api/PlanLealtad/Consulta";
                    //var ruta = "http://localhost:53997/api/PlanLealtad/Consulta";
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync(ruta).Result;

                    if (response.IsSuccessStatusCode)
                        return response.Content.ReadAsAsync<List<PlanLealtadModel>>().Result;
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                var mensaje = ex.Message;
                return null;
            }
        }
        #endregion

        #region Consultar por Llaves

        public PlanLealtadModel ConsultarPorLlaves(string pLlaveParticion, string pLlaveFila)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var ruta = BaseUrl + "api/PlanLealtad/ConsultarPorLlaves?pLlaveParticion=" + pLlaveParticion + "&pLlaveFila=" + pLlaveFila;
                    //var ruta = "http://localhost:53997/api/PlanLealtad/ConsultarPorLlaves?pLlaveParticion=" + pLlaveParticion + "&pLlaveFila=" + pLlaveFila;
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync(ruta).Result;

                    if (response.IsSuccessStatusCode)
                        return response.Content.ReadAsAsync<PlanLealtadModel>().Result;
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                var mensaje = ex.Message;
                return null;
            }
        }

        #endregion


        #region Agregar/Actualizar Promociones

        public async Task<bool> Registrar(PlanLealtadModel pPlanLealtad)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var ruta = BaseUrl + "api/PlanLealtad/Registra";
                    //var ruta = "http://localhost:53997/api/PlanLealtad/Registra";
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.PostAsJsonAsync(ruta, pPlanLealtad).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsAsync<bool>();

                    }
                    return false;

                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region Eliminar registros 

        public async Task<bool> Eliminar(PlanLealtadModel pPlanLealtad)
        {

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var ruta = BaseUrl + "api/PlanLealtad/Elimina";
                    //var ruta = "http://localhost:53997/api/PlanLealtad/Elimina";
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.PutAsJsonAsync(ruta, pPlanLealtad).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsAsync<bool>();

                    }
                    return true;

                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}
