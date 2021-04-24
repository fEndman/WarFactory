using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WarFactory.ViewPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MirageTankSettingPage : ContentPage
    {
        public MirageTankSettingPage()
        {
            InitializeComponent();

            Label1.Text = MirageTankPage.Photo1_K.ToString("F2");
            Label2.Text = MirageTankPage.Photo2_K.ToString("F2");
            Label3.Text = MirageTankPage.PhotoThreshold.ToString("F0");
            Slider1.Value = MirageTankPage.Photo1_K;
            Slider2.Value = MirageTankPage.Photo2_K;
            Slider3.Value = MirageTankPage.PhotoThreshold;
        }

        private void Slider1_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Label1.Text = Slider1.Value.ToString("F2");
            MirageTankPage.Photo1_K = (float)Slider1.Value;
        }

        private void Slider2_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Label2.Text = Slider2.Value.ToString("F2");
            MirageTankPage.Photo2_K = (float)Slider2.Value;
        }

        private void Slider3_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Label3.Text = Slider3.Value.ToString("F0");
            MirageTankPage.PhotoThreshold = (byte)Slider3.Value;
        }
    }
}