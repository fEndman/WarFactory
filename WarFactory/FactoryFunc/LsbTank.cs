using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using SkiaSharp;
using System.Text;

namespace WarFactory.FactoryFunc
{
    class LsbTank
    {
        static public MemoryStream Encode(FileStream surPicFile, FileStream insPicFile, string info, int compress)
        {
            if (compress == 0 || compress >= 8) return null;

            byte[] lsbMask = { 0x1, 0x3, 0x7, 0xF, 0x1F, 0x3F, 0x7F };
            char[] signature = "/By:f_Endman".ToCharArray();

            long insPicLength = insPicFile.Length;

            SKBitmap surPic = SKBitmap.Decode(surPicFile);

            //得到隐写里图所需的表图的尺寸，并缩放表图
            long byteForLSB = insPicLength * 8 / compress;  //隐写所有里数据所需的表图字节数
            long currentSurPicByte = surPic.Width * surPic.Height * 3;//表图现有的可用于LSB隐写的字节数
            double zoom = (double)byteForLSB / (double)currentSurPicByte * ((compress >= 6) ? 1.05d : 1.01d);   //表图需要缩放的倍数（留出1%-5%余量）
            /* 问题可转化为两矩形已知前后面积比例zoom，前者宽度高度a1,b1和a1/b1，并且两矩形长宽比相同即a1/b1=a2/b2；求后者矩形的长a2与宽b2 *
             * ∵a1/b1=a2/b2   ∴a2=a1/b1*b2   又∵a1*b1*zoom=a2*b2   ∴联立可解得b2=b1*根号zoom   ∴a2=a1*根号zoom                       */
            double squareRootZoom = Math.Sqrt(zoom);
            SKBitmap tankPic = new SKBitmap((int)(surPic.Width * squareRootZoom), (int)(surPic.Height * squareRootZoom), SKColorType.Bgra8888, SKAlphaType.Premul);
            surPic.ScalePixels(tankPic, SKFilterQuality.High);

            //为表图添加水印
            SKPaint paint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 24,
                IsAntialias = true, //抗锯齿
                Typeface = SKTypeface.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("WarFactory.Resources.simhei.ttf"))  //使用内嵌的字体
            };
            SKRect textSize = new SKRect();
            paint.MeasureText(info, ref textSize);  //得到文字的尺寸
            int textWidth = (int)(textSize.Size.Width + 2);
            if (textWidth > tankPic.Width) textWidth = tankPic.Width;
            SKBitmap infoPic = new SKBitmap(textWidth, 30); //创建水印
            SKCanvas canvas = new SKCanvas(infoPic);
            canvas.DrawColor(SKColors.White);
            canvas.DrawText(info, 0, (30 - textSize.Size.Height) / 2 - textSize.Top, paint);
            byte alpha = 0xCF;  //水印不透明度
            for (int i = 0; i < infoPic.Height; i++)    //混色
            {
                for (int j = 0; j < infoPic.Width; j++)
                {
                    SKColor infoColor = infoPic.GetPixel(j, i);
                    SKColor surColor = tankPic.GetPixel(j, i);
                    byte red = (byte)((infoColor.Red * alpha + surColor.Red * (0xFF - alpha)) / 0xFF);
                    byte green = (byte)((infoColor.Green * alpha + surColor.Green * (0xFF - alpha)) / 0xFF);
                    byte blue = (byte)((infoColor.Blue * alpha + surColor.Blue * (0xFF - alpha)) / 0xFF);
                    tankPic.SetPixel(j, i, new SKColor(red, green, blue));
                }
            }

            //写入里数据文件头（根据网页源码以及WinHex亲测推测）
            /* 结构如下：
             * 里数据大小（字符串格式）
             * '\1'
             * 里文件名称
             * '\1'
             * 里文件格式（特定字符串）：目前我见过的原作者写的有"image/jpeg"和"image/png"和"image/png"三种，这个应该是关系到网页版解码后显示（我本业是嵌入式，让我完全弄明白这玩意就是难为我QWQ）
             * '\0'
             */
            List<byte> insPicByteList = new List<byte>();
            char[] insPicLengthStr = insPicLength.ToString().ToCharArray();
            for (int i = 0; i < insPicLengthStr.Length; i++)
                insPicByteList.Add((byte)insPicLengthStr[i]);

