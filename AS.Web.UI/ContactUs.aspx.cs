using System;
using AS.Common.DBManager;
using System.Data;
using AS.Controls.Pages;
using AS.Common;
using AS.Common.Mail;
using System.Globalization;

namespace As.VisionWeb.Web
{
    [PagePermission("MSContUs")]
    public partial class ContactUs : ReportPage
    {
        enum DataBindAction
        {
            DoSendMail,
        }

        public bool IsMLSClient
        {
            get { return SessionManager.CurrentClient == WebSiteConstants.MLS_CLIENT; }
        }

        public bool IsHideContactInfo
        {
            get { return GeneralFuncsLib.GetDataOfExtendedSetting("IsHideContactInfo").Equals("true"); }
        }

        protected override void PageInitialize()
        {
            base.PageInitialize();
            this.IsSecureCSRF = true;
        }
        private string EntityDisplayNameText
        {
            get
            {
                string txtEntityDisplayName = GeneralFuncsLib.GetDataOfExtendedSetting("ContactUs_EntityDisplayName");
                if (string.IsNullOrEmpty(txtEntityDisplayName))
                {
                    txtEntityDisplayName = "ltMerchantID.Text";
                }
                return txtEntityDisplayName;
            }
        }
        public string EntityDisplayNameText_RequiredMessage
        {
            get
            {
                var entityName = GetLocalResourceObject(EntityDisplayNameText).ToString();
                var requiredMessage = GetLocalResourceObject("RequiredFieldMessage").ToString();
                requiredMessage = string.Format(requiredMessage, entityName.Replace(":", ""));
                return requiredMessage;
            }
        }
        private void BindSubject()
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentClient, System.Data.DbType.Int32));
            parameters.Add(new FilterParameter("@RefType", "SUBJECT", System.Data.DbType.AnsiString));
            parameters.AddLanguageID();
            DataTable dtSubject = WebServices.MsReportServices.GetReports("spa_GetRefTableValues", parameters);

            uxSubjectCustom.DataSource = dtSubject;
            uxSubjectCustom.DataValueField = "KeyValue";
            uxSubjectCustom.DataTextField = "KeyName";
            uxSubjectCustom.DataBind();
        }

        private void BindClientInfo()
        {
            uxContactTitle.Visible = uxContactInfo.Visible = !IsHideContactInfo;
            string ClientInformationKey = GeneralFuncsLib.GetClientContactInfoKey();
            if (!ClientInformationKey.IsNullOrEmpty())
            {
                SessionManager.ClientContactInforKey = ClientInformationKey;
            }

            if (SessionManager.ClientInfo != null)
            {
                uxCSHeader.Text = SessionManager.ClientInfo.ClientName + " " + GetLocalResourceObject("ContactUs_aspx_cs_ContactInfo").ToString();
            }
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32));
            parameters.Add(new FilterParameter("@EntityType", SessionManager.CurrentUser.EntityType, DbType.Int32));
            parameters.Add(new FilterParameter("@EntityNumber", SessionManager.CurrentUser.EntityID, DbType.AnsiString));
            parameters.Add(new FilterParameter("@ContactName", SessionManager.ClientContactInforKey, DbType.AnsiString));
            DataTable dt = WebServices.MsReportServices.GetReports("spa_GetContactUsInfoCustomize", parameters);
            if (dt != null && dt.Rows.Count > 0)
            {
                uxCSPhoneNumber.Text = VeraCodeSolution.ValidateResponseData(dt.Rows[0]["Phone"].ToString());
                uxCSFax.Text = VeraCodeSolution.ValidateResponseData(dt.Rows[0]["Fax"].ToString());                
                ValidatorLabel2.Text = GetLocalResourceObject(EntityDisplayNameText).ToString();
            }
            else
            {
                if (SessionManager.ClientInfo != null)
                {
                    uxCSPhoneNumber.Text = VeraCodeSolution.DoVeraCode(SessionManager.ClientInfo.Phone);
                    uxCSFax.Text = VeraCodeSolution.DoVeraCode(SessionManager.ClientInfo.Fax);                    
                    ValidatorLabel2.Text = GetLocalResourceObject(EntityDisplayNameText).ToString();
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindClientInfo();
                BindSubject();
            }
            // TK26080 - WRFC - Contact Us screen at MS_TK26080 
            BindMessage();
        }

        private void BindMessage()
        {
            string message = GeneralFuncsLib.GetContactUsMessage();
            uxPlaceHolderMessage.Visible = !message.IsNullOrEmpty();
            if (uxPlaceHolderMessage.Visible)
            {
                uxMessage.Text = VeraCodeSolution.GetOutputHtmlString(message);
            }
        }

        protected void uxSendEmail_Click(object sender, EventArgs e)
        {
            OnDataBindControls(DataBindAction.DoSendMail);
        }

        protected override void OnDataBindControls(Enum type, object sender)
        {
            switch ((DataBindAction)type)
            {
                case DataBindAction.DoSendMail:
                    string spaName = "spa_SEC_GetClientForUpdate";
                    FilterParameterCollection parameters = new FilterParameterCollection();
                    parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentClient, DbType.Int32));
                    string contactEmailFrom = string.Empty;
                    string contactEmailTo = string.Empty;


                    DataTable dt = WebServices.SecurityServices.GetReports(spaName, parameters);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        contactEmailFrom = VeraCodeSolution.DoVeraCode(dt.Rows[0]["ContactEmailFrom"].ToString());
                        contactEmailTo = VeraCodeSolution.DoVeraCode(dt.Rows[0]["ContactEmailTo"].ToString());
                    }

                    string mail_body = GetLocalResourceObject("ContactUs_aspx_cs_MailBody").ToString();

                    string from = uxContactEmail.Text.Trim();
                    string body = uxDescOfIssue.Text.Trim();
                    string subj = uxSubjectCustom.SelectedItem.Text;
                    mail_body = mail_body.Replace("<FROM_EMAIL>", contactEmailFrom);
                    mail_body = mail_body.Replace("<DATE>", DateTime.Now.ToString("dddd, MMMM dd, yyyy hh:mm tt", CultureInfo.CurrentCulture));
                    mail_body = mail_body.Replace("<TO_EMAIL>", contactEmailTo);
                    mail_body = mail_body.Replace("<SUBJECT>", subj);
                    mail_body = mail_body.Replace("<BEHALF_EMAIL>", from);

                    mail_body += "<br/><br/>";
                    mail_body += string.Format("{0} {1}<br/>", GetLocalResourceObject("ltBusinessName.Text").ToString(), txtBusinessName.Text.Trim());

                    mail_body += string.Format("{0} {1}<br/>", GetLocalResourceObject(EntityDisplayNameText).ToString(), txtMIDorVT.Text.Trim());
                    mail_body += string.Format("{0} {1}<br/>", GetLocalResourceObject("ltContactName.Text").ToString(), txtContactName.Text.Trim());
                    mail_body += string.Format("{0} {1}", GetLocalResourceObject("ltContactNumber.Text").ToString(), txtContactNumber.Text.Trim());

                    mail_body += "<br/><br/>";
                    mail_body += body;

                    bool result = SmtpMail.SendEmail(contactEmailFrom, contactEmailTo, subj, VeraCodeSolution.DoVeraCode(mail_body), WebSiteSettings.MailSettings);

                    if (result)
                    {
                        ClientScript.RegisterStartupScript(typeof(ContactUs), "ShowMsgBox", string.Format("setTimeout(\"alert('{0}');\", 500);", Resources.MessageManager.ContactUs_SuccessfulEmail), true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(typeof(ContactUs), "ShowMsgBox", string.Format("setTimeout(\"alert('{0}');\", 500);", Resources.MessageManager.ContactUs_FailEmail), true);
                    }
                    break;
            }
        }
    }
}


