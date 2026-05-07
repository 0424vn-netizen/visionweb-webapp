using AS.Common;
using AS.Security.WS.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControls_AccessFuntionControl : GlobalUserControl
{
    public PermissionCollection DataSource
    {
        get
        {
            return uxAccessFuncList.DataSource as PermissionCollection;
        }
        set
        {
            uxAccessFuncList.DataSource = value;
            uxAccessFuncList.DataBind();
        }
    }

    public List<AS.Controls.Global.CheckBox> Items
    {
        get
        {
            List<AS.Controls.Global.CheckBox> lst = new List<AS.Controls.Global.CheckBox>();
            foreach (RepeaterItem item in uxAccessFuncList.Items)
            {
                AS.Controls.Global.CheckBox chkbox = item.FindControl("uxPermission") as AS.Controls.Global.CheckBox;
                if (chkbox != null)
                    lst.Add(chkbox);
            }
            return lst;
        }
    }

    private bool CheckPermissionIDInList(string strPermission, PermissionCollection permissions)
    {
        foreach (Permission p in permissions)
        {
            if (string.Compare(p.PermissionCode, strPermission) == 0)
                return true;
        }
        return false;
    }

    public void SetSelectedByPermissions(PermissionCollection permissions)
    {
        foreach (RepeaterItem item in uxAccessFuncList.Items)
        {
            AS.Controls.Global.CheckBox chkbox = item.FindControl("uxPermission") as AS.Controls.Global.CheckBox;
            if (chkbox != null)
            {
                chkbox.Checked = CheckPermissionIDInList(chkbox.Value, permissions);
            }
        }
    }
    public void SetSelectedByPermissionText(string permissionsText)
    {
        foreach (RepeaterItem item in uxAccessFuncList.Items)
        {
            AS.Controls.Global.CheckBox chkbox = item.FindControl("uxPermission") as AS.Controls.Global.CheckBox;
            if (chkbox != null && chkbox.Enabled && chkbox.Text.Equals(permissionsText, StringComparison.OrdinalIgnoreCase))
            {
                chkbox.Checked = true;
                break;
            }
        }
    }

    public void SetShowHideByPermissionCodeRequired(SecMenuItemCollection menus)
    {
        foreach (RepeaterItem item in uxAccessFuncList.Items)
        {
            HtmlGenericControl divChild = item.FindControl("uxPnlGroupChecbox") as HtmlGenericControl;
            Permission p = item.DataItem as Permission;
            if (divChild != null && p != null)
            {
                var hasRequired = menus.Cast<SecMenuItem>().Where(m => m.Permissions.IndexOf(p.PermissionCodesRequire) != -1).Count() > 0;
                if (hasRequired)
                {
                    divChild.Attributes["class"] = "child";
                }
                else
                {
                    divChild.Attributes["class"] = "child hide";
                }
            }
        }
    }

    public void SetShowHideByPermissionCodeRequired(PermissionCollection permissions)
    {
        foreach (RepeaterItem item in uxAccessFuncList.Items)
        {
            HtmlGenericControl divChild = item.FindControl("uxPnlGroupChecbox") as HtmlGenericControl;
            Permission p = item.DataItem as Permission;
            if (divChild != null && p != null)
            {
                var hasRequired = permissions.Cast<Permission>().Where(m => m.PermissionCode.Equals(p.PermissionCodesRequire)).Count() > 0;
                if (hasRequired)
                {
                    divChild.Attributes["class"] = "child";
                }
                else
                {
                    divChild.Attributes["class"] = "child hide";
                }
            }
        }
    }
    string currentGroup = string.Empty;
    protected void uxAccessFuncList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Permission p = e.Item.DataItem as Permission;
        HtmlGenericControl divParent = e.Item.FindControl("uxPnlGroupLabel") as HtmlGenericControl;
        HtmlGenericControl divChild = e.Item.FindControl("uxPnlGroupChecbox") as HtmlGenericControl;

        Literal label = e.Item.FindControl("uxGroupLabel") as Literal;
        AS.Controls.Global.CheckBox chkbox = e.Item.FindControl("uxPermission") as AS.Controls.Global.CheckBox;
        if (label != null && chkbox != null && divParent != null)
        {
            if (p.GroupFuncName.Equals(currentGroup) || string.IsNullOrEmpty(p.GroupFuncName))
            {
                divParent.Visible = false;
            }
            else
            {
                currentGroup = p.GroupFuncName;
                string text = GetLocalResourceObject(string.Format("AccessFuntionControlGroupFuncLabel_{0}", p.GroupFuncName)) != null ? GetLocalResourceObject(string.Format("AccessFuntionControlGroupFuncLabel_{0}", p.GroupFuncName)).ToString() : string.Empty;
                label.Text = VeraCodeSolution.DoVeraCode(text);
                divParent.Attributes["data-target"] = p.GroupFuncName;
                divParent.Visible = true;

            }
            if (!string.IsNullOrEmpty(p.PermissionCodesRequire))
            {
                divChild.Attributes["class"] = "child hide";
                divChild.Attributes["data-target-required"] = p.PermissionCodesRequire;
            }
            else
            {
                divChild.Attributes["class"] = "child";
            }
            divChild.Attributes["data-target-default-landing-menu"] = p.PermissionCode;
            divChild.Attributes["data-target"] = p.GroupFuncName;
            chkbox.Text = VeraCodeSolution.DoVeraCode(p.Description);
            chkbox.Value = VeraCodeSolution.DoVeraCode(p.PermissionCode);
        }
    }
}