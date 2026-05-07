using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;


public interface IImageGenerator
{
    void Generate();
}

public class VisionWebImageGenerator : IImageGenerator
{
    private string _DestinationThemePath; // for Theme path
    private string _DestinationLoginPath;
    private VisionWebThemeInformation _ThemeInfo;

    public VisionWebImageGenerator(VisionWebThemeInformation themeInfo, string destinationPath, string destinationLoginPath)
    {
        this._ThemeInfo = themeInfo;
        this._DestinationThemePath = destinationPath;
        this._DestinationLoginPath = destinationLoginPath;
    }

    public void Generate()
    {
        this.GenerateLogo();
        this.GenerateLoginBanner();
        this.GenerateBanner();        
        this.GenerateButton(45, 18, 5, "btn_short.png", "png", this._DestinationThemePath);
        this.GenerateButton(45, 18, 5, "btn_short.png", "png", this._DestinationThemePath);
        this.GenerateButton(82, 34, 5, "login_btn.png", "png", this._DestinationLoginPath);    
        this.GenerateLongButton();
        this.GenerateMenuPrimaryColor();
        this.GenerateMenuMouseHoverColor();
        this.GenerateDropDown();
        this.ConvertExpandArrowsImage();
        this.GeneratePrinterFriendlyButton();
        this.ConvertOtherImages();
      
    }
    private void GenerateLogo()
    {
        string fullPath = Path.Combine(this._DestinationThemePath, "Images\\logo.png");
        Bitmap logo = new Bitmap(_ThemeInfo.Logo);
        GraphicExtension.SaveToPngImage(fullPath, logo, 100L);

        fullPath = Path.Combine(this._DestinationThemePath, "Images\\statement_logo.jpg");
        logo = new Bitmap(_ThemeInfo.StatementLogo);
        GraphicExtension.SaveToJpgImage(fullPath, logo, 100L);
    }
    private void GenerateBanner()
    {
        string logo = Path.Combine(this._DestinationThemePath, "Images\\logo.png");
        string bannerDefault = Path.Combine(this._DestinationThemePath, "Images\\top_banner_default.jpg");
        string banner = Path.Combine(this._DestinationThemePath, "Images\\top_banner.jpg");  
        Bitmap logoImage = (Bitmap)Bitmap.FromFile(logo);
        if (File.Exists(bannerDefault))
        {
            Bitmap bmpBlankHeader = (Bitmap)Bitmap.FromFile(bannerDefault);

            //
            //Check Logo to align
            int wLogo = logoImage.Width;
            int hLogo = logoImage.Height;           
           
            for (int x = 0; x < wLogo; x++)
            {
                for (int y = 0; y < hLogo; y++)
                {
                    Color cLogo = logoImage.GetPixel(x, y);
                    if (cLogo.A != 0)  // not transparent
                    {
                        bmpBlankHeader.SetPixel(x + 50, y + 4, cLogo);
                    }
                }
            }
            GraphicExtension.SaveToJpgImage(banner, bmpBlankHeader, 100L);
            logoImage.Dispose();           
            File.Delete(bannerDefault);
        }
    }
    private void GenerateLoginBanner()
    {
        string logo = Path.Combine(this._DestinationThemePath, "Images\\logo.png");
        string bannerDefault = Path.Combine(this._DestinationLoginPath, "Images\\login_logo_default.jpg");
        string banner = Path.Combine(this._DestinationLoginPath, "Images\\login_logo.jpg");
        Bitmap logoImage = (Bitmap)Bitmap.FromFile(logo);
        if (File.Exists(bannerDefault))
        {
            Bitmap bmpBlankHeader = (Bitmap)Bitmap.FromFile(bannerDefault);

            //
            //Check Logo to align
            int wLogo = logoImage.Width;
            int hLogo = logoImage.Height;

            for (int x = 0; x < wLogo; x++)
            {
                for (int y = 0; y < hLogo; y++)
                {
                    Color cLogo = logoImage.GetPixel(x, y);
                    if (cLogo.A != 0)  // not transparent
                    {                   
                        bmpBlankHeader.SetPixel(x + 200, y + 4, cLogo);
                    }
                }
            }
            GraphicExtension.SaveToJpgImage(banner, bmpBlankHeader, 100L);
            logoImage.Dispose();    
            File.Delete(bannerDefault);
        }
    }
    protected void GenerateButton(int width, int height, int radius, string buttonName, string imageType, string path)
    {
        Color mainColor = ColorTranslator.FromHtml(this._ThemeInfo.ButtonPrimaryColor);
        Color subColor = ColorTranslator.FromHtml(this._ThemeInfo.ButtonSubColor);

        string fullPath = Path.Combine(path, "Images\\" + buttonName);
        VisionWebRectangleStyle buttonStyle = new VisionWebRectangleStyle 
        {
            Width = width,
            Height = height,
            MainColor = mainColor, SubColor = subColor,
            Radius = radius 
        };

        Bitmap orginal = GraphicExtension.GenerateGradientRect(buttonStyle, string.Empty, imageType);
        Bitmap button = new Bitmap(width, height);
        Graphics graphic = GraphicExtension.CreateGraphic(button, imageType);
        graphic.DrawImage((Image)orginal, new Point(0, 0)); 

        GraphicExtension.SaveToPngImage(fullPath, button, 100L);
    }
    protected void GenerateButton(int frameWidth, int frameHeight, int frameOffsetX, int frameOffsetY, int width, int height, int radius, string buttonName, string imageType)
    {
        Color mainColor = ColorTranslator.FromHtml(this._ThemeInfo.ButtonPrimaryColor);
        Color subColor = ColorTranslator.FromHtml(this._ThemeInfo.ButtonSubColor);

        string fullPath = Path.Combine(this._DestinationThemePath, "Images\\" + buttonName);
        VisionWebRectangleStyle buttonStyle = new VisionWebRectangleStyle
        {
            Width = width,
            Height = height,
            MainColor = mainColor,
            SubColor = subColor,
            Radius = radius
        };

        Bitmap orginal = GraphicExtension.GenerateGradientRect(buttonStyle, string.Empty, imageType);
        Bitmap button = new Bitmap(frameWidth, frameHeight);
        Graphics graphic = GraphicExtension.CreateGraphic(button, imageType);
        graphic.DrawImage((Image)orginal, new Point(frameOffsetX, frameOffsetY));

        GraphicExtension.SaveToPngImage(fullPath, button, 100L);
    }
    protected void GenerateLongButton()
    {
        Color mainColor = ColorTranslator.FromHtml(this._ThemeInfo.ButtonPrimaryColor);
        Color subColor = ColorTranslator.FromHtml(this._ThemeInfo.ButtonSubColor);
        string fullPath = string.Empty;
        string fullPathTemp = string.Empty;
        VisionWebRectangleStyle loginStyle = new VisionWebRectangleStyle { Width = 8, Height = 24, MainColor = mainColor, SubColor = subColor, Radius = 6 };

        loginStyle.HasTopRight = false;
        loginStyle.HasBottomRight = false;
        loginStyle.HasTopLeft = true;
        loginStyle.HasBottomLeft = true;
        fullPath = Path.Combine(this._DestinationThemePath, "Images\\btn_long_l.png");     
        GraphicExtension.GenerateGradientRect(loginStyle, fullPath, "png");

        loginStyle.HasTopRight = true;
        loginStyle.HasBottomRight = true;
        loginStyle.HasTopLeft = false;
        loginStyle.HasBottomLeft = false;
        fullPath = Path.Combine(this._DestinationThemePath, "Images\\btn_long_r.png");
        GraphicExtension.GenerateGradientRect(loginStyle, fullPath, "png");    

        loginStyle.Width = 10;
        loginStyle.HasTopRight = false;
        loginStyle.HasBottomRight = false;
        loginStyle.HasTopLeft = false;
        loginStyle.HasBottomLeft = false;
        fullPath = Path.Combine(this._DestinationThemePath, "Images\\btn_long_m.png");
        fullPathTemp = Path.Combine(this._DestinationThemePath, "Images\\btn_long_m_temp.png");
        GraphicExtension.GenerateGradientRect(loginStyle, fullPathTemp, "png");
        RemoveLeftRightPixel(8, 24, 1, 1, fullPath, fullPathTemp, "png");
    }

