using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace WarFactory.ViewPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            Button1.ImageSource = ImageSource.FromResource("WarFactory.Resources.GitHub.png");
            Button2.ImageSource = ImageSource.FromResource("WarFactory.Resources.Tieba.png");
            Button3.ImageSource = ImageSource.FromResource("WarFactory.Resources.WeiYun.png");
            Button4.ImageSource = ImageSource.FromResource("WarFactory.Resources.BaiduNetDisk.png");
        }
        private async void Button1_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://github.com/fEndman/WarFactory");
        }

        private async void Button2_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://tieba.baidu.com/p/7308042884");
        }

        private async void Button3_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://share.weiyun.com/yvkPZ89G");
        }

        private async void Button4_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://pan.baidu.com/s/1B9Aw7J_cKA6W5GwNodRzPA");
        }

        private async void Button5_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PayPage());
        }
    }
}