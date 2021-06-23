using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using SkiaSharp;
using WarFactory.FactoryFunc;
using WarFactory.MyInterface;

namespace WarFactory.ViewPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MirageTankPage : ContentPage
    {
        private FileResult photoFile1 = null;
        private FileResult photoFile2 = null;

        public MemoryStream photoTankStream = new MemoryStream();

        private static float photo1_K = 1.2f;
        private static float photo2_K = 0.6f;
        private static byte photoThreshold = 110;

        public static float Photo1_K { get {return photo1_K; } set { photo1_K = value; } }
        public static float Photo2_K { get { return photo2_K; } set { photo2_K = value; } }
        public static byte PhotoThreshold { get { return photoThreshold; } set { photoThreshold = value; } }

        public MirageTankPage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.LightGray;
        }

        private async void Image1_Clicked(object sender, EventArgs e)
        {
            photoFile1 = await MediaPicker.PickPhotoAsync();
            if (photoFile1 == null)
                return;
            else
                Image1.Source = photoFile1.FullPath;

            LabelTips1.Text = "";
        }

        private async void Image2_Clicked(object sender, EventArgs e)
        {
            photoFile2 = await MediaPicker.PickPhotoAsync();
            if (photoFile2 == null)
                return;
            else
                Image2.Source = photoFile2.FullPath;

            LabelTips2.Text = "";
        }

        private async void Image5_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ImagePage(photoTankStream));
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            if (Switch1.IsToggled) Image5.BackgroundColor = Color.Black;
            else Image5.BackgroundColor = Color.White;
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            MemoryStream photo1Stream = new MemoryStream();
            MemoryStream photo2Stream = new MemoryStream();
            SKBitmap photo1 = null;
            SKBitmap photo2 = null;
            SKBitmap photoTank = null;

            if (photoFile1 == null ||
                photoFile2 == null ||
                string.IsNullOrEmpty(photoFile1.FullPath) ||
                string.IsNullOrEmpty(photoFile2.FullPath))
            {
                await DisplayAlert("警告", "缺少图片！", "确认");
                return;
            }
            else
            {
                photo1 = SKBitmap.Decode(photoFile1.FullPath);
                photo2 = SKBitmap.Decode(photoFile2.FullPath);
            }

            Image3.Source = null;
            Image4.Source = null;
            Image5.Source = null;

            ActivityIndicator1.IsRunning = true;

            await Task.Run(() =>
            {
                photoTank = MirageTank.Encode(ref photo1, ref photo2, photo1_K, photo2_K, photoThreshold);
            });

            photoTankStream = new MemoryStream();
            photo1.Encode(photo1Stream, SKEncodedImageFormat.Png, 100);
            photo2.Encode(photo2Stream, SKEncodedImageFormat.Png, 100);
            photoTank.Encode(photoTankStream, SKEncodedImageFormat.Png, 100);

            LabelTips3.Text = "";
            Image3.Source = ImageSource.FromStream(() => new MemoryStream(photo1Stream.ToArray()));
            Image4.Source = ImageSource.FromStream(() => new MemoryStream(photo2Stream.ToArray()));
            Image5.Source = ImageSource.FromStream(() => new MemoryStream(photoTankStream.ToArray()));

            ActivityIndicator1.IsRunning = false;
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            if (photoTankStream.Length != 0)
            {
                string path = DependencyService.Get<IPlatformService>().GetSavePath();
                path += await DependencyService.Get<IPlatformService>().ImageSave(photoTankStream);
                await DisplayAlert("完成", "已保存至：\n" + path + "\n可在 战车工厂 相册中找到", "确认");
            }
            else
            {
                await DisplayAlert("警告", "请先生成图片！", "确认");
            }
        }

        private async void Button_Clicked_3(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CourseForMirageTank());
        }


        private async void Button_Clicked_4(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MirageTankSettingPage());
        }
    }
}