using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Settings
{
    public static class Configration
    {
        public static readonly BLL.Enums.DevServer Server = BLL.Enums.DevServer.Publish;
        public static string ApiServerAddress
        {
            get
            {
                return Server switch
                {
                    Enums.DevServer.Local => "http://192.168.0.103:5000",
                    Enums.DevServer.Publish => "https://tank-eyes.azurewebsites.net",
                    _ => throw new NotImplementedException(),
                };
            }
        }
    }
}