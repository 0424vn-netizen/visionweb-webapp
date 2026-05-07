using System.Drawing;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Collections.Generic;

[Serializable]
public class VisionWebThemeInformation
{
    public string ThemeName { set; get; }

    [System.Xml.Serialization.XmlIgnore]
    public Image Logo { set; get; }
    [System.Xml.Serialization.XmlIgnore]
    public Image StatementLogo { set; get; }

    public string PrimaryColor { set; get; }
    public string PrimaryTextColor { set; get; }
    public string SecondaryColor { set; get; }
    public string MenuMouseHoverColor { set; get; }
    public string MenuMouseHoverSubColor { set; get; }
    public string MenuMouseHoverTextColor { set; get; }
    
    public string GridTablePrimaryColor { set; get; }
    public string GridTablePrimaryTextColor { set; get; }
    public string GridTableBorderColor { set; get; }
    public string GridTableAltRowColor { set; get; }
    public string GridTableAltRowTextColor { set; get; }
    public string GridTableSubHeaderColor { set; get; }
    public string GridTableSubHeaderTextColor { set; get; }
    public string GridReportTotalColor { set; get; }
    public string GridReportTotalTextColor { set; get; }
    public string PanelFooterScrollColor { set; get; }

    public string ButtonTextColor { set; get; }
    public string ButtonPrimaryColor { set; get; }
    public string ButtonSubColor { set; get; }

    [System.Xml.Serialization.XmlIgnore]
    public bool IsUpdateMode { set; get; }

    public DateTime CreatedDate { set; get; }
    public string CreatedUser { set; get; }
    public string CreatedClientID { set; get; }
}
public class VisionWebThemeInformationCollection : Collection<VisionWebThemeInformation>
{
    /// <summary>
    /// If not exists then insert else update
    /// </summary>
    /// <param name="theme"></param>
    public void AddOrUpdate(VisionWebThemeInformation theme)
    {
        foreach (VisionWebThemeInformation t in base.Items)
        {
            if (t.ThemeName.Equals(theme.ThemeName, StringComparison.OrdinalIgnoreCase))
            {
                base.Items.Remove(t);
                break;
            }
        }
        base.Items.Add(theme);
    }
    public void Sort()
    {
        var ordered = this.Items.OrderBy(t => t.ThemeName).ToList();
        this.Items.Clear();

        foreach (var t in ordered)
        {
            this.Items.Add(t);
        }
       
    }
    
}