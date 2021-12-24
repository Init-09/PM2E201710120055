using Plugin.Media;
using PM2E201710120055.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2E201710120055.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateDeleteView : ContentPage
    {
        CancellationTokenSource cts;
        public Int32 codigo;
        string lati = "", longi = "", base64Val = "";

        public UpdateDeleteView(MisSitios model)
        {
            InitializeComponent();
            lblId.Text = model.id.ToString();
            txtdescripLarga.Text = model.descripcion;
            imagen.Source = Xamarin.Forms.ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(model.fotografia))); ;
            
        }

      
        private async void btnfotocap_Clicked(object sender, EventArgs e)
        {
            var tomarfoto = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "miApp",
                Name = "Image.jpg"

            });




            if (tomarfoto != null)
            {
                imagen.Source = ImageSource.FromStream(() => { return tomarfoto.GetStream(); });
            }

            Byte[] imagenByte = null;

            using (var stream = new MemoryStream())
            {
                tomarfoto.GetStream().CopyTo(stream);
                tomarfoto.Dispose();
                imagenByte = stream.ToArray();

                base64Val = Convert.ToBase64String(imagenByte);
                //await EmpleController.SubirImagen(imagenByte);
            }
        }

        private async void btnsalnvar_Clicked(object sender, EventArgs e)
        {
            //condicion para ver si los campos estan vacios
            if (String.IsNullOrWhiteSpace(base64Val) == true)
            {
                await DisplayAlert("Mensaje", "Foto vacia", "Ok");
            }
            else
            {
                if (String.IsNullOrWhiteSpace(txtdescripLarga.Text) == true)
                {
                    await DisplayAlert("Mensaje", "La Descripcion esta vacia", "Ok");
                }
                else
                {
                    _ = TenerLocacion(true);
                }
            }
        }

        public async void EvaluarInternet()
        {
            var current = Connectivity.NetworkAccess;

            // en caso de tener internet obtener la ubicacion
            if (current == NetworkAccess.Internet)
            {
                // Connection to internet is available
                //es falso para no guardar informacion solo mostrar, si el parametro es true guarda
                _ = TenerLocacion(false);
            }
            else
            {
                await DisplayAlert("error", "Sin Internet", "Ok");

            }
        }


        async Task TenerLocacion(bool guardar)
        {

            // le mando parametros para al momento de dar click en guardar 
            //le mondo un valor que es true solo al darle click
            // le mando falso para que no guarde informacion

            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location == null)
                {
                    await DisplayAlert("error", "GPS Inactivo", "Ok");
                }

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");

                    lati = location.Latitude.ToString();
                    longi = location.Longitude.ToString();
                    lbllatitud.Text = lati;
                    lbllongitud.Text = longi;


                    //si no es true solo va a mostrar los datos y NO va guardar
                    if (guardar == true)
                    {
                        ModificarSitios();
                    }

                }
            }
            catch (FeatureNotSupportedException)
            {
                // Handle not supported on device exception
                await DisplayAlert("error", "no es compatible con la excepción del dispositivo GPS", "Ok");

            }
            catch (FeatureNotEnabledException)
            {
                // Handle not enabled on device exception
                await DisplayAlert("error", "la ubicacion no habilitado en la excepción del dispositivo", "Ok");
            }
            catch (PermissionException)
            {
                // Handle permission exception
                await DisplayAlert("error", "No tiene Permisos de ubicacion", "Ok");
            }
            catch (Exception)
            {
                // Unable to get location
                await DisplayAlert("error", "No se puede tener la ubicacion", "Ok");
            }
        }

        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            base.OnDisappearing();
        }

        private void btnseliminar_Clicked(object sender, EventArgs e)
        {

        }

        protected override void OnAppearing()
        {

            base.OnAppearing();
            EvaluarInternet();
        }

        public async void ModificarSitios()
        {


            MisSitios siti = new MisSitios
            {
                id = Convert.ToInt32(lblId.Text),
                descripcion = txtdescripLarga.Text,
                latitud = lati,
                longitud = longi,
                fotografia =  base64Val

            };
            var resultado = await App.BaseDatos.GrabarSitios(siti);

            if (resultado == 1)
            {
                await DisplayAlert("Mensaje", "Modificado exitoso!!!", "ok");
                txtdescripLarga.Text = base64Val = String.Empty;
                imagen.Source = "perfil.jpg";

                await Navigation.PushAsync(new ListViewSites());
            }
            else
            {
                await DisplayAlert("Error", "No se pudo Guardar", "ok");
            }
             

        }


    }
}