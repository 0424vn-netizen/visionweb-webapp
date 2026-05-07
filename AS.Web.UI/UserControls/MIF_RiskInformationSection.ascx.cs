using AS.Common.DBManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_MIF_RiskInformationSection : GlobalUserControl
{
    private string PATH_EXPORT_EXCEL_FILE = ConfigurationManager.AppSettings["ExportTempFolder"];
    private const string FILE_NAME_RISK_INFO = "~/App_Data/tplRiskInformation.htm";
    protected enum DataBindAction
    {
        BindRiskInformation,
    }
    public bool IsRiskInformation
    {
        get
        {
            return ((ReportPage)Page).IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RISK_INFO)
                || ((ReportPage)Page).IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_MSRISK_INFO);
        }
    }

    private DataTable _RiskInfor
    {
        get
        {
            if (ViewState["RiskInfomation"] != null)
                return (DataTable)(ViewState["RiskInfomation"]);
            else
                return null;
        }
        set
        {
            ViewState["RiskInfomation"] = value;
        }
    }
    public string BindValue(string colName)
    {
        if (_RiskInfor == null || _RiskInfor.Rows.Count <= 0)
            return WebSiteConstants.HTML_EM_DASH_ENCODE;
        DataRow dr = _RiskInfor.Rows[0];
        return dr[colName] == DBNull.Value || dr[colName].ToString().IsNullOrEmpty() ? WebSiteConstants.HTML_EM_DASH_ENCODE : dr[colName].ToString();
    }

    public UserControls_MIF_RiskInformationSection()
    {

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        pnlViewRiskReportLink.Visible = Page.IsUserWithPermission("RskRP") || Page.IsUserWithPermission("MSRskRP");

    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        if (!IsRiskInformation || string.IsNullOrEmpty(GetMerchantNrParameter()))
        {
            this.Visible = false;
            return;
        }

        if (pnlViewRiskReportLink.Visible)
        {
            //38605 - Enable MCF Risk module
            int isMCFRisk = RiskSessionManager.Risk_ModeMCF;
            btnViewRiskReport.NavigateUrl = isMCFRisk == (int)ShowRiskMCF.MCFRisk ? btnViewRiskReport.NavigateUrl.Replace(GeneralFuncsLib.RISK_OLD_URL_RULE, GeneralFuncsLib.RISK_MCF_URL_RULE)
                : btnViewRiskReport.NavigateUrl;

        }
        OnDataBindControls(DataBindAction.BindRiskInformation);
        BindRiskExtend();
    }

    private void BindRiskExtend()
    {
        if (GeneralFuncsLib.RiskReportExtend(WebSiteEnums.Filter_Extend.GverifyCode))
        {
            uxGVerifyCode.Visible = true;
        }
        if (GeneralFuncsLib.RiskReportExtend(WebSiteEnums.Filter_Extend.GauthenticateCode))
        {
            uxGauthenticate.Visible = true;
        }
        if (GeneralFuncsLib.RiskReportExtend(WebSiteEnums.Filter_Extend.G2Compass))
        {
            uxG2CompassAutoApprovalIndicator.Visible = true;
        }
    }
    private string GetMerchantNrParameter()
    {
        return ReportPage.ReportFilter.CurrentValue.Value;
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        base.OnDataBindControls(type, sender);
        int isMCFRisk = RiskSessionManager.Risk_ModeMCF;

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindRiskInformation:
                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserParamsWithRecId();
                parameters.Add(new FilterParameter("@MerchantNumber", GetMerchantNrParameter(), DbType.AnsiString));
                string spaName = (isMCFRisk == (int)ShowRiskMCF.MCFRisk) ? "spa_RM_MCF_MRS_Get_MIFClassificationByMerchant" : "spa_RM_MRS_Get_MIFClassificationByMerchant";
                _RiskInfor = WebServices.CsReportServices.GetReports(spaName, parameters);
                break;
        }
    }
    public string WriteContentToFile(string path, string fileName, string fileType, string fileContent)
    {
        string pathTemp = string.Format("{0}{1}-{2}.{3}", path, fileName, DateTime.Now.Ticks.ToString(), "xls");
        var filePath = HttpContext.Current.Server.MapPath(pathTemp);
        FileStream fs = new FileStream(filePath, FileMode.Append);
        StreamWriter sw = new StreamWriter(fs);
        sw.Write(fileContent);
        sw.Flush();
        sw.Close();
        fs.Close();
        return filePath;
    }
    public string ExcelRiskInformation()
    {
        OnDataBindControls(DataBindAction.BindRiskInformation);
        DataTable _datasource = (DataTable)(_RiskInfor);
        StringBuilder strExcelTemplate = new StringBuilder(File.ReadAllText(Server.MapPath(FILE_NAME_RISK_INFO)));
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_RiskInformationResource]", Resources.Template.tpl_htm_RiskInformation);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_ClassificationNameResource]", Resources.Template.tpl_RiskInformation_htm_ClassificationName);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_MultiplierResource]", Resources.Template.tpl_RiskInformation_htm_Multiplier);
        strExcelTemplate.Replace("[tpl_MerchantInformation_htm_RiskScoreResource]", Resources.Template.tpl_RiskInformation_htm_RiskScore);



        if (GeneralFuncsLib.RiskReportExtend(WebSiteEnums.Filter_Extend.GverifyCode))
        {
            strExcelTemplate.Replace("[GVERIFYCODEDISPLAY]", string.Empty);
            strExcelTemplate.Replace("[tpl_MerchantInformation_htm_GverifyCodeResource]", Resources.Template.tpl_MerchantInformation_htm_GverifyScoreResource);

            if (_datasource == null || _datasource.Rows.Count == 0) {
                strExcelTemplate.Replace("RI_GVERIFYCODE", WebSiteConstants.HTML_EM_DASH_ENCODE);
            }
            else
            {
                strExcelTemplate.Replace("RI_GVERIFYCODE", _datasource.Rows[0]["GVerifyCode"].IsNullOrEmpty() ? WebSiteConstants.HTML_EM_DASH_ENCODE : "&nbsp;" + _datasource.Rows[0]["GVerifyCode"].ToString());
            }
        }
        else {
            strExcelTemplate.Replace("[GVERIFYCODEDISPLAY]", "style='display: none'");
        }

        if (GeneralFuncsLib.RiskReportExtend(WebSiteEnums.Filter_Extend.GauthenticateCode))
        {
            strExcelTemplate.Replace("[GAUTHENTICATECODEDISPLAY]", string.Empty);
            strExcelTemplate.Replace("[tpl_MerchantInformation_htm_GauthenticateCodeResource]", Resources.Template.tpl_MerchantInformation_htm_GauthenticateCodeResource);

            if (_datasource == null || _datasource.Rows.Count == 0)
            {
                strExcelTemplate.Replace("RI_GAUTHENTICATECODE", WebSiteConstants.HTML_EM_DASH_ENCODE);
            }
            else
            {
                strExcelTemplate.Replace("RI_GAUTHENTICATECODE", _datasource.Rows[0]["GAuthenticateCode"].IsNullOrEmpty() ? WebSiteConstants.HTML_EM_DASH_ENCODE : "&nbsp;" + _datasource.Rows[0]["GAuthenticateCode"].ToString());
            }
        }
        else
        {
            strExcelTemplate.Replace("[GAUTHENTICATECODEDISPLAY]", "style='display: none'");
        }

        if (GeneralFuncsLib.RiskReportExtend(WebSiteEnums.Filter_Extend.G2Compass))
        {
            strExcelTemplate.Replace("[AUTOAPPOVALINDICATORDISPLAY]", string.Empty);
            strExcelTemplate.Replace("[tpl_MerchantInformation_htm_AutoApprovalIndicatorResource]", Resources.Template.tpl_MerchantInformation_htm_AutoApprovalIndicatorResource);

            if (_datasource == null || _datasource.Rows.Count == 0)
            {
                strExcelTemplate.Replace("RI_AUTOAPPOVALINDICATOR", WebSiteConstants.HTML_EM_DASH_ENCODE);
            }
            else
            {
                strExcelTemplate.Replace("RI_AUTOAPPOVALINDICATOR", _datasource.Rows[0]["G2CompassAutoApprovalIndicator"].IsNullOrEmpty() ? WebSiteConstants.HTML_EM_DASH_ENCODE : "&nbsp;" + _datasource.Rows[0]["G2CompassAutoApprovalIndicator"].ToString());
            }
        }
        else
        {
            strExcelTemplate.Replace("[AUTOAPPOVALINDICATORDISPLAY]", "style='display: none'");
        }


        if (_datasource == null || _datasource.Rows.Count == 0)
        {
            strExcelTemplate.Replace("RI_CLASSIFICATIONNAME", WebSiteConstants.HTML_EM_DASH_ENCODE);
            strExcelTemplate.Replace("RI_MULTIPLIER", WebSiteConstants.HTML_EM_DASH_ENCODE);
            strExcelTemplate.Replace("RI_RISKSCORE", WebSiteConstants.HTML_EM_DASH_ENCODE);
        }
        else
        {
            strExcelTemplate.Replace("RI_CLASSIFICATIONNAME", _datasource.Rows[0]["MerchantCLASSIFICATIONNAME"].IsNullOrEmpty() ? WebSiteConstants.HTML_EM_DASH_ENCODE : "&nbsp;" + _datasource.Rows[0]["MerchantCLASSIFICATIONNAME"].ToString());
            strExcelTemplate.Replace("RI_MULTIPLIER", _datasource.Rows[0]["MULTIPLIER"].IsNullOrEmpty() ? WebSiteConstants.HTML_EM_DASH_ENCODE : "&nbsp;" + _datasource.Rows[0]["MULTIPLIER"].ToString());
            strExcelTemplate.Replace("RI_RISKSCORE", _datasource.Rows[0]["TotalRS"].IsNullOrEmpty() ? WebSiteConstants.HTML_EM_DASH_ENCODE : "&nbsp;" + _datasource.Rows[0]["TotalRS"].ToString());
        }
        string excelFileName = GeneralFuncsLib.GetFileName(GetLocalResourceObject("RiskInformationResource.Text").ToString());
        return WriteContentToFile(PATH_EXPORT_EXCEL_FILE, excelFileName, "xls", strExcelTemplate.ToString());
    }

}