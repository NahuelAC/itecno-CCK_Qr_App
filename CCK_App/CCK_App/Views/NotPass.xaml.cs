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

        private async void CloseBtn_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}