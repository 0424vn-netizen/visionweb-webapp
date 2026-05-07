using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;

public interface IThemeGenerator
{
    void Generate();
}

/// <summary>
/// Work flow
///     Step 1: Copy Template Theme folder (images, css) to App_Themes folder///             
///     Step 2: Modify BrandingColor.css 
///     Step 3: Generate images
/// </summary>
public class VisionWebThemeGenerator : IThemeGenerator
{
    /// <summary>
    /// Path of the theme
    /// </summary>
    private string _DestinationThemePath;
    private string _DestinationLoginPath;
    private string _BrandingCSS;
    private string _MenuBrandingCSS;
    private string _ComboboxBrandingCSS;
    private string _RadGridBrandingCSS;
    
    /// <summary>
    ///  Path of template theme folder
    /// </summary>
    private string _SourceThemePath;
    private string _SourceLoginPath;

    private VisionWebThemeInformation _ThemeInfo;

    private IImageGenerator _ImageGenerator;

    public VisionWebThemeGenerator(VisionWebThemeInformation themeInfo, string sourceThemePath, string sourceLoginPath, string destinationThemePath, string destinationLoginPath, string brandingCSS, string menuBrandingCSS,
        string comboboxBrandingCSS, string radGridBrandingCSS)
    {
        this._ThemeInfo = themeInfo;
        this._DestinationLoginPath = destinationLoginPath;
        this._DestinationThemePath = destinationThemePath;        
        this._SourceLoginPath = sourceLoginPath;
        this._SourceThemePath = sourceThemePath;
        this._ImageGenerator = new VisionWebImageGenerator(this._ThemeInfo, this._DestinationThemePath, this._DestinationLoginPath);
        this._BrandingCSS = brandingCSS;
        this._MenuBrandingCSS = menuBrandingCSS;
        this._ComboboxBrandingCSS = comboboxBrandingCSS;
        this._RadGridBrandingCSS = radGridBrandingCSS;
    }

    public void Generate()
    {
        this.MoveTemplateToAppThemes();
        this.ModifyTemplateCSS();
        this.GenerateImageByColor();
      
    }

    protected void MoveTemplateToAppThemes()
    {
        VisionWebIOExtensions.CopyDirectory(this._SourceThemePath, this._DestinationThemePath);
        VisionWebIOExtensions.CopyDirectory(this._SourceLoginPath, this._DestinationLoginPath);
    }
    protected void ModifyTemplateCSS()
    {
        string brandingCSSPath = Path.Combine(this._DestinationThemePath, _BrandingCSS);
        string menuBrandingCSSPath = Path.Combine(this._DestinationThemePath, _MenuBrandingCSS);
        string comboboxBrandingCSSPath = Path.Combine(this._DestinationThemePath, _ComboboxBrandingCSS);
        string radGridBrandingCSSPath = Path.Combine(this._DestinationThemePath, _RadGridBrandingCSS);
        StringBuilder cssContent = VisionWebIOExtensions.ReadTextFile(brandingCSSPath);
        cssContent.Replace("#[PRIMARY]", this._ThemeInfo.PrimaryColor);
        cssContent.Replace("#[FOOTER_SCROLL]", this._ThemeInfo.PanelFooterScrollColor);
        cssContent.Replace("#[GRID_TABLE_SUB_HEADER]", this._ThemeInfo.GridTableSubHeaderColor);
        cssContent.Replace("#[GRID_TABLE_SUB_HEADER_TEXT]", this._ThemeInfo.GridTableSubHeaderTextColor);
        cssContent.Replace("#[GRID_TABLE_BORDER]", this._ThemeInfo.GridTableBorderColor);
        cssContent.Replace("#[GRID_TABLE_ALT_ROW]", this._ThemeInfo.GridTableAltRowColor);
        cssContent.Replace("#[GRID_TABLE_ALT_ROW_TEXT]", this._ThemeInfo.GridTableAltRowTextColor);
        VisionWebIOExtensions.WriteTextFile(brandingCSSPath, cssContent);

        cssContent = VisionWebIOExtensions.ReadTextFile(menuBrandingCSSPath);
        cssContent.Replace("#[PRIMARY]", this._ThemeInfo.PrimaryColor);
        cssContent.Replace("#[PRIMARY_TEXT]", this._ThemeInfo.PrimaryTextColor);
        cssContent.Replace("#[MENU_HOVER]", this._ThemeInfo.MenuMouseHoverColor);       
        cssContent.Replace("#[MENU_HOVER_TEXT]", this._ThemeInfo.MenuMouseHoverTextColor);
        VisionWebIOExtensions.WriteTextFile(menuBrandingCSSPath, cssContent);

        cssContent = VisionWebIOExtensions.ReadTextFile(comboboxBrandingCSSPath);
        cssContent.Replace("#[PRIMARY]", this._ThemeInfo.PrimaryColor);
        VisionWebIOExtensions.WriteTextFile(comboboxBrandingCSSPath, cssContent);

        cssContent = VisionWebIOExtensions.ReadTextFile(radGridBrandingCSSPath);
        cssContent.Replace("#[GRID_TABLE_ALT_ROW]", this._ThemeInfo.GridTableAltRowColor);
        cssContent.Replace("#[GRID_TABLE_ALT_ROW_TEXT]", this._ThemeInfo.GridTableAltRowTextColor);
        cssContent.Replace("#[GRID_TABLE_PRIMARY]", this._ThemeInfo.GridTablePrimaryColor);
        cssContent.Replace("#[GRID_TABLE_PRIMARY_TEXT]", this._ThemeInfo.GridTablePrimaryTextColor);
        cssContent.Replace("#[GRID_TABLE_BORDER]", this._ThemeInfo.GridTableBorderColor);
        cssContent.Replace("#[GRID_REPORT_TOTAL]", this._ThemeInfo.GridReportTotalColor);
        cssContent.Replace("#[GRID_REPORT_TOTAL_TEXT]", this._ThemeInfo.GridReportTotalTextColor);       
        VisionWebIOExtensions.WriteTextFile(radGridBrandingCSSPath, cssContent);       

    }
    protected void GenerateImageByColor()
    {
        this._ImageGenerator.Generate();
    }
    
}
