using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Common.DBManager;
using System.Data;
using AS.Common;
using System.ComponentModel;
using AS.Controls.Pages;
using AS.Controls.Validators;

[PagePermission("ASLandingPage")]
public partial class ClientSetting : NonReportPage
{
    public const string PRODUCT_NAME_DEFAUT = "VisionWeb";

    private int ClientID
    {
        get
        {
            return Convert.ToInt32(SecureQueryString["id"]);
        }

    }

    bool AllowEnterFullContactEmail
    {
        get
        {
            return GeneralFuncsLib.GetDataOfExtendedSetting("AllowEnterFullContactEmail", ClientID.ToString()).Equals("true", StringComparison.OrdinalIgnoreCase);
        }
    }

    void InitContactEmailControl()
    {
        if (AllowEnterFullContactEmail)
        {
            uxContactEmail.Width = new Unit("100%");
            uxContactEmailSuffixPanel.Visible = false;
            //
            uxNoReply.Width = new Unit("100%");
            uxNoReplySuffixPanel.Visible = false;
            //
            foreach (ValidationItem item in uxValidator.Items)
            {

                if (item.ControlToValidateID.Equals("uxContactEmail") && ((BasicValidationItem)item).Rule == ValidationRule.StringUnaccept)
                {
                    ((BasicValidationItem)item).Pattern = "<>#&";
                    ((BasicValidationItem)item).Message = GetLocalResourceObject("ClientSetting_aspx_BasicValidator_StringUnaccept8.Message").ToString();
                }
                if (item.ControlToValidateID.Equals("uxNoReply") && ((BasicValidationItem)item).Rule == ValidationRule.StringUnaccept)
                {
                    ((BasicValidationItem)item).Pattern = "<>#&";
                    ((BasicValidationItem)item).Message = GetLocalResourceObject("ClientSetting_aspx_BasicValidator_StringUnaccept9.Message").ToString();
                }
            }
        }
        else
        {
            for (int i = uxValidator.Items.Count - 1; i > 0; i--)
            {
                if (uxValidator.Items[i].ControlToValidateID.Equals("uxContactEmail") && ((BasicValidationItem)uxValidator.Items[i]).Rule == ValidationRule.Email)
                {
                    uxValidator.Items.Remove(uxValidator.Items[i]);
                }
                if (uxValidator.Items[i].ControlToValidateID.Equals("uxNoReply") && ((BasicValidationItem)uxValidator.Items[i]).Rule == ValidationRule.Email)
                {
                    uxValidator.Items.Remove(uxValidator.Items[i]);
                }
            } 

        }
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        InitContactEmailControl();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        if (!IsPostBack)
        {
            // Get description from default database
            this.LoadClientDescription();
            // Switch to the database of current client
            SessionManager.CurrentClient = ClientID;
            this.LoadClientDetails();
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }
    protected void LoadClientDetails()
    {
        string spaName = "spa_SEC_GetClientForUpdate";
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient", ClientID, DbType.Int32));
        DataTable dt = WebServices.SecurityServices.GetReports(spaName, parameters);
        if (dt != null && dt.Rows.Count > 0)
        {
            string clientEmail = VeraCodeSolution.DoVeraCode(dt.Rows[0]["ClientEmail"].ToString());
            string contactEmailFrom = VeraCodeSolution.DoVeraCode(dt.Rows[0]["ContactEmailFrom"].ToString());
            string contactEmailTo = VeraCodeSolution.DoVeraCode(dt.Rows[0]["ContactEmailTo"].ToString());
            string noreplayEmail = VeraCodeSolution.DoVeraCode(dt.Rows[0]["NoreplyEmail"].ToString());
            string productName;
            if(dt.Rows[0]["ProductName"] == null || string.IsNullOrEmpty(dt.Rows[0]["ProductName"].ToString()))
                productName = PRODUCT_NAME_DEFAUT;
            else
                productName = VeraCodeSolution.DoVeraCode(dt.Rows[0]["ProductName"].ToString());

            uxClientName.Text = VeraCodeSolution.DoVeraCode(dt.Rows[0]["ClientName"].ToString());
            uxClientAdd1.Text = VeraCodeSolution.DoVeraCode(dt.Rows[0]["ClientAddress1"].ToString());
            uxClientAdd2.Text = VeraCodeSolution.DoVeraCode(dt.Rows[0]["ClientAddress2"].ToString());
            uxClientZip.Text = VeraCodeSolution.DoVeraCode(dt.Rows[0]["ClientZipCode"].ToString());
            uxClientPhone.Text = VeraCodeSolution.DoVeraCode(dt.Rows[0]["ClientPhone"].ToString());
            uxClientFax.Text = VeraCodeSolution.DoVeraCode(dt.Rows[0]["ClientFax"].ToString());
            uxProductName.Text = VeraCodeSolution.DoVeraCode(productName);

            uxClientEmail.Text = VeraCodeSolution.DoVeraCode(!clientEmail.IsNullOrEmpty() ? clientEmail : string.Empty);
            uxContactEmailTo.Text = VeraCodeSolution.DoVeraCode(!contactEmailTo.IsNullOrEmpty() ? contactEmailTo : string.Empty);
            if (AllowEnterFullContactEmail)
            {
                uxContactEmail.Text = VeraCodeSolution.DoVeraCode(contactEmailFrom);
                uxNoReply.Text = VeraCodeSolution.DoVeraCode(noreplayEmail);
            }
            else
            {
                uxContactEmail.Text = VeraCodeSolution.DoVeraCode(!contactEmailFrom.IsNullOrEmpty() ? contactEmailFrom.Split('@')[0] : string.Empty);
                uxNoReply.Text = VeraCodeSolution.DoVeraCode(!noreplayEmail.IsNullOrEmpty() ? noreplayEmail.Split('@')[0] : string.Empty);
            }
            uxContactEmailSuffix.Text = VeraCodeSolution.DoVeraCode(!contactEmailFrom.IsNullOrEmpty() ? "@" + contactEmailFrom.Split('@')[1] : string.Empty);
            uxNoReplySuffix.Text = VeraCodeSolution.DoVeraCode(!noreplayEmail.IsNullOrEmpty() ? "@" + noreplayEmail.Split('@')[1] : string.Empty);
            int restricPassword = Int32.Parse(dt.Rows[1]["RefTblKey"].ToString());
            uxRestrictionPassword.Text = VeraCodeSolution.DoVeraCode(restricPassword.ToString());
        }
    }
    protected void LoadClientDescription()
    {
        string spaName = "spa_SEC_GetClient";
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient", ClientID, DbType.Int32));
        DataTable dt = WebServices.SecurityServices.GetReports(spaName, parameters);
        if (dt != null && dt.Rows.Count > 0)
        {
            uxDescription.Text = VeraCodeSolution.DoVeraCode(dt.Rows[0]["ClientDescription"].ToString());
        }
    }
    protected void SaveClientInfo(object sender, EventArgs e)
    {
        // Save to Ref_TableValue
        FilterParameterCollection paramIns = new FilterParameterCollection();
        FilterParameterCollection paramOuts = new FilterParameterCollection();
        paramIns.Add(new FilterParameter("@ASClient", this.ClientID, DbType.Int32));
        paramIns.Add(new FilterParameter("@ClientName", uxClientName.Text, DbType.AnsiString));
        paramIns.Add(new FilterParameter("@ClientAddress1", uxClientAdd1.Text, DbType.AnsiString));
        paramIns.Add(new FilterParameter("@ClientAddress2", uxClientAdd2.Text, DbType.AnsiString));
        paramIns.Add(new FilterParameter("@ClientZipCode", uxClientZip.Text, DbType.AnsiString));
        paramIns.Add(new FilterParameter("@ClientPhone", uxClientPhone.Text, DbType.AnsiString));
        paramIns.Add(new FilterParameter("@ClientFax", uxClientFax.Text, DbType.AnsiString));
        paramIns.Add(new FilterParameter("@ClientEmail", uxClientEmail.Text, DbType.AnsiString));
        paramIns.Add(new FilterParameter("@ContactEmailTo", uxContactEmailTo.Text, DbType.AnsiString));
        paramIns.Add(new FilterParameter("@ContactEmailFrom", AllowEnterFullContactEmail ? uxContactEmail.Text : string.Format("{0}{1}", uxContactEmail.Text, uxContactEmailSuffix.Text), DbType.AnsiString));
        paramIns.Add(new FilterParameter("@NoreplyEmail", AllowEnterFullContactEmail ? uxNoReply.Text : string.Format("{0}{1}", uxNoReply.Text, uxContactEmailSuffix.Text), DbType.AnsiString));
        paramIns.Add(new FilterParameter("@ProductName", uxProductName.Text, DbType.AnsiString));
        paramIns.Add(new FilterParameter("@MinPasswordHistory", uxRestrictionPassword.Text, DbType.AnsiString));
        int isSave = 1;
        try
        {
            WebServices.SecurityServices.GetReports("spa_SEC_UpdateClientInfo", paramIns);

            // Save Client Description to SEC_Client
            SessionManager.CurrentClient = SessionManager.CurrentUser.ASClient;
            paramIns.Clear();
            paramOuts.Clear();
            paramIns.Add(new FilterParameter("@ASClient", this.ClientID, DbType.Int32));
            paramIns.Add(new FilterParameter("@ClientDescription", uxDescription.Text, DbType.AnsiString));
            WebServices.SecurityServices.GetReports("spa_SEC_UpdateClient", paramIns);
        }
        catch (Exception)
        {

            isSave = 0;
        }

        // Switch to the database of current client
        SessionManager.CurrentClient = this.ClientID;
        if (isSave == 0)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyAlert", "alert('" + GetLocalResourceObject("ClientSetting_aspx_cs_SaveDataFail").ToString() + "');", true);
        }
        else
        {
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "CloseModal", "ClosePopupModal();", true);
        }
    }
}
