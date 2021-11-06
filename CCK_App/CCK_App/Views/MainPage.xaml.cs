using System;
using CCK_App.Models;
using CCK_App;
using Plugin.DeviceInfo;
using Xamarin.Forms;
using Acr.UserDialogs;
using static CCK_App.ApiClient;

namespace CCK_App.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            deviceNumber.Text = CrossDeviceInfo.Current.Id;
        }

        private async void BtnScanShow_OnClicked(object sender, EventArgs e)
        {
            try
            {
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                scanner.TopText = "Escanear Qr";
                var result = await scanner.Scan();

                var separator = '-';
                string[] qr_data = result.Text.Split(separator);
                var x = qr_data[2].Split(' ');
                qr_data[2] = x[0] + ' ' + x[1];

                if (result != null)
                {
                    UserDialogs.Instance.ShowLoading();

                    DateTime dt_actual = DateTime.Now;
                    int qr_bloque = Convert.ToInt32(qr_data[0]);
                    string qr_dni = qr_data[1];
                    DateTime qr_dt = Convert.ToDateTime(qr_data[2]);


                    if (dt_actual.AddHours(-12).TimeOfDay < qr_dt.AddMinutes(30).TimeOfDay && dt_actual.AddHours(-12).TimeOfDay > qr_dt.AddHours(-1).AddMinutes(-30).TimeOfDay)
                    {
                        var data = await ApiClient.ApiGetTicketsByDni(qr_dni);

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
                                UserDialogs.Instance.HideLoading();
                                await Navigation.PushModalAsync(new Pass(entrada.Nombre, entrada.DNI, entrada.idEventos));
                            }
                            else
                            {
                                UserDialogs.Instance.HideLoading();
                                await Navigation.PushModalAsync(
                                    new NotPass($"Qr escaneado a las {Convert.ToDateTime(entrada.Show).ToString("hh:mm:ss")}"));
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
                await DisplayAlert("Error", exception.Message, "Ok");
            }
        }

        private async void BtnScanPreshow_OnClicked(object sender, EventArgs e)
        {
            try
            {
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                scanner.TopText = "Escanear Qr";
                var result = await scanner.Scan();

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

                    UserDialogs.Instance.ShowLoading();

                    if (true)
                    {
                        var data = await ApiClient.ApiGetTicketsByDni(qr_dni);
                        
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
                            if (entrada.Preshow == null)
                            {
                                await ApiClient.ApiPutTicketPreshow(entrada.idEntradas);
                                UserDialogs.Instance.HideLoading();
                                await Navigation.PushModalAsync(new Pass(entrada.Nombre, entrada.DNI, entrada.idEventos));
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
                        await Navigation.PushModalAsync(
                            new NotPass($"Su horario es a las {qr_dt.ToString("hh:mm:ss")}"));
                    }
                }
            }
            catch (Exception exception)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Error", exception.Message, "Ok");
            }
        }
    }
}