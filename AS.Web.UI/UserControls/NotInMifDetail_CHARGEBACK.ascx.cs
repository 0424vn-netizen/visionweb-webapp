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

public partial class UserControls_NotInMifDetail_CHARGEBACK : GlobalUserControl
{
    enum DataBindAction
    {
        BindReportGrid,
    }

    private const string PARTIAL_CARD_NUMBER = "PartialCardNumber";
    private const string CARD_NUMBER = "CardNumber";
    private const string MERCHANT_NUMBER = "MerchantNumber";
    private const string REFERENCE_NUMBER = "ReferenceNumber";
    private const string RECORD_ID = "RecordID";
    private const string ISSUES_BANK = "IssueBank";
    private const string EXP_DATE = "ExpDate";
    private const string CB_TYPE = "CBType";
    private const string CB_TYPE_DES = "CBTypeDescription";
    private const string DISPOSITION = "Disposition";
    private const string CB_SEQ_NUM = "CBSequenceNumber";
    private const string CB_AMOUNT = "CBAmount";
    private const string CARD_TYPE = "CardType";
    private const string CARD_TYPE_DESC = "CardTypeDesc";
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

    public string CBSequenceIntruderQuery
    {
        get
        {
            return GeneralFuncsLib.BuildIntruderInfoForQueryStr(uxNotInMifDetailGridChargeback.ID, new string[] { CB_SEQ_NUM });
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
            uxltFileType.Text = VeraCodeSolution.DoVeraCode(GetLocalResourceObject("FileTyeChargeback").ToString());
            uxltReportDate.Text = ReportDate.ToString("MM/dd/yyyy");

            OnDataBindControls(DataBindAction.BindReportGrid, uxNotInMifDetailGridChargeback);
            litGridTitle.Text = string.Format("{0}: {1}", GetLocalResourceObject("Merchant").ToString(), MerchantName.IsNullOrEmpty() ? MerchantNumber : string.Format("{0} - {1}", MerchantNumber, MerchantName));
        }
        DoSwitchView();
    }

    private void DoSwitchView()
    {
        if (GeneralFuncsLib.CheckCSViewFullCard((SecurePage)this.Page))
        {
            uxNotInMifDetailGridChargeback.Columns.FindByUniqueName(CARD_NUMBER).Visible = true;
            uxNotInMifDetailGridChargeback.Columns.FindByUniqueName(PARTIAL_CARD_NUMBER).Visible = false;

            if (IsShowRoutingAccount)
            {
                uxNotInMifDetailGridChargeback.Columns.FindByUniqueName("RoutingAccountNumber").Visible = true;
                uxNotInMifDetailGridChargeback.Columns.FindByUniqueName("PartialRoutingACC").Visible = false;
            }
        }
        else
        {
            uxNotInMifDetailGridChargeback.Columns.FindByUniqueName(CARD_NUMBER).Visible = false;
            uxNotInMifDetailGridChargeback.Columns.FindByUniqueName(PARTIAL_CARD_NUMBER).Visible = true;

            if (IsShowRoutingAccount)
            {
                uxNotInMifDetailGridChargeback.Columns.FindByUniqueName("RoutingAccountNumber").Visible = false;
                uxNotInMifDetailGridChargeback.Columns.FindByUniqueName("PartialRoutingACC").Visible = true;
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
                    ((ASGrid)sender).DataSourceInvoker = new AS.Controls.Grid.ASFuncInvoker(WebServices.CsReportServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { "spa_MerchantsNotInMif_Get_ChargebackDetail", ReportServices.ConvertToFilterParamWSArray(parames) });
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

            dataItem[CARD_TYPE].ToolTip = dataRow[CARD_TYPE_DESC].ToString();
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
            //Handle Reference #
            if (!dataRow[REFERENCE_NUMBER].IsNullOrEmpty())
            {
                string queryString = Page.BuildSecureQueryString(string.Format("referenceNumber={0}&merchantNumber={1}&rcType=c&isNotInMif=1{2}", dataRow[REFERENCE_NUMBER].ToString(), MerchantNumber, ChargebackIntruderQuery(uxNotInMifDetailGridChargeback)));
                dataItem[REFERENCE_NUMBER].Text = VeraCodeSolution.DoVeraCode(string.Format("<a href=\"#\" onclick=\"return  parent.ShowPopupModalChild(1,'{0}','auto');\" >{1}</a>", "RetrievalsChargebacksDetail.aspx?"
                    + queryString, dataRow[REFERENCE_NUMBER].ToString()));
            }

            if (SessionManager.CurrentUser.ASClient != 23)
            {
                if (dataRow[CB_SEQ_NUM].ToString().Trim() != String.Empty)
                    dataItem[CB_SEQ_NUM].Text = VeraCodeSolution.DoVeraCode(string.Format("<a href=\"#\" onclick=\"return parent.ShowPopupModalChild(1, '{0}','auto');\" >{1}</a>",
                        "CbSequenceDetail.aspx?" + Page.BuildSecureQueryString("CBSequenceNumber=" + dataRow[CB_SEQ_NUM].ToString() + "&rptDate=" + dataRow["ReportDate"] + "&recid=" + dataRow["RecordID"] 
                        + CBSequenceIntruderQuery + "&isNotInMif=true"), dataRow[CB_SEQ_NUM].ToString()));
            }
        }
    }
    protected void uxNotInMifDetailGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindReportGrid, sender);
    }

    protected void uxExporterChargeback_NeedExportConfig(object sender, AS.Controls.Exporter.ExportConfig exportConfig)
    {
        _isExporting = true;
        exportConfig.FileName = string.Format(GetLocalResourceObject("ExportFileName").ToString(), MerchantNumber, FileType.ToUpper(), FileSource, ReportDate.ToString("MM_dd_yyyy"));
        if (MerchantName.IsNullOrEmpty())
            exportConfig.ReportHeader = string.Format("{0} - {1}", GetLocalResourceObject("ExportTitle").ToString(), MerchantNumber);
        else
            exportConfig.ReportHeader = string.Format("{0} - {1} - {2}", GetLocalResourceObject("ExportTitle").ToString(), MerchantNumber, MerchantName);

        if (uxNotInMifDetailGridChargeback.Columns.FindByUniqueNameSafe(CARD_NUMBER) != null)
            uxNotInMifDetailGridChargeback.Columns.FindByUniqueNameSafe(CARD_NUMBER).Visible = false;
        if (uxNotInMifDetailGridChargeback.Columns.FindByUniqueNameSafe(PARTIAL_CARD_NUMBER) != null)
            uxNotInMifDetailGridChargeback.Columns.FindByUniqueNameSafe(PARTIAL_CARD_NUMBER).Visible = true;

        if (IsShowRoutingAccount)
        {
            uxNotInMifDetailGridChargeback.Columns.FindByUniqueName("RoutingAccountNumber").Visible = false;
            uxNotInMifDetailGridChargeback.Columns.FindByUniqueName("PartialRoutingACC").Visible = true;
        }
    }
    #region helper

    private string ChargebackIntruderQuery(ASGrid grid)
    {
        return GeneralFuncsLib.BuildIntruderInfoForQueryStr(grid.ID, new string[] { "ReferenceNumber" });
    }

    #endregion
}