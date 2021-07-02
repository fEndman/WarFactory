using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WarFactory.ViewPage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LsbTankSettingPage : ContentPage
	{
		public LsbTankSettingPage(int compression, string info, bool compatibleMode)
		{
			InitializeComponent();
			this.BackgroundColor = Color.AliceBlue;

			Label1.Text = "压缩度：" + compression.ToString();
			Stepper1.Value = compression;
			Entry1.Text = info;
			Switch1.IsToggled = compatibleMode;
		}

		private void Stepper1_ValueChanged(object sender, ValueChangedEventArgs e)
        {
			Label1.Text = "压缩度：" + ((int)Stepper1.Value).ToString();
		}

		private async void Switch1_Toggled(object sender, ToggledEventArgs e)
		{
			if (DeviceInfo.Platform == DevicePlatform.iOS && Switch1.IsToggled == false)
				await DisplayAlert("警告", "iOS的选择文件控件以及多选控件有问题！请保持兼容模式开启！", "确认");
		}

		override protected void OnDisappearing()
		{
			LsbTankPage.Compression = (int)Stepper1.Value;
			LsbTankPage.Info = Entry1.Text;
			LsbTankPage.CompatibleMode = Switch1.IsToggled;
		}
	}
}