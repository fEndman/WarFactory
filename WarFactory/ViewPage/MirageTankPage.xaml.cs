using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private float photo1_K = 1.2f;
        private float photo2_K = 0.6f;
        private byte photoThreshold = 110;

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
        }

        private async void Image2_Clicked(object sender, EventArgs e)
        {
            photoFile2 = await MediaPicker.PickPhotoAsync();
            if (photoFile2 == null)
                return;
            else
                Image2.Source = photoFile2.FullPath;
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

            Image3.Source = ImageSource.FromStream(() => new MemoryStream(photo1Stream.ToArray()));
            Image4.Source = ImageSource.FromStream(() => new MemoryStream(photo2Stream.ToArray()));
            Image5.Source = ImageSource.FromStream(() => new MemoryStream(photoTankStream.ToArray()));

            ActivityIndicator1.IsRunning = false;
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            if (photoTankStream.Length != 0)
            {
                string path = await DependencyService.Get<ISaveFileService>().ImageSave(photoTankStream);
                await DisplayAlert("完成", "已保存至：\n" + path + "\n可在WarFactory相册中找到", "确认");
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

        private void Entry1_Completed(object sender, EventArgs e)
        {
            float.TryParse(Entry1.Text, out photo1_K);
            if (photo1_K > 10f) photo1_K = 10f;
            if (photo1_K < 0.6f) photo1_K = 0.6f;
            Entry1.Text = photo1_K.ToString("F2");
        }

        private void Entry2_Completed(object sender, EventArgs e)
        {
            float.TryParse(Entry2.Text, out photo2_K);
            if (photo2_K > 1.4f) photo2_K = 1.4f;
            if (photo2_K < 0.01f) photo2_K = 0.01f;
            Entry2.Text = photo2_K.ToString("F2");
        }

        private void Entry3_Completed(object sender, EventArgs e)
        {
            byte.TryParse(Entry3.Text, out photoThreshold);
            if (photoThreshold > 200) photoThreshold = 200;
            if (photoThreshold < 50) photoThreshold = 50;
            Entry3.Text = photoThreshold.ToString("D3");
        }
    }
}