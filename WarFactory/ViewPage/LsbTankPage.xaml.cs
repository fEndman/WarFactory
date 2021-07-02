using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using WarFactory.FactoryFunc;
using WarFactory.MyInterface;
using MimeMapping;

namespace WarFactory.ViewPage
{
    public struct SInsideFile
    {
        public SInsideFile(string name, MemoryStream file)
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
        static private int compression = 4;
        static private bool compatibleMode;
        static private string info = "TK";
        static private bool captureMode = false;
        static private bool isBackstage = false;
        static private DateTime lastOpenedTime = DateTime.MaxValue;

        static public int Compression { get { return compression; } set { compression = value; } }
        static public bool CompatibleMode { get { return compatibleMode; } set { compatibleMode = value; } }
        static public string Info { get { return info; } set { info = value; } }
        static public bool CaptureMode { get { return captureMode; } set { captureMode = value; } }
        static public bool IsBackstage { get { return isBackstage; } set { isBackstage = value; } } //在APP.xaml.cs切后台事件里操作
        static public DateTime LastOpenedTime { get { return lastOpenedTime; } set { lastOpenedTime = value; } }

        private List<FileResult> photoFile1 = new List<FileResult>();
        private List<FileResult> photoFile2 = new List<FileResult>();
        private List<FileResult> photoFile3 = new List<FileResult>();
        List<SInsideFile> insideFile = new List<SInsideFile>();

        public LsbTankPage()
        {
            InitializeComponent();
            BackgroundColor = Color.AliceBlue;

            compression = 4;
            compatibleMode = false;
            info = "TK";
            captureMode = false;
            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                LabelTips1.Text = "点击";
                LabelTips2.Text = "点击";
                LabelTips3.Text = "点击(原图保存)";
                compatibleMode = true;
            }
        }

        private async void Image1_Clicked(object sender, EventArgs e)
        {
            if (compatibleMode)
            {
                photoFile1.Clear();
                FileResult file = await MediaPicker.PickPhotoAsync();
                if (file == null)
                {
                    Image1.Source = null;
                    return;
                }
                else
                {
                    photoFile1.Add(file);
                    Image1.Source = photoFile1[0].FullPath;
                }
            }
            else
            {
                photoFile1.Clear();
                IEnumerable<FileResult> files = await FilePicker.PickMultipleAsync(PickOptions.Images);
                if (files == null)
                {
                    Image1.Source = null;
                    return;
                }

                foreach (FileResult file in files)
                    photoFile1.Add(file);
                photoFile1.Reverse();   //选取器多选是先选的排在最后，我们要的是先选在前，所以颠倒排序一下
                if (photoFile1.Count == 1)
                    Image1.Source = photoFile1[0].FullPath;
                else
                    Image1.Source = ImageSource.FromResource("WarFactory.Resources.Images.png");
            }

            LabelTips1.Text = "";
        }

        private async void Image2_Clicked(object sender, EventArgs e)
        {
            if (compatibleMode)
            {
                photoFile2.Clear();
                FileResult file = await MediaPicker.PickPhotoAsync();
                if (file == null)
                {
                    Image2.Source = null;
                    return;
                }
                else
                {
                    photoFile2.Add(file);
                    Image2.Source = photoFile2[0].FullPath;
                }
            }
            else
            {
                photoFile2.Clear();
                IEnumerable<FileResult> files = await FilePicker.PickMultipleAsync();   //可选取其他文件作为里图
                if (files == null)
                {
                    Image2.Source = null;
                    return;
                }

                foreach (FileResult file in files)
                    photoFile2.Add(file);
                photoFile2.Reverse();   //选取器多选是先选的排在最后，我们要的是先选在前，所以颠倒排序一下
                if (photoFile2.Count == 1)
                {
                    string extension = photoFile2[0].FileName.Substring(photoFile2[0].FileName.LastIndexOf(".") + 1).ToLower();
                    if (extension == "png" || extension == "jpg" || extension == "jpeg" || extension == "bmp" || extension == "gif")
                        Image2.Source = photoFile2[0].FullPath;
                    else
                        Image2.Source = ImageSource.FromResource("WarFactory.Resources.File.png");
                }
                else
                    Image2.Source = ImageSource.FromResource("WarFactory.Resources.Images.png");
            }

            LabelTips2.Text = "";
        }

