using AS.Common;
using AS.Common.DBManager;
using AS.Common.Formater;
using AS.Common.WebServiceExport;
using AS.Controls.Exporter;
using AS.Controls.Grid;
using AS.Controls.UserControls;
using AS.Web.Business;
using AS.Web.UI.AppCode.General;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Web;
using System.Xml;
using Telerik.Web.UI;

public partial class UserControls_RelatedMerchants : GlobalUserControl
{
    private const string SPA_GET_RELATED_MERCHANTS = "spa_RM_MCF_RiskReport_GetRelatedMerchants";
    private const string MATCHED_COLOR = "#F0B2B2";
    private DataTable dtRelatedMerchants = null;
    enum DataBindAction
    {
        BindRelatedMerchantsGrid
    }

    public string MerchantNumber
    {
        get
        {
            if (ViewState["MerchantNum"] != null) return ViewState["MerchantNum"].ToString();
            else return string.Empty;
        }
        set
        {
            ViewState["MerchantNum"] = value;
        }
    }    

    protected void uxRelatedMerchant_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (string.IsNullOrEmpty(this.MerchantNumber))
        {
            uxRelatedMerchant.DataSource = new DataTable();
            return;
        }        

        if (dtRelatedMerchants != null)
        {
            ASRadControlHelper.BindGridWithPaging(uxRelatedMerchant, dtRelatedMerchants);
        }
        else
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserRiskParams();
            parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.String));

            string decryptDataParams = GetDecryptDataParams();

            if (!string.IsNullOrEmpty(decryptDataParams))
            {
                parameters.AddDecryptDataParams(decryptDataParams, _IsExporting);
            }

            uxRelatedMerchant.DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices,
                        WebSiteConstants.GET_REPORT_METHOD_NAME,
                        new object[] { SPA_GET_RELATED_MERCHANTS,
                ReportServices.ConvertToFilterParamWSArray(parameters) });
        }
    }   

    protected void uxRelatedMerchant_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;

            var merchantNameMatched = dataRow["MerchantNameMatched"].ToBoolean();
            var ssnMatched = dataRow["SSNMatched"].ToBoolean();
            var tinMatched = dataRow["TINMatched"].ToBoolean();
            var ownerNameMatched = dataRow["OwnerNameMatched"].ToBoolean();
            var phoneNumberMatched = dataRow["PhoneNumberMatched"].ToBoolean();
            var addressMatched = dataRow["AddressMatched"].ToBoolean();
            var emailMatched = dataRow["EmailMatched"].ToBoolean();
            var urlMatched = dataRow["URLMatched"].ToBoolean();
            var chainIDMatched = dataRow["ChainIDMatched"].ToBoolean();
            var corporateNameMatched = dataRow["CorporateNameMatched"].ToBoolean();
            var corporateAddressMatched = dataRow["CorporateAddressMatched"].ToBoolean();

            if (merchantNameMatched)
            {
                dataItem["MerchantName"].BackColor = ColorTranslator.FromHtml(MATCHED_COLOR);
            }

            if (ssnMatched)
            {
                if (PermissionManager.HasFullTaxView(Page))
                {
                    dataItem["EncryptedOwnerSSN"].BackColor = ColorTranslator.FromHtml(MATCHED_COLOR);
                }
                else
                {
                    dataItem["PartialOwnerSSN"].BackColor = ColorTranslator.FromHtml(MATCHED_COLOR);
                }
            }

            if (tinMatched)
            {
                if (PermissionManager.HasFullTaxView(Page))
                {
                    dataItem["EncryptedTaxId"].BackColor = ColorTranslator.FromHtml(MATCHED_COLOR);
                }
                else
                {
                    dataItem["PartialTaxId"].BackColor = ColorTranslator.FromHtml(MATCHED_COLOR);
                }
            }

            if (ownerNameMatched)
            {
                dataItem["OwnerName"].BackColor = ColorTranslator.FromHtml(MATCHED_COLOR);
            }

            if (phoneNumberMatched)
            {
                dataItem["PhoneNumber"].BackColor = ColorTranslator.FromHtml(MATCHED_COLOR);
            }

            if (addressMatched)
            {
                dataItem["Address"].BackColor = ColorTranslator.FromHtml(MATCHED_COLOR);
            }

            if (emailMatched)
            {
                dataItem["Email"].BackColor = ColorTranslator.FromHtml(MATCHED_COLOR);
            }

            if (urlMatched)
            {
                dataItem["URL"].BackColor = ColorTranslator.FromHtml(MATCHED_COLOR);
            }

            if (chainIDMatched)
            {
                dataItem["ChainID"].BackColor = ColorTranslator.FromHtml(MATCHED_COLOR);
            }

            if (corporateNameMatched)
            {
                dataItem["CorporateName"].BackColor = ColorTranslator.FromHtml(MATCHED_COLOR);
            }

            if (corporateAddressMatched)
            {
                dataItem["CorporateAddress"].BackColor = ColorTranslator.FromHtml(MATCHED_COLOR);
            }

            var merchantNumber = dataRow["MerchantNumber"].ToString();

            string queryString = Page.BuildSecureQueryString("MerchantNumber=" + merchantNumber);
            string url = "rm_MCF_RiskReport.aspx?" + queryString;
            string taga = "<a href=\"" + url + "\" style=\"cursor:pointer\">";
            dataItem["MerchantNumber"].Text = VeraCodeSolution.DoVeraCode(taga + merchantNumber + "</a>");
        }
    }

    private void ShowColumnsByPermissions()
    {
        if (PermissionManager.HasFullTaxView(Page))
        {
            uxRelatedMerchant.MasterTableView.Columns.FindByUniqueName("EncryptedTaxId").Visible = true;
            uxRelatedMerchant.MasterTableView.Columns.FindByUniqueName("PartialTaxId").Visible = false;

            uxRelatedMerchant.MasterTableView.Columns.FindByUniqueName("EncryptedOwnerSSN").Visible = true;
            uxRelatedMerchant.MasterTableView.Columns.FindByUniqueName("PartialOwnerSSN").Visible = false;
        }
        else
        {
            uxRelatedMerchant.MasterTableView.Columns.FindByUniqueName("EncryptedTaxId").Visible = false;
            uxRelatedMerchant.MasterTableView.Columns.FindByUniqueName("PartialTaxId").Visible = true;

            uxRelatedMerchant.MasterTableView.Columns.FindByUniqueName("EncryptedOwnerSSN").Visible = false;
            uxRelatedMerchant.MasterTableView.Columns.FindByUniqueName("PartialOwnerSSN").Visible = true;
        }

        if (PermissionManager.HasFullDDANumberView(Page))
        {
            uxRelatedMerchant.MasterTableView.Columns.FindByUniqueName("EncryptedDDANumber").Visible = true;
            uxRelatedMerchant.MasterTableView.Columns.FindByUniqueName("PartialDDANumber").Visible = false;
        }
        else
        {
            uxRelatedMerchant.MasterTableView.Columns.FindByUniqueName("EncryptedDDANumber").Visible = false;
            uxRelatedMerchant.MasterTableView.Columns.FindByUniqueName("PartialDDANumber").Visible = true;
        }
    }

    private bool _IsExporting = false;
    protected void uxExport_OnNeedExportConfig(object sender, ExportConfig exportConfig)
    {
        _IsExporting = true;
        uxRelatedMerchant.Columns.FindByUniqueName("EncryptedOwnerSSN").Visible = false;
        uxRelatedMerchant.Columns.FindByUniqueName("EncryptedTaxId").Visible = false;
        uxRelatedMerchant.Columns.FindByUniqueName("EncryptedDDANumber").Visible = false;

        uxRelatedMerchant.Columns.FindByUniqueName("MerchantName").Visible = false;
        uxRelatedMerchant.Columns.FindByUniqueName("MerchantNameExport").Visible = true;

        uxRelatedMerchant.Columns.FindByUniqueName("PartialOwnerSSN").Visible = false;
        uxRelatedMerchant.Columns.FindByUniqueName("PartialOwnerSSNExport").Visible = true;

        uxRelatedMerchant.Columns.FindByUniqueName("PartialTaxId").Visible = false;
        uxRelatedMerchant.Columns.FindByUniqueName("PartialTaxIdExport").Visible = true;

        uxRelatedMerchant.Columns.FindByUniqueName("OwnerName").Visible = false;
        uxRelatedMerchant.Columns.FindByUniqueName("OwnerNameExport").Visible = true;

        uxRelatedMerchant.Columns.FindByUniqueName("PhoneNumber").Visible = false;
        uxRelatedMerchant.Columns.FindByUniqueName("PhoneNumberExport").Visible = true;

        uxRelatedMerchant.Columns.FindByUniqueName("Address").Visible = false;
        uxRelatedMerchant.Columns.FindByUniqueName("AddressExport").Visible = true;

        uxRelatedMerchant.Columns.FindByUniqueName("Email").Visible = false;
        uxRelatedMerchant.Columns.FindByUniqueName("EmailExport").Visible = true;

        uxRelatedMerchant.Columns.FindByUniqueName("URL").Visible = false;
        uxRelatedMerchant.Columns.FindByUniqueName("URLExport").Visible = true;

        uxRelatedMerchant.Columns.FindByUniqueName("ChainID").Visible = false;
        uxRelatedMerchant.Columns.FindByUniqueName("ChainIDExport").Visible = true;

        uxRelatedMerchant.Columns.FindByUniqueName("CorporateName").Visible = false;
        uxRelatedMerchant.Columns.FindByUniqueName("CorporateNameExport").Visible = true;

        uxRelatedMerchant.Columns.FindByUniqueName("CorporateAddress").Visible = false;
        uxRelatedMerchant.Columns.FindByUniqueName("CorporateAddressExport").Visible = true;       

        if (!string.IsNullOrEmpty(hfLastUpdatedDateTime.Value)) {
            exportConfig.FileName = string.Format("RiskReport-RelatedMerchant_{0}", DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss"));
            exportConfig.ReportHeader = "Related Merchants \nLast updated date time: " + ConvertDateTimeToMMddyyyyhhmmnsstt(hfLastUpdatedDateTime.Value);
        }
        else
        {
            exportConfig.FileName = "RiskReport-RelatedMerchant";
            exportConfig.ReportHeader = "Related Merchants \nLast updated date time: ";
        }
        string fileName = GeneralFuncsLib.FormatFileName(exportConfig.FileName);
        exportConfig.FileName = HttpUtility.UrlEncode(fileName);        
    }  

    protected void uxRelatedMerchant_PreRender(object sender, EventArgs e)
    {
        var dt = uxRelatedMerchant.AS_DataSource;
        if(dt!=null && dt.Rows.Count>0)
        {
            hfLastUpdatedDateTime.Value = dt.Rows[0]["UpdatedDTS"].ToString();
        }
        else
        {
            hfLastUpdatedDateTime.Value = "";
           
        }
        BindGridTitle();
        ShowColumnsByPermissions();
    }

    private void BindGridTitle()
    {
        litGridSubTitle.Text = "Last updated date time: ";
        if (!string.IsNullOrEmpty(hfLastUpdatedDateTime.Value))
        {
            litGridSubTitle.Text += ConvertDateTimeToMMddyyyyhhmmnsstt(hfLastUpdatedDateTime.Value);
        }
    }

    private string ConvertDateTimeToMMddyyyyhhmmnsstt(string dateTime)
    {
        return Convert.ToDateTime(dateTime).ToString("MM/dd/yyyy hh:mm:ss tt");
    }

    #region Thread SPA
    private FilterParameterCollection GetRelatedMerchantsParameters(DataBindAction action)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserRiskParams();
        switch (action)
        {
            case DataBindAction.BindRelatedMerchantsGrid:
                {
                    parameters.Add(new FilterParameter("@MerchantNumber", MerchantNumber, DbType.String));
                    parameters.Add(new FilterParameter("@PageNo", 1, DbType.Int32));
                    parameters.Add(new FilterParameter("@PageSize", 10, DbType.Int32));
                    parameters.Add(new FilterParameter("@IsPaging", true, DbType.Boolean));

                    string decryptDataParams = GetDecryptDataParams();

                    if (!string.IsNullOrEmpty(decryptDataParams))
                    {
                        parameters.AddDecryptDataParams(decryptDataParams, _IsExporting);
                    }

                    break;
                }

        }
        return parameters;
    }

    private string GetDecryptDataParams()
    {
        string decryptDataParams = string.Empty;

        if (PermissionManager.HasFullTaxView(Page))
        {
            decryptDataParams = "EncryptedOwnerSSN,EncryptedTaxId";
        }

        if (PermissionManager.HasFullDDANumberView(Page))
        {
            if (string.IsNullOrEmpty(decryptDataParams))
            {
                decryptDataParams = "EncryptedDDANumber";
            }
            else
            {
                decryptDataParams += ",EncryptedDDANumber";
            }
        }
        return decryptDataParams;
    }


    public void BindDataFromThread(List<DataSourceParallelResponse> dataSources)
    {
        if (dataSources.IsNotNullData())
        {
            var data = dataSources.SingleOrDefault(m => m.FeatureName == DataBindAction.BindRelatedMerchantsGrid.ToString());
            if (data.IsNotNullData() && data !=null)
                dtRelatedMerchants = data.DataSource;
        }
        uxRelatedMerchant.Rebind();
    }

    public SpaInfo SpaGetRelatedMerchants
    {
        get
        {
            return new SpaInfo()
            {
                FeatureName = DataBindAction.BindRelatedMerchantsGrid.ToString(),
                SpaName = SPA_GET_RELATED_MERCHANTS,
                Parameters = GetRelatedMerchantsParameters(DataBindAction.BindRelatedMerchantsGrid)
            };
        }
    }

    #endregion 
}
