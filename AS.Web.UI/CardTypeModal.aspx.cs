using AS.Common.DBManager;
using AS.Common.Formater;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CardTypeModal : NonReportPage
{
    private string _CardType = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        if (IsSecureQueryString)
        {
            if (SecureQueryString["cardtype"] != null)
            {
                _CardType = SecureQueryString["cardtype"];
            }
        }
        if (!IsPostBack)
        {
            BindData();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    private void BindData()
    {
        FilterParameterCollection _params = new FilterParameterCollection();
        _params.AddLoggedInUserParams(0);
        _params.Add(new FilterParameter("@CardType", _CardType, DbType.AnsiString));
        _params.AddLanguageID();
        DataTable data = WebServices.CsReportServices.GetReports("spa_ms_Reskin_GetDashboardCardVolumeDetail", _params);
        if (data.Rows.Count > 0)
        {
            rptCardVolume.DataSource = data;
            rptCardVolume.DataBind();
            uxExportCardVolume.Title = _CardType + " " + GetLocalResourceObject("CardTypeModal_aspx_cs_Cards").ToString();
        }
        else
        {
            uxExportCardVolume.Title = _CardType + " " + GetLocalResourceObject("CardTypeModal_aspx_cs_Cards").ToString();
            uxExportCardVolume.ShowExportIcon = false;
            plhNoRecords.Visible = true;
        }
    }

    protected void uxExportCardVolume_ExportExcel(object sender, string title, string subtitle)
    {

        this.IsNoCache = false;
        StringBuilder strExcelTemplate = new StringBuilder(File.ReadAllText(Server.MapPath("~/App_Data/tpl_CardTypeModal.htm")));

        string excelFileName = GeneralFuncsLib.GetFileName(string.Format(GetLocalResourceObject("CardTypeModal_aspx_cs_CardVolumeCardFileName").ToString(), _CardType));
        ASCIIEncoding encoding = new ASCIIEncoding();
        Response.ClearContent();
        try
        {
            Response.ClearHeaders();
        }
        catch (Exception ex)
        {
            AS.Common.Logger.LoggerManager.Error("ClearHeaders: CardTypeModal - uxExportCardVolume_ExportExcel:\n" + ex.ToString());
        }
        Response.ContentType = "application/vnd.ms-excel";
        Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + excelFileName + ".xls\"");
        StringWriter sw = new StringWriter();
        HtmlTextWriter writer = new HtmlTextWriter(sw);
        rptCardVolume.RenderControl(writer);
        strExcelTemplate.Replace("[tpl_CardTypeModal_htm_Cards]", Resources.Template.tpl_CardTypeModal_htm_Cards);
        strExcelTemplate.Replace("[tpl_CardTypeModal_htm_CardTypes]", Resources.Template.tpl_CardTypeModal_htm_CardTypes);
        strExcelTemplate.Replace("[tpl_CardTypeModal_htm_Previousday]", Resources.Template.tpl_CardTypeModal_htm_Previousday);
        strExcelTemplate.Replace("[tpl_CardTypeModal_htm_MTD]", Resources.Template.tpl_CardTypeModal_htm_MTD);
        strExcelTemplate.Replace("[tpl_CardTypeModal_htm_YTD]", Resources.Template.tpl_CardTypeModal_htm_YTD);
        strExcelTemplate.Replace("CARD_TYPE", _CardType);
        strExcelTemplate.Replace("CARDTYPEMODAL_CONTENT", sw.ToString());
        Response.Write(strExcelTemplate);
        sw.Close();
        writer.Close();
        Response.Flush();
        Response.End();
    }

    #region Format Currency
    protected string FormatCurrency(object data)
    {
        if (data == DBNull.Value)
            return string.Empty;
        else return FormatData.FormatCurrency(data, SessionManager.CurrencyFortmat);
    }
    protected string FormatCurrency(object data, int count)
    {
        if (data == DBNull.Value)
            return string.Empty;
        else return FormatData.FormatCurrency(data, count, SessionManager.CurrencyFortmat);
    }
    #endregion
}