        private async void Image3_Clicked(object sender, EventArgs e)
        {
            if (compatibleMode)
            {
                photoFile3.Clear();
                FileResult file = await MediaPicker.PickPhotoAsync();
                if (file == null)
                {
                    Image3.Source = null;
                    return;
                }
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
                if (files == null)
                {
                    Image3.Source = null;
                    return;
                }

                foreach (FileResult file in files)
                    photoFile3.Add(file);
                photoFile3.Reverse();   //选取器多选是先选的排在最后，我们要的是先选在前，所以颠倒排序一下
                if (photoFile3.Count == 1)
                    Image3.Source = photoFile3[0].FullPath;
                else
                    Image3.Source = ImageSource.FromResource("WarFactory.Resources.Images.png");
            }

            LabelTips3.Text = "";
        }

        private async void Image4_Clicked(object sender, EventArgs e)
        {
            if (insideFile.Count == 1)
            {
                string extension = (insideFile[0].Name == null) ? "png" : insideFile[0].Name.Substring(insideFile[0].Name.LastIndexOf(".") + 1).ToLower();
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
                photoFile1.Count == 0 ||
                photoFile2.Count == 0)
            {
                await DisplayAlert("警告", "缺少图片！", "确认");
                return;
            }
            foreach (FileResult pf1 in photoFile1)
            {
                foreach (FileResult pf2 in photoFile2)
                {
                    if (pf1.FullPath == pf2.FullPath)
                    {
                        await DisplayAlert("警告", "里图不能与表图重复！", "确认");
                        return;
                    }
                }
            }

            foreach (SInsideFile insFile in insideFile)
                insFile.File.Dispose();
            insideFile.Clear();
            LabelFileName.Text = "";
            Image4.Source = null;

            ActivityIndicator1.IsRunning = true;
            Button1.IsEnabled = false;
            Button2.IsEnabled = false;

            if(compression >= 6)
                await DisplayAlert("警告", "支持压缩度大于等于6的无影坦克的网站很少，很有可能只有本APP能够现形，并且十分不稳定，请酌情考虑是否保存！", "确认");

            string fileName = "Tank_" + DateTime.Now.ToLocalTime().ToString("yyyyMMdd_HHmmss");
            int photoIndex = 0;
            if ((string.IsNullOrEmpty(info) || string.IsNullOrWhiteSpace(info)) && photoFile2.Count == 1)
                info = "TK";
            await Task.Run(() =>
            {
                foreach (FileResult fr in photoFile2)
                {
                    photoIndex++;
                    MemoryStream tempStream = LsbTank.Encode(new FileStream(photoFile1[photoIndex % photoFile1.Count].FullPath, FileMode.Open),
                                                             new FileStream(fr.FullPath, FileMode.Open),
                                                             info + (photoFile2.Count == 1 ? "" : photoIndex.ToString()),
                                                             compression);
                    if (tempStream != null)
                    {
                        insideFile.Add(new SInsideFile(fileName + "_" + photoIndex.ToString() + ".png", tempStream));
                        tempStream.Dispose();
                    }
                }
            });

            LabelTips4.Text = "";
            if (insideFile.Count == 1)
            {
                Image4.Source = ImageSource.FromStream(() => new MemoryStream(insideFile[0].File.ToArray()));
                LabelFileName.Text = "成功生成！发送时记得原图发送并且关闭水印！";
            }
            else
            {
                Image4.Source = ImageSource.FromResource("WarFactory.Resources.Images.png");
                LabelFileName.Text = "成功生成" + insideFile.Count.ToString() + "个文件，点击进入查看列表";
            }

            if(DeviceInfo.Platform == DevicePlatform.iOS)
                await DisplayAlert("警告", "iOS设备似乎即使发送原图也会被压缩，这也就注定了iOS发出去的坦克图全是锤子，因此请谨慎发图！", "确认");

            Button1.IsEnabled = true;
            Button2.IsEnabled = true;
            ActivityIndicator1.IsRunning = false;
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            if (photoFile3.Count == 0)
            {
                await DisplayAlert("警告", "请选择要现形的图片！", "确认");
                return;
            }

            List<FileStream> photo3Stream = new List<FileStream>();
            try
            {
                foreach (FileResult file in photoFile3)
                    photo3Stream.Add(new FileStream(file.FullPath, FileMode.Open));
            }
            catch (UnauthorizedAccessException)
            {
                await DisplayAlert("警告", "权限异常！", "确认");
                photo3Stream.Clear();
                return;
            }
            catch (FileNotFoundException)
            {
                await DisplayAlert("警告", "文件不存在！", "确认");
                photo3Stream.Clear();
                return;
            }

            foreach (SInsideFile insFile in insideFile)
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
                    MemoryStream tempStream = LsbTank.Decode(fs, out string tempName);
                    if (tempStream != null)
                    {
                        insideFile.Add(new SInsideFile(tempName, tempStream));
                        tempStream.Dispose();
                    }
                }
            });

            LabelTips4.Text = "";
            if (insideFile.Count == 0)
            {
                await DisplayAlert("警告", "这些图不是无影坦克！", "确认");
                if(MimeUtility.GetMimeMapping(photoFile3[0].FileName) != MimeUtility.GetMimeMapping(".png"))
                    await DisplayAlert("警告", "如果确定这不是一张锤子图，请 *查看原图* 后 保存 再现形！", "确认");
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

                foreach (SInsideFile insFile in insideFile)
                {
                    string name = await DependencyService.Get<IPlatformService>().ImageSave(insFile.File, compatibleMode, insFile.Name);
                    tempStr += "\n" + name;
                }

                await DisplayAlert("完成", tempStr, "确认");
            }
        }

        private async void Button_Clicked_4(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CourseForLsbTank());
        }

        private async void Button_Clicked_5(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LsbTankSettingPage(compression, info, compatibleMode));
        }

        private async void Button_Clicked_6(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LsbTankAdvancedFunctionPage(this, captureMode));
        }

        public void RunScanner()
        {
            Task.Run(() =>
            {
                while (captureMode)
                {
                    DateTime utc = new DateTime(1970, 1, 1, 0, 0, 0, 0);    //UNIX时间戳
                    long timestamp = Convert.ToInt64((DateTime.UtcNow - utc).TotalMilliseconds);
                    while (!isBackstage && captureMode) ;   //等待进入后台
                    while (isBackstage && captureMode) ;    //等待切回应用
                    string[] newPics = DependencyService.Get<IPlatformService>().GetLatestPictures(timestamp);  //获取这期间新增的图片
                    foreach (string newPic in newPics)
                        photoFile3.Add(new FileResult(newPic));
                    //在主线程上刷新UI
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplayAlert("完成", "这次在后台捕获到了" + newPics.Length.ToString() + "张图片！您可以继续下载图片或手动关闭捕获模式！", "确认");
                        if (photoFile3.Count == 1)
                            Image3.Source = photoFile3[0].FullPath;
                        else if (photoFile3.Count > 1)
                            Image3.Source = ImageSource.FromResource("WarFactory.Resources.Images.png");
                        if (photoFile3.Count != 0)
                            LabelTips3.Text = "";
                    });
                }
            });
        }

        public void AddTankPicture(FileResult fileResult)
        {
            foreach (FileResult file in photoFile3)
                if (fileResult.FullPath == file.FullPath)
                    return;
            photoFile3.Add(fileResult);

            if (photoFile3.Count == 1)
                Image3.Source = photoFile3[0].FullPath;
            else if(photoFile3.Count > 1)
                Image3.Source = ImageSource.FromResource("WarFactory.Resources.Images.png");
            if (photoFile3.Count != 0)
                LabelTips3.Text = "";
        }
    }
}
