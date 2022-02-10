using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CCK_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InputDniPS : ContentPage
    {
        public InputDniPS()
        {
            InitializeComponent();
        }
        
        private async void CloseBtn_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void EnterBtn_OnClicked(object sender, EventArgs e)
        {
            var dni = EntryDni.Text;
            var can_pass = false;
            var data = await ApiClient.ApiGetTicketsByDni(dni, null);
            foreach (var item in data)
            {
                if (item.FechaV.Day == DateTime.Now.Day && item.Preshow == null)
                {
                    await ApiClient.ApiPutTicketPreshow(item.idEntradas);
                    var evento = await ApiClient.ApiGetEventoById(item.idEventos);
                    await Navigation.PushModalAsync(new Pass(item.Nombre, item.DNI, item.Visitantes, evento.Evento));
                    can_pass = true;
                    break;
                }
            }

            if (!can_pass)
            {
                await Navigation.PushModalAsync(new NotPass("No se encontro en la base de datos"));
            }
        }
    }
}