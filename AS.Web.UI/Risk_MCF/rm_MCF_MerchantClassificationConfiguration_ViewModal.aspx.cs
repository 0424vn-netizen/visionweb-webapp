using AS.Common.DBManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class rm_MCF_MerchantClassificationConfiguration_ViewModal : NonReportPage
{
    #region props
    int ClassId
    {
        get
        {
            if (IsSecureQueryString && SecureQueryString["ClassId"] != null)
                return SecureQueryString["ClassId"].ToInt();
            else
                return 0;
        }
    }
    private DataTable _ClassificationInformation;
    DataTable ClassificationInformation
    {
        get
        {
            if (_ClassificationInformation == null)
                _ClassificationInformation = GetClassificationInformation();
            return _ClassificationInformation;
        }
    }

    public string BindValue(string colName)
    {
        if (ClassificationInformation == null || ClassificationInformation.Rows.Count <= 0)
            return string.Empty;
        DataRow dr = ClassificationInformation.Rows[0];
        return dr[colName].ToString();
    }
    #endregion

    #region method
    private DataTable GetClassificationInformation()
    {
        DataTable tb = new DataTable();
        FilterParameterCollection prams = new FilterParameterCollection();
        prams.AddLoggedInUserParamsWithRecId();
        prams.AddLanguageID();
        prams.Add(new FilterParameter("@ClassificationID", ClassId, DbType.Int32));
        tb = WebServices.RiskServices.GetReports("spa_RM_MCF_MRS_Get_MerchantClassificationDetails", prams);
        return tb;
    }
    #endregion

    #region events
    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;

    }
    #endregion
    protected void uxAttributeGrid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        uxAttributeGrid.DataSource = ClassificationInformation;
    }
}