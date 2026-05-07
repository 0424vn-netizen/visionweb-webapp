using System;
using Telerik.Web.UI;
using AS.Common.DBManager;
using System.Data;
using System.Web.UI;
using AS.Controls.Pages;

[PagePermission("RskQueue,RskAdhoc,MSRskQueue,MSRskAdhoc")]
public partial class rm_MCF_ColorLegend : NonReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        base.PageType = SecurePageType.Modal;
        //string isBarometer = this.Request.QueryString["isBarometer"];
        //this.pnlMerchantNumberColor.Visible = string.IsNullOrEmpty(isBarometer) ? true : false;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "refresh_print_content", "<script>$(document).ready(function () {   GetRadWindow().BrowserWindow.SetPrintContent();});</script>", false);
    }

    protected void uxParam_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        if (this.IsIntruderDetected) return;
        this.uxParam.DataSource = GetFlagColorForParameters();
    }

    private DataTable GetFlagColorForParameters()
    {
        FilterParameterCollection parameterList = new FilterParameterCollection();
        parameterList.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameterList.AddLanguageID();
        DataTable table = WebServices.RiskServices.GetReports("spa_RM_MCF_GetFlagColorForParameters", parameterList);
        DataTable newTable = table.Clone();
        const string PARAM_GROUPNAMEDUMMY = "ParameterGroupNameDummy";
        const string PARAM_GROUPNAME = "ParameterGroupName";
        const string PARAM_GROUPDESC = "ParameterGroupDescription";
        const string PARAM_GROUPID = "ParameterGroupID";

        newTable.Columns.Add(PARAM_GROUPNAMEDUMMY, typeof(string));
        newTable.Columns.Add(PARAM_GROUPNAME, typeof(string));

        for (int i = 0; i < table.Rows.Count; i++)
        {
            DataRow row = table.Rows[i];
            DataRow newRow = newTable.NewRow();

            for (int j = 0; j < row.ItemArray.Length; j++)
                newRow[j] = row[j];
            newRow[PARAM_GROUPNAME] = row[PARAM_GROUPDESC];
            switch (row[PARAM_GROUPID].ToString())
            {
                //case "Volume/Batch/Ticket":
                case "1":
                    newRow[PARAM_GROUPNAMEDUMMY] = "A";
                    break;
                //case "Duplicates":
                case "2":
                    newRow[PARAM_GROUPNAMEDUMMY] = "B";
                    break;
                //case "Credits":
                case "3":
                    newRow[PARAM_GROUPNAMEDUMMY] = "C";
                    break;
                //case "Contract":
                case "4":
                    newRow[PARAM_GROUPNAMEDUMMY] = "D";
                    break;
                //case "Chargebacks/Retrievals":
                case "5":
                    newRow[PARAM_GROUPNAMEDUMMY] = "E";
                    break;
                //case "Attrition":
                case "6":
                    newRow[PARAM_GROUPNAMEDUMMY] = "F";
                    break;
                //case "ACH Rejects":
                case "7":
                    newRow[PARAM_GROUPNAMEDUMMY] = "G";
                    break;
                //case "Auth":
                case "8":
                    newRow[PARAM_GROUPNAMEDUMMY] = "H";
                    break;
                //case "Deposits":
                case "9":
                    newRow[PARAM_GROUPNAMEDUMMY] = "I";
                    break;
                //case "BIN":
                case "10":
                    newRow[PARAM_GROUPNAMEDUMMY] = "J";
                    break;
                //case "Merchant Filter"
                case "11":
                    newRow[PARAM_GROUPNAMEDUMMY] = "K";
                    break;
                default: break;
            }

            newTable.Rows.Add(newRow);
        }

        return newTable;

    }

}
