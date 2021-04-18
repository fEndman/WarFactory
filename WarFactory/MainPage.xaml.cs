using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using WarFactory.ViewPage;

namespace WarFactory
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            MirageTankButton.Source = ImageSource.FromResource("WarFactory.Resources.MirageTank.png");
            LsbTankButton.Source = ImageSource.FromResource("WarFactory.Resources.LsbTank.png");
        }

        private async void MirageTankButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MirageTankPage());
        }

        private async void LsbTankButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LsbTankPage());
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage());
        }
    }
}
