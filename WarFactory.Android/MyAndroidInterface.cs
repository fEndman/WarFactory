using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using WarFactory.MyInterface;
using Environment = Android.OS.Environment;
using Android.Media;

[assembly: Dependency(typeof(WarFactory.Droid.PlatformService))]
namespace WarFactory.Droid
{
    public class PlatformService : IPlatformService
    {
        private readonly string albumName = "战车工厂";

        public async Task<string> ImageSave(MemoryStream stream, bool compatibleMode, string fileName = null)
        {
            await Permissions.RequestAsync<Permissions.StorageWrite>();
            if (Permissions.ShouldShowRationale<Permissions.StorageWrite>()) return "...保存个屁！不给爷权限还想让爷造坦克？";
            await Permissions.RequestAsync<Permissions.StorageRead>();
            if (Permissions.ShouldShowRationale<Permissions.StorageRead>()) return "...保存个屁！不给爷权限还想让爷造坦克？";

            //用于替代Environment.ExternalStorageDirectory
            //GetExternalFilesDir()会获取到 /storage/emulated/0/Android/data/应用包名/files
            string externalRootPath;
            if (compatibleMode)
            {
                externalRootPath = "/storage/emulated/0/";
            }
            else
            {
                DirectoryInfo externalStorageDir = new DirectoryInfo(Platform.AppContext.GetExternalFilesDir(null).AbsolutePath);
                externalRootPath = externalStorageDir.Parent.Parent.Parent.Parent.FullName;
            }
            string path = Path.Combine(externalRootPath, Environment.DirectoryPictures, albumName);
            if (fileName == null || fileName == "") fileName = "Tank_" + DateTime.Now.ToLocalTime().ToString("yyyyMMdd_HHmmss") + ".png";

            try
            {
                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);
            }
            catch(UnauthorizedAccessException)
            {
                return "。。。保存失败！创建目录失败！！动态获取的目录为：" + path + "请尝试兼容模式！！";
            }

            path = Path.Combine(path, fileName);
            FileStream photoTankFile = new FileStream(path, FileMode.Create);
            byte[] photoTank = stream.ToArray();
            photoTankFile.Write(photoTank, 0, photoTank.Length);
            photoTankFile.Flush();
            photoTankFile.Close();

            string[] paths = { path };
            MediaScannerConnection.ScanFile(Platform.AppContext, paths, null, null);

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
            //用于替代Environment.ExternalStorageDirectory
            //GetExternalFilesDir()会获取到 /storage/emulated/0/Android/data/应用包名/files
            DirectoryInfo externalStorageDir = new DirectoryInfo(Platform.AppContext.GetExternalFilesDir(null).AbsolutePath);
            return Path.Combine(externalStorageDir.Parent.Parent.Parent.Parent.FullName, Environment.DirectoryPictures, albumName);
        }

        public string GetSavePath()
        {
            return Path.Combine(Environment.DirectoryPictures, albumName);
        }

        public string GetVersion()
        {
            return AppInfo.VersionString;
        }
    }
}