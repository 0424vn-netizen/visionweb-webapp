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

[PagePermission("ResetSubHierarchyPassword")]
public partial class ResetUserPwd : NonReportPage
{
    #region Enums


    enum PostBackAction
    {
        ResetUserPassword

    }

    #endregion

    private string MSG_Invalid_User_Name = string.Empty;
    private string MSG_Merchant_Opted_Out = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        MSG_Invalid_User_Name = GetLocalResourceObject("ResetUserPwd_aspx_cs_InvalidMSUsername").ToString();
        MSG_Merchant_Opted_Out = GetLocalResourceObject("ResetUserPwd_aspx_cs_MerchantOptedOut").ToString();
        uxMsg.Message = "";

        btnSubmit.Attributes["onclick"] = "return ValidateInput()";
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.ResetUserPassword:
                {
                    string username = txtUsername.Text.Trim();

                    //Check user existed
                    User user = WebServices.SecurityServices.GetUser(SessionManager.CurrentClient, username);

                    if (user != null)
                    {

                        //Check CS User
                        if (user.SiteID == 0)
                        {
                            ShowMessage(MSG_Invalid_User_Name);
                            return;
                        }
                        
                        FilterParameterCollection param = new FilterParameterCollection();
                        param.Add(new FilterParameter("@ASClientID", SessionManager.CurrentClient, DbType.Int32));
                        param.Add(new FilterParameter("@UserID", SessionManager.CurrentUser.UserID, DbType.String));
                        param.Add(new FilterParameter("@UserPwdChange", username, DbType.String));
                        DataTable checkBelongToUser = WebServices.SecurityServices.GetReports("spa_SEC_CheckUserAndMerchantBelongedTo", param);
                        if (checkBelongToUser != null || checkBelongToUser.Rows.Count > 0)
                        {
                            if (checkBelongToUser.Rows[0]["IsUserBelong"].ToString() == "0")
                            {
                                ShowMessage(MSG_Invalid_User_Name);
                                return;
                            }
                        }
                        else
                        {
                            ShowMessage(MSG_Invalid_User_Name);
                            return;
                        }

                        param = new FilterParameterCollection();
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
                                    ShowMessage(MSG_Merchant_Opted_Out);
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
                        ShowMessage(MSG_Invalid_User_Name);
                    }
                }
                break;

        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;

        OnPostBackActions(PostBackAction.ResetUserPassword);

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

    private void ShowMessage(string message)
    {
        uxMsg.Message = VeraCodeSolution.DoVeraCode(message);
        uxMsg.ShowOnLoad = true;
        txtUsernameLabel.CssClass = RiskGeneral.CSS_ERROR_LABEL;
    } 
}
