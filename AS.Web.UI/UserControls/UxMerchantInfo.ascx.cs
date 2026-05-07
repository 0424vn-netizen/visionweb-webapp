using AS.Common.DBManager;
using AS.Controls.Pages;
using System;
using System.Data;
using System.Web.UI;

public partial class UserControls_UxMerchantInfo : GlobalUserControl
{
    private string OPTED_IN = string.Empty;

    #region Enums
    enum DataBindAction
    {
        BindMerchantInfo
    }
    enum PostBackAction
    {
    }
    #endregion

    public DataTable _MifTable
    {
        get
        {
            if (ViewState["MerchantInfomation"] != null)
                return (DataTable)(ViewState["MerchantInfomation"]);
            else
                return null;
        }
        set
        {
            ViewState["MerchantInfomation"] = value;
        }
    }

    protected string _OptedIn
    {
        get
        {
            return (string)ViewState["OptStatus"];
        }
        set
        {
            ViewState["OptStatus"] = value;
        }
    }

    protected string _MifEmail
    {
        get
        {
            return (string)ViewState["MifEmail"];
        }
        set
        {
            ViewState["MifEmail"] = value;
        }
    }

    protected string _TempStr
    {
        get
        {
            return (string)ViewState["TempStr"];
        }
        set
        {
            ViewState["TempStr"] = value;
        }
    }

    public string MerchantNumber
    {
        get
        {
            return (string)ViewState["MerchantNumber"];
        }
        set
        {
            ViewState["MerchantNumber"] = value;
        }
    }

    private string RecipientEmail
    {
        get
        {
            return (string)ViewState["RecipientEmail"];
        }
        set
        {
            ViewState["RecipientEmail"] = value;
        }
    }

    private string LastBatchDate
    {
        get
        {
            return (string)ViewState["LastBatchDate"];
        }
        set
        {
            ViewState["LastBatchDate"] = value;
        }
    }

    private bool CustomReport
    {
        get
        {
            return (bool)ViewState["CustomReport"];
        }
        set
        {
            ViewState["CustomReport"] = value;
        }
    }

