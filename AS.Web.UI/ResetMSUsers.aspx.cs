using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AS.Controls.Pages;
using AS.Security.WS.Entities;
using AS.Common.DBManager;
using AS.Common;
using AS.Controls.Pages;

[PagePermission("RstMerPwd")]
public partial class ResetMSUsers : NonReportPage
{
    #region Enums
   

    enum PostBackAction
    {
        ResetMSPassword
        
    }

    #endregion

    private string MSG_Invalid_MS_User_Name = string.Empty;
    private string MSG_Merchant_Opted_Out = string.Empty;
    private string MSG_Validate_SSO_UserName = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        MSG_Invalid_MS_User_Name = GetLocalResourceObject("ResetMSUsers_aspx_cs_InvalidMSUserName").ToString();
        MSG_Merchant_Opted_Out = GetLocalResourceObject("ResetMSUsers_aspx_cs_MerchantOptedOut").ToString();
        MSG_Validate_SSO_UserName = GetLocalResourceObject("ResetMSUsers_aspx_cs_Validate_SSO_UserName").ToString();
        uxMsg.Message = "";        

        btnSubmit.Attributes["onclick"] = "return ValidateInput()";
        uxMsg.ShowOnLoad = false;
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.ResetMSPassword:
                {
                    string username = txtUsername.Text.Trim();
                    txtUsernameLabel.CssClass = "valign-middle";
                    //Check user existed
                    User user = WebServices.SecurityServices.GetUser(SessionManager.CurrentClient, username);

                    var isSSOUser = GeneralFuncsLib.IsSSOUser(WebSiteConstants.SOFTEK_CLIENT, username);

                    if (isSSOUser)
                    {
                        ShowMessageBox(MSG_Validate_SSO_UserName);
                        return;
                    }

                   if (user != null && !IsInactiveUser(user))
                    {

                        //Check CS User
                        if (user.SiteID == 0)
                        {
                            ShowMessageBox(MSG_Invalid_MS_User_Name);
                            return;
                        }


                        FilterParameterCollection param = new FilterParameterCollection();
                        param.Add(new FilterParameter("@ASClientID", SessionManager.CurrentClient, DbType.Int32));
                        param.Add(new FilterParameter("@HierarchyMode", "MERCHANTNUMBER", DbType.AnsiString));
                        DataTable dt = WebServices.CsReportServices.GetReports("spa_config_GetEntityTypeOfHierarchyMode", param);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            //Check user whether is Merchant Primary or not
                            if (user.EntityType == Int32.Parse(dt.Rows[0]["EntityType"].ToString()))
                            {
                                //Check merchant Opted Out - Close
                                int merStatus = CheckMerchantOptedOut_Closed(user.EntityID);

                                if (merStatus == 1)   //1: Opted Out or Closed
                                {
                                    ShowMessageBox(MSG_Merchant_Opted_Out);
                                    return;
                                }

                            }


                        }
                        SessionManager.ResetPasswordUser = user;
                        string js = string.Format("setTimeout('ShowPopupModal(\"{0}\",\"auto\")',500);", "ManageUserResetPwd_Step1.aspx");

                        Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenModal", js, true);
                    }
                    else
                    {
                        ShowMessageBox(MSG_Invalid_MS_User_Name);
                    }
                }
                break;

        }
    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;

        OnPostBackActions(PostBackAction.ResetMSPassword);

    }


    /// <summary>
    /// Compare Merchant whether is Opted Out or Close
    /// </summary>
    /// <returns></returns>
    private int CheckMerchantOptedOut_Closed(string merchantNum)
    {
        FilterParameterCollection _parameters = null;
        _parameters = new FilterParameterCollection();
        _parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        _parameters.Add(new FilterParameter("@MerchantNumber", merchantNum, System.Data.DbType.String));

        DataTable _datasource = WebServices.SecurityServices.GetReports("spa_cs_GetMerchantOptOutStatus", _parameters);
        if (_datasource != null && _datasource.Rows.Count > 0)
        {
            if (_datasource.Rows[0]["OptOut"].ToString() == "0")  // Opted In
            {
                return 0;
            }
            else
            {
                return 1;
            }
          
        }

        return 0;
    }

    private bool IsInactiveUser(User user)
    {     
        string entityTypeIDs = GeneralFuncsLib.GetDataOfExtendedSetting("DisableResetPasswordForInactiveUser");
        return user.ActvStat == "0" && GeneralFuncsLib.DisableResetPasswordForInactiveUser(user.EntityType.ToString(), entityTypeIDs);     
    }


    private string GetHyperlinkString(string url, string hpkText, int width, int height)
    {
        string re;
        if (width != null && height != null)
        {
            re = string.Format("<a href='#' onclick=\"openPopupModal('{0}',{1},{2});return false;\">{3}</a>",
                    url, width.ToString(), height.ToString(), hpkText);
            return re;
        }
        else
        {
            return string.Format("<a href='#' onclick=\"openPopupModal('{0}');return false;\">{1}</a>",
                   url, hpkText);
        }

    }
    private void ShowMessageBox(string message)
    {
        uxMsg.Message = VeraCodeSolution.DoVeraCode(message);
        uxMsg.ShowOnLoad = true;
        txtUsernameLabel.CssClass = RiskGeneral.CSS_ERROR_LABEL;
    } 
     
}
