using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CMS_CarnesDonFernando.Models;
using System.Net.Http;
using System.Net.Http.Headers;


namespace CMS_CarnesDonFernando.Helpers
{
    public class SucursalesHelpers
    {
        #region URL

        private readonly string BaseUrl;

        public SucursalesHelpers()
        {
            var AppSetting = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            BaseUrl = AppSetting.GetSection("ApiUrl").Value;
        }

        #endregion

        #region Consulta Sucursales
        public List<SucursalesModel> ConsultarSucursales()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var ruta = BaseUrl + "api/Sucursales/ConsultarDatosSucursal";
                    //var ruta = "http://localhost:53997/api/Sucursales/ConsultarDatosSucursal";
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync(ruta).Result;

                    if (response.IsSuccessStatusCode)
                        return response.Content.ReadAsAsync<List<SucursalesModel>>().Result;
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

        public SucursalesModel ConsultarPorLlaves(string pLlaveParticion, string pLlaveFila)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                   var ruta = BaseUrl + "api/Sucursales/ConsultarPorLlaves?pLlaveParticion=" + pLlaveParticion + "&pLlaveFila=" + pLlaveFila;
                    //var ruta = "http://localhost:53997/api/Sucursales/ConsultarPorLlaves?pLlaveParticion=" + pLlaveParticion + "&pLlaveFila=" + pLlaveFila;

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync(ruta).Result;

                    if (response.IsSuccessStatusCode)
                        return response.Content.ReadAsAsync<SucursalesModel>().Result;
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

        #region Agregar/Actualizar Sucursales

        public async Task<bool> Registrar(SucursalesModel pSucursales)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var ruta = BaseUrl + "api/Sucursales/Registra";
                    //var ruta = "http://localhost:53997/api/Sucursales/Registra";
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.PostAsJsonAsync(ruta, pSucursales).Result;
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

        public async Task<bool> Eliminar(SucursalesModel prom)
        {

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var ruta = BaseUrl + "api/Sucursales/Elimina";
                    //var ruta = "http://localhost:53997/api/Sucursales/Elimina";
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
