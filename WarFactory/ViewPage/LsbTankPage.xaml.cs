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
    public struct sInsideFile
    {
        public sInsideFile(string name, MemoryStream file)
        {
            Name = name;
            File = file;
        }

        public string Name;
        public MemoryStream File;
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LsbTankPage : ContentPage
    {
        byte[] compressArray = { 1, 2, 4 };
        byte compression = 4;

        static private bool compatibleMode;

        private FileResult photoFile1 = null;
        private FileResult photoFile2 = null;
        private List<FileResult> photoFile3 = new List<FileResult>();
        List<sInsideFile> insideFile = new List<sInsideFile>();

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
            if (compatibleMode)
            {
                photoFile2 = await MediaPicker.PickPhotoAsync();
                if (photoFile2 == null)
                    return;
                else
                    Image2.Source = photoFile2.FullPath;
            }
            else
            {
                photoFile2 = await FilePicker.PickAsync();  //可选取其他文件作为里图
                if (photoFile2 == null)
                    return;
                else
                {
                    string extension = photoFile2.FileName.Substring(photoFile2.FileName.LastIndexOf(".") + 1).ToLower();

                    if (extension == "png" || extension == "jpg" || extension == "jpeg" || extension == "bmp" || extension == "gif")
                        Image2.Source = photoFile2.FullPath;
                    else
                        Image2.Source = ImageSource.FromResource("WarFactory.Resources.File.png");
                }
            }
        }

        private async void Image3_Clicked(object sender, EventArgs e)
        {
            if (compatibleMode)
            {
                photoFile3.Clear();
                FileResult file = await MediaPicker.PickPhotoAsync();
                if (file == null)
                    return;
                else
                {
                    photoFile3.Add(file);
                    Image3.Source = photoFile3[0].FullPath;
                }
            }
            else
            {
                photoFile3.Clear();
                IEnumerable<FileResult> files = await FilePicker.PickMultipleAsync(PickOptions.Images);
                if (files == null) return;

                foreach (FileResult file in files)
                    photoFile3.Add(file);
                if (photoFile3.Count == 1)
                    Image3.Source = photoFile3[0].FullPath;
                else
                    Image3.Source = ImageSource.FromResource("WarFactory.Resources.Images.png");
            }
        }

        private async void Image4_Clicked(object sender, EventArgs e)
        {
            if (insideFile.Count == 1)
            {
                string extension = (insideFile[0].Name == null) ? "" : insideFile[0].Name.Substring(insideFile[0].Name.LastIndexOf(".") + 1).ToLower();
                if (extension == "png" || extension == "jpg" || extension == "jpeg" || extension == "bmp")
                    await Navigation.PushAsync(new ImagePage(insideFile[0].File));
                else
                {
                    string fileName = Path.Combine(DependencyService.Get<IPlatformService>().GetAbsoluteSavePath(), insideFile[0].Name);
                    if (File.Exists(fileName) == false)
                        await DisplayAlert("警告", "这个文件需要先保存才能打开！", "确认");
                    else
                        await Launcher.OpenAsync(new OpenFileRequest { File = new ReadOnlyFile(fileName) });
                }
            }
            else if (insideFile.Count > 1)
            {
                await Navigation.PushAsync(new ImageListPage(insideFile));
            }
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

            foreach (sInsideFile insFile in insideFile)
                insFile.File.Dispose();
            insideFile.Clear();
            LabelFileName.Text = "";
            Image4.Source = null;

            ActivityIndicator1.IsRunning = true;
            Button1.IsEnabled = false;
            Button2.IsEnabled = false;

            MemoryStream photoTank = new MemoryStream();
            await Task.Run(() =>
            {
                photoTank = LsbTank.Encode(photo1Stream, photo2Stream, compression);
            });

            insideFile.Add(new sInsideFile(null, photoTank));
            Image4.Source = ImageSource.FromStream(() => new MemoryStream(photoTank.ToArray()));

            Button1.IsEnabled = true;
            Button2.IsEnabled = true;
            ActivityIndicator1.IsRunning = false;

            photo1Stream.Close();
            photo2Stream.Close();
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            if (photoFile3.Count == 0)
            {
                await DisplayAlert("警告", "请选择要现形的图片！", "确认");
                return;
            }

            List<FileStream> photo3Stream = new List<FileStream>();
            foreach (FileResult file in photoFile3)
                photo3Stream.Add(new FileStream(file.FullPath, FileMode.Open));

            foreach (sInsideFile insFile in insideFile)
                insFile.File.Dispose();
            insideFile.Clear();
            Image4.Source = null;

            ActivityIndicator1.IsRunning = true;
            Button1.IsEnabled = false;
            Button2.IsEnabled = false;

            await Task.Run(() =>
            {
                foreach (FileStream fs in photo3Stream)
                {
                    string tempName = null;
                    MemoryStream tempStream = LsbTank.Decode(fs, out tempName);
                    if (tempStream != null)
                    {
                        insideFile.Add(new sInsideFile(tempName, tempStream));
                        tempStream.Dispose();
                    }
                }
            });

            if (insideFile.Count == 0)
            {
                await DisplayAlert("警告", "这些图不是无影坦克！", "确认");
                LabelFileName.Text = "";
            }
            else if(insideFile.Count == 1)
            {
                string extension = insideFile[0].Name.Substring(insideFile[0].Name.LastIndexOf(".") + 1).ToLower();
                if (extension == "png" || extension == "jpg" || extension == "jpeg" || extension == "bmp" || extension == "gif")
                    Image4.Source = ImageSource.FromStream(() => new MemoryStream(insideFile[0].File.ToArray()));
                else
                    Image4.Source = ImageSource.FromResource("WarFactory.Resources.File.png");
                LabelFileName.Text = insideFile[0].Name;
            }
            else
            {
                Image4.Source = ImageSource.FromResource("WarFactory.Resources.Images.png");
                LabelFileName.Text = "成功现形" + insideFile.Count.ToString() + "个文件，点击进入查看列表";
            }

            Button1.IsEnabled = true;
            Button2.IsEnabled = true;
            ActivityIndicator1.IsRunning = false;

            foreach (FileStream ms in photo3Stream)
                ms.Close();
        }

        private async void Button_Clicked_3(object sender, EventArgs e)
        {
            if (insideFile.Count == 0)
            {
                await DisplayAlert("警告", "请先生成图片！", "确认");
            }
            else
            {
                string tempStr = 
                    "已保存" + insideFile.Count.ToString() + "个文件至：\n" +
                    DependencyService.Get<IPlatformService>().GetSavePath() +
                    "\n可在 战车工厂 相册中找到\n文件名如下：";

                foreach (sInsideFile insFile in insideFile)
                {
                    string name = await DependencyService.Get<IPlatformService>().ImageSave(insFile.File, insFile.Name);
                    tempStr += "\n" + name;
                }

                await DisplayAlert("完成", tempStr, "确认");
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

        private void Switch1_Toggled(object sender, ToggledEventArgs e)
        {
            compatibleMode = Switch1.IsToggled;
        }
    }
}