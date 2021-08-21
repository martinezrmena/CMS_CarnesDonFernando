using Microsoft.Azure.NotificationHubs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarnesDonFernando.EL
{
    public class Notifications
    {
        public static Notifications Instance = new Notifications();
        public NotificationHubClient Hub { get; set; }

        private Notifications()
        {
            Hub = NotificationHubClient.CreateClientFromConnectionString("Endpoint=sb://momentosdonfernando.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=JzJkwDwBvO2MpXIg04UuCjMJ2jGQ1O2VgIg+GyDiVbQ=",
                "appcarnesDFN", false);
        }
    }
}
