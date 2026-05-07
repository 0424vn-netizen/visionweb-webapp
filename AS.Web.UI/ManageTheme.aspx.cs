using System;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using Telerik.Web.UI;
using Telerik.Web.UI.Upload;
using AS.Controls.Validators;
using System.Linq;
using AS.Controls.Pages;

[PagePermission("ASManageTheme")]
public partial class ManageTheme : ReportPage
{
    #region constant
    protected const string THEME_TEMPLATE_PATH = "~/App_Themes/Template/Theme";
    protected const string LOGIN_TEMPLATE_PATH = "~/App_Themes/Template/Login";
    protected const string THEME_LIST_PATH = "~/App_Data/ThemeInformation.xml";
    protected const string THEME_PATH = "~/App_Themes/{0}";
    protected const string LOGIN_PATH = "~/res/login/{0}";
    protected const string THEME_NAME_DEFAULT_FOR_CREATE = "{0}_DefaultForCreate";
    
    /// <summary>
    /// Max image size in KB
    /// </summary>
    protected const int MAX_IMAGE_SIZE = 500;
    #endregion

    protected enum PostBackAction 
    {
        Submit,        
        ChangeMode,
        ChangeThemeName

    }
    protected enum DataBindAction
    {        
        BindValidateItem
    }
   

