using AS.Controls.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class UserControls_MecchantProfileNavigator : System.Web.UI.UserControl
{
    public string Processor { get; set; }
    private const string KEY_UNDERLINE = "_";
    #region Methods

    #region Protected Methods


    protected override void OnPreRender(EventArgs e)
    {
        BuildNavigator();
        if (GeneralFuncsLib.HasCMPermission((SecurePage)Page))
        {
            uxCaseHistory.Visible = true;
        }
        base.OnPreRender(e);
    }
    #endregion Protected Methods

    #region Private Methods

    private void BuildNavigator()
    {
        string navigatorKey = string.Empty;
        Dictionary<string, string> navigatorDic =
            GeneralFuncsLib.GetMerchantProfileNavigator(SessionManager.CurrentUser.ASClient.ToString());

        foreach (KeyValuePair<string, string> navigator in navigatorDic)
        {
            navigatorKey = navigator.Key;
            if ((string.Compare(navigator.Key, "memosection", true) == 0) && (string.Compare(GeneralFuncsLib.GetDataOfExtendedSetting("InvisibleMemosGrid"), "true", true) == 0))
            {
                continue;
            }
            if ((string.Compare(navigator.Key, "RiskScoreInfo", true) == 0) && !(((ReportPage)Page).IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RISK_INFO)  || ((ReportPage)Page).IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_MSRISK_INFO)))
            {
                continue;
            }
            // If client has multi left menu then we will switch menu left by processor.
            if (navigator.Key.Contains(KEY_UNDERLINE))
            {
                if (!navigator.Key.EndsWith(KEY_UNDERLINE + Processor, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                //Get real key without processer name.
                navigatorKey = navigator.Key.Substring(0, navigator.Key.LastIndexOf(KEY_UNDERLINE));
            }
            uxPlaceHolderNavigator.Controls.Add(GenerateNavigatorItem(navigatorKey, navigator.Value));
        }
    }

    private Control GenerateNavigatorItem(string id, string text)
    {
        // Generate navigator item like below:
        //<li>
        //    <a href="#merchinfo">
        //        <i class="icon-chevron-right"></i>
        //        Merch Info
        //    </a>
        //</li>
        HtmlGenericControl italicTagControl = new HtmlGenericControl("i");
        italicTagControl.Attributes.Add("class", "icon-chevron-right");

        HtmlGenericControl aTagControl = new HtmlGenericControl("a");
        aTagControl.Attributes.Add("href", "#" + id);
        aTagControl.InnerHtml = text;
        aTagControl.Controls.AddAt(1, italicTagControl);

        HtmlGenericControl liControl = new HtmlGenericControl("li");
        if (id.ToLower().Equals("reportaccess"))
        {
            liControl.Attributes.Add("id", "li_report_access");
        }
        liControl.Controls.Add(aTagControl);

        return liControl;
    }

    #endregion Private Methods

    #endregion Methods
}