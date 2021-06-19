using SkiaSharp;

namespace WarFactory.FactoryFunc
{
    class MirageTank
    {
        static public SKBitmap Encode(ref SKBitmap photo1, ref SKBitmap photo2, float photo1K, float photo2K, byte threshold)
        {
            //灰度处理
            SKColor[] photo1ColorArray = null;
            photo1ColorArray = photo1.Pixels;
            for (int i = 0; i < photo1ColorArray.Length; i++)
            {
                int tmpValue = (int)(GetGrayNumColor(photo1ColorArray[i]) * photo1K);
                if (tmpValue < threshold + 1) tmpValue = threshold + 1;
                if (tmpValue > 254) tmpValue = 254;
                photo1ColorArray[i] = new SKColor((byte)tmpValue, (byte)tmpValue, (byte)tmpValue);
            }
            photo1.Pixels = photo1ColorArray;

            SKColor[] photo2ColorArray = null;
            photo2ColorArray = photo2.Pixels;
            for (int i = 0; i < photo2ColorArray.Length; i++)
            {
                int tmpValue = (int)(GetGrayNumColor(photo2ColorArray[i]) * photo2K);
                if (tmpValue > threshold) tmpValue = threshold;
                if (tmpValue < 1) tmpValue = 1;
                photo2ColorArray[i] = new SKColor((byte)tmpValue, (byte)tmpValue, (byte)tmpValue);
            }
            photo2.Pixels = photo2ColorArray;

            //调整图像大小
            int height = 0, width = 0;
            if (photo1.Height != photo2.Height || photo1.Width != photo2.Width)
            {
                if (photo1.Height > photo2.Height) height = photo1.Height;
                else height = photo2.Height;
                if (photo1.Width > photo2.Width) width = photo1.Width;
                else width = photo2.Width;
            }
            else
            {
                width = photo1.Width;
                height = photo1.Height;
            }
            float aspectRatio = width / height; //高宽比
            if (aspectRatio > photo1.Width / photo1.Height) //根据高宽比拉伸图像
                photo1 = photo1.Resize(new SKSizeI(photo1.Width * height / photo1.Height, height), SKFilterQuality.High);
            else
                photo1 = photo1.Resize(new SKSizeI(width, photo1.Height * width / photo1.Width), SKFilterQuality.High);
            if (aspectRatio > photo2.Width / photo2.Height) //根据高宽比拉伸图像
                photo2 = photo2.Resize(new SKSizeI(photo2.Width * height / photo2.Height, height), SKFilterQuality.High);
            else
                photo2 = photo2.Resize(new SKSizeI(width, photo2.Height * width / photo2.Width), SKFilterQuality.High);
            SKBitmap photo1Temp = new SKBitmap(width, height);
            SKBitmap photo2Temp = new SKBitmap(width, height);
            SKCanvas photo1Canvas = new SKCanvas(photo1Temp);
            SKCanvas photo2Canvas = new SKCanvas(photo2Temp);
            photo1Canvas.DrawColor(SKColors.LightGray);
            photo2Canvas.DrawColor(SKColors.DimGray);
            photo1Canvas.DrawBitmap(photo1, width / 2 - photo1.Width / 2, height / 2 - photo1.Height / 2);
            photo2Canvas.DrawBitmap(photo2, width / 2 - photo2.Width / 2, height / 2 - photo2.Height / 2);
            photo1ColorArray = photo1Temp.Pixels;
            photo2ColorArray = photo2Temp.Pixels;

            /*  /!/装配坦克/!/  */
            SKBitmap photoTank = new SKBitmap(width, height);
            SKColor[] photoTankColorArray = new SKColor[width * height];
            for (int i = 0; i < width * height; i++)
            {
                int pixel1 = photo1ColorArray[i].Red;
                int pixel2 = photo2ColorArray[i].Red;

                int alpha = 255 - (pixel1 - pixel2);
                int gray = (int)(255 * pixel2 / alpha);
                if (gray > 255) gray = 255;

                photoTankColorArray[i] = new SKColor((byte)gray, (byte)gray, (byte)gray, (byte)alpha);
            }
            photoTank.Pixels = photoTankColorArray;

            return photoTank;
        }

        static private int GetGrayNumColor(SKColor codecolor)
        {
            //return (codecolor.Red * 19595 + codecolor.Green * 38469 + codecolor.Blue * 7472) >> 16;
            return (int)((float)codecolor.Red * 0.229f + (float)codecolor.Green * 0.587f + (float)codecolor.Blue * 0.114f);
        }
    }
}