            insPicByteList.Add(0x01);

            char[] insPicLengthFileName = insPicFile.Name.Substring(insPicFile.Name.LastIndexOf("/") + 1).ToCharArray(); //获取包含扩展名的文件名
            for (int i = 0; i < insPicLengthFileName.Length; i++)
                insPicByteList.Add((byte)insPicLengthFileName[i]);

            insPicByteList.Add(0x01);

            char[] insFormat;
            string extName = insPicFile.Name.Substring(insPicFile.Name.LastIndexOf(".") + 1).ToLower();

            switch (extName)
            {
                case "png":
                    insFormat = "image/png".ToCharArray();
                    break;
                case "gif":
                    insFormat = "image/gif".ToCharArray();
                    break;
                case "jpg":
                case "jpeg":
                    insFormat = "image/jpeg".ToCharArray();
                    break;
                case "avi":
                    insFormat = "video/avi".ToCharArray();
                    break;
                case "mp4":
                    insFormat = "video/mp4".ToCharArray();
                    break;
                case "mp3":
                case "wav":
                case "ogg":
                    insFormat = "audio/mpeg".ToCharArray();
                    break;
                default:
                    insFormat = "".ToCharArray();
                    break;
            }

            for (int i = 0; i < insFormat.Length; i++)
                insPicByteList.Add((byte)insFormat[i]);

            insPicByteList.Add(0x00);

            //读取里数据
            byte[] insPicByte = new byte[insPicFile.Length];
            insPicFile.Read(insPicByte, 0, (int)insPicFile.Length);
            insPicFile.Seek(0, SeekOrigin.Begin);
            insPicByteList.AddRange(new List<byte>(insPicByte));

            //BGRA转RGB
            SKColor[] tankColorArray = tankPic.Pixels;
            byte[] tankByteArray = new byte[tankColorArray.Length * 3];
            for (int i = 0; i < tankColorArray.Length; i++)
            {
                tankByteArray[i * 3 + 0] = (tankColorArray[i].Red);
                tankByteArray[i * 3 + 1] = (tankColorArray[i].Green);
                tankByteArray[i * 3 + 2] = (tankColorArray[i].Blue);
            }

            //前三个字节为数据标识保留（根据网页源码推测）
            /* 原图数据前三个字节推测如下：
             *  Byte[0]:低3位固定为0x0
             *  Byte[1]:低3位固定为0x3
             *  Byte[2]:低3位数据为LSB隐写的位数，对应网页版的压缩度    1 <= (Byte[2] & 0x7) <= 4
             */
            tankByteArray[0] &= 0xF8;
            tankByteArray[0] |= 0x00;
            tankByteArray[1] &= 0xF8;
            tankByteArray[1] |= 0x03;
            tankByteArray[2] &= 0xF8;
            tankByteArray[2] |= (byte)(compress & 0x7);

            //---LSB隐写，具体细节很繁琐，用到了一堆位运算---//
            int Count = 0, snCount = 0;
            Int32 FIFO = 0;     //先进先出，用于缓存LSB
            int FifoCount = 0;  //FIFO中剩余的要写入的LSB的数量
            byte[] insPicByteArray = insPicByteList.ToArray();  //直接用List速度比较慢
            insPicByteList.Clear();
            for (int i = 3; i < tankByteArray.Length; i++)
            {
                if (FifoCount < compress)   //FIFO不够写了就读一个字节
                {
                    //无影坦克的LSB是大端的，所以从"左"取数据，即先取高位
                    //如果里数据已经全部写入，就填充签名字符串
                    FIFO |= (Int32)(((Count < insPicByteArray.Length) ? insPicByteArray[Count++] : (byte)signature[snCount++ % signature.Length]) << (/* 32 - 8 */ 24 - FifoCount));
                    FifoCount += 8;
                }
                tankByteArray[i] &= (byte)~lsbMask[compress - 1];   //清除低n位
                //无影坦克的LSB是大端的，所以从"左"取数据，即先取高位
                tankByteArray[i] |= (byte)((FIFO >> (32 - compress)) & lsbMask[compress - 1]);
                FIFO <<= compress;
                FifoCount -= compress;
            }

