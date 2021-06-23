using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WarFactory.ViewPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VersionPage : ContentPage
    {
        public VersionPage()
        {
            InitializeComponent();

            StreamReader textReader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("WarFactory.Resources.Version.txt"));
            Label.Text = textReader.ReadToEnd(); ;
        }
    }
}