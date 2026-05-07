using System.Data;

using Telerik.Web.UI;
using AS.Common.DBManager;



public partial class UserControls_AssignmentListSimple : GlobalUserControl
{  
    protected void uxGrid_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("ASClient", SessionManager.CurrentClient, DbType.Int32));
        parameters.Add(new FilterParameter("UserId", this.Page.SecureQueryString["UserIDFilter"], DbType.String));
        parameters.Add(new FilterParameter("Mode", 0, DbType.Int32)); // Get all assignment
        uxGrid.DataSource = WebServices.RiskServices.GetReports("spa_rm_cs_GetAssignmentByLastUser", parameters); 
      
    }

}
