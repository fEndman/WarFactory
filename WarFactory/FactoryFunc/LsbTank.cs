using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using SkiaSharp;
using System.Text;
using System.Text.RegularExpressions;

namespace WarFactory.FactoryFunc
{
    class LsbTank
    {
        static public MemoryStream Encode(FileStream surPicFile, FileStream insPicFile, string info, int compress)
        {
            if (compress == 0 || compress == 3 || compress >= 5) return null;

            byte[] lsbMask = { 0x1, 0x3, 0x7, 0xF };
            char[] signature = "/By:f_Endman".ToCharArray();

            long insPicLength = insPicFile.Length;

            SKBitmap surPic = SKBitmap.Decode(surPicFile);

            //得到隐写里图所需的表图的尺寸，并缩放表图
            long byteForLSB = insPicLength * 8 / compress;  //隐写所有里数据所需的表图字节数
            long currentSurPicByte = surPic.Width * surPic.Height * 3;//表图现有的可用于LSB隐写的字节数
            double zoom = (double)byteForLSB / (double)currentSurPicByte * 1.05d;   //表图需要缩放的倍数（留出5%余量）
            /* 问题可转化为两矩形已知前后面积比例zoom，前者宽度高度a1,b1和a1/b1，并且两矩形长宽比相同即a1/b1=a2/b2；求后者矩形的长a2与宽b2 *
             * ∵a1/b1=a2/b2   ∴a2=a1/b1*b2   又∵a1*b1*zoom=a2*b2   ∴联立可解得b2=b1*根号zoom   ∴a2=a1*根号zoom                       */
            double squareRootZoom = Math.Sqrt(zoom);
            SKBitmap tankPic = new SKBitmap((int)(surPic.Width * squareRootZoom), (int)(surPic.Height * squareRootZoom), SKColorType.Bgra8888, SKAlphaType.Premul);
            surPic.ScalePixels(tankPic, SKFilterQuality.High);

            //为表图添加水印
            SKPaint paint = new SKPaint();
            paint.Color = SKColors.Black;
            paint.TextSize = 24;
            paint.IsAntialias = true;   //抗锯齿
            Assembly asm = Assembly.GetExecutingAssembly();
            Stream infoTypeface = asm.GetManifestResourceStream("WarFactory.Resources.simhei.ttf"); //打开内嵌的字体
            paint.Typeface = SKTypeface.FromStream(infoTypeface);
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

            char[] insFormat = null;
            if (insPicFile.Name.Substring(insPicFile.Name.LastIndexOf(".") + 1) == "png")
                insFormat = "image/png".ToString().ToCharArray();
            else if (insPicFile.Name.Substring(insPicFile.Name.LastIndexOf(".") + 1) == "gif")
                insFormat = "image/gif".ToString().ToCharArray();
            else
                insFormat = "image/jpeg".ToString().ToCharArray();
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

            //---LSB隐写---//
            int bitCount = 8, Count = 0, snCount = 0;
            byte[] insPicByteArray = insPicByteList.ToArray();  //直接用List速度比较慢
            insPicByteList.Clear();
            for (int i = 3; i < tankByteArray.Length; i++)
            {
                tankByteArray[i] &= (byte)~lsbMask[compress - 1];   //清除低n位
                tankByteArray[i] |= (byte)(insPicByteArray[Count] >> (8 - compress) & lsbMask[compress - 1]);
                insPicByteArray[Count] <<= compress;
                if ((bitCount -= compress) == 0)
                {
                    bitCount = 8;
                    if (Count < insPicByteArray.Length - 1)
                        Count++;
                    else
                        insPicByteArray[Count] = ((byte)signature[snCount++ % signature.Length]);
                }
            }

            for (int i = 0; i < tankColorArray.Length; i++)
                tankColorArray[i] = new SKColor(tankByteArray[i * 3 + 0], tankByteArray[i * 3 + 1], tankByteArray[i * 3 + 2]);
            tankPic.Pixels = tankColorArray;

            byte[] tankPicArray = tankPic.Encode(SKEncodedImageFormat.Png, 100).ToArray();

            return new MemoryStream(tankPicArray);
        }

        static public MemoryStream Decode(FileStream tankPicFile, out string lsbFileName)
        {
            byte[] lsbMask = { 0x1, 0x3, 0x7, 0xF, 0x1F };
            SKColor[] sPicColorArray = null;
            lsbFileName = "";

            SKBitmap sPic = SKBitmap.Decode(tankPicFile);
            if (sPic == null) return null;

            sPicColorArray = sPic.Pixels;
            byte[] sPicByteArray = new byte[sPicColorArray.Length * 3];
            List<byte> lsbByte = new List<byte>();

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
             *  Byte[2]:低3位数据为LSB隐写的位数，对应网页版的压缩度    1 <= (Byte[2] & 0x7) <= 5
             */
            if ((sPicByteArray[0] & 0x7) != 0x0 ||
                (sPicByteArray[1] & 0x7) != 0x3 ||
                (sPicByteArray[2] & 0x7) == 0 ||
                (sPicByteArray[2] & 0x7) > 5)
                return null;
            int lsbCompress = sPicByteArray[2] & 0x7;

            //反正就是把LSB数据都读出来了，具体细节很烦，用到了一堆位运算
            int Fifo = 0;   //先进先出，用于缓存LSB
            int FifoCount = 0;   //已经读取的LSB数量
            for (int i = 0; i < sPicByteArray.Length - 2; i++)
            {
                Fifo |= (sPicByteArray[i + 2]) & lsbMask[lsbCompress - 1];
                if (FifoCount >= 8)   //已经读取了一个字节
                {
                    lsbByte.Add((byte)((Fifo >> (FifoCount - 8)) & 0xFF));
                    FifoCount -= 8;
                }
                Fifo <<= lsbCompress;
                FifoCount += lsbCompress;
            }

            //循环检测至少256个字节来获取LSB文件信息
            string sLsbCount = "", lsbFileType = "";
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
                if (lsbByte[offset] != 0x01) lsbFileName += (char)lsbByte[offset];
                else break;
                offset++;
            }
            offset++;
            while (offset < 0xFF)
            {
                if (lsbByte[offset] != 0x00) lsbFileType += (char)lsbByte[offset];
                else break;
                offset++;
            }
            if (offset == 0xFF) return null;
            offset++;

            int LsbCount = 0;
            if(int.TryParse(sLsbCount, out LsbCount) == false)return null;
            int lsbCount = int.Parse(sLsbCount);
            byte[] lsbByteArray = lsbByte.GetRange(offset, lsbCount).ToArray();

            return new MemoryStream(lsbByteArray);
        }
    }
}