            for (int i = 0; i < tankColorArray.Length; i++)
                tankColorArray[i] = new SKColor(tankByteArray[i * 3 + 0], tankByteArray[i * 3 + 1], tankByteArray[i * 3 + 2]);
            tankPic.Pixels = tankColorArray;

            byte[] tankPicArray = tankPic.Encode(SKEncodedImageFormat.Png, 100).ToArray();

            surPicFile.Close();
            surPicFile.Dispose();
            insPicFile.Close();
            insPicFile.Dispose();

            return new MemoryStream(tankPicArray);
        }

        static public MemoryStream Decode(FileStream tankPicFile, out string lsbFileName)
        {
            byte[] lsbMask = { 0x1, 0x3, 0x7, 0xF, 0x1F, 0x3F, 0x7F };
            lsbFileName = "";

            SKBitmap sPic = SKBitmap.Decode(tankPicFile);
            if (sPic == null) return null;

            SKColor[] sPicColorArray = sPic.Pixels;
            byte[] sPicByteArray = new byte[sPicColorArray.Length * 3];

            //读取所有像素的RGB字节
            for (int i = 0; i < sPicColorArray.Length; i++)
            {
                sPicByteArray[3 * i + 0] = sPicColorArray[i].Red;
                sPicByteArray[3 * i + 1] = sPicColorArray[i].Green;
                sPicByteArray[3 * i + 2] = sPicColorArray[i].Blue;
            }

            //前三个字节为数据标识保留（根据网页源码推测），因此从第三字节开始读取LSB数据
            /* 原图数据前三个字节推测如下：
             *  Byte[0]:低3位固定为0x0
             *  Byte[1]:低3位固定为0x3
             *  Byte[2]:低3位数据为LSB隐写的位数，对应网页版的压缩度    1 <= (Byte[2] & 0x7) <= 7
             */
            if ((sPicByteArray[0] & 0x7) != 0x0 ||
                (sPicByteArray[1] & 0x7) != 0x3 ||
                (sPicByteArray[2] & 0x7) == 0 ||
                (sPicByteArray[2] & 0x7) > 7)
                return null;
            int lsbCompress = sPicByteArray[2] & 0x7;

            //反正就是把LSB数据都读出来了，具体细节很繁琐，用到了一堆位运算
            int FIFO = 0;   //先进先出，用于缓存LSB
            int FifoCount = 0;   //已经读取的LSB数量
            List<byte> lsbByte = new List<byte>();
            for (int i = 2; i < sPicByteArray.Length; i++)
            {
                FIFO |= (sPicByteArray[i]) & lsbMask[lsbCompress - 1];
                if (FifoCount >= 8)   //已经读取了一个字节
                {
                    lsbByte.Add((byte)((FIFO >> (FifoCount - 8)) & 0xFF));
                    FifoCount -= 8;
                }
                FIFO <<= lsbCompress;
                FifoCount += lsbCompress;
            }

            //循环检测至少256个字节来获取LSB文件信息
            string sLsbCount = "", lsbFileType = "";
            List<byte> lsbFileNameList = new List<byte>();
            int offset = 0;
            while (offset < 0xFF)
            {
                if (lsbByte[offset] != 0x01) sLsbCount += (char)lsbByte[offset];
                else break;
                offset++;
            }
            offset++;
            while (offset < 0xFF)
            {
                if (lsbByte[offset] != 0x01) lsbFileNameList.Add(lsbByte[offset]);
                else break;
                offset++;
            }
            lsbFileName = Encoding.UTF8.GetString(lsbFileNameList.ToArray());   //UTF8转ASCII
            offset++;
            while (offset < 0xFF)
            {
                if (lsbByte[offset] != 0x00) lsbFileType += (char)lsbByte[offset];
                else break;
                offset++;
            }
            if (offset == 0xFF) return null;
            offset++;

            if(int.TryParse(sLsbCount, out int LsbCount) == false)return null;
            byte[] lsbByteArray = lsbByte.GetRange(offset, LsbCount).ToArray();

            tankPicFile.Close();
            tankPicFile.Dispose();

            return new MemoryStream(lsbByteArray);
        }
    }
}
