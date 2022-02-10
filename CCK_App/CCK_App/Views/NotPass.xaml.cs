using Acr.UserDialogs;
using CCK_App.Models;
using Plugin.DeviceInfo;
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
    public partial class NotPass : ContentPage
    {
        public NotPass(string message = null)
        {
            InitializeComponent();
            NPMessage.Text = message;
        }

        private async void PEBtn_OnClicked(object sender, EventArgs e)
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

                    //if (dt_actual.AddHours(-12).TimeOfDay < qr_dt.AddMinutes(30).TimeOfDay && dt_actual.AddHours(-12).TimeOfDay > qr_dt.AddHours(-1).AddMinutes(-30).TimeOfDay)
                    if (true)

                    {
                        var data = await ApiClient.ApiGetTicketsByDni(qr_dni, qr_data[0]);

                        Entradas entrada = null;
                        foreach (var d in data)
                        {
                            if (d.idEventos == qr_bloque)
                            {
                                entrada = d;
                            }
                        }

                        if (entrada != null)
                        {
                            if (entrada.Show == null)
                            {
                                await ApiClient.ApiPutTicketShow(entrada.idEntradas);
                                var evento = await ApiClient.ApiGetEventoById(entrada.idEventos);
                                UserDialogs.Instance.HideLoading();
                                await Navigation.PushModalAsync(new Pass(entrada.Nombre, entrada.DNI, entrada.Visitantes, evento.Evento));
                            }
                            else
                            {
                                string device = "";
                                if (entrada.Sid == CrossDeviceInfo.Current.Id)
                                    device = "En esta Terminal";
                                else
                                    device = "En otra Terminal";

                                UserDialogs.Instance.HideLoading();
                                await Navigation.PushModalAsync(
                                    new NotPass($"Qr escaneado a las {Convert.ToDateTime(entrada.Show).ToString("hh:mm:ss")} \n{device}"));
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
                        await Navigation.PushModalAsync(
                            new NotPass($"Su horario es a las {qr_dt.ToString("hh:mm:ss")}"));
                    }
                }
            }
            catch (Exception exception)
            {
                UserDialogs.Instance.HideLoading();
                string invalid_qr = "Index was outside";
                string no_con = "Unable to resolve host";
                string anerror = "Object reference not set to an instance of an object";
                var x = exception.Message.IndexOf(invalid_qr);
                var z = exception.Message.IndexOf(anerror);
                var y = exception.Message.IndexOf(no_con);
                if (x != -1)
                    await Navigation.PushModalAsync(
                        new NotPass("Codigo invalido"));
                else if (z != -1) { }
                else if (y != -1)
                    await Navigation.PushModalAsync(
                        new NotPass("No tiene conexion a internet"));
                else
                    await DisplayAlert("Error", exception.Message, "Ok");
            }
        }

        private async void BackToMenuBtn_OnClicked(object sender, EventArgs e)
        {
            int numModals = Application.Current.MainPage.Navigation.ModalStack.Count;
            for (int currModal = 0; currModal < numModals; currModal++)
            {
                Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }
    }
}