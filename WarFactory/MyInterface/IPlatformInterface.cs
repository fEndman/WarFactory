using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace WarFactory.MyInterface
{
    public interface IPlatformService
    {
        Task<string> ImageSave(MemoryStream stream, bool compatibleMode, string fileName = null);
        Task RequestPermissions();
        string GetAbsoluteSavePath();
        string GetSavePath();
        string GetVersion();
        string[] GetLatestPictures(long timestamp);  //返回所有保存时间比这个时间点晚的图片
    }
}
