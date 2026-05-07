using AS.Common.DBManager;
using AS.Controls.Pages;
using System;
using System.Data;
using System.Web.UI;

[PagePermission("RetChbRpt,MSRetChbRpt")]
public partial class CbSequenceDetail : ReportPage
{
    private const string IMAGE = "<a class=\"image-link\" href=\"#\" style=\"cursor:pointer\" onclick=\" return parent.ShowPopupModalChild({0},'{1}','auto');\"><img src='res/img/information.png' border=\"0\"/></a>&nbsp; &nbsp;";

    int _index = 0;
    private int Index
    {
        get
        {
            if (SecureQueryString["idx"] != null)
            {
                int.TryParse(SecureQueryString["idx"].ToString(), out _index);
            }
            return _index;
        }
    }

    private bool IsNotInMif
    {
        get
        {
            if (SecureQueryString["isNotInMif"].IsNotNullData() && SecureQueryString["isNotInMif"].ToLower().Equals("true"))
                return true;
            return false;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
        if (!IsPostBack)
        {
            GetCBSequenceDetail();

        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    private void GetCBSequenceDetail()
    {
        string CBSequenceNumber = this.SecureQueryString["CBSequenceNumber"];
        FilterParameterCollection _parameters = new FilterParameterCollection();
        _parameters.Add(new AS.Common.DBManager.FilterParameter("@MerchantNumber", this.SavedReportFilterValue.Value, System.Data.DbType.String));
        _parameters.Add(new AS.Common.DBManager.FilterParameter("@CBSeqNo", CBSequenceNumber, DbType.AnsiString));
        _parameters.Add(new AS.Common.DBManager.FilterParameter("@ReportDate", this.SecureQueryString["rptDate"], DbType.AnsiString));
        _parameters.Add(new AS.Common.DBManager.FilterParameter("@RecordID", this.SecureQueryString["recid"], DbType.AnsiString));
        _parameters.Add(new AS.Common.DBManager.FilterParameter("@IsNotInMif", IsNotInMif, DbType.Boolean));
        _parameters.AddLanguageID();
        _parameters.AddLoggedInUserReportingParams();
        string spName = string.Empty;
        DataTable tbl = null;
        if (IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC) && !GeneralFuncsLib.HasIPForFullCard(this))
        {
            spName = "spa_cs_GetCBSequenceDetail";
            _parameters.AddDecryptDataParams("CardNumber");
            if (GeneralFuncsLib.Show_RoutingAccountNumber)
                _parameters.AddDecryptDataParams("RoutingAccountNumber");

            tbl = WebServices.CsReportServices.GetReports(spName, _parameters);
        }
        else
        {
            if (GeneralFuncsLib.HasIPForFullCard(this))
            {
                spName = "spa_cs_GetCBSequenceDetail";
                _parameters.AddDecryptDataParams("CardNumber");
                if (GeneralFuncsLib.Show_RoutingAccountNumber)
                    _parameters.AddDecryptDataParams("RoutingAccountNumber");
                tbl = WebServices.CsReportServices.GetReports(spName, _parameters);
            }
            else
            {
                _parameters.AddLanguageID();
                spName = "spa_ms_GetCBSequenceDetail";
                tbl = WebServices.CsReportServices.GetReports(spName, _parameters);
            }
        }
        rptCBDetail.DataSource = tbl;
        rptCBDetail.DataBind();
    }
    protected string FormatCurrency(object data)
    {
        if (data == DBNull.Value) return string.Empty;
        else
            return AS.Common.Formater.FormatData.FormatCurrency(data, SessionManager.CurrencyFortmat);
    }
    protected string FormatInteger(object data)
    {
        if (data == DBNull.Value) return string.Empty;
        else
            return AS.Common.Formater.FormatData.FormatInteger(data);
    }
    protected string FormatDateTime(object data)
    {
        if (data == DBNull.Value) return string.Empty;
        else
            return AS.Common.Formater.FormatData.FormatDate(data.ToString());
    }
    protected string DisplayCardNumber(object cardNumber, object partialCardNumber, object RecordID, object ReportDate)
    {
        string result = string.Empty;
        if (IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CC) && !GeneralFuncsLib.HasIPForFullCard(this))
            result = cardNumber.ToString();
        else if (GeneralFuncsLib.HasIPForFullCard(this))
            result = string.Format(IMAGE, (Index + 1), BuildUrlForFullCard(RecordID.ToString(), ReportDate.ToString())) + partialCardNumber.ToString();
        else
            result = partialCardNumber.ToString();
        return result;
    }
    private string BuildUrlForFullCard(string RecordID, string ReportDate)
    {
        string queryString = BuildSecureQueryString("rt=cbSequenceDetail" + "&cn=" + Server.UrlEncode(RecordID) + "&reportdate=" + ReportDate + "&idx=" + (Index + 1));
        queryString = ResolveUrl("~/") + "FullCC.aspx?" + queryString;
        return queryString;
    }
}
