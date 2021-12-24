using System;
using System.Collections.Generic;
using System.Text;

namespace PM2E201710120055.Controles
{
    

    public class ServidorFoursquare
    {
        #region Conexion con el Servidor de Foursquare
        public const string client_id = "VJMKF4LTPUGX5LYESDTNFJ51TGFYO5GPHDHCN3UBR0PGKRHE";
        public const string client_secret = "PRXJ0FEJY3HBHCQBCIMBX0EJWRACCOXSRWWKYYU1SKPBXYZV";
        #endregion
        public const string EndPointFoursquare = "https://api.foursquare.com/v2/venues/search?ll={0},{1}&client_id={2}&client_secret={3}&v={4}";

        public static String getUrl(double latitud, double longitud)
        {
            var url = String.Format(EndPointFoursquare, latitud, longitud,
                 client_id, client_secret, DateTime.Now.ToString("yyyyMMdd"));

            return url;
        }

    }
}
