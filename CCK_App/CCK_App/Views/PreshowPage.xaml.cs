using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CCK_App.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Connectivity;

namespace CCK_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PreshowPage : ContentPage
    {
        public PreshowPage()
        {
            InitializeComponent();
        }

        private async void CloseBtn_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void BtnScanPreshow_OnClicked(object sender, EventArgs e)
        {
            try
            {
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                scanner.TopText = "Escanear Qr";
                var result = await scanner.Scan();

                await Task.Delay(100);
                UserDialogs.Instance.ShowLoading();

                var separator = '-';
                string[] qr_data = result.Text.Split(separator);
                var x = qr_data[2].Split(' ');
                qr_data[2] = x[0] + ' ' + x[1];

                if (result != null)
                {

                    DateTime dt_actual = DateTime.Now;
                    int qr_bloque = Convert.ToInt32(qr_data[0]);
                    string qr_dni = qr_data[1];
                    DateTime qr_dt = Convert.ToDateTime(qr_data[2]);
                    Entradas entrada = null;

                    if (CrossConnectivity.Current.IsConnected)
                    {
                        var data = await ApiClient.ApiGetTicketsByDni(qr_dni, qr_data[0]);

                        foreach (var d in data)
                        {
                            if (d.idEventos == qr_bloque)
                            {
                                entrada = d;
                            }
                        }

                        if (entrada != null)
                        {
                            if (entrada.Preshow == null)
                            {
                                await ApiClient.ApiPutTicketPreshow(entrada.idEntradas);
                                var evento = await ApiClient.ApiGetEventoById(entrada.idEventos);
                                UserDialogs.Instance.HideLoading();
                                await Navigation.PushModalAsync(new Pass(entrada.Nombre, entrada.DNI, entrada.Visitantes, evento.Evento));
                            }
                            else
                            {
                                UserDialogs.Instance.HideLoading();
                                await Navigation.PushModalAsync(
                                    new NotPass($"Qr escaneado a las {Convert.ToDateTime(entrada.Preshow).ToString("hh:mm:ss")}"));
                            }
                        }
                        else
                        {
                            UserDialogs.Instance.HideLoading();
                            await Navigation.PushModalAsync(
                                new NotPass("No esta registrado"));
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        if ( dt_actual.AddHours(-12).TimeOfDay > qr_dt.AddHours(-2).TimeOfDay)
                        {
                            await Navigation.PushModalAsync(new Pass("", qr_dni, 1));
                        }
                        else
                        {
                            await Navigation.PushModalAsync(
                                new NotPass($"Su horario es a las {qr_dt.ToString("hh:mm:ss")}"));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                UserDialogs.Instance.HideLoading();
                string invalid_qr = "Index was outside";
                string anerror = "Object reference not set to an instance of an object";
                var x = exception.Message.IndexOf(invalid_qr);
                var z = exception.Message.IndexOf(anerror);
                if (x != -1)
                    await Navigation.PushModalAsync(
                        new NotPass("Codigo invalido"));
                else if (z != -1) {}
                else
                    await DisplayAlert("Error", exception.Message, "Ok");
            }
        }

        private async void InputDni_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new InputDniPS());
        }
    }
}