    private void GenerateMenuPrimaryColor()
    {
        Color mainColor = ColorTranslator.FromHtml(this._ThemeInfo.ButtonPrimaryColor);
        Color subColor = ColorTranslator.FromHtml(this._ThemeInfo.ButtonSubColor);
        string fullPath = string.Empty;
        string fullPathTemp = string.Empty;
        VisionWebRectangleStyle loginStyle = new VisionWebRectangleStyle { Width = 498, Height = 26, MainColor = mainColor, SubColor = subColor, Radius = 5 };

        loginStyle.HasTopRight = false;
        loginStyle.HasBottomRight = false;
        loginStyle.HasTopLeft = true;
        loginStyle.HasBottomLeft = false;        
        fullPath = Path.Combine(this._DestinationThemePath, "Images\\menutableft.jpg");
        GraphicExtension.GenerateGradientRect(loginStyle, fullPath, "jpg");

        loginStyle.Width = 14;
        loginStyle.HasTopRight = true;
        loginStyle.HasBottomRight = false;
        loginStyle.HasTopLeft = false;
        loginStyle.HasBottomLeft = false;        
        fullPath = Path.Combine(this._DestinationThemePath, "Images\\menutabright.jpg");
        fullPathTemp = Path.Combine(this._DestinationThemePath, "Images\\menutabright_temp.jpg");
        GraphicExtension.GenerateGradientRect(loginStyle, fullPathTemp, "jpg");
        RemoveLeftRightPixel(12, 26, 2, 0, fullPath, fullPathTemp, "jpg");
      
    }

