using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace WarFactory.MyInterface
{
    public interface IPlatformService
    {
        Task<string> ImageSave(MemoryStream stream, string fileName = null);
        Task RequestPermissions();
        string GetAbsoluteSavePath();
        string GetSavePath();
    }
}
