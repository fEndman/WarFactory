using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarFactory.MyInterface;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WarFactory.ViewPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LsbTankAdvancedFunctionPage : ContentPage
	{
        LsbTankPage page = null;

        public LsbTankAdvancedFunctionPage(LsbTankPage myPage, bool isCaptureMode)
		{
			InitializeComponent();
            BackgroundColor = Color.AliceBlue;

            Switch1.IsToggled = isCaptureMode;
            page = myPage;
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            string[] fileNames = DependencyService.Get<IPlatformService>().GetLatestPictures(Convert.ToInt64((LsbTankPage.LastOpenedTime - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds));
            foreach (string fileName in fileNames)
                page.AddTankPicture(new FileResult(fileName));
            DisplayAlert("完成", "已添加自上次打开应用(" + LsbTankPage.LastOpenedTime.ToLocalTime().ToString("yyyy.MM.dd HH:mm:ss") + ")以来新增的" + fileNames.Length.ToString() + "张图片到要现形的队列！", "确认");
            Navigation.PopAsync();
        }

        private void Switch1_Toggled(object sender, ToggledEventArgs e)
        {
            LsbTankPage.CaptureMode = Switch1.IsToggled;
            if (Switch1.IsToggled && page != null)  //page用来判断是否是在初始化
            {
                DisplayAlert("完成", "您现在可以切到后台下载图片，过程中新增的图都会被添加进要现形的队列！", "确认");
                page.RunScanner();
                Navigation.PopAsync();
            }
        }
    }
}