    public string HierachyMode
    {
        get;
        set;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        OPTED_IN = GetLocalResourceObject("UxMerchantInfo_ascx_cs_OptedIn").ToString();
        Rebind();
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (Page.IsIntruderDetected)
            return;
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindMerchantInfo:
                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserReportingParams();
                parameters.AddLoggedInUserPrimaryUserID();

                parameters.Add(new FilterParameter("@MerchantNumber", Page.SecureQueryString["MerchantNumber"], DbType.AnsiString));
                parameters.Add(new FilterParameter("@FlagPermission", SessionManager.CurrentUserViewMode, DbType.Int32));
                parameters.AddLanguageID();
                DataTable dtMerch = WebServices.CsReportServices.GetReports("spa_cs_GetMerchantProfile", parameters);

                if (dtMerch.Rows.Count > 0)
                {
                    _MifTable = dtMerch;
                    this._OptedIn = dtMerch.Rows[0]["SiteAccess"].ToString().Equals(OPTED_IN) ? "1" : "0";
                    this._MifEmail = dtMerch.Rows[0]["Email"].ToString();
                    phdMerchantDetail.Visible = true;
                    if (string.IsNullOrEmpty(MerchantNumber))
                    {
                        CustomReport = (bool)dtMerch.Rows[0]["CustomReport"];
                        RecipientEmail = dtMerch.Rows[0]["RecipientEmail"].ToString();
                        DateTime tempDate = new DateTime(1900, 1, 1);
                        DateTime.TryParse(FormatDate(dtMerch.Rows[0]["LastBactchActivity"]), out tempDate);
                        LastBatchDate = tempDate.Year == 1900 ? "0" : tempDate.Ticks.ToString();
                    }
                    else
                    {

                    }
                    this.rptMerchantInfo.DataSource = dtMerch;
                    this.rptMerchantInfo.DataBind();
                }
                else
                {
                    phdMerchantDetail.Visible = false;
                }
                break;
        }
    }

    protected void lnkGrid_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
    {
        Response.Redirect(GeneralFuncsLib.BuildBatchHistoryUrl(
                   (SecurePage)Page, LastBatchDate, MerchantNumber));
    }

    protected string BindAddress(object add1, object add2, object add3, object city, object state, object zip)
    {
        return MerchantProfileHelper.BindAddress(add1, add2, add3, city, state, zip);
    }

    private object ProcessNullValue(object obj)
    {
        return obj.GetType() == typeof(DBNull) ? null : obj;
    }

    protected string FormatCurrency(object abc)
    {
        return MerchantProfileHelper.FormatCurrency(abc);
    }

    protected string FormatCurrency(object abc, int count)
    {
        return MerchantProfileHelper.FormatCurrency(abc);
    }

    protected string FormatDate(object dt)
    {
        return MerchantProfileHelper.FormatDate(dt);
    }

    protected string FormatPhone(object phone)
    {
        return AS.Common.Formater.FormatData.FormatPhoneNumber(phone.ToString());
    }

    protected string DoVeraCode(DataRow row, string key)
    {
        if (row == null)
        {
            return string.Empty;
        }

        object obj = row[key];
        if (obj == DBNull.Value || obj == null)
        {
            return string.Empty;
        }
        else
        {
            return obj.ToString();
        }
    }
    protected string FormatDate(DataRow row, string key)
    {
        if (row == null)
        {
            return string.Empty;
        }

        object obj = row[key];
        if (obj == DBNull.Value || obj == null)
        {
            return string.Empty;
        }
        else
        {
            return ((DateTime)obj).ToGenericDateString();
        }
    }

    protected bool setButton(object MerchantStatusDesc)
    {
        if (MerchantStatusDesc.ToString() != string.Empty)
        {
            string status = MerchantStatusDesc.ToString().ToLower();
            return !status.Equals("closed");
        }
        return false;
    }

    protected string setOptInStatus(object optIn, string merchantStatus)
    {
        return MerchantProfileHelper.SetOptInStatus(optIn, merchantStatus);
    }

    public string GetRoutingNumber(string full)
    {
        if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Merchant)
        {
            if (full.Length > 10)
            {
                string part2 = full.Substring(full.Length - 4, 4);
                full = "xxxx" + part2;
            }
        }
        return full;
    }

    public string GetTaxDDANumber(string full, string partial)
    {
        full = WebServices.CsReportServices.DecryptText(full, SessionManager.CurrentUser.ASClient);
        if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Merchant)
        {
            return partial;
        }
        return full;
    }

    public string GetHierarchyNumber(string strInput, bool isChain)
    {
        if (strInput.Length >= 4)
        {
            if (isChain)
            {
                strInput = strInput.Remove(0, 3);
            }
            else
            {
                strInput = strInput.Remove(0, 4);
            }
        }
        return strInput;
    }

    public void Rebind()
    {
        this.OnDataBindControls(DataBindAction.BindMerchantInfo);
    }

    protected string SetStatusText(object obj)
    {
        return MerchantProfileHelper.SetStatusForSiteAccess(obj);
    }

    protected bool SetVisible(object obj)
    {
        bool b = Page.IsUserWithPermission("CSUser");
        b = (Page.IsUserWithPermission("CSUser") && String.Compare(obj.ToString(), "opted in", true) == 0 || String.Compare(obj.ToString(), "opted out", true) == 0);
        int i = String.Compare(obj.ToString(), "opted out", true);
        return (Page.IsUserWithPermission("CSUser") && String.Compare(obj.ToString(), "opted in", true) == 0 || String.Compare(obj.ToString(), "opted out", true) == 0);
    }

    protected string CheckPermisson(object obj, string permissionCode)
    {
        return MerchantProfileHelper.CheckPermisson(obj, permissionCode, Page);
    }
}
