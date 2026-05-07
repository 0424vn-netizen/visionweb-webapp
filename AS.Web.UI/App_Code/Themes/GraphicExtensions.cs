using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;

public static class GraphicExtension
{
    public static Bitmap CreateIndexedImage(Bitmap bmp)
    {
        BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);

        int byteCount = bmpData.Stride * bmpData.Height;
        byte[] bytes = new byte[byteCount];

        Marshal.Copy(bmpData.Scan0, bytes, 0, byteCount);
        bmp.UnlockBits(bmpData);

        Bitmap bmpNew = new Bitmap(bmp.Width, bmp.Height);
        BitmapData bmpData1 = bmpNew.LockBits(new Rectangle(new Point(), bmpNew.Size), ImageLockMode.ReadWrite, bmp.PixelFormat);
        Marshal.Copy(bytes, 0, bmpData1.Scan0, bytes.Length);
        bmpNew.UnlockBits(bmpData1);
        bmp.Dispose();
        return bmpNew;

    }
    public static Image SetImgOpacity(Image imgPic, float imgOpac)
    {
        Bitmap bmpPic = new Bitmap(imgPic.Width, imgPic.Height);
        Graphics gfxPic = Graphics.FromImage(bmpPic);
        ColorMatrix cmxPic = new ColorMatrix();
        cmxPic.Matrix33 = imgOpac;

        ImageAttributes iaPic = new ImageAttributes();
        iaPic.SetColorMatrix(cmxPic, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
        gfxPic.DrawImage(imgPic, new Rectangle(0, 0, bmpPic.Width, bmpPic.Height), 0, 0, imgPic.Width, imgPic.Height, GraphicsUnit.Pixel, iaPic);
        gfxPic.Dispose();

        return bmpPic;
    }
    public static ImageCodecInfo GetEncoderInfo(string mimeType)
    {
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

        for (int i = 0; i < codecs.Length; i++)
        {
            if (codecs[i].MimeType == mimeType)
                return codecs[i];
        }
        return null;
    }
    public static Graphics CreateGraphic(Bitmap img, string type)
    {
        Graphics g = Graphics.FromImage(img);
        if (type == "png")
        {
            g.Clear(Color.FromArgb(0, 0, 0, 0));
        }
        else
        {
            g.Clear(Color.White);
        }
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.CompositingQuality = CompositingQuality.HighQuality;
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        return g;
    }
    public static Graphics CreateGraphicNotClear(Bitmap img)
    {
        Graphics g = Graphics.FromImage(img);     
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.CompositingQuality = CompositingQuality.HighQuality;
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        return g;
    }
    public static string ToHtml(this Color c)
    {
        return ColorTranslator.ToHtml(c);
    }
    public static Image ReadFromFile(string fullPath)
    {
        if (!File.Exists(fullPath))
        {
            return null;
        }
        else 
        {
            Image image = Image.FromFile(fullPath);
            Bitmap bitmap = new Bitmap(image);
            Image indexedImage = CreateIndexedImage(bitmap) as Image;
            image.Dispose();            
            return indexedImage;
        }
    }

    public static GraphicsPath GenerateRoundRect(RectangleF r, float r1, float r2, float r3, float r4)
    {
        float x = r.X, y = r.Y, w = r.Width, h = r.Height;
        GraphicsPath rr = new GraphicsPath();
        rr.AddBezier(x, y + r1, x, y, x + r1, y, x + r1, y);
        rr.AddLine(x + r1, y, x + w - r2, y);
        rr.AddBezier(x + w - r2, y, x + w, y, x + w, y + r2, x + w, y + r2);
        rr.AddLine(x + w, y + r2, x + w, y + h - r3);
        rr.AddBezier(x + w, y + h - r3, x + w, y + h, x + w - r3, y + h, x + w - r3, y + h);
        rr.AddLine(x + w - r3, y + h, x + r4, y + h);
        rr.AddBezier(x + r4, y + h, x, y + h, x, y + h - r4, x, y + h - r4);
        rr.AddLine(x, y + h - r4, x, y + r1);
        return rr;
    }
    public static GraphicsPath GenerateRoundRect(RectangleF r, float r1, float r2, float r3, float r4, bool HasTopLeft, bool HasTopRight, bool HasBottomLeft, bool HasBottomRight)
    {
        float x = r.X, y = r.Y, w = r.Width, h = r.Height;
        GraphicsPath rr = new GraphicsPath();
        if (HasTopLeft)
        {
            rr.AddBezier(x, y + r1, x, y, x + r1, y, x + r1, y);
            rr.AddLine(x + r1, y, x + w - r2, y);
        }
        else
        {
            //Ve duong tren dinh
            rr.AddLine(x, y, x + w, y);
        }

        if (HasTopRight)
        {
            rr.AddBezier(x + w - r2, y, x + w, y, x + w, y + r2, x + w, y + r2);
            rr.AddLine(x + w, y + r2, x + w, y + h - r3);
        }
        else
        {
            //Ve duong tay phai
            rr.AddLine(x + w, y, x + w, y + h);
        }

        if (HasBottomRight)
        {
            rr.AddBezier(x + w, y + h - r3, x + w, y + h, x + w - r3, y + h, x + w - r3, y + h);
            rr.AddLine(x + w - r3, y + h, x + r4, y + h);
        }
        else
        {
            //Ve duong bottom
            rr.AddLine(x + w, y + h, x, y + h);
        }

        if (HasBottomLeft)
        {
            rr.AddBezier(x + r4, y + h, x, y + h, x, y + h - r4, x, y + h - r4);
            rr.AddLine(x, y + h - r4, x, y + r1);
        }
        else
        {
            //Ve duong ben trai
            rr.AddLine(x, y + h, x, y + r1);
        }

        return rr;
    }
    public static Bitmap GenerateGradientRect(VisionWebRectangleStyle button, string fullPath, string imageType)
    {
        int bLeft = 0;
        int bTop = 0;

        Bitmap tempButton = new Bitmap(button.Width, button.Height);
        Graphics mainGrap = GraphicExtension.CreateGraphic(tempButton, imageType);
        Rectangle mainRect = new Rectangle(bLeft, bTop, button.Width - 1, button.Height - 1);
        Color subColor = Color.FromArgb(button.Alpha, button.SubColor);
        Color mainColor = Color.FromArgb(button.Alpha, button.MainColor);
        LinearGradientBrush mainBrush = new LinearGradientBrush(mainRect, mainColor, subColor, 90, true);

        Pen penBorder = new Pen(new SolidBrush(button.BorderColor), button.BorderWidth);
        mainGrap.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 0, 0)), 0, 0, button.Width, button.Height);

        using (GraphicsPath outerGraph = GraphicExtension.GenerateRoundRect(mainRect, button.Radius, button.Radius, button.Radius, button.Radius, button.HasTopLeft, button.HasTopRight, button.HasBottomLeft, button.HasBottomRight))
        {
            mainGrap.FillPath(mainBrush, outerGraph);
            if (button.HasBorder)
            {
                mainGrap.DrawPath(penBorder, outerGraph);
            }
        }
        if (!string.IsNullOrEmpty(fullPath))
        {
            if (imageType == "png")
            {
                GraphicExtension.SaveToPngImage(fullPath, tempButton, 100L);
            }
            else if (imageType == "jpg")
            {
                GraphicExtension.SaveToJpgImage(fullPath, tempButton, 100L);
            }
            tempButton.Dispose();
        }
        mainGrap.Dispose();
        return tempButton;
    }
    public static Bitmap GenerateTriangle(int bWidth, int bHeight, Color mainColor)
    {
        Point p1 = new Point(0, 0);
        Point p2 = new Point(bWidth, 0);
        Point p3 = new Point(bWidth / 2, bHeight);
        Bitmap bmp = new Bitmap(bWidth, bHeight);
        Graphics g = CreateGraphic(bmp, "png");
        g.FillPolygon(new SolidBrush(mainColor), new Point[] { p1, p2, p3 });
        return bmp;
    }

    public static void DrawTriangle(this Bitmap bitmap, int startX, int startY, int width, Color color)
    {
        int haftWidth = (int)(width / 2);

        int absStartX = startX, absEndX = startX + width;
        int absStartY = startY, absEndY = startY + haftWidth;

        for (int y = absStartY; y <= absEndY; y++)
        {
            for (int x = absStartX; x <= absEndX; x++)
            {
                bitmap.SetPixel(x, y, color);
            }
            absStartX++;
            absEndX--;
        }
    }
    public static void DrawAngleTriangle(this Bitmap bitmap, int startX, int startY, int width, Color color)
    {
        int haftWidth = (int)(width / 2);
        int absStartX = startX + haftWidth, absEndX = absStartX;
        int absStartY = startY, absEndY = startY + haftWidth;

        for (int y = absStartY; y <= absEndY; y++)
        {
            for (int x = absStartX; x <= absEndX; x++)
            {
                bitmap.SetPixel(x, y, color);
            }
            absStartX--;
            absEndX++;
        }
    }
    
    /// <summary>
    /// Draw big rect
    /// </summary>
    /// <param name="bitmap"></param>
    /// <param name="startX"></param>
    /// <param name="startY"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static Bitmap FillRectangle(this Bitmap bitmap, int startX, int startY, int width, int height, Color color)
    {
        Graphics mainGrap = Graphics.FromImage(bitmap);        
        mainGrap.SmoothingMode = SmoothingMode.AntiAlias;
        mainGrap.CompositingQuality = CompositingQuality.HighQuality;
        mainGrap.InterpolationMode = InterpolationMode.HighQualityBicubic;
       
        SolidBrush brush = new SolidBrush(color);
        mainGrap.FillRectangle(brush, new Rectangle(startX, startY, width, height));
        return bitmap;
    }
    
    /// <summary>
    /// Draw small rect
    /// </summary>
    /// <param name="bitmap"></param>
    /// <param name="startX"></param>
    /// <param name="startY"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static Bitmap DrawRectangle(this Bitmap bitmap, int startX, int startY, int width, int height, Color color)
    {
        int absStartX = startX, absEndX = startX + width;
        int absStartY = startY, absEndY = startY + height;
        for (int x = absStartX; x <= absEndX; x++)
        {
            for (int y = absStartY; y <= absEndY; y++)
            {
                bitmap.SetPixel(x, y, color);
            }          
        }      
        return bitmap;
    }

    public static void SaveToPngImage(string path, Bitmap img, long quality)
    {
        Save(path, img, quality, "image/png");
    }
    public static void SaveToJpgImage(string path, Bitmap img, long quality)
    {
        Save(path, img, quality, "image/jpeg");
    }
    public static void SaveToGifImage(string path, Bitmap img, long quality)
    {
        Save(path, img, quality, "image/gif");
    }
    public static void Save(string path, Bitmap img, long quality, string mimeType)
    {
        EncoderParameter qualityParam
              = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);
        //Copy an image from orginal image           
        Bitmap btnNew = GraphicExtension.CreateIndexedImage(img);
        ImageCodecInfo pngCodec = GetEncoderInfo(mimeType);
        EncoderParameters encoderParams = new EncoderParameters(1);
        encoderParams.Param[0] = qualityParam;
        //Remove readonly to resolve GDI+ error
        if (File.Exists(path))
        {
            File.SetAttributes(path, FileAttributes.Normal);
        }
        btnNew.Save(path, pngCodec, encoderParams);
        btnNew.Dispose();
    }
}

