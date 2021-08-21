using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CarnesDonFernando.EL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.NotificationHubs;

namespace WebApi_CarnesDonFernando.Controllers
{
    /// <summary>
    /// Clase encargada del envio de las notificaciones Push
    /// </summary>
    [Produces("application/json")]
    [Route("api/Notificaciones")]
    [ApiController]
    public class NotificacionesController : ControllerBase
    {
        #region Envio de Notificacion
        /// <summary>
        /// Permite enviar una notificación push a los dispositivos Android.
        /// </summary>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     POST /Send
        ///     {
        ///        "PromocionesEntity": [
        ///                             {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX", //Clave unica
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", //Clave unica
        ///                                 "Titulo": "Promoción de lujo"
        ///                             }
        ///                         ]
        ///     }
        ///
        /// </remarks>
        /// <returns>Código de resultado.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("Enviar_Android")]
        public async Task<IActionResult> SendNotification(PromocionesEntity pPromocion)
        {
            string mensaje = "Nueva promoción: " + pPromocion.Titulo;

            NotificationOutcome outcome = null;
            var ret = HttpStatusCode.InternalServerError;

            string androidNotification = "{ \"data\" : {\"message\":\"" + mensaje + "\"}}";

            // Android
            outcome = await Notifications.Instance.Hub.SendFcmNativeNotificationAsync(androidNotification).ConfigureAwait(false);

            if (outcome != null)
            {

                if (!((outcome.State == NotificationOutcomeState.Abandoned) ||
                (outcome.State == NotificationOutcomeState.Unknown)))
                {
                    ret = HttpStatusCode.OK;
                }

            }

            return StatusCode((int)ret);

        }

        /// <summary>
        /// Permite enviar una notificación push a los dispositivos IOS.
        /// </summary>
        /// <remarks>
        /// Ejemplo de petición:
        ///
        ///     POST /Send
        ///     {
        ///        "PromocionesEntity": [
        ///                             {
        ///                                 "PartitionKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX", //Clave unica
        ///                                 "RowKey": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", //Clave unica
        ///                                 "Titulo": "Promoción de lujo"
        ///                             }
        ///                         ]
        ///     }
        ///
        /// </remarks>
        /// <returns>Código de resultado.</returns>
        /// <response code="200">Resultado exitoso</response>
        /// <response code="400">Sí el elemento es null</response>
        /// <response code="500">Error interno en el servidor</response>
        [HttpPost]
        [Route("Enviar_iOS")]
        public async Task<IActionResult> SendNotification_iOS(PromocionesEntity pPromocion)
        {
            string mensaje = "Nueva promoción: " + pPromocion.Titulo;
            NotificationOutcome outcome = null;
            var ret = HttpStatusCode.InternalServerError;

            string appleNotification = "{\"aps\":{\"alert\":\"" + mensaje + "\"}}";

            // Send the push notification and log the results.

            // iOS
            outcome = await Notifications.Instance.Hub.SendAppleNativeNotificationAsync(appleNotification).ConfigureAwait(false);


            if (outcome != null)
            {

                if (!((outcome.State == NotificationOutcomeState.Abandoned) ||
                (outcome.State == NotificationOutcomeState.Unknown)))
                {
                    ret = HttpStatusCode.OK;
                }

            }

            return StatusCode((int)ret);

        }
        #endregion

    }
}