using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Web.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControls_NotInMifDetail_BATCH : GlobalUserControl
{
    enum DataBindAction
    {
        BindReportGrid,
    }

    private const string PARTIAL_CARD_NUMBER = "PartialCardNumber";
    private const string CARD_NUMBER = "CardNumber";
    private const string MERCHANT_NUMBER = "MerchantNumber";
    private const string AUTH_NUMBER = "AuthNumber";
    private const string RECORD_ID = "RecordID";
    private const string ISSUES_BANK = "IssueBank";
    private const string KEYED = "Keyed";
    private const string CARD_TYPE = "CardType";
    private const string CARD_TYPE_DESC = "CardTypeDescription";
    private const string KEYED_DESCRIPTION = "KeyedDescription";
    private const string TRANS_CODE = "TransCode";
    private const string TRANS_CODE_DESC = "TransCodeDescription";
    private int _curIdx = 0;
    private bool _isExporting = false;

    #region Properties
    public DateTime ReportDate
    {
        get
        {
            DateTime dt = DateTime.Now;
            if (Page.IsSecureQueryString)
            {
                DateTime.TryParse(Page.SecureQueryString["reportDate"].ToString(), out dt);
            }
            return dt;
        }
    }

    public string FileSource
    {
        get
        {
            if (Page.IsSecureQueryString)
            {
                return Page.SecureQueryString["fileSource"].ToString();
            }
            return string.Empty;
        }
    }

    public string FileType
    {
        get
        {
            if (Page.IsSecureQueryString)
            {
                return Page.SecureQueryString["fileType"].ToString();
            }
            return string.Empty;
        }
    }


    public string MerchantNumber
    {
        get
        {
            if (Page.IsSecureQueryString)
            {
                return Page.SecureQueryString["merchantNumber"].ToString();
            }
            return string.Empty;
        }
    }

    public string MerchantName
    {
        get
        {
            if (Page.IsSecureQueryString)
            {
                return GeneralFuncsLib.GetMerchantName(MerchantNumber, true);
            }
            return string.Empty;
        }
    }

    private bool IsShowRoutingAccount
    {
        get
        {
            if (ViewState["ShowRoutingAccount"] != null)
                return (bool)(ViewState["ShowRoutingAccount"]);
            else
            {
                ViewState["ShowRoutingAccount"] = GeneralFuncsLib.Show_RoutingAccountNumber;
                return (bool)ViewState["ShowRoutingAccount"];
            }
        }
        set
        {
            ViewState["ShowRoutingAccount"] = value;
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            uxltFileSource.Text = VeraCodeSolution.DoVeraCode(FileSource);
            uxltFileType.Text = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("FileTypeBatch").ToString());
            uxltReportDate.Text = ReportDate.ToString("MM/dd/yyyy");

            OnDataBindControls(DataBindAction.BindReportGrid, uxNotInMifDetailGridBatch);
            litGridTitle.Text = string.Format("{0}: {1}", GetLocalResourceObject("Merchant").ToString(), MerchantName.IsNullOrEmpty() ? MerchantNumber : string.Format("{0} - {1}", MerchantNumber, MerchantName));
        }
        DoSwitchView();
    }

    private void DoSwitchView()
    {
        if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page))
        {
            uxNotInMifDetailGridBatch.Columns.FindByUniqueName(CARD_NUMBER).Visible = true;
            uxNotInMifDetailGridBatch.Columns.FindByUniqueName(PARTIAL_CARD_NUMBER).Visible = false;

            if (IsShowRoutingAccount)
            {
                uxNotInMifDetailGridBatch.Columns.FindByUniqueName("RoutingAccountNumber").Visible = true;
                uxNotInMifDetailGridBatch.Columns.FindByUniqueName("PartialRoutingACC").Visible = false;
            }
        }
        else
        {
            uxNotInMifDetailGridBatch.Columns.FindByUniqueName(CARD_NUMBER).Visible = false;
            uxNotInMifDetailGridBatch.Columns.FindByUniqueName(PARTIAL_CARD_NUMBER).Visible = true;

            if (IsShowRoutingAccount)
            {
                uxNotInMifDetailGridBatch.Columns.FindByUniqueName("RoutingAccountNumber").Visible = false;
                uxNotInMifDetailGridBatch.Columns.FindByUniqueName("PartialRoutingACC").Visible = true;
            }
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindReportGrid:
                {
                    FilterParameterCollection parames = new FilterParameterCollection();
                    //parames.AddLoggedInUserReportingParams(true);
                    parames.AddLoggedInUserParams(10000);
                    parames.AddLanguageID();
                    parames.Add(new FilterParameter("@ReportDate", ReportDate, DbType.DateTime));
                    parames.Add(new FilterParameter("@FileSource", FileSource, DbType.AnsiString));
                    parames.Add(new FilterParameter("@FileType", FileType, DbType.AnsiString));
                    parames.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.AnsiString));

                    if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page))
                    {
                        parames.AddDecryptDataParams("CardNumber", _isExporting);

                        if (IsShowRoutingAccount)
                            parames.AddDecryptDataParams("RoutingAccountNumber", _isExporting);
                    }
                    ((ASGrid)sender).DataSourceInvoker = new AS.Controls.Grid.ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { "spa_MerchantsNotInMif_Get_BatchDetail", ReportServices.ConvertToFilterParamWSArray(parames) });
                    break;
                }
        }
    }

    protected void uxNotInMifDetailGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;

            dataItem[KEYED].ToolTip = dataRow[KEYED_DESCRIPTION].ToString();
            dataItem[CARD_TYPE].ToolTip = dataRow[CARD_TYPE_DESC].ToString();
            dataItem[TRANS_CODE_DESC].ToolTip = dataRow[TRANS_CODE].ToString();
            // Handle for Card Number
            if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page)
                && (!dataItem[CARD_NUMBER].Text.Equals(GeneralFuncsLib.NBSP) || !dataItem["RoutingAccountNumber"].Text.Equals(GeneralFuncsLib.NBSP)))
            {
                dataItem[CARD_NUMBER].Text = VeraCodeSolution.GetOutputHtmlString(GeneralFuncsLib.BuildUrlForNotInMifCardNumber(
                    (SecurePage)this.Page, dataRow[PARTIAL_CARD_NUMBER], dataRow[CARD_NUMBER],
                    MerchantNumber, dataItem[CARD_NUMBER].Text, true));

                if (IsShowRoutingAccount)
                    dataItem["RoutingAccountNumber"].Text = VeraCodeSolution.GetOutputHtmlString(GeneralFuncsLib.BuildUrlForNotInMifCardNumber(
                    (SecurePage)this.Page, dataRow["PartialRoutingACC"], dataRow["RoutingAccountNumber"],
                    MerchantNumber, dataItem["RoutingAccountNumber"].Text, true));
            }
            else
            {
                if (!dataItem[PARTIAL_CARD_NUMBER].Text.Equals(GeneralFuncsLib.NBSP) || !dataItem["PartialRoutingACC"].Text.Equals(GeneralFuncsLib.NBSP))
                {
                    if (GeneralFuncsLib.HasIPForFullCard((SecurePage)Page))
                    {
                        dataItem[PARTIAL_CARD_NUMBER].Text = GeneralFuncsLib.BuildUrlForNotInMifFullCard(
                            (SecurePage)this.Page, ReportType.TRANSACTION_DETAIL,
                            dataRow[RECORD_ID].ToString(), dataRow[ISSUES_BANK].ToString(),
                            ReportDate.ToString(), dataRow[PARTIAL_CARD_NUMBER],
                            dataRow[CARD_NUMBER], MerchantNumber, dataRow[PARTIAL_CARD_NUMBER].ToString(),
                            _curIdx + 1, true, true);

                        if (IsShowRoutingAccount)
                            dataItem["PartialRoutingACC"].Text = GeneralFuncsLib.BuildUrlForNotInMifFullCard(
                            (SecurePage)this.Page, ReportType.NOT_IN_MIF_DETAIL,
                            dataRow[RECORD_ID].ToString(), dataRow[ISSUES_BANK].ToString(),
                            ReportDate.ToString(), dataRow["PartialRoutingACC"],
                            dataRow["RoutingAccountNumber"], MerchantNumber, dataRow["PartialRoutingACC"].ToString(),
                            _curIdx + 1, true, true);
                    }
                    else
                    {
                        dataItem[PARTIAL_CARD_NUMBER].Text = GeneralFuncsLib.BuildUrlForNotInMifCardNumber(
                            (SecurePage)this.Page, dataRow[PARTIAL_CARD_NUMBER],
                            dataRow[CARD_NUMBER], MerchantNumber, dataItem[PARTIAL_CARD_NUMBER].Text, true);

                        if (IsShowRoutingAccount)
                            dataItem["PartialRoutingACC"].Text = GeneralFuncsLib.BuildUrlForNotInMifCardNumber(
                           (SecurePage)this.Page, dataRow["PartialRoutingACC"],
                           dataRow["RoutingAccountNumber"], MerchantNumber, dataItem["PartialRoutingACC"].Text, true);
                    }
                }
            }
           
            if (!dataRow[AUTH_NUMBER].IsNullOrEmpty())
            {
                dataItem[AUTH_NUMBER].Text = GeneralFuncsLib.BuildAuthUrlPopupModalChild(
                    (SecurePage)Page, dataRow[AUTH_NUMBER], MerchantNumber,
                    _curIdx + 1, AuthIntruderQuery(uxNotInMifDetailGridBatch),
                    dataItem[AUTH_NUMBER].Text, true, true);
            }
        }
    }
    protected void uxNotInMifDetailGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindReportGrid, sender);
    }

    protected void uxExporterBath_NeedExportConfig(object sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        _isExporting = true;
        exportConfig.FileName = string.Format(GetLocalResourceObject("ExportFileName").ToString(), MerchantNumber, FileType.ToUpper(), FileSource, ReportDate.ToString("MM_dd_yyyy"));
        if (MerchantName.IsNullOrEmpty())
            exportConfig.ReportHeader = string.Format("{0} - {1}", GetLocalResourceObject("ExportTitle").ToString(), MerchantNumber);
        else
            exportConfig.ReportHeader = string.Format("{0} - {1} - {2}", GetLocalResourceObject("ExportTitle").ToString(), MerchantNumber, MerchantName);

        uxNotInMifDetailGridBatch.Columns.FindByUniqueName("CardNumber").Visible = false;
        uxNotInMifDetailGridBatch.Columns.FindByUniqueName("PartialCardNumber").Visible = true;

        if (IsShowRoutingAccount)
        {
            uxNotInMifDetailGridBatch.Columns.FindByUniqueName("RoutingAccountNumber").Visible = false;
            uxNotInMifDetailGridBatch.Columns.FindByUniqueName("PartialRoutingACC").Visible = true;
        }
    }

    #region helper
    private string AuthIntruderQuery(ASGrid grid)
    {
        return GeneralFuncsLib.BuildIntruderInfoForQueryStr(grid.ID, new string[] { "AuthNumber" });
    }

    #endregion
}