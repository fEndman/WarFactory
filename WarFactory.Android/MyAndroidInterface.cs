using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using WarFactory.MyInterface;
using Android.Media;
using Android.Provider;
using Android.Graphics;
using Android.Database;
using Environment = Android.OS.Environment;
using Path = System.IO.Path;
using System.Collections.Generic;

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

        //返回所有保存时间比这个时间点晚的图片的路径
        [Obsolete]  //烦死了!
        public string[] GetLatestPictures(long timestamp)
        {
            //相当于是得到了一个纵列是Data和DateTaken，横排是按DateTaken从新到旧排列的列表(cursor)
            ICursor cursor = Platform.AppContext.ContentResolver.Query(
                MediaStore.Images.Media.ExternalContentUri,
                new string[] { MediaStore.Images.Media.InterfaceConsts.Data, MediaStore.Images.Media.InterfaceConsts.DateTaken },
                null,
                null,
                MediaStore.Images.Media.InterfaceConsts.DateTaken + " desc");
            List<string> paths = new List<string>();
            while (cursor.MoveToNext())
            {
                long picTimestamp = cursor.GetLong(cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.DateTaken));
                if (timestamp < picTimestamp)
                {
                    string fileName = cursor.GetString(cursor.GetColumnIndex(MediaStore.Images.Media.InterfaceConsts.Data));
                    paths.Add(fileName);
                }
                else    //由于DateTaken是从新到旧的，所以现在这个不是新增的，之后的也都不会是
                    break;
            }
            cursor.Dispose();
            return paths.ToArray();
        }
    }
}