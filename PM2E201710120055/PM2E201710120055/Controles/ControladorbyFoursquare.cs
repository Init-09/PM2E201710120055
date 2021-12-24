using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PM2E201710120055.Controles
{
    public class ControladorbyFoursquare
    {

        public async static Task<List<Modelos.FourSquare.Venue>> GetListSites(double latitud, double longitud)
        {

            List<Modelos.FourSquare.Venue> sitioscercas = new List<Modelos.FourSquare.Venue>();


            using (HttpClient cliente = new HttpClient())
            {
                var respuesta = await cliente.GetAsync(ServidorFoursquare.getUrl(latitud, longitud));

                if (respuesta.IsSuccessStatusCode)
                {
                    var json = respuesta.Content.ReadAsStringAsync().Result;

                    var lugares = JsonConvert.DeserializeObject<Modelos.FourSquare.VenuesRest>(json);

                    sitioscercas = lugares.response.venues as List<Modelos.FourSquare.Venue>;
                }

            }



            return sitioscercas;
        }



    }
}
