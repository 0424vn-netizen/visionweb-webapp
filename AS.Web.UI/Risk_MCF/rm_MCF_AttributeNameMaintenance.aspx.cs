using AS.Common.DBManager;
using AS.Controls.Pages;
using AS.Controls.Validators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

[PagePermission("RskAttribute,MSRskAttribute")]
public partial class rm_MCF_AttributeNameMaintenance : ReportPage
{
    enum DataBindAction
    {
        BindAttributeList,

    }

    enum PostBackAction
    {
        Save
    }
    protected override void OnPostBackActions(Enum type, object param)
    {
        if (this.IsIntruderDetected) return;

        switch ((PostBackAction)type)
        {
            case PostBackAction.Save:
                {

                }
                break;
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (this.IsIntruderDetected) return;
        FilterParameterCollection parameters = new FilterParameterCollection();

        switch ((DataBindAction)type)
        {
            case DataBindAction.BindAttributeList:
                {
                    parameters.AddLoggedInUserParamsWithRecId();
                    uxListAttributeName.DataSource = WebServices.RiskServices.GetReports("spa_RM_MCF_MRS_Get_AttributeList", parameters);
                    uxListAttributeName.DataBind();
                }
                break;

        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            OnDataBindControls(DataBindAction.BindAttributeList);
        }
    }
    protected void uxListAttributeName_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView rowItem = e.Item.DataItem as DataRowView;

            ValidatorLabel uxSystemAttributeName = e.Item.FindControl("uxSystemAttributeName") as ValidatorLabel;
            HiddenField uxSystemAttributeID = e.Item.FindControl("uxSystemAttributeID") as HiddenField;
            TextBox uxPreferredAttributeName = e.Item.FindControl("uxPreferredAttributeName") as TextBox;
            TextBox uxPreferredAttributeNameOld = e.Item.FindControl("uxPreferredAttributeNameOld") as TextBox;
            Button uxUpdate = e.Item.FindControl("uxUpdate") as Button;
            HiddenField uxRecordID = e.Item.FindControl("uxRecordID") as HiddenField;

            uxSystemAttributeName.Text = rowItem["SystemAttributeName"].ToString();
            uxSystemAttributeID.Value = rowItem["AttributeID"].ToString();
            uxPreferredAttributeName.Text = uxPreferredAttributeNameOld.Text = rowItem["PreferredAttributeName"].ToString();
            uxRecordID.Value = rowItem["RecordID"].ToString();

            uxUpdate.OnClientClick = string.Format("return doValidation(this, '{0}');", uxUpdate.UniqueID);

        }
    }
    protected void uxListAttributeName_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName.Equals("Save", StringComparison.OrdinalIgnoreCase))
        {
            ValidatorLabel uxSystemAttributeName = e.Item.FindControl("uxSystemAttributeName") as ValidatorLabel;
            HiddenField uxSystemAttributeID = e.Item.FindControl("uxSystemAttributeID") as HiddenField;
            TextBox uxPreferredAttributeName = e.Item.FindControl("uxPreferredAttributeName") as TextBox;
            TextBox uxPreferredAttributeNameOld = e.Item.FindControl("uxPreferredAttributeNameOld") as TextBox;
            ValidatorMessage uxPreferredAttributeNameMsg = e.Item.FindControl("uxPreferredAttributeNameMsg") as ValidatorMessage;
            HiddenField uxRecordID = e.Item.FindControl("uxRecordID") as HiddenField;
            Button uxUpdate = e.Item.FindControl("uxUpdate") as Button;
            Button uxCancel = e.Item.FindControl("uxCancel") as Button;
            string spaName = "spa_RM_MCF_MRS_Update_PreferredAttributeName";
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserParamsWithRecId();
            parameters.Add(new FilterParameter("@AttributeID", uxSystemAttributeID.Value, DbType.Int32));
            parameters.Add(new FilterParameter("@PreferredName", uxPreferredAttributeName.Text.Trim(), DbType.AnsiString));
            parameters.Add(new FilterParameter("@RecordID", uxRecordID.Value, DbType.Int32));
            parameters.Add(new FilterParameter("@ReturnValue", 0, DbType.Int32, true));
            WebServices.RiskServices.ExecuteNonQueryCommand(spaName, parameters, out parameters);

            int result = parameters.FindFilterParameterByName("@ReturnValue", false).ParameterValue.ToInt();
            // 1 is success, 0 - failed, -1 duplicate
            if (result == 2)
            {
                uxSystemAttributeName.CssClass += " label-error";
                uxPreferredAttributeNameMsg.ShowOnLoad = true;
                uxPreferredAttributeNameMsg.Message = GetLocalResourceObject("uniqueMsg").ToASString();
                ((ReportPage)Page).AjaxAddResponseScript("ShowHideControls();");
                uxUpdate.CssClass = uxUpdate.CssClass.Replace("hide", "");
                uxCancel.CssClass = uxCancel.CssClass.Replace("hide", "");
            }
            else if (result == 3)
            {
                uxSystemAttributeName.CssClass += " label-error";
                uxPreferredAttributeNameMsg.ShowOnLoad = true;
                uxPreferredAttributeNameMsg.Message = GetLocalResourceObject("sameMsg").ToASString();
                ((ReportPage)Page).AjaxAddResponseScript("ShowHideControls();");
                uxUpdate.CssClass = uxUpdate.CssClass.Replace("hide", "");
                uxCancel.CssClass = uxCancel.CssClass.Replace("hide", "");
            }
            else
            {

                ((ReportPage)Page).ReloadPage();
            }
        }
    }
}