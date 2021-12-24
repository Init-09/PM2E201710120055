using PM2E201710120055.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2E201710120055.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListViewSites : ContentPage
    {
        public ListViewSites()
        {
            InitializeComponent();
        }

        public async void Mostrar()
        {

            var lista = await App.BaseDatos.ObtenerListaSitios();
            lstUbicaciones.ItemsSource = lista;
        }



        protected override void OnAppearing()
        {
            base.OnAppearing();
            Mostrar();
        }

        private async void tlbeliminar_Clicked(object sender, EventArgs e)
        {


            var model = new Modelos.MisSitios
            {
                id = Convert.ToInt32(this.txtcodigo.Text),
                latitud = txtlatitud.Text,
                longitud = txtlongitud.Text,
                descripcion = this.txtdescripcion.Text,
                fotografia = this.txtdfoto.Text,

            };

            await Navigation.PushAsync(new UpdateDeleteView(model));

        }

        private void lstUbicaciones_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Modelos.MisSitios item = (Modelos.MisSitios)e.Item;
            
            this.txtcodigo.Text = Convert.ToString(item.id);
            this.txtlatitud.Text = Convert.ToString(item.latitud);
            this.txtlongitud.Text = Convert.ToString(item.longitud);
            this.txtdescripcion.Text = Convert.ToString(item.descripcion);
            this.txtdfoto.Text = Convert.ToString(item.fotografia);
        }


        private async void tlbmostrar_Clicked(object sender, EventArgs e)
        {
            var lat = Convert.ToDouble(txtlatitud.Text);
            var lon = Convert.ToDouble(txtlongitud.Text);

            await Navigation.PushAsync(new ViewFoursquare(lat, lon));
        }



        private async void SwipeItem_Invoked_2(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            MisSitios model = item.BindingContext as MisSitios;

            Double lat = Convert.ToDouble(model.latitud);
            Double longi = Convert.ToDouble(model.longitud);

            var location = new Location(lat, longi);
            var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };
            await Map.OpenAsync(location, options);
        }
    }
}