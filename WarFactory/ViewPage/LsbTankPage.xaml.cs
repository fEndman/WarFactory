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
    public partial class LsbTankPage : ContentPage
    {
        byte[] compressArray = { 1, 2, 4 };
        byte compression = 4;

        string insideFileName = null;
        private FileResult photoFile1 = null;
        private FileResult photoFile2 = null;
        private FileResult photoFile3 = null;
        public MemoryStream photoTankStream = new MemoryStream();

        public LsbTankPage()
        {
            InitializeComponent();
            this.BackgroundColor = Color.AliceBlue;
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
            //photoFile2 = await MediaPicker.PickPhotoAsync();
            photoFile2 = await FilePicker.PickAsync();  //可选取其他文件作为里图
            if (photoFile2 == null)
                return;
            else
                Image2.Source = photoFile2.FullPath;
        }

        private async void Image3_Clicked(object sender, EventArgs e)
        {
            photoFile3 = await MediaPicker.PickPhotoAsync();
            if (photoFile3 == null)
                return;
            else
                Image3.Source = photoFile3.FullPath;
        }

        private async void Image4_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ImagePage(photoTankStream));
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            if (photoFile1 == null ||
                photoFile2 == null ||
                string.IsNullOrEmpty(photoFile1.FullPath) ||
                string.IsNullOrEmpty(photoFile2.FullPath))
            {
                await DisplayAlert("警告", "缺少图片！", "确认");
                return;
            }
            FileStream photo1Stream = new FileStream(photoFile1.FullPath, FileMode.Open);
            FileStream photo2Stream = new FileStream(photoFile2.FullPath, FileMode.Open);

            Image4.Source = null;

            insideFileName = null;

            ActivityIndicator1.IsRunning = true;
            Button1.IsEnabled = false;

            byte[] photoTank = null;
            await Task.Run(() =>
            {
                photoTank = LsbTank.Encode(photo1Stream, photo2Stream, compression);
            });

            photoTankStream = new MemoryStream(photoTank);
            Image4.Source = ImageSource.FromStream(() => new MemoryStream(photoTank));

            Button1.IsEnabled = true;
            ActivityIndicator1.IsRunning = false;

            photo1Stream.Close();
            photo2Stream.Close();
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            if (photoFile3 == null ||
                string.IsNullOrEmpty(photoFile3.FullPath))
            {
                await DisplayAlert("警告", "请选择要现形的图片！", "确认");
                return;
            }

            FileStream photo3Stream = new FileStream(photoFile3.FullPath, FileMode.Open);

            Image4.Source = null;

            ActivityIndicator1.IsRunning = true;

            byte[] insidePic = null;
            await Task.Run(() =>
            {
                insidePic = LsbTank.Decode(photo3Stream, out insideFileName);
            });

            if (insidePic == null)
            {
                await DisplayAlert("警告", "这幅图不是无影坦克！", "确认");
                photo3Stream.Close();
                ActivityIndicator1.IsRunning = false;
                return;
            }

            photoTankStream = new MemoryStream(insidePic);
            Image4.Source = ImageSource.FromStream(() => new MemoryStream(insidePic));

            ActivityIndicator1.IsRunning = false;

            photo3Stream.Close();
        }

        private async void Button_Clicked_3(object sender, EventArgs e)
        {
            if (photoTankStream.Length != 0)
            {
                string path = await DependencyService.Get<ISaveFileService>().ImageSave(photoTankStream, insideFileName);
                await DisplayAlert("完成", "已保存至：\n" + path + "\n可在WarFactory相册中找到", "确认");
            }
            else
            {
                await DisplayAlert("警告", "请先生成图片！", "确认");
            }
        }

        private async void Button_Clicked_4(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CourseForLsbTank());
        }

        private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            compression = compressArray[(int)Stepper1.Value];
            Label1.Text = compression.ToString();
        }
    }
}