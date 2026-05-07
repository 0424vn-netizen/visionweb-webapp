using AS.Common.DBManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Security.WS.Entities;
using Telerik.Web.UI;

public partial class AssignActivityGroupModal : NonReportPage
{
    protected DataTable PermissionsList
    {
        get
        {
            DataTable source = new DataTable();
            source.Columns.Add("GroupName");
            source.Columns.Add("View");
            source.Columns.Add("Edit");
            source.Columns.Add("Manage");
            source.Columns.Add("IsVisible");
            source.Rows.Add(new object[] { GetLocalResourceObject("lblContent1.Text"), "ViewDealReview", "EditDealReview", "ManageDealReview", string.Empty });
            source.Rows.Add(new object[] { GetLocalResourceObject("lblContent2.Text"), "ViewUWPrimaryReview", "EditUWPrimaryReview", "ManageUWPrimaryReview", string.Empty });
            source.Rows.Add(new object[] { GetLocalResourceObject("lblContent3.Text"), "ViewUWSecondaryReview", "EditUWSecondaryReview", "ManageUWSecondaryReview", string.Empty });
            source.Rows.Add(new object[] { GetLocalResourceObject("lblContent4.Text"), "ViewUWEscalationReview", "EditUWEscalationReview", "ManageUWEscalationReview", string.Empty });
            source.Rows.Add(new object[] { GetLocalResourceObject("lblContent7.Text"), "ViewBoardingFrontEnd", "EditBoardingFrontEnd", "ManageBoardingFrontEnd", string.Empty });
            source.Rows.Add(new object[] { GetLocalResourceObject("lblContent8.Text"), "ViewBoardingBackEnd", "EditBoardingBackEnd", "ManageBoardingBackEnd", string.Empty });
            source.Rows.Add(new object[] { GetLocalResourceObject("lblContent9.Text"), "ViewEquipmentOrdering", "EditEquipmentOrdering", "ManageEquipmentOrdering", string.Empty });
            source.Rows.Add(new object[] { GetLocalResourceObject("lblContent5.Text"), "ViewQAReview", "EditQAReview", "ManageQAReview", string.Empty });
            source.Rows.Add(new object[] { GetLocalResourceObject("lblContent10.Text"), "ViewTraining", "EditTraining", "ManageTraining", string.Empty });
            source.Rows.Add(new object[] { GetLocalResourceObject("lblContent11.Text"), "ViewRiskReview", "EditRiskReview", "ManageRiskReview", string.Empty });

            return source;
        }
    }

    protected DataTable PermissionsListForUserMode()
    {
        var persForUser = PermissionsList;
        var rolePers = SessionManager.RoleGroupPermissions.Cast<Permission>();
        for (var i = 0; i < persForUser.Rows.Count; i++)
        {
            if (!rolePers.Any(m => m.PermissionCode == PermissionsList.Rows[i]["View"].ToString()) &&
                !rolePers.Any(m => m.PermissionCode == PermissionsList.Rows[i]["Edit"].ToString()) &&
                !rolePers.Any(m => m.PermissionCode == PermissionsList.Rows[i]["Manage"].ToString()))
            {
                persForUser.Rows[i]["IsVisible"] = "hide";
            }
        }
        return persForUser;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) 
            return;
        PageType = SecurePageType.Modal;
        if (!IsPostBack)
        {
            String Type = Request.QueryString["type"];
            if (Type != null && Type.Equals("Role", StringComparison.OrdinalIgnoreCase))
                uxPermissionsList.DataSource = PermissionsList;
            else
                uxPermissionsList.DataSource = PermissionsListForUserMode();
            uxPermissionsList.DataBind();
        }
    }
    
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string Type = Request.QueryString["type"];
        string[] listPermissions = hdSelectedPers.Value.Split(',');
        PermissionCollection perCollection = new PermissionCollection();

        foreach (var per in listPermissions)
        {
            perCollection.Add(new Permission { PermissionCode = per });

        }
        if (Type != null && Type.Equals("Role", StringComparison.OrdinalIgnoreCase))
        {
            SessionManager.RoleGroupPermissions = perCollection;
        }
        else
        {
            SessionManager.UserGroupPermissions = perCollection;
        }

        SessionManager.SelectedGroupPermission = perCollection;

    }
    protected void uxPermissionsList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        RadCheckBox ckView = e.Item.FindControl("uxView") as RadCheckBox;
        if (ckView.IsNotNullData())
        {
            SetValueForControl(ckView);
        }
        RadCheckBox ckEdit = e.Item.FindControl("uxEdit") as RadCheckBox;
        if (ckEdit.IsNotNullData())
        {
            SetValueForControl(ckEdit);
        }
        RadCheckBox ckManage = e.Item.FindControl("uxManage") as RadCheckBox;
        if (ckManage.IsNotNullData())
        {
            SetValueForControl(ckManage);
        }
    }
    private void SetValueForControl(RadCheckBox control)
    {
        String Type = Request.QueryString["type"];
        var rolePers = SessionManager.RoleGroupPermissions.Cast<Permission>();
        var userPers = SessionManager.UserGroupPermissions.Cast<Permission>();

        if (control.IsNotNullData())
        {
            if (Type != null && Type.Equals("Role", StringComparison.OrdinalIgnoreCase))
            {
                if (rolePers.Any(m => m.PermissionCode == control.Value))
                    control.Checked = true;
            }
            else
            {
                if (!rolePers.Any(m => m.PermissionCode == control.Value))
                {
                    control.CssClass += " disabled";
                    control.Visible = false;
                }
                else if (userPers.Any(m => m.PermissionCode == control.Value))
                    control.Checked = true;
            }
        }
    }
}