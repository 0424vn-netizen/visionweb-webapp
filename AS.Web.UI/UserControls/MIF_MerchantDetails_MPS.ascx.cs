using AS.Common.DBManager;
using AS.Controls.Pages;
using System;
using System.Collections.Generic;
using System.Data;

public partial class UserControls_MIF_MerchantDetails_MPS : GlobalUserControl
{
    #region Propertise using for Case Management

    public bool IsCaseManagement
    {
        get
        {
            bool re = false;
            if (ViewState["IsCaseManagement"] != null)
                Boolean.TryParse(ViewState["IsCaseManagement"].ToString(), out re);
            return re;
        }
        set
        {
            ViewState["IsCaseManagement"] = value;
        }
    }

    private string MerchantNumber
    {
        get
        {
            string re = string.Empty;
            if (ViewState["MerchantNumber"] != null)
                re = ViewState["MerchantNumber"].ToString();
            return re;
        }
        set
        {
            ViewState["MerchantNumber"] = value;
        }
    }

    protected string OptedIn
    {
        get
        {
            string re = string.Empty;
            if (ViewState["OptStatus"] != null)
                re = ViewState["OptStatus"].ToString();
            return re;
        }
        set
        {
            ViewState["OptStatus"] = value;
        }
    }

    protected string MifEmail
    {
        get
        {
            string re = string.Empty;
            if (ViewState["MifEmail"] != null)
                re = ViewState["MifEmail"].ToString();
            return re;
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
    #endregion

    #region Constants

    protected const string OPT_IN = "0";

    private const string SPA_GET_MERCHANT_PROFILE = "spa_cs_GetMerchantProfile_MPS";

    protected enum DataBindAction
    {
        BindMerchantDetails
    }

    #endregion Constants

    #region Fields

    string[] noprefix = { "mr ", "ms ", "sir ", "jr ", "mr.", "ms.", "sir.", "jr." };
    bool isShowRelationshipManager = GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_RELATIONSHIP_MANAGER").Equals("true") ? true : false;

    #endregion Fields

    #region Propeties

    private string RelationShipManager
    {
        get;
        set;
    }

    #endregion Propeties

    #region Methods

    protected string SetURLForSiteAccessButton()
    {
        return MerchantProfileHelper.BuildURLForSiteAccessInMIF(
            (SecurePage)Page, MerchantNumber, OptedIn, MifEmail);
    }

    private object ProcessNullValue(object obj)
    {
        return obj.GetType() == typeof(DBNull) ? null : obj;
    }

    protected string SetStatusText(object obj)
    {
        return MerchantProfileHelper.SetStatusForSiteAccess(obj);
    }

    private bool HasMSProductEnvironment()
    {
        return GeneralFuncsLib.HasMSProductEnvironment();
    }

    protected bool SetVisible(object obj)
    {
        return MerchantProfileHelper.SetVisibleSiteAccessLink(obj, (SecurePage)Page);
    }

    protected string FormatPhone(object phone)
    {
        return AS.Common.Formater.FormatData.FormatPhoneNumber(phone.ToString());
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        base.OnDataBindControls(type, sender);
        if (IsCaseManagement)
            ReportPage.ReportFilter.CurrentValue.Value = Page.SecureQueryString["MerchantNumber"];
        this.MerchantNumber = ReportPage.ReportFilter.CurrentValue.Value;
        switch ((DataBindAction)type)
        {

            case DataBindAction.BindMerchantDetails:
                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.AnsiString));
                parameters.Add(new FilterParameter("@FlagPermission", SessionManager.CurrentUserViewMode, DbType.Int32));
                DataTable dtMerch = WebServices.CsReportServices.GetReports(SPA_GET_MERCHANT_PROFILE, parameters);
                DataRow drMerch = (dtMerch.Rows.Count > 0) ? dtMerch.Rows[0] : null;

                if (dtMerch.Rows.Count == 0)
                {
                    uxPnlDetail.Visible = false;
                    uxNorecords.Visible = true;
                }
                else
                {
                    uxPnlDetail.Visible = true;
                    uxNorecords.Visible = false;
                }
                // Region merchant information
                this.uxMI_MerchantNumber.Text = this.DoVeraCode(drMerch, "MerchantNumber");
                this.uxMI_MerchantName.Text = this.DoVeraCode(drMerch, "MerchantName");
                this.uxMI_Phone.Text = FormatPhone(this.DoVeraCode(drMerch, "Phone"));
                this.uxMI_Fax.Text = FormatPhone(this.DoVeraCode(drMerch, "Fax"));
                this.uxMI_MerchantAddress1.Text = this.DoVeraCode(drMerch, "Address1");
                this.uxMI_MerchantAddress2.Text = this.DoVeraCode(drMerch, "Address2");
                this.uxMI_MerchantCityStateZip.Text =
                    this.DoVeraCode(drMerch, "City")
                    + (string.IsNullOrEmpty(this.DoVeraCode(drMerch, "State").Trim()) == true
                            ? string.Empty : ", " + this.DoVeraCode(drMerch, "State"))
                    + (string.IsNullOrEmpty(this.DoVeraCode(drMerch, "Zip").Trim()) == true
                            ? string.Empty : ", " + this.DoVeraCode(drMerch, "Zip"));

                this.uxMI_CooperateName.Text = this.DoVeraCode(drMerch, "CorporateName");
                this.uxMI_CooperateAddressLine.Text = this.DoVeraCode(drMerch, "CorporateAddressLine1")
                    + (string.IsNullOrEmpty(this.DoVeraCode(drMerch, "CorporateAddressLine2").Trim()) == true
                        ? string.Empty : ", " + this.DoVeraCode(drMerch, "CorporateAddressLine2"));
                this.uxMI_CooperateCityStateZip.Text = this.DoVeraCode(drMerch, "CorporateCity")
                    + (string.IsNullOrEmpty(this.DoVeraCode(drMerch, "CorporateState").Trim()) == true
                        ? string.Empty : ", " + this.DoVeraCode(drMerch, "CorporateState"))
                    + (string.IsNullOrEmpty(this.DoVeraCode(drMerch, "CorporateZip").Trim()) == true
                        ? string.Empty : ", " + this.DoVeraCode(drMerch, "CorporateZip"));
                this.uxMI_Processor.Text = this.DoVeraCode(drMerch, "BEProcessor");

                // Region Ownership information
                this.uxOI_OwnerName.Text = this.DoVeraCode(drMerch, "Owner");
                this.uxMI_1stDepositDate.Text = this.FormatDate(drMerch, "FirstDepositDate");
                this.uxMI_SICMCCCode.Text = this.DoVeraCode(drMerch, "SICCode")
                    + (string.IsNullOrEmpty(this.DoVeraCode(drMerch, "SICCodeDesc").Trim()) == true
                            ? string.Empty : " - " + this.DoVeraCode(drMerch, "SICCodeDesc"));

                this.uxMarketData.Text = this.DoVeraCode(drMerch, "MDDescription");

                // Hierarchy Information
                string chainNumber = GetHierarchyNumber(this.DoVeraCode(drMerch, "Chain"), true);
                if (CheckHierarchy("CHN"))
                {
                    uxLHChain.Visible = false;
                    uxHChain.Visible = true;
                    this.uxHI_Chain.Text = chainNumber;
                    this.uxLinkChain.Attributes.Add("onclick", "return rf_SubmitReportFilterValues(85, 'CHN', '" + chainNumber + "');");
                }
                else
                {
                    uxLHChain.Visible = true;
                    uxHChain.Visible = false;
                    this.uxLHChain.Text = chainNumber;
                }
                // Activity Status
                this.uxAS_MerchantStatus.Text = this.DoVeraCode(drMerch, "MerchantStatus");
                this.uxAS_SeasonalCode.Text = this.DoVeraCode(drMerch, "Seasonal");
                this.uxAS_DateOpened.Text = this.FormatDate(drMerch, "ApprovalDate");
                this.uxAS_DateClosed.Text = this.FormatDate(drMerch, "ClosedDate");
                DateTime dLastActiveDate = new DateTime(1970, 1, 1);
                bool dateParse = DateTime.TryParse(this.FormatDate(drMerch, "LastActiveDate"), out dLastActiveDate);
                if (dateParse && dLastActiveDate.Year > 1970)
                {
                    string sLastActiveDate = dLastActiveDate.Ticks.ToString();
                    //string url = "<a href='BatchHistory.aspx?" + Page.BuildSecureQueryString(string.Format("date={0}&merchantnumber={1}", sLastActiveDate, MerchantNumber)) + "'>" + this.FormatDate(drMerch, "LastActiveDate") + "</a>";
                    string url = GeneralFuncsLib.BuildLastBatchHistoryLink(
                        (SecurePage)Page, sLastActiveDate, MerchantNumber, this.FormatDate(drMerch, "LastActiveDate"));
                    this.uxAS_DateLastActive.Text = url;
                }
                else
                {
                    this.uxAS_DateLastActive.Text = string.Empty;
                }
                if (HasMSProductEnvironment())
                {
                    this.uxAS_OptIn.Text = this.DoVeraCode(drMerch, "OptOut");
                }
                else
                {
                    this.uxAS_OptIn.Text = GetLocalResourceObject("MIF_MerchantDetail_MPSJS_Text_NotAvailable").ToString();
                }
                this.uxHddOptOut.Value = this.DoVeraCode(drMerch, "OptOut");

                this.uxBankName.Text = this.DoVeraCode(drMerch, "BankName");
                this.uxDDANumber.Text = CheckPermisson(this.DoVeraCode(drMerch, "DDANumber"), WebSiteConstants.SEC_PERMISSION_DDA);
                this.uxMI_TaxID.Text = CheckPermisson(this.DoVeraCode(drMerch, "CompanyTaxID"), WebSiteConstants.SEC_PERMISSION_TAX_ID);
                this.uxRoutingNumber.Text = GetRoutingNumber(this.DoVeraCode(drMerch, "TransitNumber"), this.DoVeraCode(drMerch, "PartialTransitNumber"));


                if (drMerch != null)
                {
                    this.uxAS_ButtonOpt.Visible = false;
                    bool isOptOut = drMerch["OptOut"].ToString() == OPT_IN;
                    if (HasMSProductEnvironment())
                    {
                        this.uxAS_OptIn.Text = this.setOptInStatus(drMerch["OptOut"].ToString(), uxAS_MerchantStatus.Text);
                    }
                    else
                    {
                        this.uxAS_OptIn.Text = GetLocalResourceObject("LiteralResource29").ToString();
                    }
                    if (uxAS_MerchantStatus.Text.ToLower().Equals("open")
                        || uxAS_MerchantStatus.Text.ToLower().Equals("reopened"))
                    {
                        if (HasMSProductEnvironment()
                            && (Page.IsUserWithPermission("OptInOut")
                            || Page.IsUserWithPermission("MSOptInOut")))
                        {
                            this.uxAS_ButtonOpt.Visible = true;
                        }
                        if (isOptOut)
                        {
                            this.uxAS_ButtonOpt.Text = GetLocalResourceObject("LiteralResource30").ToString();
                            this.OptedIn = "1";
                        }
                        else
                        {
                            this.uxAS_ButtonOpt.Text = GetLocalResourceObject("LiteralResource31").ToString();
                            this.OptedIn = "0";
                        }
                    }

                    if (drMerch["Email"] != null)
                        this.MifEmail = drMerch["Email"].ToString();

                }

                this.uxAS_ButtonOpt.OnClientClick = this.SetURLForSiteAccessButton();
                //  Relationship Manager
                if (dtMerch.Rows.Count > 0 && dtMerch.Columns.Contains("RelationShipManager"))
                {
                    RelationShipManager = dtMerch.Rows[0]["RelationShipManager"].ToString();
                }

                // ShowRelationshipManager
                ShowHideRelationshipManager();

                break;
        }
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
            return string.Empty;
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

    protected string setOptInStatus(object OptIn, string MerchantStatus)
    {
        return MerchantProfileHelper.SetOptInStatus(OptIn, MerchantStatus);
    }

    public string GetRoutingNumber(string full, string partial)
    {
        return MerchantProfileHelper.GetRoutingNumberWithoutDecrypt(full, partial);
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
        //this.DoMultiExportExcel();
        this.OnDataBindControls(DataBindAction.BindMerchantDetails, this);
    }

    protected string CheckPermisson(object obj, string permissionCode)
    {
        return MerchantProfileHelper.CheckPermisson(obj, permissionCode, Page);
    }

    protected bool CheckHierarchy(string hierarchy)
    {
        return MerchantProfileHelper.CheckHierarchy(hierarchy);
    }

    protected override void OnLoad(EventArgs e)
    {
        uxpnlSaveCancel.Visible = false;
        ShowHideRelationshipManager();
    }

    protected void uxbtnCancel_click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(RelationShipManager))
        {
            uxbtnAdd.Visible = true;
            uxbtnEdit.Visible = false;
        }
        else
        {
            uxbtnAdd.Visible = false;
            uxbtnEdit.Visible = true;
            uxlbRelationshipManager.Text = RelationShipManager;
            uxlbRelationshipManager.Visible = true;
        }
        uxpnlSaveCancel.Visible = false;
    }

    protected void uxbtnEdit_click(object sender, EventArgs e)
    {
        uxbtnAdd.Visible = false;
        uxbtnEdit.Visible = false;
        uxpnlSaveCancel.Visible = true;
        uxtxtRelationShipManager.Text = RelationShipManager;
        uxlbRelationshipManager.Visible = false;
    }

    protected void uxbtnAdd_click(object sender, EventArgs e)
    {
        uxbtnAdd.Visible = false;
        uxbtnEdit.Visible = false;
        uxpnlSaveCancel.Visible = true;
        uxtxtRelationShipManager.Text = string.Empty;
        uxlbRelationshipManager.Text = string.Empty;
        uxlbRelationshipManager.Visible = false;
    }

    protected void uxbtnSave_click(object sender, EventArgs e)
    {

        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentClient, DbType.Int32));
        parameters.Add(new FilterParameter("@MerchantNumber", GetMerchantNrParameter(), DbType.AnsiString));
        parameters.Add(new FilterParameter("@RelationshipManager", uxtxtRelationShipManager.Text.Trim(), DbType.String));
        WebServices.CsReportServices.ExecuteNonQueryCommand(
            MerchantProfileHelper.SPA_UPDATE_RELATIONSHIP_MANAGER, parameters, out parameters);

        uxpnlSaveCancel.Visible = false;
        if (uxtxtRelationShipManager.Text.Trim() == string.Empty)
        {
            uxbtnAdd.Visible = true;
            uxbtnEdit.Visible = false;
            uxlbRelationshipManager.Text = string.Empty;
        }
        else
        {
            uxbtnEdit.Visible = true;
            uxbtnAdd.Visible = false;
            uxlbRelationshipManager.Text = uxtxtRelationShipManager.Text.Trim();
            uxlbRelationshipManager.Visible = true;
        }
    }

    #region Private Methods

    private void ShowHideRelationshipManager()
    {
        pnlRelationshipmanager.Visible = isShowRelationshipManager;
        uxlbRelationshipManager.Text = string.Empty;
        if (isShowRelationshipManager)
        {
            uxpnlRMLabel.Visible = true;
            uxpnlRMCtrls.Visible = true;
            if (string.IsNullOrEmpty(RelationShipManager))
            {
                if (GeneralFuncsLib.HasRelationshipManagerPermission((ReportPage)Page))
                {
                    uxbtnAdd.Visible = true;
                    uxbtnEdit.Visible = false;
                    uxlbRelationshipManager.Visible = false;
                    uxlbRelationshipManager.Text = string.Empty;
                }
                else
                {
                    uxbtnAdd.Visible = false;
                    uxbtnEdit.Visible = false;
                }
            }
            else
            {
                if (GeneralFuncsLib.HasRelationshipManagerPermission((ReportPage)Page))
                {
                    uxbtnAdd.Visible = false;
                    uxbtnEdit.Visible = true;
                }
                else
                {
                    uxbtnAdd.Visible = false;
                    uxbtnEdit.Visible = false;
                }
                uxlbRelationshipManager.Visible = true;
                uxlbRelationshipManager.Text = RelationShipManager;
            }
        }
        else
        {
            uxpnlRMLabel.Visible = false;
            uxpnlRMCtrls.Visible = false;
        }
    }

    private string GetMerchantNrParameter()
    {
        return IsCaseManagement ? Page.SecureQueryString["MerchantNumber"] : ReportPage.ReportFilter.CurrentValue.Value;
    }

    #endregion Private Methods

    #endregion Methods
}
