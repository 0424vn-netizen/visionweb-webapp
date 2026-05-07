using AS.Common.DBManager;
using AS.Controls.Pages;
using AS.Controls.Validators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

[PagePermission("AddNewMerchant")]
public partial class AddNewMerchant : ReportPage
{
    public SecurePage _page;
    public enum DataBindAction
    {
        Reseller,
        State,
        Country,
        SIC,
        BusinessType
    }

    public enum PostBackAction
    {
        Add,
    }

    public bool IsOpenCase
    {
        get
        {
            _page = (SecurePage)Page;
            return _page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_CM_OPEN_CASE);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            _page = (SecurePage)Page;
            OnDataBindControls(DataBindAction.Country);
            OnDataBindControls(DataBindAction.SIC);

            //Init filter report
            if (SavedReportFilterValue.IsNullData())
            {
                GeneralFuncsLib.LoadHierarchyConfigurations();
            }

            DataTable HierarchyFilter = SessionManager.HierarchyFilter;
            foreach (DataRow row in HierarchyFilter.Rows)
            {
                if (row["HierarchyMode"].ToString() == "MERCHANTNUMBER")
                {
                    SavedReportFilterValue = new AS.Web.UI.Controls.HierarchyFilterValue();
                    SavedReportFilterValue.ID = row["HierarchyID"].ToString();
                    SavedReportFilterValue.HierarchyMode = "MERCHANTNUMBER";
                    break;
                }
            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (ValidateInput())
        {
            SavedReportFilterValue.Value = uxMerchantNum.Text;
            SessionManager.CurrentMerchantNumber = uxMerchantNum.Text;
            OnPostBackActions(PostBackAction.Add);
            string url = ResolveUrl("~/JumpToCase.aspx?") + string.Format("type=6&IsShowDefaultValue=1");
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "openNewCase", string.Format("addMerchantCallback('{0}')", url), true);
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {

            case DataBindAction.Country:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.AddLanguageID();
                    DataTable dt = WebServices.SecurityServices.GetReports("spa_GetCountry", parameters);
                    uxCountry.DataSource = dt;
                    uxCountry.DataBind();

                    RadComboBoxItem emptyItem = new RadComboBoxItem(string.Empty, string.Empty);
                    emptyItem.Height = Unit.Pixel(13);
                    emptyItem.Selected = true;
                    uxCountry.Items.Insert(0, emptyItem);
                    break;
                }
            case DataBindAction.State:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.AddLanguageID();
                    parameters.Add(new FilterParameter("@CountryCode", uxCountry.SelectedValue, DbType.AnsiString));
                    DataTable dt = WebServices.SecurityServices.GetReports("spa_GetState", parameters);
                    uxStateProvince.DataSource = dt;
                    uxStateProvince.DataBind();
                    break;
                }
            case DataBindAction.SIC:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.AddLanguageID();
                    DataTable dt = WebServices.SecurityServices.GetReports("spa_GetSICCode", parameters);
                    uxSicMcc.DataSource = dt;
                    uxSicMcc.DataBind();

                    RadComboBoxItem emptyItem = new RadComboBoxItem(string.Empty, string.Empty);
                    emptyItem.Height = Unit.Pixel(13);
                    emptyItem.Selected = true;
                    uxSicMcc.Items.Insert(0, emptyItem);
                    break;
                }
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.Add:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    FilterParameterCollection outParameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@MerchantNumber", uxMerchantNum.Text, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@DBAName", uxDBAName.Text, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@AgentReferral", uxAgentReferral.Text, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@ResellerId", "", DbType.AnsiString));
                    parameters.Add(new FilterParameter("@ResellerName", uxResellerName.Text, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@ResellerPhone", uxResellerPhone.Text, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@ResellerEmail", uxResellerEmail.Text, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@CompanyName", uxMerchantCorporate.Text, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@CountryCode", uxCountry.SelectedValue, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@Line1_Business", uxBusinessAddLine1.Text, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@Line2_Business", uxBusinessAddLine2.Text, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@City", uxCity.Text, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@State", uxStateProvince.SelectedValue, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@Zip", uxZip.Text, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@BusinessEmail", uxBusinessEmail.Text, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@BusinessWebsite", uxBusinessWebsite.Text, DbType.AnsiString));

                    parameters.Add(new FilterParameter("@BusinessWebsite2", uxBusinessWebsite2.Text, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@BusinessWebsite3", uxBusinessWebsite3.Text, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@BusinessWebsite4", uxBusinessWebsite4.Text, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@BusinessWebsite5", uxBusinessWebsite5.Text, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@BusinessWebsite6", uxBusinessWebsite6.Text, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@BusinessWebsite7", uxBusinessWebsite7.Text, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@BusinessWebsite8", uxBusinessWebsite8.Text, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@BusinessWebsite9", uxBusinessWebsite9.Text, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@BusinessWebsite10", uxBusinessWebsite10.Text, DbType.AnsiString));

                    parameters.Add(new FilterParameter("@SICCode", uxSicMcc.SelectedValue, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@BusinessTypeNotes", uxBusinessType.Text, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@BusinessRegistration", uxBusinessRegistration.Text, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@ProductSold", uxProductSold.Text, DbType.AnsiString));

                    WebServices.CsReportServices.ExecuteNonQueryCommand("spa_MerchantInformation_Ins_AlliedWallet", parameters, out outParameters);
                    break;
                }
        }
    }

    private bool ValidateInput()
    {
        if (AddNewMerchant.CheckMerchantExist(uxMerchantNum.Text))
        {
            ShowMessageBox(Resources.ValMsg.DuplicateMerchantMsg, ltMerchantNum, msgMerchantNum);
            return false;
        }
        if (!AddNewMerchant.CheckStateIsBelongToCountry(uxCountry.SelectedValue, uxStateProvince.SelectedValue))
        {
            ShowMessageBox(Resources.ValMsg.InvalidState, ltStateProvince, msgStateProvice);
            return false;
        }

        if (!ValidateUrl(uxBusinessWebsite))
        {
            ShowMessageBox(Resources.ValMsg.InvalidURL, ltBusinessWebsite, msgBusinessWebsite);
            return false;
        }
        if (!ValidateUrl(uxBusinessWebsite2))
        {
            ShowMessageBox(Resources.ValMsg.InvalidURL, ltBusinessWebsite2, msgBusinessWebsite2);
            return false;
        }
        if (!ValidateUrl(uxBusinessWebsite3))
        {
            ShowMessageBox(Resources.ValMsg.InvalidURL, ltBusinessWebsite3, msgBusinessWebsite3);
            return false;
        }
        if (!ValidateUrl(uxBusinessWebsite4))
        {
            ShowMessageBox(Resources.ValMsg.InvalidURL, ltBusinessWebsite4, msgBusinessWebsite4);
            return false;
        }
        if (!ValidateUrl(uxBusinessWebsite5))
        {
            ShowMessageBox(Resources.ValMsg.InvalidURL, ltBusinessWebsite5, msgBusinessWebsite5);
            return false;
        }
        if (!ValidateUrl(uxBusinessWebsite6))
        {
            ShowMessageBox(Resources.ValMsg.InvalidURL, ltBusinessWebsite6, msgBusinessWebsite6);
            return false;
        }
        if (!ValidateUrl(uxBusinessWebsite7))
        {
            ShowMessageBox(Resources.ValMsg.InvalidURL, ltBusinessWebsite7, msgBusinessWebsite7);
            return false;
        }
        if (!ValidateUrl(uxBusinessWebsite8))
        {
            ShowMessageBox(Resources.ValMsg.InvalidURL, ltBusinessWebsite8, msgBusinessWebsite8);
            return false;
        }
        if (!ValidateUrl(uxBusinessWebsite9))
        {
            ShowMessageBox(Resources.ValMsg.InvalidURL, ltBusinessWebsite9, msgBusinessWebsite9);
            return false;
        }
        if (!ValidateUrl(uxBusinessWebsite10))
        {
            ShowMessageBox(Resources.ValMsg.InvalidURL, ltBusinessWebsite10, msgBusinessWebsite10);
            return false;
        }
        return true;
    }

    private void ShowMessageBox(string message, ValidatorLabel lbctrl, ValidatorMessage msgctrl)
    {
        ShowServerErrorMessage(msgctrl, message);
        msgctrl.ShowOnLoad = true;
        if (!string.IsNullOrEmpty(message))
        {
            lbctrl.CssClass = "control-label label-error";
        }
        else
        {
            lbctrl.CssClass = "control-label";
        }
    }

    public bool ValidateUrl(AS.Controls.Global.TextBox businessWeb)
    {
        if (businessWeb.Visible && !businessWeb.Text.IsNullOrEmpty())
        {
            Regex reg = new Regex(@"^(http://www.|https://www.|http://|https://)?[a-z0-9A-Z]+([-_.]{1}[a-z0-9A-Z]+)*.[a-z]{1,5}(:[0,9]{1,5})?([/.A-Za-z0-9?_=-]+)?$");
            if (!reg.IsMatch(businessWeb.Text))
                return false;
            return true;
        }
        return true;
    }

    public void ShowServerErrorMessage(ValidatorMessage val, String customMessage = null)
    {
        if (customMessage != null)
        {
            val.Message = AS.Common.VeraCodeSolution.DoVeraCode(customMessage);
        }
        val.ShowOnLoad = true;
    }


    [WebMethod]
    public static bool CheckMerchantExist(string merchantNum)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@MerchantNumber", merchantNum, DbType.AnsiString));

        DataTable result = WebServices.RiskServices.GetReports("spa_CheckExistsMerchantNumber", parameters);

        //0: merchant is exist
        return result.Rows[0][0].ToInt() == 0;
    }

    [WebMethod]
    public static bool CheckStateIsBelongToCountry(string countryCode, string state)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.AddLanguageID();
        parameters.Add(new FilterParameter("@CountryCode", countryCode, DbType.AnsiString));
        DataTable dt = WebServices.SecurityServices.GetReports("spa_GetState", parameters);
        DataRow[] dr = dt.Select("State = '" + state + "'");
        if (dr.Count() == 0)
            return false;
        return true;
    }

    protected void uxCountry_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        uxStateProvince.Enabled = true; ;
        OnDataBindControls(DataBindAction.State);

    }

    protected void uxRemoveBusinessWeb_Click(object sender, EventArgs e)
    {
        var businessWebId = hdBusinessWebId.Value;
        var trId = hdTrBusinessWebId.Value;
        AS.Controls.Global.TextBox inpBusinessWeb = pnBusinessWebsite.FindControl(businessWebId) as AS.Controls.Global.TextBox;
        HtmlControl tr = pnBusinessWebsite.FindControl(trId) as HtmlControl;
        inpBusinessWeb.Visible = false;
        inpBusinessWeb.Text = string.Empty;
        tr.Attributes["class"] += " hide";

        uxAddBWLink.CssClass = "heading link-back fw-normal";
    }

    protected void uxAddBusinessWeb_Click(object sender, EventArgs e)
    {
        var businessWebId = hdBusinessWebId.Value;
        var trId = hdTrBusinessWebId.Value;
        AS.Controls.Global.TextBox inpBusinessWeb = pnBusinessWebsite.FindControl(businessWebId) as AS.Controls.Global.TextBox;
        HtmlControl tr = pnBusinessWebsite.FindControl(trId) as HtmlControl;
        inpBusinessWeb.Visible = true;
        tr.Attributes["class"] = "b-website";
        if (hdHideTrCount.Value.ToInt() <= 1)
        {
            uxAddBWLink.CssClass += " disabled-link";
        }
        else
        {
            uxAddBWLink.CssClass = "heading link-back fw-normal";
        }
    }
}