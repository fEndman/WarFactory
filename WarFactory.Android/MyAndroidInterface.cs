using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using WarFactory.MyInterface;
using Environment = Android.OS.Environment;

[assembly: Dependency(typeof(WarFactory.Droid.SaveFileService))]
namespace WarFactory.Droid
{
    public class SaveFileService : ISaveFileService
    {
        public async Task<string> ImageSave(MemoryStream stream, string fileName = null)
        {
            await Permissions.RequestAsync<Permissions.StorageWrite>();
            if (Permissions.ShouldShowRationale<Permissions.StorageWrite>()) return "...保存个屁！不给爷权限还想让爷给你造坦克？";
            await Permissions.RequestAsync<Permissions.StorageRead>();
            if (Permissions.ShouldShowRationale<Permissions.StorageRead>()) return "...保存个屁！不给爷权限还想让爷给你造坦克？";

            string path = Path.Combine(Environment.ExternalStorageDirectory.AbsolutePath, Environment.DirectoryPictures, "WarFactory");
            if (fileName == null || fileName == "") fileName = "Tank_" + DateTime.Now.ToLocalTime().ToString("yyyyMMdd_HHmmss") + ".png";

            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);

            path = Path.Combine(path, fileName);
            FileStream photoTankFile = new FileStream(path, FileMode.Create);
            byte[] photoTank = stream.ToArray();

            photoTankFile.Write(photoTank, 0, photoTank.Length);
            photoTankFile.Flush();
            photoTankFile.Close();

            return (Environment.DirectoryPictures + "/WarFactory/" + fileName);
        }
    }
}