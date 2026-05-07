using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Controls.Exporter;
using Telerik.Web.UI;

public partial class UserControls_UxExportTable : System.Web.UI.UserControl
{
    public event ExcelClick ExportExcel;
    public delegate void ExcelClick(object sender, string title, string subtitle);
    public event CSVClick ExportCSV;
    public delegate void CSVClick(object sender, string title, string subtitle);
    public event PdfClick ExportPdf;
    public delegate void PdfClick(object sender, string title, string subtitle);

    public string GridID { get; set; }
    
    /// <summary>
    /// Show button excel or not.
    /// </summary>
    public bool ShowExcel 
    {
        get 
        {
            return this.ViewState["ShowExcel"] != null ?
                (bool)this.ViewState["ShowExcel"] : true;
        }
        set 
        {
            this.ViewState["ShowExcel"] = value;
        }
    }
    /// <summary>
    /// Show button CSV or not.
    /// </summary>
    public bool ShowCSV
    {
        get
        {
            return this.ViewState["ShowCSV"] != null ?
                (bool)this.ViewState["ShowCSV"] : true;
        }
        set
        {
            this.ViewState["ShowCSV"] = value;
        }
    }
    /// <summary>
    /// Show button Word or not.
    /// </summary>
    public bool ShowWord
    {
        get
        {
            return this.ViewState["ShowWord"] != null ?
                (bool)this.ViewState["ShowWord"] : false; 
        }
        set
        {
            this.ViewState["ShowWord"] = value;
        }
    }
    /// <summary>
    /// Show button PDF or not.
    /// </summary>
    public bool ShowPDF
    {
        get
        {
            return this.ViewState["ShowPDF"] != null ?
                (bool)this.ViewState["ShowPDF"] : false;
        }
        set
        {
            this.ViewState["ShowPDF"] = value;
        }
    }

    /// <summary>
    /// Show button PDF or not.
    /// </summary>
    public bool ShowExportIcon
    {
        get
        {
            return this.ViewState["ShowExportIcon"] != null ?
                (bool)this.ViewState["ShowExportIcon"] : true;
        }
        set
        {
            this.ViewState["ShowExportIcon"] = value;
        }
    }


    /// <summary>
    /// Show button PDF or not.
    /// </summary>
    public bool IsOnTop
    {
        get
        {
            return this.ViewState["IsOnTop"] != null ?
                (bool)this.ViewState["IsOnTop"] : false;
        }
        set
        {
            this.ViewState["IsOnTop"] = value;
        }
    }



    /// <summary>
    /// Title
    /// </summary>
    public string Title
    {
        get
        {
            return this.ViewState["Title"] != null ?
                this.ViewState["Title"].ToString() : string.Empty;
        }
        set
        {
            this.ViewState["Title"] = value;
        }
    }

    /// <summary>
    /// Title
    /// </summary>
    public string SubTitle
    {
        get
        {
            return this.ViewState["SubTitle"] != null ?
                this.ViewState["SubTitle"].ToString() : string.Empty;
        }
        set
        {
            this.ViewState["SubTitle"] = value;
        }
    }

    protected override void OnLoad(EventArgs e)
    {
       
        //invisible buttons
        this.imgExcel.Visible = this.ShowExcel;
        this.imgCSV.Visible = this.ShowCSV;
        this.imgWord.Visible = this.ShowWord;
        this.imgPDF.Visible = this.ShowPDF;
        this.divExport.Visible = this.ShowExportIcon;

        if (IsOnTop)
        {
            h2Title.Attributes.Add("class", h2Title.Attributes["class"] + " on-top");
            divExport.Attributes.Add("class", divExport.Attributes["class"] + " on-top");
        }

        this.litTitle.Text = this.Title;
        this.litSubTitle.Text = this.SubTitle;

      
        
    }
    

    private ExportButtonType GetExpButtonType(LinkButton imgBtn)
    {
        if (imgBtn.ID == "imgExcel")
            return ExportButtonType.EXCEL;
        if (imgBtn.ID == "imgWord")
            return ExportButtonType.WORD;
        if (imgBtn.ID == "imgCSV")
            return ExportButtonType.CSV;
        return ExportButtonType.PDF;
    }

    private string GetDefaultFileName(LinkButton imgBtn)
    {
        if (imgBtn.ID == "imgExcel")
            return "ExportExcel";
        else if (imgBtn.ID == "imgWord")
            return "ExportWord";
        else if (imgBtn.ID == "imgCSV")
            return "ExportCSV";
        else
            return "ExportPDF";
    }

        
    
    protected void ExportButton_Click(object sender, EventArgs e)
    {
        switch (GetExpButtonType((LinkButton)sender))
        {
            case ExportButtonType.EXCEL:
                if (ExportExcel != null)
                {
                    ExportExcel(this, Title, SubTitle);
                }
                break;
            case ExportButtonType.CSV:
                if (ExportCSV != null)
                {
                    ExportCSV(this, Title, SubTitle);
                }
                break;
            case ExportButtonType.PDF:
                if (ExportPdf != null)
                {
                    ExportPdf(this, Title, SubTitle);
                }
                break;
        }
               
    }
}

public enum ExportButtonType
{ 
    EXCEL,
    CSV,
    WORD,
    PDF
}
