using AS.Common.DBManager;
using AS.Controls.Validators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_rm_MCF_AttributeRiskScore : GlobalUserControl
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
    AttributeRiskScoreMode fieldTypeMode
    {
        get
        {
            if (uxAttributeNameList.SelectedValue != null && uxAttributeNameList.SelectedValue.ToString().IndexOf('_') != -1)
                return (AttributeRiskScoreMode)Convert.ToInt32(uxAttributeNameList.SelectedValue.ToString().Split('_')[1]); //  1 is from/to mode, 2 is operand, 3 matric
            else
                return AttributeRiskScoreMode.None;
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

    string MinValue
    {
        get
        {
            if (uxAttributeNameList.SelectedValue != null && uxAttributeNameList.SelectedValue.ToString().IndexOf('_') != -1)
                return uxAttributeNameList.SelectedValue.ToString().Split('_')[2];
            else
                return string.Empty;
        }
    }

    string MaxValue
    {
        get
        {
            if (uxAttributeNameList.SelectedValue != null && uxAttributeNameList.SelectedValue.ToString().IndexOf('_') != -1)
                return uxAttributeNameList.SelectedValue.ToString().Split('_')[3];
            else
                return string.Empty;
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
                    DataTable td = WebServices.RiskServices.GetReports("spa_RM_MCF_MRS_Get_AttributeList", parameters);
                    uxAttributeNameList.Items.Clear();
                    foreach (DataRow r in td.Rows)
                    {
                        uxAttributeNameList.Items.Add(new Telerik.Web.UI.RadComboBoxItem(r["AttributeName"].ToASString(), string.Format("{0}_{1}_{2}_{3}", r["AttributeID"].ToASString(), r["FieldType"].ToASString(), r["MinValue"], r["MaxValue"])));
                    }
                }
                break;
            case DataBindAction.BindOperandList:
                {
                    parameters.AddLanguageID();
                    parameters.Add(new FilterParameter("@AttributeID", SelectedAttributeID, DbType.Int32));
                    DataTable td = WebServices.RiskServices.GetReports("spa_RM_MCF_MRS_Get_MetricOperandByAttribute", parameters);
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
                            var newItem = new Telerik.Web.UI.RadComboBoxItem(HttpUtility.HtmlDecode(o.Text), o.Value);
                            newItem.Attributes.Add("FieldType", o.FieldType);
                            uxOperand.Items.Add(newItem);
                        }
                        // uxOperand.Items.Insert(0, "");
                        if (fieldTypeMode == AttributeRiskScoreMode.Operand || fieldTypeMode == AttributeRiskScoreMode.SingleSelect || fieldTypeMode == AttributeRiskScoreMode.OperandIfEmpty)
                            OnDataBindControls(DataBindAction.BindMetricList);
                    }
                }
                break;
            case DataBindAction.BindMetricList:
                {
                    //4: signle select
                    if (uxOperand.SelectedItem != null && !string.IsNullOrEmpty(uxOperand.SelectedItem.Attributes["FieldType"]) &&
                        uxOperand.SelectedItem.Attributes["FieldType"].Trim().Equals("4"))
                    {
                        uxMetric.CheckBoxes = false;
                    }
                    else if (uxOperand.SelectedItem != null && (uxOperand.SelectedItem.Text.Trim().Equals("=")))
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
                        if (obj.Metrics != null || fieldTypeMode == AttributeRiskScoreMode.OperandIfEmpty)
                        {
                            uxMetric.Visible = true;
                            uxMetricListPanel.Visible = false;
                            uxValidatorLabeluxMetricListText.Visible = false;
                            uxValidatorLabeluxMetric.Visible = true;
                            List<Metric> metrics = new List<Metric>();
                            if (obj.Metrics != null)
                            {
                                metrics = obj.Metrics.GetMetricByOperand(uxOperand.SelectedValue);
                                foreach (Metric m in metrics)
                                {
                                    uxMetric.Items.Add(new Telerik.Web.UI.RadComboBoxItem(HttpUtility.HtmlDecode(m.Text), m.Value));
                                }
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
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        ((ReportPage)Page).AjaxAddResponseScript("addCheckSpecialCharacters();");
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
            if (fieldTypeMode == AttributeRiskScoreMode.FromTo)
            {
                uxFrom.Enabled = uxTo.Enabled = uxScore.Enabled = true;
                uxOperand.Enabled = uxMetric.Enabled = false;
                uxOperand.Text = string.Empty;
                uxOperand.ClearSelection();
                uxMetric.Text = string.Empty;
                uxMetric.ClearSelection();
                uxMetric.ClearCheckedItems();
                uxMetricFrom.Visible = uxMetricTo.Visible = uxValidatorLabelMetricFrom.Visible = uxValidatorLabelMetricTo.Visible = false;
                hddMinValueFromTo.Value = MinValue;
                hddMaxValueFromTo.Value = MaxValue;
            }
            else
            {
                uxFrom.Enabled = uxTo.Enabled = false;
                uxFrom.Text = uxTo.Text = uxMetricFrom.Text = uxMetricTo.Text = string.Empty;
                uxMetric.ClearSelection();
                uxOperand.ClearSelection();
                uxOperand.Enabled = uxScore.Enabled = true;
                if (fieldTypeMode == AttributeRiskScoreMode.OperandMetric)
                {
                    uxMetric.Visible = false;
                    uxMetricFrom.Visible = true;
                    uxMetricTo.Visible = uxValidatorLabelMetricTo.Visible = uxValidatorLabelMetricFrom.Visible = false;
                    uxMetricFrom.Width = 100;
                    uxValidatorLabeluxMetric.ApplyFor = "uxMetricFrom";
                    hddMinValueMetric.Value = MinValue;
                    hddMaxValueMetric.Value = MaxValue;
                }
                else
                {
                    uxValidatorLabeluxMetric.ApplyFor = "uxMetric";
                    uxMetric.Visible = true;
                    uxMetricFrom.Visible = uxMetricTo.Visible = uxValidatorLabelMetricFrom.Visible = uxValidatorLabelMetricTo.Visible = false;
                    uxMetric.Enabled = true;
                }
                OnDataBindControls(DataBindAction.BindOperandList);
            }
            SetValidattionMode();
            ((ReportPage)Page).AjaxAddResponseScript("clearError();");
            ((ReportPage)Page).AjaxAddResponseScript("addCheckSpecialCharacters();");
        }
    }
    protected void uxOperand_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (fieldTypeMode == AttributeRiskScoreMode.OperandMetric)
        {
            if (uxOperand.SelectedItem.Text.Trim().Contains("<>"))
            {
                uxMetricFrom.Visible = uxValidatorLabelMetricFrom.Visible = uxMetricTo.Visible = uxValidatorLabelMetricTo.Visible = true;
                uxMetricFrom.Width = uxMetricTo.Width = 50;
                uxValidatorLabeluxMetric.ApplyFor = "uxMetric";
            }
            else
            {
                uxMetricFrom.Visible = true;
                uxMetricTo.Visible = uxValidatorLabelMetricTo.Visible = uxValidatorLabelMetricFrom.Visible = false;
                uxMetricFrom.Width = 100;
                uxValidatorLabeluxMetric.ApplyFor = "uxMetricFrom";
            }
        }
        else
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
    }
    void SetValidattionMode()
    {
        if (fieldTypeMode == AttributeRiskScoreMode.FromTo)
        {
            uxUpdate.OnClientClick = "return doValidFromTo();";
        }
        else
        {
            if (fieldTypeMode == AttributeRiskScoreMode.OperandMetric)
            {
                uxUpdate.OnClientClick = "return doValidMetricType3();";
            }
            else
            {
                AttributeInformation obj = (AttributeInformation)GeneralFuncsLib.ConvertXMLToObject(XMLMetricOperand, typeof(AttributeInformation));

                if (obj.Metrics != null || fieldTypeMode == AttributeRiskScoreMode.OperandIfEmpty)
                {
                    uxUpdate.OnClientClick = "return doValidOperandCombobox();";
                }
                else
                {
                    uxUpdate.OnClientClick = "return doValidOperandModal();";
                }
            }
        }
    }

    void Save()
    {
        string spaName = "spa_RM_MCF_MRS_Save_AttributeRiskScore";
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParamsWithRecId();
        parameters.Add(new FilterParameter("@RecordID", RedId, DbType.Int32));
        parameters.Add(new FilterParameter("@AttributeID", SelectedAttributeID, DbType.Int32));
        parameters.Add(new FilterParameter("@RangName", RangeName, DbType.AnsiString));
        if (fieldTypeMode == AttributeRiskScoreMode.FromTo)
        {
            parameters.Add(new FilterParameter("@FromValue", FromValue, DbType.Int32));
            parameters.Add(new FilterParameter("@ToValue", ToValue, DbType.Int32));
        }
        else
        {
            parameters.Add(new FilterParameter("@OperandKey", OperandKey, DbType.Int32));
            if (fieldTypeMode == AttributeRiskScoreMode.OperandMetric)
            {
                int mectricfrom = int.MinValue;
                int mectricto = int.MaxValue;
                string mectric = string.Empty;
                if (uxOperand.SelectedItem.Text.Trim().Contains("<>"))
                {
                    mectricfrom = int.Parse(uxMetricFrom.Text);
                    mectricto = int.Parse(uxMetricTo.Text);
                    mectric = string.Format("{0};{1}", mectricfrom, mectricto);
                }
                else
                {
                    if (uxOperand.SelectedItem.Text.Trim().StartsWith("<"))
                        mectricto = int.Parse(uxMetricFrom.Text);
                    else mectricfrom = int.Parse(uxMetricFrom.Text);
                    mectric = uxMetricFrom.Text;
                }
                parameters.Add(new FilterParameter("@FromValue", mectricfrom, DbType.Int32));
                parameters.Add(new FilterParameter("@ToValue", mectricto, DbType.Int32));
                parameters.Add(new FilterParameter("@MetricValue", mectric, DbType.AnsiString));
            }
            else
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
            if (fieldTypeMode != AttributeRiskScoreMode.FromTo)
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

        if (obj.Metrics != null || fieldTypeMode == AttributeRiskScoreMode.OperandIfEmpty)
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
    protected bool checkRangeName_ValidateInput(Control control)
    {
        Regex reg = new Regex(WebSiteConstants.REG_SPECIAL_CHARACTERS);
        return reg.IsMatch(uxRangeName.Text);
    }
    protected bool uxMetricListText_ValidateInput(Control control)
    {
        return uxMetricListTextHide.Value.Trim().Length > 0;
    }

    protected void uxCancel_Click(object sender, EventArgs e)
    {
        uxMetric.ClearCheckedItems();
        uxMetric.ClearSelection();
        uxMetric.DataSource = null;
        uxMetric.SelectedValue = string.Empty;
        uxMetric.Items.Clear();
        uxMetric.DataBind();
    }
}