using CarnesDonFernando.EL;
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
    public class NotificacionHelper
    {
        #region URL

        private readonly string BaseUrl;

        public NotificacionHelper()
        {
            var AppSetting = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            BaseUrl = AppSetting.GetSection("ApiUrl").Value;
        }

        #endregion

        #region Enviar notificaciones

       public async Task<bool> Enviar_Android(PromocionesModel promo)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var ruta = BaseUrl + "api/Notificaciones/Enviar_Android";
                    //var ruta = "http://localhost:53997/api/Notificaciones/Enviar_Android";
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.PostAsJsonAsync(ruta, promo).Result;
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

       public async Task<bool> Enviar_iOS(PromocionesModel promo)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var ruta = BaseUrl + "api/Notificaciones/Enviar_iOS";
                    //var ruta = "http://localhost:53997/api/Notificaciones/Enviar_iOS";
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.PostAsJsonAsync(ruta, promo).Result;
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
    }
}
