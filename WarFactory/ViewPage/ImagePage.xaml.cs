using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WarFactory.ViewPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImagePage : ContentPage
    {
        public ImagePage(MemoryStream photoStream)
        {
            InitializeComponent();
            Image1.Source = ImageSource.FromStream(() => new MemoryStream(photoStream.ToArray()));
        }

        private void Switch1_Toggled(object sender, ToggledEventArgs e)
        {
            if (Switch1.IsToggled)
            {
                Image1.BackgroundColor = Color.Black;
                this.BackgroundColor = Color.Black;
            }
            else
            {
                Image1.BackgroundColor = Color.White;
                this.BackgroundColor = Color.White;
            }
        }
    }
}