using System;
using Xamarin.Forms;
using WarFactory.MyInterface;
using WarFactory.ViewPage;
using System.IO;
using Xamarin.Essentials;

namespace WarFactory
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //获取上次打开应用的时间戳
            string fileName = Path.Combine(FileSystem.AppDataDirectory, "LastOpenedTimeStamp.txt");
            if (File.Exists(fileName))
            {
                DateTime LastOpenedTime = DateTime.Parse(File.ReadAllText(fileName));
                File.WriteAllText(fileName, DateTime.UtcNow.ToString());
                LsbTankPage.LastOpenedTime = LastOpenedTime;
            }
            else
            {
                File.WriteAllText(fileName, DateTime.UtcNow.ToString());
                LsbTankPage.LastOpenedTime = DateTime.UtcNow;
            }

            MainPage = new NavigationPage(new MainPage());
        }

        protected override async void OnStart()
        {
            await DependencyService.Get<IPlatformService>().RequestPermissions(); //申请权限
        }

        protected override void OnSleep()
        {
            LsbTankPage.IsBackstage = true;
        }

        protected override void OnResume()
        {
            LsbTankPage.IsBackstage = false;
        }
    }
}
