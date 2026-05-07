using AS.Common.DBManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_MerchantNoteDefaultSetting : GlobalUserControl
{
    enum DataBindAction
    {
        BindSourceList
    }

    //"1": Merchant Profile, "2": Risk Report
    public string FromPage
    {
        get
        {
            if (Page.IsSecureQueryString)
                return Page.SecureQueryString["fromPage"].ToString();
            return string.Empty;
        }
    }

    public string DefaultSourceConfig
    {
        get
        {
            if (FromPage == "1")
                return UserConfigNames.CONFIG_MERCHANT_PROFILE_SOURCE_DEFAULT_SETTING;
            return UserConfigNames.CONFIG_RISK_REPORT_SOURCE_DEFAULT_SETTING;
        }
    }

    public bool IsHasMerchantProfile
    {
        get
        {
            return (SessionManager.CurrentUserPermissions.Contains("MerchProfile")
                || SessionManager.CurrentUserPermissions.Contains("MSMerchProfile"));
        }
    }

    public bool IsHasRiskReport
    {
        get
        {
            return (SessionManager.CurrentUserPermissions.Contains("RskRP")
                || SessionManager.CurrentUserPermissions.Contains("MSRskRP"));
        }
    }

    public bool IsHasOpenNewCase
    {
        get
        {
            return SessionManager.CurrentUserPermissions.Contains("CMOpenCase");
        }
    }

    public bool IsHasOpenRiskCase
    {
        get
        {
            return SessionManager.CurrentUserPermissions.Contains("CMOpenRiskCase");
        }
    }

    public bool IsHasShadowUnderwriting
    {
        get
        {
            return SessionManager.CurrentUserPermissions.Contains("SponsorMerchantShadowUnderwriting");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetDefaultSourceValue();
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindSourceList:
                {
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    parameters.Add(new FilterParameter("@IsHasMerchantProfile", IsHasMerchantProfile, DbType.Boolean));
                    parameters.Add(new FilterParameter("@IsHasRiskReport", IsHasRiskReport, DbType.Boolean));
                    parameters.Add(new FilterParameter("@IsHasOpenNewCase", IsHasOpenNewCase, DbType.Boolean));
                    parameters.Add(new FilterParameter("@IsHasOpenRiskCase", IsHasOpenRiskCase, DbType.Boolean));
                    parameters.Add(new FilterParameter("@IsHasShadowUnderwriting", IsHasShadowUnderwriting, DbType.Boolean));
                    parameters.AddLanguageID();

                    uxSourceList.DataValueField = "NoteSourceID";
                    uxSourceList.DataTextField = "Description";
                    uxSourceList.DataSource = WebServices.RiskServices.GetReports("spa_MerchantNotes_GetSource", parameters);
                    uxSourceList.DataBind();
                    break;
                }
        }
    }

    private void SetDefaultSourceValue()
    {
        var defaultSetting = ExcludeDefaultSettingByPermission(PersonalDataHelper.GetJSONConfig<string>(DefaultSourceConfig));        
        OnDataBindControls(DataBindAction.BindSourceList);
        uxSourceList.SetSelectedValue(defaultSetting.Split(',').ToArray());
    }

    protected void uxSubmit_Click(object sender, EventArgs e)
    {
        PersonalDataHelper.SaveConfigAsJSON(DefaultSourceConfig, HandleString(uxSourceList.SelectedItems));
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "doneSubmit", "ClosePopupModal();", true);
    }

    #region Helpers

    private string HandleString(ListItemCollection items)
    {
        if (items.Count == 0)
            return string.Empty;
        StringBuilder result = new StringBuilder();
        string delim = "";
        foreach (ListItem item in items)
        {
            result.Append(delim); delim = ",";
            result.Append(item.Value);
        }
        return result.ToString();
    }

    private string ExcludeDefaultSettingByPermission(string defaultSetting)
    {
        //Doesn't config before
        if (string.IsNullOrEmpty(defaultSetting))
            return string.Empty;

        string[] arrDefaultSetting = defaultSetting.Split(',');
        if (!IsHasMerchantProfile)
            arrDefaultSetting = arrDefaultSetting.Where(m => m != "1").ToArray();
        if (!IsHasRiskReport)
        {
            arrDefaultSetting = arrDefaultSetting.Where(m => m != "2").ToArray();
        }
        if (!IsHasOpenNewCase)
        {
            arrDefaultSetting = arrDefaultSetting.Where(m => m != "3").ToArray();
        }
        if (!IsHasOpenRiskCase)
        {
            arrDefaultSetting = arrDefaultSetting.Where(m => m != "4").ToArray();
        }
        if (!IsHasShadowUnderwriting)
        {
            arrDefaultSetting = arrDefaultSetting.Where(m => m != "5").ToArray();
        }
        return string.Join(",", arrDefaultSetting);
    }

    #endregion
}