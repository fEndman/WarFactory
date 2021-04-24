using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WarFactory.MyInterface;

namespace WarFactory
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override async void OnStart()
        {
            await DependencyService.Get<IPlatformService>().RequestPermissions(); //申请权限
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
