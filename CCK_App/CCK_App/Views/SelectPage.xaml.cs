using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.DeviceInfo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CCK_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectPage : ContentPage
    {
        public SelectPage()
        {
            InitializeComponent();

            deviceNumber.Text = CrossDeviceInfo.Current.Id;
        }

        private async void BtnShow_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ShowPage());
        }

        private async void BtnPreshow_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PreshowPage());
        }
    }
}