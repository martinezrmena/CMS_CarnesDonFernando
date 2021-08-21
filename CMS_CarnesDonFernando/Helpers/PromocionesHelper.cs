using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CMS_CarnesDonFernando.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using CarnesDonFernando.EL;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;

namespace CMS_CarnesDonFernando.Helpers
{
    public class PromocionesHelper
    {
        #region URL

        private readonly string BaseUrl;

        public PromocionesHelper()
        {
            var AppSetting = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            BaseUrl = AppSetting.GetSection("ApiUrl").Value;
        }

        #endregion

        #region Consulta Promociones
        public List<PromocionesModel> ConsultarPromociones()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var ruta = BaseUrl + "api/Promociones/Consulta";

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync(ruta).Result;

                    if (response.IsSuccessStatusCode)
                        return response.Content.ReadAsAsync<List<PromocionesModel>>().Result;
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
        
        public PromocionesModel ConsultarPorLlaves(string pLlaveParticion, string pLlaveFila)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var ruta = BaseUrl + "api/Promociones/ConsultarPorLlaves?pLlaveParticion=" + pLlaveParticion + "&pLlaveFila=" + pLlaveFila;

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync(ruta).Result;

                    if (response.IsSuccessStatusCode)
                        return response.Content.ReadAsAsync<PromocionesModel>().Result;
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

        #region Consulta si existe promocion

        public PromocionesModel ExistePromocion(string pTitulo, string pEnlace, string pFechaIni, string pFechaFin)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var ruta = BaseUrl + "api/Promociones/Existe?pTitulo=" + pTitulo + "&pEnlace=" + pEnlace + "&pFechaIni=" + pFechaIni + "&pFechaFin=" + pFechaFin;

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync(ruta).Result;

                    if (response.IsSuccessStatusCode)
                    
                        return  response.Content.ReadAsAsync<PromocionesModel>().Result;
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

        public async Task<bool> Registrar(PromocionesModel pPromocion)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var ruta = BaseUrl + "api/Promociones/Registra";

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.PostAsJsonAsync(ruta, pPromocion).Result;
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

        public async Task<bool> Eliminar(PromocionesModel prom)
        {
            
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var ruta = BaseUrl + "api/Promociones/Elimina";
                    //var ruta = "http://localhost:53997/api/Promociones/Elimina";
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.PutAsJsonAsync(ruta, prom).Result;
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
