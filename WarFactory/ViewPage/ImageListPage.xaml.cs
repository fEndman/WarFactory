using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WarFactory.ViewPage;
using System.Collections.ObjectModel;
using WarFactory.MyInterface;
using Xamarin.Essentials;

namespace WarFactory.ViewPage
{
    public class ImageObj
    {
        public ImageObj(ImageSource imgSource, MemoryStream file, string name)
        {
            ImgSource = imgSource;
            File = file;
            Name = name;
        }

        public ImageSource ImgSource { get; set; }
        public MemoryStream File { get; set; }
        public string Name { get; set; }
    }


    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageListPage : ContentPage
    {

        List<ImageObj> imageObjs = new List<ImageObj>();

        public ImageListPage(List<SInsideFile> insideFile)
        {
            InitializeComponent();

            foreach (SInsideFile insFile in insideFile)
            {
                ImageSource tempSource = null;

                string extension = insFile.Name.Substring(insFile.Name.LastIndexOf(".") + 1).ToLower();
                if (extension == "png" || extension == "jpg" || extension == "jpeg" || extension == "bmp" || extension == "gif")
                    tempSource = ImageSource.FromStream(() => new MemoryStream(insFile.File.ToArray()));
                else
                    tempSource = ImageSource.FromResource("WarFactory.Resources.File.png");

                imageObjs.Add(new ImageObj(tempSource, insFile.File, insFile.Name));
            }

            imageView.ItemsSource = imageObjs;

            imageView.ItemTapped += eItemTapped;
        }

        private async void eItemTapped(object sender, ItemTappedEventArgs e)
        {

            ImageObj img = e.Item as ImageObj;

            string extension = img.Name.Substring(img.Name.LastIndexOf(".") + 1).ToLower();
            if (extension == "png" || extension == "jpg" || extension == "jpeg" || extension == "bmp")
                await Navigation.PushAsync(new ImagePage(img.File));
            else
            {
                string fileName = Path.Combine(DependencyService.Get<IPlatformService>().GetAbsoluteSavePath(), img.Name);
                if (File.Exists(fileName) == false)
                    await DisplayAlert("警告", "这个文件需要先保存才能打开！", "确认");
                else
                    await Launcher.OpenAsync(new OpenFileRequest { File = new ReadOnlyFile(fileName) });
            }
        }
    }
}

