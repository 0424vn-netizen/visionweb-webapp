using AS.Common.DBManager;
using AS.Controls.Global;
using AS.Controls.Validators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class UserControls_AttributeRiskScore : GlobalUserControl
{
    #region Properties

    enum DataBindAction
    {
        BindAttributeList,
        BindOperandList,
        BindMetricList,
        Save
    }


    int SelectedAttributeID
    {
        get
        {
            if (uxAttributeNameList.SelectedValue != null && !string.IsNullOrEmpty(uxAttributeNameList.SelectedValue))
                return uxAttributeNameList.SelectedValue.Split('_')[0].ToInt();
            else
                return 0;
        }
    }
    string FieldType
    {
        get
        {
            if (uxAttributeNameList.SelectedValue != null && !string.IsNullOrEmpty(uxAttributeNameList.SelectedValue))
                return uxAttributeNameList.SelectedValue.Split('_')[1];
            else
                return string.Empty;
        }
    }
    int RedId
    {
        get
        {
            if (ViewState["RedId"] != null)
                return ViewState["RedId"].ToInt();
            else
                return 0;
        }
        set
        {
            ViewState["RedId"] = value;
        }
    }

    string RangeName
    {
        get
        {
            return uxRangeName.Text.Trim();
        }
        set
        {
            uxRangeName.Text = value;
        }
    }

    int FromValue
    {
        get
        {
            int v = 0;
            int.TryParse(uxFrom.Text, out v);
            return v;
        }
        set
        {
            uxFrom.Text = value.ToString();
        }
    }
    int ToValue
    {
        get
        {
            int v = 0;
            int.TryParse(uxTo.Text, out v);
            return v;
        }
        set
        {
            uxTo.Text = value.ToString();
        }
    }

    int OperandKey
    {
        get
        {

            if (!string.IsNullOrEmpty(uxOperand.SelectedValue))
                return uxOperand.SelectedValue.ToInt();
            else
                return 0;
        }
        set
        {
            uxOperand.SelectedValue = value.ToASString();
        }
    }

    string MetricValue
    {
        get
        {
            AttributeInformation obj = (AttributeInformation)GeneralFuncsLib.ConvertXMLToObject(XMLMetricOperand, typeof(AttributeInformation));

            if (obj.Metrics != null)
            {
                if (uxMetric.CheckBoxes)
                {
                    if (uxMetric.CheckedItems.Count > 0)
                    {
                        return string.Join(";", uxMetric.CheckedItems.Select(x => x.Value).ToList());
                    }
                }
                else
                {
                    return uxMetric.SelectedValue.ToString();
                }
                return string.Empty;
            }
            else
            {

                return uxMetricListTextHide.Value.Trim().Replace(',', ';').TrimStart(';').TrimEnd(';');


            }
        }
        set
        {
            AttributeInformation obj = (AttributeInformation)GeneralFuncsLib.ConvertXMLToObject(XMLMetricOperand, typeof(AttributeInformation));

            if (obj.Metrics != null)
            {
                uxMetric.Text = value;
            }
            else
            {
                uxMetricListText.Text = value;
                uxMetricListTextHide.Value = value;
            }
        }
    }
    int Score
    {
        get
        {
            return uxScore.Text.Trim().ToInt();
        }
        set
        {
            uxScore.Text = value.ToString();
        }
    }
    bool isFromTo
    {
        get
        {
            if (uxAttributeNameList.SelectedValue != null && uxAttributeNameList.SelectedValue.ToString().IndexOf('_') != -1)
                return uxAttributeNameList.SelectedValue.ToString().Split('_')[1].Equals(((int)AttributeRiskScoreMode.FromTo).ToString()); //  1 is from/to mode, 2 is operand
            else
                return false;
        }

    }

    string XMLMetricOperand
    {
        get
        {
            if (ViewState["XMLMetricOperand"] != null)
                return ViewState["XMLMetricOperand"].ToString();
            else
                return null;
        }
        set
        {
            ViewState["XMLMetricOperand"] = value;
        }

    }
    string SelectedMetricID
    {
        get
        {
            return uxMetricListText.Text;
        }
    }

    #endregion
    #region events

    protected override void OnDataBindControls(Enum type, object sender)
    {
        if (this.Page.IsIntruderDetected) return;
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParamsWithRecId();
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindAttributeList:
                {
                    DataTable td = WebServices.RiskServices.GetReports("spa_RM_MRS_Get_AttributeList", parameters);
                    uxAttributeNameList.Items.Clear();
                    foreach (DataRow r in td.Rows)
                    {
                        uxAttributeNameList.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r["AttributeName"].ToASString(), string.Format("{0}_{1}", r["AttributeID"].ToASString(), r["FieldType"].ToASString())));
                    }
                }
                break;
            case DataBindAction.BindOperandList:
                {
                    parameters.Add(new FilterParameter("@AttributeID", SelectedAttributeID, DbType.Int32));
                    DataTable td = WebServices.RiskServices.GetReports("spa_RM_MRS_Get_MetricOperandByAttribute", parameters);
                    if (td.HasData())
                    {
                        XMLMetricOperand = td.Rows[0][0].ToASString();
                    }
                    AttributeInformation obj = (AttributeInformation)GeneralFuncsLib.ConvertXMLToObject(XMLMetricOperand, typeof(AttributeInformation));
                    uxOperand.Items.Clear();
                    if (obj.Operands != null)
                    {
                        foreach (Operand o in obj.Operands.Items)
                        {
                            uxOperand.Items.Add(new Telerik.Web.UI.RadComboBoxItem(HttpUtility.HtmlDecode(o.Text), o.Value));
                        }
                        // uxOperand.Items.Insert(0, "");
                        OnDataBindControls(DataBindAction.BindMetricList);
                    }
                }
                break;
            case DataBindAction.BindMetricList:
                {
                    if (uxOperand.SelectedItem != null && (uxOperand.SelectedItem.Text.Trim().Equals("=")))
                    {
                        uxMetric.CheckBoxes = true;
                    }
                    else
                    {
                        uxMetric.CheckBoxes = false;
                    }
                    AttributeInformation obj = (AttributeInformation)GeneralFuncsLib.ConvertXMLToObject(XMLMetricOperand, typeof(AttributeInformation));
                    uxMetric.Items.Clear();
                    if (uxOperand.SelectedValue != null && !string.IsNullOrEmpty(uxOperand.SelectedValue))
                    {
                        if (obj.Metrics != null)
                        {
                            uxMetric.Visible = true;
                            uxMetricListPanel.Visible = false;
                            uxValidatorLabeluxMetricListText.Visible = false;
                            uxValidatorLabeluxMetric.Visible = true;
                            List<Metric> metrics = obj.Metrics.GetMetricByOperand(uxOperand.SelectedValue);
                            foreach (Metric m in metrics)
                            {
                                uxMetric.Items.Add(new Telerik.Web.UI.RadComboBoxItem(HttpUtility.HtmlDecode(m.Text), m.Value));
                            }
                        }
                        else
                        {
                            uxMetric.Visible = false;
                            uxMetricListPanel.Visible = true;
                            uxValidatorLabeluxMetricListText.Visible = true;
                            uxValidatorLabeluxMetric.Visible = false;
                            string queryString = this.Page.BuildSecureQueryString("SelectedAttributeID=" + uxAttributeNameList.SelectedValue.ToString().Split('_')[0] + "&OperandValue=" + uxOperand.SelectedValue + "&SelectedMetricID=" + SelectedMetricID + "&FromEdit=0");
                            btnlinkFindOwner.Attributes["onclick"] = "return ShowPopupModal('" + ResolveUrl("~/") + "FindOwners.aspx?" + queryString + "', 'auto'); return false;";
                        }
                    }
                    //uxMetric.Items.Insert(0, "");

                    foreach (Telerik.Web.UI.RadComboBoxItem radItem in uxMetric.Items)
                    {
                        radItem.CssClass = "metric-item-checkbox";
                    }
                }
                break;
            case DataBindAction.Save:
                {
                    Save();
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
        uxMetricListText.Attributes["onkeypress"] = "return false;";

    }

    protected void uxAttributeNameList_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (uxAttributeNameList.SelectedValue != null)
        {
            uxRangeName.Enabled = true;
            uxMetric.Visible = true;
            uxMetricListPanel.Visible = false;
            uxMetricListText.Text = string.Empty;
            uxMetricListTextHide.Value = string.Empty;
            uxValidatorLabeluxMetricListText.Visible = false;
            uxValidatorLabeluxMetric.Visible = true;
            if (isFromTo)
            {
                uxFrom.Enabled = uxTo.Enabled = uxScore.Enabled = true;
                uxOperand.Enabled = uxMetric.Enabled = false;
                uxOperand.Text = string.Empty;
                uxOperand.ClearSelection();
                uxMetric.Text = string.Empty;
                uxMetric.ClearSelection();
                uxMetric.ClearCheckedItems();

            }
            else
            {
                uxFrom.Enabled = uxTo.Enabled = false;
                uxFrom.Text = uxTo.Text = string.Empty;
                uxMetric.ClearSelection();
                uxOperand.ClearSelection();
                uxOperand.Enabled = uxMetric.Enabled = uxScore.Enabled = true;
                OnDataBindControls(DataBindAction.BindOperandList);
            }
            SetValidattionMode();
            ((ReportPage)Page).AjaxAddResponseScript("clearError();");
        }
    }

    void SetValidattionMode()
    {
        if (isFromTo)
        {
            uxUpdate.OnClientClick = "return doValidFromTo();";
        }
        else
        {
            AttributeInformation obj = (AttributeInformation)GeneralFuncsLib.ConvertXMLToObject(XMLMetricOperand, typeof(AttributeInformation));

            if (obj.Metrics != null)
            {
                uxUpdate.OnClientClick = "return doValidOperandCombobox();";
            }
            else
            {
                uxUpdate.OnClientClick = "return doValidOperandModal();";
            }
        }
    }

    void Save()
    {
        string spaName = "spa_RM_MRS_Save_AttributeRiskScore";
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParamsWithRecId();
        parameters.Add(new FilterParameter("@RecordID", RedId, DbType.Int32));
        parameters.Add(new FilterParameter("@AttributeID", SelectedAttributeID, DbType.Int32));
        parameters.Add(new FilterParameter("@RangName", RangeName, DbType.AnsiString));
        if (isFromTo)
        {
            parameters.Add(new FilterParameter("@FromValue", FromValue, DbType.Int32));
            parameters.Add(new FilterParameter("@ToValue", ToValue, DbType.Int32));
        }
        else
        {
            parameters.Add(new FilterParameter("@OperandKey", OperandKey, DbType.Int32));
            parameters.Add(new FilterParameter("@MetricValue", MetricValue, DbType.AnsiString));
        }
        parameters.Add(new FilterParameter("@Score", Score, DbType.Int32));
        parameters.Add(new FilterParameter("@FieldType", FieldType, DbType.Int32));
        parameters.Add(new FilterParameter("@ReturnValue", 0, DbType.Int32, true));
        WebServices.RiskServices.ExecuteNonQueryCommand(spaName, parameters, out parameters);

        int result = parameters.FindFilterParameterByName("@ReturnValue", false).ParameterValue.ToInt();
        // 1 is success, 0 - failed, > 1 duplicate
        if (result > 0)
        {
            uxAttributeRiskScoreMsg.ShowOnLoad = true;
            if (!isFromTo)
                uxAttributeRiskScoreMsg.Message = GetLocalResourceObject("AttributeRiskScoreOperandOverlap").ToASString();
            else
                uxAttributeRiskScoreMsg.Message = GetLocalResourceObject("AttributeRiskScoreRangeOverlap").ToASString();
        }
        else
        {
            Page.ReloadPage();
        }
    }
    protected void uxUpdate_Click(object sender, EventArgs e)
    {
        if ((ctrlValidatorAttribute.IsValid() && ctrlValidatorOperand.IsValid()) || ctrlValidatorAttribute.IsValid() && ctrlValidatorFromTo.IsValid())
        {
            Save();
        }
    }
    #endregion

    protected bool AttributeName_ValidateInput(Control control)
    {
        return uxAttributeNameList.SelectedValue != null;
    }
    protected bool uxToGreaterOrEqualThan_ValidateInput(Control control)
    {
        return ToValue >= FromValue;
    }
    protected bool uxMetric_ValidateInput(Control control)
    {
        AttributeInformation obj = (AttributeInformation)GeneralFuncsLib.ConvertXMLToObject(XMLMetricOperand, typeof(AttributeInformation));

        if (obj.Metrics != null)
        {
            if (uxMetric.CheckBoxes)
                return uxMetric.CheckedItems.Count > 0;
            else
                return uxMetric.SelectedValue != null;
        }
        else
        {
            return uxMetricListTextHide.Value.Trim().Length > 0;
        }

    }
    protected void uxOperand_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (uxOperand.SelectedItem != null && (uxOperand.SelectedItem.Text.Trim().Equals("=")))
        {
            uxMetric.CheckBoxes = true;
        }
        else
        {
            uxMetric.CheckBoxes = false;
        }
        OnDataBindControls(DataBindAction.BindMetricList);
    }
    protected bool checkRangeName_ValidateInput(Control control)
    {
        Regex reg = new Regex(WebSiteConstants.REG_SPECIAL_CHARACTERS);
        return reg.IsMatch(uxRangeName.Text);
    }
    protected bool uxMetricListText_ValidateInput(Control control)
    {
        return uxMetricListTextHide.Value.Trim().Length > 0;
    }
}