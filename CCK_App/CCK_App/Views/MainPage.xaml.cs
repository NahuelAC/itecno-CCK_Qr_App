using System;
using CCK_App.Models;
using CCK_QR_App;
using Plugin.DeviceInfo;
using Xamarin.Forms;
using static CCK_QR_App.ApiClient;

namespace CCK_App.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            deviceNumber.Text = CrossDeviceInfo.Current.Id;
        }

        private async void BtnScannerQR_OnClicked(object sender, EventArgs e)
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
                    string qr_bloque = qr_data[0];
                    string qr_dni = qr_data[1];
                    DateTime qr_dt = Convert.ToDateTime(qr_data[2]);
                    
                    var data = await ApiClient.ApiGetTicketsByDni(qr_dni);

                    if (dt_actual.Hour > qr_dt.Hour - 1 && dt_actual.Hour < qr_dt.Hour + 1)
                    {
                        Entradas entrada = null;
                        foreach (var d in data)
                        {
                            if (d.idEventos == Convert.ToDecimal(qr_bloque))
                            {
                                entrada = d;
                            }
                        }

                        if (entrada != null && entrada.Show == null)
                        {
                            
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private async void BtnScanDNI_OnClicked(object sender, EventArgs e)
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
                    string qr_bloque = qr_data[0];
                    string qr_dni = qr_data[1];
                    DateTime qr_dt = Convert.ToDateTime(qr_data[2]);
                    
                    var data = await ApiClient.ApiGetTicketsByDni(qr_dni);

                    if (dt_actual.Hour > qr_dt.Hour - 1 && dt_actual.Hour < qr_dt.Hour + 1)
                    {
                        Entradas entrada = null;
                        foreach (var d in data)
                        {
                            if (d.idEventos == Convert.ToDecimal(qr_bloque))
                            {
                                entrada = d;
                            }
                        }

                        if (entrada != null && entrada.Preshow == null)
                        {
                            await ApiClient.ApiPutTicketIngresado(qr_dni, qr_bloque, dt_actual)
                        }
                        else
                        {
                            await Navigation.PushModalAsync(new NotPass($"Qr escaneado a las {entrada.Preshow.TimeOfDay}"));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}