    private void GenerateMenuMouseHoverColor()
    {
        Color mainColor = ColorTranslator.FromHtml(this._ThemeInfo.MenuMouseHoverColor);
        Color subColor = ColorTranslator.FromHtml(this._ThemeInfo.MenuMouseHoverSubColor);
        string fullPath = string.Empty;
        string fullPathTemp = string.Empty;
        VisionWebRectangleStyle loginStyle = new VisionWebRectangleStyle { Width = 498, Height = 26, MainColor = mainColor, SubColor = subColor, Radius = 5 };

        loginStyle.HasTopRight = false;
        loginStyle.HasBottomRight = false;
        loginStyle.HasTopLeft = true;
        loginStyle.HasBottomLeft = false;
        fullPath = Path.Combine(this._DestinationThemePath, "Images\\menu-over-left.jpg");
        GraphicExtension.GenerateGradientRect(loginStyle, fullPath, "jpg");

        loginStyle.Width = 14;
        loginStyle.HasTopRight = true;
        loginStyle.HasBottomRight = false;
        loginStyle.HasTopLeft = false;
        loginStyle.HasBottomLeft = false;
        fullPath = Path.Combine(this._DestinationThemePath, "Images\\menu-over-right.jpg");
        fullPathTemp = Path.Combine(this._DestinationThemePath, "Images\\menu-over-right_temp.jpg");
        GraphicExtension.GenerateGradientRect(loginStyle, fullPathTemp, "jpg");
        RemoveLeftRightPixel(12, 26, 2, 0, fullPath, fullPathTemp, "jpg"); 
    }

