using System;
using System.IO;
using System.Threading.Tasks;
using WarFactory.MyInterface;
using Xamarin.Essentials;
using Xamarin.Forms;
using Photos;
using Foundation;
using MimeMapping;

[assembly: Dependency(typeof(WarFactory.iOS.PlatformService))]
namespace WarFactory.iOS
{
    class PlatformService : IPlatformService
    {
        private readonly string albumName = "战车工厂";

        public async Task<string> ImageSave(MemoryStream stream, bool compatibleMode, string fileName = null)
        {
            NSError error = null;

            //虽然对于iOS没有这两个权限，但要保证方法异步，所以还是保留下来了
            await Permissions.RequestAsync<Permissions.StorageWrite>();
            await Permissions.RequestAsync<Permissions.StorageRead>();

            //判断相册是否存在，不存在就创建
            PHAssetCollection appAlbum = null;
            PHFetchResult albums = PHAssetCollection.FetchAssetCollections(PHAssetCollectionType.Album, PHAssetCollectionSubtype.Any, null);
            foreach (PHAssetCollection album in albums)
            {
                if (album.LocalizedTitle == albumName)
                    appAlbum = album;
            }
            if (appAlbum == null)   //相册不存在，新建
            {
                string[] albumID = new string[1];
                PHPhotoLibrary.SharedPhotoLibrary.PerformChangesAndWait(() =>
                {
                    albumID[0] = PHAssetCollectionChangeRequest.CreateAssetCollection(albumName).PlaceholderForCreatedAssetCollection.LocalIdentifier;
                }, out error);
                appAlbum = PHAssetCollection.FetchAssetCollections(albumID, null)[0] as PHAssetCollection;
            }

            //获取路径及名称
            string documentsPath;
            documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (fileName == null || fileName == "") fileName = "Tank_" + DateTime.Now.ToLocalTime().ToString("yyyyMMdd_HHmmss") + ".png";
            string path = Path.Combine(documentsPath, fileName);

            //保存
            FileStream photoTankFile = new FileStream(path, FileMode.Create);
            byte[] photoTank = stream.ToArray();
            photoTankFile.Write(photoTank, 0, photoTank.Length);
            photoTankFile.Flush();
            photoTankFile.Close();

            //如果是图片或视频，就添加到相册里
            string MimeType = MimeUtility.GetMimeMapping(path);
            if (MimeType.IndexOf("image") != -1 || MimeType.IndexOf("video") != -1)
            {
                string[] assetID = new string[1];
                PHPhotoLibrary.SharedPhotoLibrary.PerformChangesAndWait(() =>
                {
                    if (MimeType.IndexOf("image") != -1)
                        assetID[0] = PHAssetChangeRequest.FromImage(new NSUrl(path, true)).PlaceholderForCreatedAsset.LocalIdentifier;
                    if (MimeType.IndexOf("video") != -1)
                        assetID[0] = PHAssetChangeRequest.FromVideo(new NSUrl(path, true)).PlaceholderForCreatedAsset.LocalIdentifier;
                }, out error);
                PHAsset asset = PHAsset.FetchAssetsUsingLocalIdentifiers(assetID, null)[0] as PHAsset;
                PHObject[] objs = { asset };
                PHPhotoLibrary.SharedPhotoLibrary.PerformChangesAndWait(() =>
                {
                    PHAssetCollectionChangeRequest collectionChangeRequest = PHAssetCollectionChangeRequest.ChangeRequest(appAlbum);
                    collectionChangeRequest.InsertAssets(objs, new NSIndexSet(0));
                }, out error);
            }

            return Path.Combine(fileName);
        }

        public async Task RequestPermissions()
        {
            await Permissions.RequestAsync<Permissions.StorageWrite>();
            await Permissions.RequestAsync<Permissions.StorageRead>();
        }

        public string GetAbsoluteSavePath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        }

        public string GetSavePath()
        {
            return "(不会真的有人想看iOS的全部路径吧(￢_￢) )";//GetAbsoluteSavePath();
        }

        public string GetVersion()
        {
            return AppInfo.VersionString;
        }

        public string[] GetLatestPictures(long timestamp)
        {
            return null;
        }
    }
}