using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Web.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class FindOwners : NonReportPage
{
    enum DataBindAction
    {
        BindMetricList
    }
    int SelectedAttributeID
    {
        get
        {
            if (SecureQueryString["SelectedAttributeID"] != null)
                return SecureQueryString["SelectedAttributeID"].ToInt();
            else
                return 0;
        }
    }
    string OperandValue
    {
        get
        {
            if (SecureQueryString["OperandValue"] != null)
                return SecureQueryString["OperandValue"].ToString();
            else
                return string.Empty;
        }
    }

    string SelectedMetricID
    {
        get
        {
            if (SecureQueryString["SelectedMetricID"] != null)
                return SecureQueryString["SelectedMetricID"].ToString();
            else
                return string.Empty;
        }

    }
    protected bool FromEdit
    {
        get
        {
            if (SecureQueryString["FromEdit"] != null)
                return SecureQueryString["FromEdit"].ToString().Trim() == "1";
            else
                return false;
        }

    }

    protected bool FromChildModal
    {
        get
        {
            if (SecureQueryString["FromChildModal"] != null)
                return SecureQueryString["FromChildModal"].ToString().Trim() == "1";
            else
                return false;
        }
    }

    protected string MetricControlID
    {
        get
        {
            if (SecureQueryString["MetricControlID"] != null)
                return SecureQueryString["MetricControlID"].ToString();
            else
                return string.Empty;
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //  9 --Zip
        if (SelectedAttributeID == 9)
        {
            this.Page.Title = GetLocalResourceObject("PageTitleResourceZip").ToString();
            GridColumn gc = uxOwnerGrid.Columns.FindByUniqueNameSafe("DataText");
            if (gc != null)
            {
                gc.HeaderText = GetLocalResourceObject("ASGridBoundColumnResourceZip.HeaderText").ToString();
                gc.HeaderTooltip = GetLocalResourceObject("ASGridBoundColumnResourceZip.HeaderTooltip").ToString();
            }
        }
        else
        {
            this.Page.Title = GetLocalResourceObject("PageTitleResourceOwner.Title").ToString();
            GridColumn gc = uxOwnerGrid.Columns.FindByUniqueNameSafe("DataText");
            if (gc != null)
            {
                gc.HeaderText = GetLocalResourceObject("ASGridBoundColumnResourceOwner.HeaderText").ToString();
                gc.HeaderTooltip = GetLocalResourceObject("ASGridBoundColumnResourceOwner.HeaderTooltip").ToString();
            }
        }
        PageType = SecurePageType.Modal;
        if (!IsPostBack)
        {
            uxOwnerGrid.Rebind();
            uxSelectedOwners.Value = SelectedMetricID;

        }
        if (FromEdit)
        {
            if (string.IsNullOrEmpty(MetricControlID))
                uxSubmit.OnClientClick = "doSubmitEdit(); return false;";
            else
                uxSubmit.OnClientClick = "doSubmitEditFromChildModal('" + MetricControlID + "'); return false;";
        }
        else
        {
            if (string.IsNullOrEmpty(MetricControlID))
                uxSubmit.OnClientClick = "doSubmit(); return false;";
            else
                uxSubmit.OnClientClick = "doSubmitFromChildModal('" + MetricControlID + "'); return false;";
        }

        if (FromChildModal)
        {
            uxClose.OnClientClick = "parent.HidePopupModalChild(1); return false;";

        }
        else
        {
            uxClose.OnClientClick = "parent.HidePopupModal(); return false;";
        }

    }

    private void RemoveFilterMenuItem()
    {
        //show filter menu
        var grids = new RadGrid[] { uxOwnerGrid };
        var removedItems = new string[] { 
            "GreaterThan",
            "LessThan", "GreaterThanOrEqualTo", "LessThanOrEqualTo", "Between", "NotBetween",
            "IsEmpty", "NotIsEmpty", "IsNull", "NotIsNull" 
        };
        foreach (var grid in grids)
        {
            for (int i = 0; i < removedItems.Length; i++)
            {
                var mi = grid.FilterMenu.Items.FindItemByText(removedItems[i]);
                if (mi != null)
                    grid.FilterMenu.Items.Remove(mi);
            }
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (this.IsIntruderDetected) return;
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserRiskParams();
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindMetricList:
                {
                    parameters.Add(new FilterParameter("@AttributeID", SelectedAttributeID, DbType.Int32));
                    parameters.Add(new FilterParameter("@OperandID", OperandValue, DbType.String));
                    string spaName = "spa_RM_MRS_Get_MetricValuesByAttributeID";
                    this.uxOwnerGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, WebSiteConstants.GET_REPORT_METHOD_NAME,
                        new object[] { spaName, ReportServices.ConvertToFilterParamWSArray(parameters) });
                }
                break;
        }
    }

    protected void uxOwnerGrid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        OnDataBindControls(DataBindAction.BindMetricList, sender);
    }
    protected void uxOwnerGrid_Init(object sender, EventArgs e)
    {
        RemoveFilterMenuItem();
    }
}