    protected void GenerateDropDown()
    {
        Color mainColor = ColorTranslator.FromHtml(this._ThemeInfo.ButtonPrimaryColor);
        Color subColor = ColorTranslator.FromHtml(this._ThemeInfo.ButtonSubColor);
        Color textColor = ColorTranslator.FromHtml(this._ThemeInfo.PrimaryTextColor);

        VisionWebRectangleStyle comboStyle = new VisionWebRectangleStyle
        {
            Radius = 0,
            Width = 28,
            Height = 26,
            MainColor = mainColor,
            SubColor = subColor,
            HasTopRight = false,
            HasBottomRight = false,
            HasTopLeft = false,
            HasBottomLeft = false,
        };
        string fullPath = Path.Combine(this._DestinationThemePath, "Images\\rcbArrowCell.jpg");
        string fullPathTemp = Path.Combine(this._DestinationThemePath, "Images\\rcbArrowCell_temp.jpg");
        GraphicExtension.GenerateGradientRect(comboStyle, fullPathTemp, "jpg");
        RemoveLeftRightPixel(26, 26, 1, 1, fullPath, fullPathTemp, "jpg");
        Bitmap rect = (Bitmap)Bitmap.FromFile(fullPath);
        rect.DrawTriangle(9, 10, 7, textColor);
        GraphicExtension.SaveToJpgImage(fullPath, rect, 100L);
        

    }
    private void ConvertExpandArrowsImage()
    {
        string fullPath = Path.Combine(this._DestinationThemePath, "Images\\ExpandArrows-default.png");
        Color menuMouseOverColor = System.Drawing.ColorTranslator.FromHtml(this._ThemeInfo.MenuMouseHoverColor);
        Bitmap bmp = (Bitmap)Bitmap.FromFile(fullPath);
        for (int x = 0; x < bmp.Width; x++)
        {
            for (int y = 0; y < bmp.Height; y++)
            {
                Color test = bmp.GetPixel(x, y);
                if (test.A == 255 && test.R <= 251 && test.G <= 240 && test.B <= 240)
                {
                    bmp.SetPixel(x, y, menuMouseOverColor);
                }
            }
        }
        fullPath = Path.Combine(this._DestinationThemePath, "Images\\ExpandArrows.gif");
        bmp.Save(fullPath);
        bmp.Dispose();
        fullPath = Path.Combine(this._DestinationThemePath, "Images\\ExpandArrows-default.png");
        File.Delete(fullPath);

    }

    private void GeneratePrinterFriendlyButton()
    {
        GenerateButton(153, 35, 0, 10, 153, 19, 3, "print-button-temp.png", "png");
        string printButtonTemp = Path.Combine(this._DestinationThemePath, "Images\\print-button-temp.png");
        string printButton = Path.Combine(this._DestinationThemePath, "Images\\print-button.png");
        string printButtonDefault = Path.Combine(this._DestinationThemePath, "Images\\print-button-default.jpg");
        if (File.Exists(printButtonTemp))
        {
            Bitmap bmpPrintButtonDefault = (Bitmap)Bitmap.FromFile(printButtonDefault);
            Bitmap bmpPrintButtonTemp = (Bitmap)Bitmap.FromFile(printButtonTemp);
            //                          
            for (int x = 0; x < 33; x++)
            {
                for (int y = 0; y < 35; y++)
                {
                    Color cLogo = bmpPrintButtonDefault.GetPixel(x, y);
                    bmpPrintButtonTemp.SetPixel(x, y, cLogo);
                }
            }
            Graphics graphicImage = Graphics.FromImage(bmpPrintButtonTemp);
            graphicImage.SmoothingMode = SmoothingMode.AntiAlias;
            Color textColor = System.Drawing.ColorTranslator.FromHtml(this._ThemeInfo.ButtonTextColor);
            SolidBrush brush = new SolidBrush(textColor);


            graphicImage.DrawString("Printer Friendly", new Font("Verdana", 8, FontStyle.Bold)
                , brush, new Point(42, 12));
            GraphicExtension.SaveToPngImage(printButton, bmpPrintButtonTemp, 100L);
            if (bmpPrintButtonDefault != null)
            {
                bmpPrintButtonDefault.Dispose();
                bmpPrintButtonDefault = null;
            }
            graphicImage.Dispose();
            File.Delete(printButtonTemp);
            File.Delete(printButtonDefault);
        }
    }

