using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace WarFactory.MyInterface
{
    public interface ISaveFileService
    {
        Task<string> ImageSave(MemoryStream stream, string fileName = null);
    }
}