    protected bool IsUpdateMode
    {
        get 
        {
            return this.uxRadioEdit.Checked;
        }
    }
    protected VisionWebThemeInformationCollection ThemeList
    {
        get
        {

            string xmlFilePath = Server.MapPath(THEME_LIST_PATH);
            string xmlThemes = VisionWebIOExtensions.ReadTextFile(xmlFilePath).ToString();
            VisionWebThemeInformationCollection themeList = null;
            if (string.IsNullOrEmpty(xmlThemes))
            {
                themeList = new VisionWebThemeInformationCollection();
            }
            else
            {
                themeList = (VisionWebThemeInformationCollection)GeneralFuncsLib.ConvertXMLToObject(xmlThemes, typeof(VisionWebThemeInformationCollection));
            }
            return themeList;
        }
    }
    protected VisionWebThemeInformationCollection CurrentThemeList
    {
        get
        {
            string xmlFilePath = Server.MapPath(THEME_LIST_PATH);
            string xmlThemes = VisionWebIOExtensions.ReadTextFile(xmlFilePath).ToString();
            VisionWebThemeInformationCollection currentThemeList = new VisionWebThemeInformationCollection(); ;
            VisionWebThemeInformationCollection themeList = new VisionWebThemeInformationCollection();
            if (!string.IsNullOrEmpty(xmlThemes))          
            {
                themeList = (VisionWebThemeInformationCollection)GeneralFuncsLib.ConvertXMLToObject(xmlThemes, typeof(VisionWebThemeInformationCollection));
            }

            foreach (VisionWebThemeInformation t in themeList)
            {
                if (string.IsNullOrEmpty(t.CreatedClientID) || int.Parse(t.CreatedClientID) == SessionManager.CurrentUser.ASClient)
                {
                    currentThemeList.Add(t);
                }
            }
            if (currentThemeList != null)
            {
                currentThemeList.Sort();
            }
           
            return currentThemeList;
        }
    }
    protected VisionWebThemeInformation CurrentEditTheme
    {
        get 
        {
            VisionWebThemeInformation theme = new VisionWebThemeInformation();
            if (this.IsUpdateMode)
            {
                theme.ThemeName = this.uxThemeList.SelectedValue.Trim();
            }
            else
            {
                theme.ThemeName = this.uxThemeName.Text.Trim();
            }

            theme.PrimaryColor = this.uxPrimaryColor.SelectedColor.ToHtml();
            theme.PrimaryTextColor = this.uxPrimaryTextColor.SelectedColor.ToHtml();
            theme.SecondaryColor = this.uxSecondaryColor.SelectedColor.ToHtml();
            theme.MenuMouseHoverColor = this.uxMenuMouseHoverColor.SelectedColor.ToHtml();
            theme.MenuMouseHoverSubColor = this.uxMenuMouseHoverSubColor.SelectedColor.ToHtml();
            theme.MenuMouseHoverTextColor = this.uxMenuMouseHoverTextColor.SelectedColor.ToHtml();
            theme.GridTablePrimaryColor = this.uxGridTablePrimaryColor.SelectedColor.ToHtml();
            theme.GridTablePrimaryTextColor = this.uxGridTablePrimaryTextColor.SelectedColor.ToHtml();
            theme.GridTableBorderColor = this.uxGridTableBorderColor.SelectedColor.ToHtml();
            theme.GridTableAltRowColor = this.uxGridTableAltRowColor.SelectedColor.ToHtml();
            theme.GridTableAltRowTextColor = this.uxGridTableAltRowTextColor.SelectedColor.ToHtml();
            theme.GridTableSubHeaderColor = this.uxGridTableSubHeaderColor.SelectedColor.ToHtml();
            theme.GridTableSubHeaderTextColor = this.uxGridTableSubHeaderTextColor.SelectedColor.ToHtml();
            theme.GridReportTotalColor = this.uxGridReportTotalColor.SelectedColor.ToHtml();
            theme.GridReportTotalTextColor = this.uxGridReportTotalTextColor.SelectedColor.ToHtml();
            theme.ButtonTextColor = this.uxButtonTextColor.SelectedColor.ToHtml();
            theme.ButtonPrimaryColor = this.uxButtonPrimaryColor.SelectedColor.ToHtml();
            theme.ButtonSubColor = this.uxButtonSubColor.SelectedColor.ToHtml();
            theme.PanelFooterScrollColor = this.uxPanelFooterScrollColor.SelectedColor.ToHtml();            
            
            theme.CreatedUser = SessionManager.CurrentUser.UserID;
            theme.CreatedClientID = SessionManager.CurrentUser.ASClient.ToString();
            theme.CreatedDate = DateTime.Now;
            
            return theme;
        }
        set 
        {
            VisionWebThemeInformation theme = value;

            this.uxPrimaryColor.SelectedColor = ColorTranslator.FromHtml(theme.PrimaryColor);
            this.uxPrimaryTextColor.SelectedColor = ColorTranslator.FromHtml(theme.PrimaryTextColor);
            this.uxSecondaryColor.SelectedColor = ColorTranslator.FromHtml(theme.SecondaryColor);
            this.uxMenuMouseHoverColor.SelectedColor = ColorTranslator.FromHtml(theme.MenuMouseHoverColor);
            this.uxMenuMouseHoverSubColor.SelectedColor = ColorTranslator.FromHtml(theme.MenuMouseHoverSubColor);
            this.uxMenuMouseHoverTextColor.SelectedColor = ColorTranslator.FromHtml(theme.MenuMouseHoverTextColor);
            this.uxGridTablePrimaryColor.SelectedColor = ColorTranslator.FromHtml(theme.GridTablePrimaryColor);
            this.uxGridTablePrimaryTextColor.SelectedColor = ColorTranslator.FromHtml(theme.GridTablePrimaryTextColor);
            this.uxGridTableBorderColor.SelectedColor = ColorTranslator.FromHtml(theme.GridTableBorderColor);
            this.uxGridTableAltRowColor.SelectedColor = ColorTranslator.FromHtml(theme.GridTableAltRowColor);
            this.uxGridTableAltRowTextColor.SelectedColor = ColorTranslator.FromHtml(theme.GridTableAltRowTextColor);
            this.uxGridTableSubHeaderColor.SelectedColor = ColorTranslator.FromHtml(theme.GridTableSubHeaderColor);
            this.uxGridTableSubHeaderTextColor.SelectedColor = ColorTranslator.FromHtml(theme.GridTableSubHeaderTextColor);
            this.uxGridReportTotalColor.SelectedColor = ColorTranslator.FromHtml(theme.GridReportTotalColor);
            this.uxGridReportTotalTextColor.SelectedColor = ColorTranslator.FromHtml(theme.GridReportTotalTextColor);
            this.uxButtonPrimaryColor.SelectedColor = ColorTranslator.FromHtml(theme.ButtonPrimaryColor);
            this.uxButtonSubColor.SelectedColor = ColorTranslator.FromHtml(theme.ButtonSubColor);
            this.uxButtonTextColor.SelectedColor = ColorTranslator.FromHtml(theme.ButtonTextColor);
            this.uxPanelFooterScrollColor.SelectedColor = ColorTranslator.FromHtml(theme.PanelFooterScrollColor);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {        
            this.IgnoreWritingAspxLogging = true;
        }
        OnDataBindControls(DataBindAction.BindValidateItem, sender);
       
    }   

    protected override void OnPostBackActions(Enum type, object sender)
    {
        base.OnPostBackActions(type, sender);
        switch ((PostBackAction)type)
        {
            case PostBackAction.Submit:
                bool isValid = this.Validate();
                if (!isValid) return;

                VisionWebThemeInformation theme = this.CurrentEditTheme;
                if (this.uxLogoImage.UploadedFiles.Count > 0)
                {
                    theme.Logo = Image.FromStream(this.uxLogoImage.UploadedFiles[0].InputStream);
                }
                else if (this.IsUpdateMode)
                {
                    string logoPath = Server.MapPath(string.Format(THEME_PATH, theme.ThemeName + "/Images/logo.png"));
                    theme.Logo = GraphicExtension.ReadFromFile(logoPath);
                }

                if (this.uxStatementLogoImage.UploadedFiles.Count > 0)
                {
                    theme.StatementLogo = Image.FromStream(this.uxStatementLogoImage.UploadedFiles[0].InputStream);
                }
                else if (this.IsUpdateMode)
                {
                    string logoPath = Server.MapPath(string.Format(THEME_PATH, theme.ThemeName + "/Images/statement_logo.jpg"));
                    theme.StatementLogo = GraphicExtension.ReadFromFile(logoPath);
                }
                

                theme.IsUpdateMode = this.IsUpdateMode;

                if (this.uxChkGenerate.Checked)
                    this.GenerateTheme(theme);

                if (this.uxChkSaveTheme.Checked)
                    this.SaveThemeInfomation(theme);
               
                if (!this.IsUpdateMode)
                {
                    this.uxThemeName.Text = string.Empty;                   
                    this.ShowMessage("Theme created successfully.");
                }
                else 
                {
                    this.ShowMessage("Theme updated successfully.");
                }
                break;
            case PostBackAction.ChangeMode:
                if (this.IsUpdateMode)
                {
                    VisionWebThemeInformationCollection themeList = this.CurrentThemeList;
                    this.uxThemeList.Visible = true;
                    this.uxThemeName.Visible = false;
                    this.uxThemeList.DataSource = themeList;
                    this.uxThemeList.DataTextField = "ThemeName";
                    this.uxThemeList.DataValueField = "ThemeName";
                    this.uxThemeList.DataBind();
                    RadComboBoxItem currentTheme = this.uxThemeList.FindItemByValue(this.Page.Theme);
                    if (currentTheme != null)
                    {
                        this.uxThemeList.SelectedValue = currentTheme.Value;
                    }
                    else
                    {
                        this.uxThemeList.SelectedIndex = 0;
                    }
                    if (themeList.Count > 0)
                    {
                        this.CurrentEditTheme = this.FindThemeByName(this.uxThemeList.SelectedValue);
                    }                   
                }
                else
                {
                    Response.Redirect("ManageTheme.aspx");
                }
                this.SetPanelOption();
                break;
            case PostBackAction.ChangeThemeName:
                this.SetPanelOption();
                this.CurrentEditTheme = this.FindThemeByName(this.uxThemeList.SelectedValue);
                break;
        }
    }  
    protected override void OnDataBindControls(Enum type, object sender)
    {
        base.OnDataBindControls(type, sender);
        switch ((DataBindAction)type)
        {            
            case DataBindAction.BindValidateItem:
                if (!this.IsUpdateMode)
                {
                    BasicValidationItem item = new BasicValidationItem
                    {
                        Rule = ValidationRule.Required,
                        Message = "Logo Image: This is a required field.",
                        ControlToValidateID = this.uxLogoImage.ID
                    };
                    this.uxValdator.Items.Add(item);
                    item = new BasicValidationItem
                    {
                        Rule = ValidationRule.Required,
                        Message = "Statement Logo Image: This is a required field.",
                        ControlToValidateID = this.uxStatementLogoImage.ID
                    };
                    this.uxValdator.Items.Add(item);
                }
                break;
        }

    }   

    protected void uxSubmit_Click(object sender, EventArgs e)
    {
        this.OnPostBackActions(PostBackAction.Submit, sender);
    }
    protected void ChangeFormMode(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.ChangeMode, sender);
    }
    protected void uxThemeList_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        this.OnPostBackActions(PostBackAction.ChangeThemeName, sender);
    }
   
    protected void SaveThemeInfomation(VisionWebThemeInformation theme)
    {
        string xmlFilePath = Server.MapPath(THEME_LIST_PATH);
        VisionWebThemeInformationCollection themeList = this.ThemeList;
        themeList.AddOrUpdate(theme);
        string xmlThemes = GeneralFuncsLib.ConvertObjectToXML(themeList, typeof(VisionWebThemeInformationCollection));
        VisionWebIOExtensions.WriteTextFile(xmlFilePath, new StringBuilder(xmlThemes));
    }    
    
    protected void SetPanelOption()
    {

        this.uxPanelOption.Visible = true;
        this.uxChkGenerate.Checked = true;
        this.uxChkSaveTheme.Checked = true;

    }
    protected VisionWebThemeInformation FindThemeByName(string themeName)
    {
        VisionWebThemeInformationCollection themeList = this.ThemeList;
        foreach (VisionWebThemeInformation theme in themeList)
        {
            if (theme.ThemeName.Equals(themeName, StringComparison.OrdinalIgnoreCase))
            {
                return theme;
            }
        }
        return null;
    }

    protected void GenerateTheme(VisionWebThemeInformation theme)
    {
        string sourceThemePath = Server.MapPath(THEME_TEMPLATE_PATH);
        string sourceLoginPath = Server.MapPath(LOGIN_TEMPLATE_PATH);
        string destThemePath = Server.MapPath(string.Format(THEME_PATH, theme.ThemeName));
        string destLoginPath = Server.MapPath(string.Format(LOGIN_PATH, theme.ThemeName));
        string brandingCSS = Server.MapPath(string.Format(THEME_PATH, theme.ThemeName + "\\branding.css"));
        string menuBrandingCSS = Server.MapPath(string.Format(THEME_PATH, theme.ThemeName + "\\menubranding.css"));
        string comboboxBrandingCSS = Server.MapPath(string.Format(THEME_PATH, theme.ThemeName + "\\radcomboboxbranding.css"));
        string radGridBrandingCSS = Server.MapPath(string.Format(THEME_PATH, theme.ThemeName + "\\radgridbranding.css"));

        IThemeGenerator generator = new VisionWebThemeGenerator(theme, sourceThemePath, sourceLoginPath, destThemePath, destLoginPath, brandingCSS, menuBrandingCSS, comboboxBrandingCSS, radGridBrandingCSS);
        generator.Generate();
    }

    protected bool Validate()
    {
        if (!ValidateThemeName(this.uxThemeName.Text.Trim()))
            return false;
        
        // Validate Logo Image
        if (this.uxLogoImage.UploadedFiles.Count <= 0 && !this.IsUpdateMode)
        {
            this.ShowMessage("Logo Image: This is a required field.");
            return false;
        }
        else if (this.uxStatementLogoImage.UploadedFiles.Count <= 0 && !this.IsUpdateMode)
        {
            this.ShowMessage("Statement Logo Image: This is a required field.");
            return false;
        }        
        else if (this.uxLogoImage.UploadedFiles.Count > 0)
        {
            if (!ValidateUploadFile(this.uxLogoImage, 300, 70, "Logo Image", ".png"))
                return false;
        }
        else if (this.uxStatementLogoImage.UploadedFiles.Count > 0)
        {
            if (!ValidateUploadFile(this.uxStatementLogoImage, 169, 41, "Statement Logo Image", ".png,.jpg,.bmp"))
                return false;
        } 
     
        return true;
    }
    protected bool ValidateUploadFile(RadUpload radUpload, int width, int height, string label, string extension)
    {      
        UploadedFile upFile = radUpload.UploadedFiles[0];
        int maxImageSizeInByte = MAX_IMAGE_SIZE * 1024;

        if (!extension.Contains(upFile.GetExtension().ToLower()))
        {
            this.ShowMessage(string.Format("{0}: The uploaded image must be {1} file.", label, extension));
            return false;
        }

        if (upFile.ContentLength > maxImageSizeInByte)
        {
            this.ShowMessage(string.Format("{0}: File exceeds {1} KB.", label, MAX_IMAGE_SIZE));
            return false;
        }
        else
        {
            Image image = Image.FromStream(upFile.InputStream);
            if (image.Width > width || image.Height > height)
            {
                this.ShowMessage(string.Format("{0}: The uploaded image must be {1} x {2} pixels (width by height).", label, width, height));
                return false;
            }
        }
        return true;

    }   
    protected bool ValidateThemeName(string themeName)
    {
        if (!this.IsUpdateMode)
        {
            string appThemeDir = Server.MapPath(string.Format(THEME_PATH, string.Empty));
            themeName = Server.MapPath(string.Format(THEME_PATH, themeName));
            string[] themes = Directory.GetDirectories(appThemeDir);
            foreach (string folder in themes)
            {
                if (folder.Equals(themeName, StringComparison.OrdinalIgnoreCase))
                {
                    this.ShowMessage("The theme name already exists.");
                    return false;
                }
            }
        }        
        return true;
    }
    protected void ShowMessage(string message)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "myScript", string.Format("DoAlert('{0}');", message), true);
    }

    
}

