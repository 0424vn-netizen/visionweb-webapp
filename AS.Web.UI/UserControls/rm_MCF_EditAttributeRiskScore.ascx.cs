using AS.Common.DBManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_rm_MCF_EditAttributeRiskScore : GlobalUserControl
{
    #region Properties


    public delegate void SubmitHander(object sender, int resultCode);
    public event SubmitHander Submit;

    enum DataBindAction
    {
        BindAttributeList,
        BindOperandList,
        BindMetricList,
        Save
    }


    public int SelectedAttributeID
    {
        get
        {
            if (uxAttributeNameList.SelectedValue != null)
                return uxAttributeNameList.SelectedValue.Split('_')[0].ToInt();
            else
                return 0;
        }
    }
    string FieldType
    {
        get
        {
            if (uxAttributeNameList.SelectedValue != null)
                return uxAttributeNameList.SelectedValue.Split('_')[1];
            else
                return string.Empty;
        }
    }
    public int RedId
    {
        get
        {
            if (!string.IsNullOrEmpty(uxhdRedId.Value))
                return uxhdRedId.Value.ToInt();
            else
                return 0;
        }
        set
        {
            uxhdRedId.Value = value.ToString();
        }
    }

    public string RangeName
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

    public int FromValue
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
    public int ToValue
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
    public int OperandKey
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
            uxOperand.SelectedValue = value.ToString();
            uxOperand_SelectedIndexChanged(null, null);
        }
    }

    public bool IsBinded
    {
        get
        {
            if (ViewState["IsBinded"] != null)
                return ViewState["IsBinded"].ToBoolean();
            else
                return false;
        }
        set
        {
            ViewState["IsBinded"] = value;
        }
    }

    public string MetricValue
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
                if (uxMetric.CheckBoxes)
                {
                    foreach (var v in value.Split(';'))
                    {
                        uxMetric.Items.Where(i => i.Value.Equals(v)).FirstOrDefault().Checked = true;
                    }
                }
                else
                {
                    uxMetric.SelectedValue = value;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(value) && value.ToString().Split(';').Count() > 2)
                    uxMetricListText.Text = value.ToString().Split(';').Count() + " " + GetLocalResourceObject("LiteralResourceFindMoreitem").ToString();
                else
                    uxMetricListText.Text = value;

                uxMetricListTextHide.Value = value;
            }
        }
    }
    public int Score
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
    public string MetricFromTo
    {
        set
        {
            var valueFromto = value.Split(';');
            uxMetricFrom.Text = valueFromto[0];
            if (valueFromto.Length > 1)
            {
                uxMetricTo.Text = valueFromto[1];
            }
        }
    }
    public AttributeRiskScoreMode fieldTypeMode
    {
        get
        {
            return (AttributeRiskScoreMode)Convert.ToInt32(uxAttributeNameList.SelectedValue.ToString().Split('_')[1]); //  1 is from/to mode, 2 is operand
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
                    foreach (Operand o in obj.Operands.Items)
                    {
                        var newItem = new Telerik.Web.UI.RadComboBoxItem(HttpUtility.HtmlDecode(o.Text), o.Value);
                        newItem.Attributes.Add("FieldType", o.FieldType);
                        uxOperand.Items.Add(newItem);
                    }
                    if (fieldTypeMode != AttributeRiskScoreMode.OperandMetric)
                        OnDataBindControls(DataBindAction.BindMetricList);
                }
                break;
            case DataBindAction.BindMetricList:
                {
                    //4: signle select
                    if (!string.IsNullOrEmpty(uxOperand.SelectedItem.Attributes["FieldType"]) &&
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
                            string queryString = this.Page.BuildSecureQueryString("SelectedAttributeID=" + uxAttributeNameList.SelectedValue.ToString().Split('_')[0] + "&OperandValue=" + uxOperand.SelectedValue + "&SelectedMetricID=" + "&FromEdit=1");
                            btnlinkFindOwner.Attributes["onclick"] = "return ShowPopupModal('" + ResolveUrl("~/") + "FindOwners.aspx?" + queryString + "', 'auto'); return false;";

                        }
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

    public void BindAttributeList()
    {

        OnDataBindControls(DataBindAction.BindAttributeList);
        if (uxAttributeNameList.SelectedItem == null)
        {
            SetSelectedAttributeName(uxhdAttributeId.Value.ToASString());
        }
        if (string.IsNullOrEmpty(uxRangeName.Text))
            uxRangeName.Text = uxhdRangeName.Value.ToASString();
    }

    public void BindMetricList()
    {
        OnDataBindControls(DataBindAction.BindMetricList);
    }

    public void SetSelectedAttributeName(string value)
    {
        var item = uxAttributeNameList.Items.Where(i => i.Value.Split('_')[0].Equals(value)).FirstOrDefault();
        if (item != null)
        {
            item.Selected = true;
            uxAttributeNameList_SelectedIndexChanged(null, null);
            hddMinValueFromTo.Value = MinValue;
            hddMaxValueFromTo.Value = MinValue;
            hddMinValueMetric.Value = MinValue;
            hddMaxValueMetric.Value = MaxValue;
        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        uxMetricListText.Attributes["onkeypress"] = "return false;";
    }

    protected void uxAttributeNameList_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (uxAttributeNameList.SelectedValue != null)
        {
            uxRangeName.Enabled = true;
            if (fieldTypeMode == AttributeRiskScoreMode.FromTo)
            {
                uxFrom.Enabled = uxTo.Enabled = uxScore.Enabled = true;
                uxOperand.Enabled = uxMetric.Enabled = false;
                uxOperand.ClearSelection();
                uxMetric.ClearSelection();
                uxScore.Text = string.Empty;
                uxMetricFrom.Visible = uxMetricTo.Visible = uxValidatorLabelMetricFrom.Visible = uxValidatorLabelMetricTo.Visible = false;
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
                    uxMetricListPanel.Visible = false;
                    uxValidatorLabeluxMetricListText.Visible = false;
                    uxMetric.Visible = false;
                    uxMetricFrom.Visible = true;
                    uxMetricTo.Visible = uxValidatorLabelMetricTo.Visible = uxValidatorLabelMetricFrom.Visible = false;
                    uxMetricFrom.Width = 100;
                    uxValidatorLabeluxMetric.ApplyFor = "uxMetricFrom";
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
        }
    }

    void SetValidattionMode()
    {
        if (fieldTypeMode == AttributeRiskScoreMode.FromTo)
        {
            uxUpdate.OnClientClick = "return doValidFromToEdit();";
        }
        else
        {
            if (fieldTypeMode == AttributeRiskScoreMode.OperandMetric)
            {
                uxUpdate.OnClientClick = "return doValidMetricType3Edit();";
            }
            else
            {
                AttributeInformation obj = (AttributeInformation)GeneralFuncsLib.ConvertXMLToObject(XMLMetricOperand, typeof(AttributeInformation));

                if (obj.Metrics != null)
                {
                    uxUpdate.OnClientClick = "return doValidOperandEditCombobox();";
                }
                else
                {
                    uxUpdate.OnClientClick = "return doValidOperandEditModal();";
                }
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
        // 1 is success, 0 - failed, -1 duplicate
        if (result > 0)
        {
            if (fieldTypeMode != AttributeRiskScoreMode.FromTo)
                uxAttributeRiskScoreMsg.Message = GetLocalResourceObject("AttributeRiskScoreOperandOverlap").ToASString();
            else
                uxAttributeRiskScoreMsg.Message = GetLocalResourceObject("AttributeRiskScoreRangeOverlap").ToASString();

            uxAttributeRiskScoreMsg.ShowOnLoad = true;
        }
        if (result == 0 && this.Submit != null)
        {
            Submit(this, 1);
        }

    }
    protected void uxUpdate_Click(object sender, EventArgs e)
    {
        if ((ctrlValidatorAttribute.IsValid() && ctrlValidatorOperand.IsValid()) || ctrlValidatorAttribute.IsValid() && ctrlValidatorFromTo.IsValid())
        {
            Save();
        }
    }
    protected void uxCancel_Click(object sender, EventArgs e)
    {

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
        if (uxMetric.CheckBoxes)
            return uxMetric.CheckedItems.Count > 0;
        else
            return uxMetric.SelectedValue != null;
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
    protected bool checkRangeName_ValidateInput(Control control)
    {
        Regex reg = new Regex(WebSiteConstants.REG_SPECIAL_CHARACTERS);
        return reg.IsMatch(uxRangeName.Text);

    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        ((ReportPage)Page).AjaxAddResponseScript("addCheckSpecialCharacters();");
    }
    protected bool uxMetricListText_ValidateInput(Control control)
    {
        return uxMetricListTextHide.Value.Trim().Length > 0;
    }

}
