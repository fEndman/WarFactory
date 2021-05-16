using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using WarFactory.MyInterface;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;
using File = Java.IO.File;
using Android.Content;

[assembly: Dependency(typeof(WarFactory.Droid.PlatformService))]
namespace WarFactory.Droid
{
    public class PlatformService : IPlatformService
    {
        private string folder = "战车工厂";

        public async Task<string> ImageSave(MemoryStream stream, string fileName = null)
        {
            await Permissions.RequestAsync<Permissions.StorageWrite>();
            if (Permissions.ShouldShowRationale<Permissions.StorageWrite>()) return "...保存个屁！不给爷权限还想让爷给你造坦克？";
            await Permissions.RequestAsync<Permissions.StorageRead>();
            if (Permissions.ShouldShowRationale<Permissions.StorageRead>()) return "...保存个屁！不给爷权限还想让爷给你造坦克？";

            string path = Path.Combine(Environment.ExternalStorageDirectory.AbsolutePath, Environment.DirectoryPictures, folder);
            if (fileName == null || fileName == "") fileName = "Tank_" + DateTime.Now.ToLocalTime().ToString("yyyyMMdd_HHmmss") + ".png";

            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);

            path = Path.Combine(path, fileName);
            FileStream photoTankFile = new FileStream(path, FileMode.Create);
            byte[] photoTank = stream.ToArray();

            photoTankFile.Write(photoTank, 0, photoTank.Length);
            photoTankFile.Flush();
            photoTankFile.Close();

            Intent intent = new Intent(Intent.ActionMediaScannerScanFile);
            Uri uri = Uri.FromFile(new File(path));
            intent.SetData(uri);
            Platform.AppContext.SendBroadcast(intent);

            return Path.Combine(fileName);
        }

        public async Task RequestPermissions()
        {
            await Permissions.RequestAsync<Permissions.StorageWrite>();
            if (Permissions.ShouldShowRationale<Permissions.StorageWrite>()) return;
            await Permissions.RequestAsync<Permissions.StorageRead>();
            if (Permissions.ShouldShowRationale<Permissions.StorageRead>()) return;
        }
        public string GetAbsoluteSavePath()
        {
            return Path.Combine(Environment.ExternalStorageDirectory.AbsolutePath, Environment.DirectoryPictures, folder);
        }
        public string GetSavePath()
        {
            return Path.Combine(Environment.DirectoryPictures, folder);
        }
    }
}