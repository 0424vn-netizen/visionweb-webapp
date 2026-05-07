using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Controls.Pages;
using AS.Common.DBManager;
using System.Data;
using AS.Controls.Grid;
using Telerik.Web.UI;
using AS.Common;
using AS.Common.DataProtection;
using AS.Web.Business;


public partial class AuthorizationSearchDetail : ReportPage
{
    #region Enums
    enum DataBindAction
    {
        BindReportGrid,
    }
    enum PostBackAction
    {
        ViewCapture,
    }

    #endregion
    int _CurrentIndex = 0;
    string _AccountNumber = string.Empty;
    string _Last4Number = string.Empty;
    string _First6Number = string.Empty;
    string _BeginDate = string.Empty;
    string _EndDate = string.Empty;
    string _CardDescription = string.Empty;
    const string CAPTURE_DETAIL = "<a href=\"#\" style=\"cursor:pointer\" onclick=\"return ShowPopupModal(1, '{0}','max');\"><img src='res/images/view.png' border=\"0\"/></a>";
    protected override void PageInitialize()
    {
        this.ExporterIDs.Add("uxExporterBottom");
        base.PageInitialize();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
        IsBindDataOnLoad = true;
        if (SecureQueryString != null)
        {
            ProcessQueryString();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        uxReportGrid.Columns.FindByUniqueName("Capture").Visible = false;

        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(GetLocalResourceObject("Text_CardNumber").ToString() + ": " + _First6Number + GetLocalResourceObject("Text_xxxxxx").ToString() + _Last4Number + " " + GetLocalResourceObject("Text_CardType").ToString() + ": " + _CardDescription);
        exportConfig.ReportHeader = GetLocalResourceObject("Text_CardNumber").ToString() + ": " + _First6Number + GetLocalResourceObject("Text_xxxxxx").ToString() + _Last4Number + " " + GetLocalResourceObject("Text_CardType").ToString() + ": " + _CardDescription;
    }

    protected override void DoGridNeedDataSource(ASGrid sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindReportGrid, sender);
    }

    protected override void DoItemDataBound(ASGrid sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            string accountNumber = string.Empty;
            string captureUrl = "CaptureScreenModal.aspx?{0}";
            if (IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC))
            {
                string key = Cryptophy.MD5(Session.SessionID + Request.UserHostAddress).Substring(0, 8);
                accountNumber = Cryptophy.EncryptText(_AccountNumber, key);
            }
            else
            {
                accountNumber = _AccountNumber;
            }
            captureUrl = string.Format(captureUrl, this.BuildSecureQueryString("AccountNumber=" + HttpUtility.UrlEncode(accountNumber) + "&BeginDate=" + _BeginDate + "&EndDate=" + _EndDate + "&RecordID=" + dataRow["RecordID"].ToString() + "&idx=" + _CurrentIndex + 1));
            dataItem["Capture"].Text = VeraCodeSolution.DoVeraCode("<a href=\"#\" style=\"cursor:pointer\" onclick=\"return parent.ShowPopupModalChild(" + _CurrentIndex + 1 + ", '" + captureUrl + "','max');return false;\"><img src='res/images/view.png' border=\"0\"/></a>");

            //dataItem["TransType"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["TransactionDescription"].ToString());
            dataItem["AVS"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["AVSDescription"].ToString());
        }
    }


    private void ProcessQueryString()
    {
        if (this.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC))
        {
            string key = Cryptophy.MD5(Session.SessionID + Request.UserHostAddress).Substring(0, 8);
            _AccountNumber = Cryptophy.DecryptText(SecureQueryString["AccountNumber"], key);
        }
        else
        {
            _AccountNumber = SecureQueryString["AccountNumber"];
        }
        //if (_AccountNumber.Contains("xxx"))
        //{
        _First6Number = _AccountNumber.Substring(0, 6);
        _Last4Number = _AccountNumber.Substring(_AccountNumber.Length - 4, 4);
        //}

        _CardDescription = SecureQueryString["CardDescription"];
        _BeginDate = SecureQueryString["BeginDate"];
        _EndDate = SecureQueryString["EndDate"];
    }


    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:
                {
                    uxExporter.GridHeader = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("Text_CardNumber").ToString() + ": " + _AccountNumber + " " + GetLocalResourceObject("Text_CardType").ToString() + ": " + _CardDescription);
                    FilterParameterCollection _params = new FilterParameterCollection();
                    _params.AddLoggedInUserParams(0);
                    if (this.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC))
                    {
                        _params.AddEncryptedInputParams("FullCardNumber");
                        _params.Add(new FilterParameter("@FullCardNumber", _AccountNumber, DbType.AnsiString));
                    }
                    else
                    {
                        _params.Add(new FilterParameter("@First6CardNumber", _First6Number, DbType.AnsiString));
                        _params.Add(new FilterParameter("@Last4CardNumber", _Last4Number, DbType.AnsiString));
                    }
                    _params.AddLanguageID();
                    string spaName = "spa_cs_GetAuthorizationSearchDetail";
                    ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(_params) });
                }
                break;
        }
    }
}