    private void ConvertOtherImages()
    {
        string fullPath = Path.Combine(this._DestinationThemePath, "Images\\SingleMinus.png");
        Color color = System.Drawing.ColorTranslator.FromHtml(this._ThemeInfo.PrimaryColor);
        Bitmap bmp = (Bitmap)Bitmap.FromFile(fullPath);
        for (int x = 0; x < bmp.Width; x++)
        {
            for (int y = 0; y < bmp.Height; y++)
            {
                Color test = bmp.GetPixel(x, y);
                if (test.A == 255 && test.R <= 0 && test.G <= 0 && test.B <= 0)
                {
                    bmp.SetPixel(x, y, color);
                }
            }
        }
        fullPath = Path.Combine(this._DestinationThemePath, "Images\\SingleMinus.gif");
        bmp.Save(fullPath);
        bmp.Dispose();
        fullPath = Path.Combine(this._DestinationThemePath, "Images\\SingleMinus.png");
        File.Delete(fullPath);

        fullPath = Path.Combine(this._DestinationThemePath, "Images\\SinglePlus.png");
        color = System.Drawing.ColorTranslator.FromHtml(this._ThemeInfo.PrimaryColor);
        bmp = (Bitmap)Bitmap.FromFile(fullPath);
        for (int x = 0; x < bmp.Width; x++)
        {
            for (int y = 0; y < bmp.Height; y++)
            {
                Color test = bmp.GetPixel(x, y);
                if (test.A == 255 && test.R <= 0 && test.G <= 0 && test.B <= 0)
                {
                    bmp.SetPixel(x, y, color);
                }
            }
        }
        fullPath = Path.Combine(this._DestinationThemePath, "Images\\SinglePlus.gif");
        bmp.Save(fullPath);
        bmp.Dispose();
        fullPath = Path.Combine(this._DestinationThemePath, "Images\\SinglePlus.png");
        File.Delete(fullPath);

    }
    private void DrawBackground(Graphics g, int width, int height, int CornerRadius, LinearGradientBrush lgb)
    {
        //int alpha = 204;
        Rectangle r = new Rectangle(0, 0, width, height);
        r.Width--; r.Height--;
        using (GraphicsPath rr = RoundRect(r, CornerRadius, CornerRadius, CornerRadius, CornerRadius))
        {
            g.FillPath(lgb, rr);                      
        }
    }
    private GraphicsPath RoundRect(RectangleF r, float r1, float r2, float r3, float r4)
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
    private void SetClip(Graphics g, int width, int height, int CornerRadius)
    {
        Rectangle r = new Rectangle(1, 1, width, height);
        r.X++; r.Y++; r.Width -= 3; r.Height -= 3;
        using (GraphicsPath rr = RoundRect(r, CornerRadius, CornerRadius, CornerRadius, CornerRadius))
        {
            g.SetClip(rr);
        }
    }

    protected void GenerateButtonFilter()
    {
        Color mainColor = ColorTranslator.FromHtml(this._ThemeInfo.PrimaryColor);
        Color subColor = ColorTranslator.FromHtml(this._ThemeInfo.SecondaryColor);
        VisionWebRectangleStyle slideStyle = new VisionWebRectangleStyle
        {
            Width = 154,
            Height = 41,
            MainColor = mainColor,
            SubColor = subColor,
            Radius = 15,
            HasTopLeft = false,
            HasTopRight = false
        };

        string fullPath = Path.Combine(this._DestinationThemePath, "Images\\btn-slide.png");
        GraphicExtension.GenerateGradientRect(slideStyle, fullPath, "png");
    }
    protected void GenerateFilterArrow()
    {
        Color mainColor = ColorTranslator.FromHtml(this._ThemeInfo.PrimaryTextColor);
        Bitmap arrow = new Bitmap(24, 70);
        arrow.DrawAngleTriangle(0, 5, 10, mainColor);
        arrow.DrawRectangle(0, 12, 10, 1, mainColor);

        arrow.DrawRectangle(0, 61, 10, 1, mainColor);
        arrow.DrawTriangle(0, 64, 10, mainColor);

        string fullPath = Path.Combine(this._DestinationThemePath, "Images\\white-arrow.png");
        GraphicExtension.SaveToPngImage(fullPath, arrow, 100L);
    }

