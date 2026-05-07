using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Controls.Pages;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Web.Business;
using AS.Common.DataProtection;
using System.Data;
using AS.Common;
using Telerik.Web.UI;


public partial class CaptureScreenModal : ReportPage
{
    #region Enums
    enum DataBindAction
    {
        BindReportGrid,
    }

    #endregion

    string _AccountNumber = string.Empty;
    string _First6Number = string.Empty;
    string _Last4Number = string.Empty;
    string _RecordID = string.Empty;

    int _CurrentIndex = 0;

    const string CAPTURE_DETAIL_MODAL = "<a href=\"#\" style=\"cursor:pointer\" onclick=\"return parent.ShowPopupModalChild({0}, '{1}','auto');\"><img src='res/images/view.png' border=\"0\"/></a>";
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
            if (SecureQueryString["idx"] != null)
                int.TryParse(SecureQueryString["idx"], out _CurrentIndex);
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
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

            string captureUrl = "CaptureScreenModalDetail.aspx?{0}";
            captureUrl = string.Format(captureUrl, this.BuildSecureQueryString("RecordID=" + dataRow["RecordID"].ToString()));
            dataItem["CaptureDetail"].Text = VeraCodeSolution.DoVeraCode(string.Format(CAPTURE_DETAIL_MODAL, _CurrentIndex + 1, captureUrl));

            dataItem["MOTO"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["MOTODescription"].ToString());
            dataItem["CardType"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["CardTypeDescription"].ToString());
            //dataItem["TransType"].ToolTip = VeraCodeSolution.DoVeraCode(dataRow["TransactionDescription"].ToString());
        }
    }

    bool _isExporting = false;
    protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        _isExporting = true;
        uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
        uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
        uxReportGrid.Columns.FindByUniqueName("CaptureDetail").Visible = false;
        base.DoNeedExportConfig(sender, exportConfig);
        exportConfig.ReportHeader = GetLocalResourceObject("CaptureScreenModal_aspx_cs_CardNumber").ToString() + " " + _First6Number + GetLocalResourceObject("CaptureScreenModal_aspx_cs_xxxxxx").ToString() + _Last4Number;
        exportConfig.FileName = GeneralFuncsLib.FormatFileName(GetLocalResourceObject("CaptureScreenModal_aspx_cs_CardNumber").ToString() + " " + _First6Number + GetLocalResourceObject("CaptureScreenModal_aspx_cs_xxxxxx").ToString() + _Last4Number);
    }

    protected override void DoSwitchView()
    {
        if (this.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC))
        {
            uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = true;
            uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = false;
        }
        else
        {
            uxReportGrid.Columns.FindByUniqueName("AccountNumber").Visible = false;
            uxReportGrid.Columns.FindByUniqueName("PartialCardNumber").Visible = true;
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:
                {
                    uxExporter.GridHeader = VeraCodeSolution.ValidateResponseData(GetLocalResourceObject("CaptureScreenModal_aspx_cs_CardNumber").ToString() + " " + _AccountNumber);
                    FilterParameterCollection _params = new FilterParameterCollection();
                    _params.AddLoggedInUserParams(0);
                    if (this.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC))
                    {
                        _params.AddDecryptDataParams("AccountNumber", _isExporting);
                        _params.AddEncryptedInputParams("FullCardNumber");
                        _params.Add(new FilterParameter("@FullCardNumber", _AccountNumber, DbType.AnsiString));
                    }
                    else
                    {
                        _params.Add(new FilterParameter("@First6CardNumber", _First6Number, DbType.AnsiString));
                        _params.Add(new FilterParameter("@Last4CardNumber", _Last4Number, DbType.AnsiString));
                    }

                    int recordID = 0;
                    int.TryParse(_RecordID, out recordID);
                    _params.Add(new FilterParameter("@RecordID", recordID, DbType.Int32));
                    _params.AddLanguageID();
                    string spaName = "spa_cs_GetCaptureSearch";
                    ((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(_params) });

                }
                break;
        }
    }

    private void ProcessQueryString()
    {
        _RecordID = SecureQueryString["RecordID"];
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
    }

}
