using Plugin.Media;
using PM2E201710120055.Modelos;
using PM2E201710120055.Vistas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PM2E201710120055
{
    public partial class MainPage : ContentPage
    {
        CancellationTokenSource cts;
        string lati = "", longi = "", base64 = "";

        public MainPage()
        {
            InitializeComponent();
        }

        private async void BtnCam_Clicked(object sender, EventArgs e)
        {
            var tomarfoto = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "miApp",
                Name = "Image.jpg"

            });




            if (tomarfoto != null)
            {
                imgCam.Source = ImageSource.FromStream(() => { return tomarfoto.GetStream(); });
            }

            Byte[] imagenByte = null;

            using (var stream = new MemoryStream())
            {
                tomarfoto.GetStream().CopyTo(stream);
                tomarfoto.Dispose();
                imagenByte = stream.ToArray();

                base64 = Convert.ToBase64String(imagenByte);
            }


        }

        private async void btnguardar_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(base64) == true)
            {
                await DisplayAlert("Mensaje", "Error, no hay imagen aun", "OK");
            }
            else
            {
                if (String.IsNullOrWhiteSpace(this.txtlatitud.Text) == true)
                {
                    await DisplayAlert("Mensaje", "Datos incompletos", "OK");
                }
                else
                {
                    if (String.IsNullOrWhiteSpace(this.txtlongitud.Text) == true)
                    {
                        await DisplayAlert("Mensaje", "Datos incompletos", "OK");
                    }
                    else
                    {
                        if (String.IsNullOrWhiteSpace(this.txtdescripcion.Text) == true)
                        {
                            await DisplayAlert("Mensaje", "Datos incompletos", "OK");
                        }
                        else
                        {
                            _ = TenerLocacion(true);
                        }
                    }
                }
            }


        }

        private async void btn02_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListViewSites());
        }


        public async void EvaluarInternet()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                _ = TenerLocacion(false);
            }
            else
            {
                await DisplayAlert("error", "Sin Internet", "Ok");

            }
        }

        async Task TenerLocacion(bool guardar)
        {
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
                    txtlatitud.Text = lati;
                    txtlongitud.Text = longi;
                    if (guardar == true)
                    {
                        GuardarSitios();
                    }

                }
            }
            catch (FeatureNotSupportedException)
            {
                await DisplayAlert("error", "no es compatible con la excepción del dispositivo GPS", "Ok");

            }
            catch (FeatureNotEnabledException)
            {
                await DisplayAlert("error", "la ubicacion no habilitado en la excepción del dispositivo", "Ok");
            }
            catch (PermissionException)
            {
                await DisplayAlert("error", "No tiene Permisos de ubicacion", "Ok");
            }
            catch (Exception)
            {
                await DisplayAlert("error", "No se puede tener la ubicacion", "Ok");
            }
        }


        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            base.OnDisappearing();
        }


        protected override void OnAppearing()
        {

            base.OnAppearing();
            EvaluarInternet();
        }

        public async void GuardarSitios()
        {


            MisSitios siti = new MisSitios
            {
                descripcion = txtdescripcion.Text,
                latitud = lati,
                longitud = longi,
                fotografia = base64

            };

            var resultado = await App.BaseDatos.GrabarSitios(siti);

            if (resultado == 1)
            {
                await DisplayAlert("Mensaje", "Registro exitoso!!!", "ok");
                txtdescripcion.Text = base64 = String.Empty;
                imgCam.Source = "https://www.segundamano.mx/img/nga/inmuebles.png";
            }
            else
            {
                await DisplayAlert("Error", "No se pudo Guardar", "ok");
            }

            
             

        }


    }
}