    protected void GeneratePageHeader()
    {
        Color rightColor = ColorTranslator.FromHtml(this._ThemeInfo.PrimaryColor);
        Color leftColor = ColorTranslator.FromHtml(this._ThemeInfo.SecondaryColor);
        Bitmap pageHeader = new Bitmap(2500, 70);
        pageHeader.FillRectangle(0, -2, 960, 74, leftColor);
        pageHeader.FillRectangle(960, -2, 1540, 74, rightColor);  
        string fullPath = Path.Combine(this._DestinationThemePath, "Images\\shade.png");
        GraphicExtension.SaveToPngImage(fullPath, pageHeader, 100L);
    }
    protected void GenerateClientBox()
    {
        Color mainColor = ColorTranslator.FromHtml(this._ThemeInfo.PrimaryColor);
        Color subColor = ColorTranslator.FromHtml(this._ThemeInfo.SecondaryColor);
        string fullPath = string.Empty;
        VisionWebRectangleStyle clientStyle = new VisionWebRectangleStyle
        {
            Radius = 10,
            Width = 335, Height = 13,
            MainColor = mainColor, SubColor = subColor,
            HasTopRight = false, HasBottomRight = false,  HasTopLeft = true,  HasBottomLeft = false
        };
        fullPath = Path.Combine(this._DestinationThemePath, "Images\\box_grey_t.png");
        GraphicExtension.GenerateGradientRect(clientStyle, fullPath,"png");

        clientStyle.HasTopLeft = false;
        clientStyle.HasBottomLeft = true;
        fullPath = Path.Combine(this._DestinationThemePath, "Images\\box_grey_b.png");
        GraphicExtension.GenerateGradientRect(clientStyle, fullPath,"png");
    }
    protected void GenerateGridImages()
    {
        Color mainColor = ColorTranslator.FromHtml(this._ThemeInfo.PrimaryColor);        
        string fullPath = string.Empty;        

        Bitmap gridHeader = new Bitmap(70, 50);
        gridHeader.FillRectangle(-1, -1, 72, 52, mainColor);
        fullPath = Path.Combine(this._DestinationThemePath, "Images\\header-grid.jpg");
        GraphicExtension.SaveToJpgImage(fullPath, gridHeader, 100L);

        
        Bitmap gridFooterLeft = new Bitmap(28,28);
        gridFooterLeft.FillRectangle(-1, -1, 30, 30, mainColor);
        fullPath = Path.Combine(this._DestinationThemePath, "Images\\menutableft.jpg");
        GraphicExtension.SaveToJpgImage(fullPath, gridFooterLeft, 100L);

        Bitmap gridFooterRight = new Bitmap(28, 28);
        gridFooterRight.FillRectangle(-1, -1, 30, 30, mainColor);
        fullPath = Path.Combine(this._DestinationThemePath, "Images\\menutabright.jpg");
        GraphicExtension.SaveToJpgImage(fullPath, gridFooterRight, 100L);

    }
   

    protected void GenerateSprite()
    {
        string spritePath = Path.Combine(this._DestinationThemePath, "Images\\sprite.gif");       
        Image img = GraphicExtension.ReadFromFile(spritePath);
        Bitmap bitmap = new Bitmap(img);
        Graphics graphics = GraphicExtension.CreateGraphicNotClear(bitmap);
        SolidBrush pen = new SolidBrush(ColorTranslator.FromHtml(this._ThemeInfo.GridTablePrimaryTextColor));
        graphics.FillPolygon(pen, new Point[] { new Point(0, 1256), new Point(5, 1250), new Point(5, 1261) }, FillMode.Alternate);
        graphics.FillPolygon(pen, new Point[] { new Point(26, 1250), new Point(26, 1261), new Point(31, 1256) }, FillMode.Winding);
       
        GraphicExtension.SaveToPngImage(spritePath, bitmap, 100L);
    }

    protected void RemoveLeftRightPixel(int width, int height, int left, int right, string fullPath, string fullPathTemp, string imageType)
    {
        Bitmap bmpTemp = (Bitmap)Bitmap.FromFile(fullPathTemp);
        Bitmap bmp = new Bitmap(width, height);
        for (int x = 0 + left; x < bmpTemp.Width - right; x++)
        {
            for (int y = 0; y < bmpTemp.Height; y++)
            {
                Color color = bmpTemp.GetPixel(x, y);
                bmp.SetPixel(x - left, y, color);

            }
        }
        if (imageType == "png")
        {
            GraphicExtension.SaveToPngImage(fullPath, bmp, 100L);
        }
        else if (imageType == "jpg")
        {
            GraphicExtension.SaveToJpgImage(fullPath, bmp, 100L);
        }       
        bmpTemp.Dispose();
        File.Delete(fullPathTemp);
    }
}
