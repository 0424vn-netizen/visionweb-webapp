using AS.Common.DBManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using Telerik.Web.UI;
using AS.Common.Serialization;
using System.Text;
using AS.Controls.Pages;
using System.ComponentModel.DataAnnotations;
using AS.Common.Formater;
using System.Web.UI.HtmlControls;

public partial class UserControls_rm_MerchantClassification_Attribute : GlobalUserControl
{
    private string _ClassAttribute = "ClassAttribute_";
    private const string CT_ATTRIBUTE = "uxAttributeList";
    private const string CT_OPENRAND = "uxOperand";
    private const string CT_METRIC = "uxMetric";
    private const string CT_METRIC_TEXTLISTHIDE = "uxMetricListTextHide";
    private const string CT_METRIC_TEXTLIST = "uxMetricTextList";

    private const string CT_METRIC_TEXTLIST_PANEL = "uxMetricTextList_Panel";
    private const string CT_METRIC_FINDLINK = "btnlinkFindOwner";
    private const string CT_TO = "uxAttributeTo";
    private const string CT_FROM = "uxAttributeFrom";

    public string ClassificationId
    {
        get { return ViewState["ClassificationId"].ToString(); }
        set { ViewState["ClassificationId"] = value; }
    }

    public DataTable AttributeTable
    {
        get
        {
            if (ViewState["AttributeInformation"] != null)
                return (DataTable)(ViewState["AttributeInformation"]);
            else
                return null;
        }
        set
        {
            ViewState["AttributeInformation"] = value;
        }
    }

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            DoBindData();
        }
    }

    protected void uxAttributeDataGrid_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            GridDataItem item = e.Item as GridDataItem;
            DataRowView dataRow = e.Item.DataItem as DataRowView;
            RadComboBox attribute = item.FindControl(CT_ATTRIBUTE) as RadComboBox;

            attribute.DataSource = GetAttributes();
            attribute.DataBind();
            attribute.EmptyMessage = GetLocalResourceObject("Attribute.SelectText").ToString();
            attribute.SelectedValue = dataRow[PAGE_GRID_OBJECT.ATTRIBUTE_ID].ToString();

            AttributeRiskScoreMode fieldType = (AttributeRiskScoreMode)int.Parse(attribute.SelectedItem != null ? attribute.SelectedItem.Attributes["FieldType"].ToString() : "0");
            DisplayControl(item, fieldType);

            GetOperandMetric(dataRow[PAGE_GRID_OBJECT.ATTRIBUTE_ID].ToString());
            LoadOperand(dataRow[PAGE_GRID_OBJECT.ATTRIBUTE_ID].ToString(), item);
            SetData(item);

        }
    }

    protected void uxAttributeDataGrid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        uxAttributeDataGrid.DataSource = AttributeTable;

    }

    protected void uxAttributeDataGrid_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.DeleteCommandName)
        {
            GridDataItem item = (GridDataItem)e.Item;
            string value = item.GetDataKeyValue(PAGE_GRID_OBJECT.RECORDID).ToString();
            DataTable dt = (DataTable)AttributeTable;
            DataRow row = dt.Rows.Find(value);
            dt.Rows.Remove(row);
            uxAttributeDataGrid.Rebind();

        }

    }

    protected void uxAttributeList_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        // Load Combo for Operand and Metric        
        RadComboBox combo = (RadComboBox)sender;
        GridDataItem item = (GridDataItem)combo.NamingContainer;

        AttributeRiskScoreMode fieldType = (AttributeRiskScoreMode)int.Parse(combo.SelectedItem.Attributes["FieldType"].ToString());
        DisplayControl(item, fieldType);

        switch (fieldType)
        {
            case AttributeRiskScoreMode.Operand:
                GetOperandMetric(combo.SelectedValue);
                LoadOperand(combo.SelectedValue, item);
                TextBox metric_textList = item.FindControl(CT_METRIC_TEXTLIST) as TextBox;
                HiddenField metric_textListHide = item.FindControl(CT_METRIC_TEXTLISTHIDE) as HiddenField;
                if (metric_textList != null)
                {
                    metric_textList.Text = string.Empty;
                    metric_textListHide.Value = string.Empty;
                }

                break;
        }

        //((BaseMasterPage)this.Page.Master).AjaxAddResponseScript("reStyleMetricDropdow();");

    }

    protected void operand_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        RadComboBox combo = (RadComboBox)sender;
        GridDataItem item = (GridDataItem)combo.NamingContainer;
        RadComboBox attrs = item.FindControl(CT_ATTRIBUTE) as RadComboBox;
        LoadMetric(attrs.SelectedValue, combo.SelectedValue, combo.SelectedItem.Text, item);
        TextBox metric_textList = item.FindControl(CT_METRIC_TEXTLIST) as TextBox;
        HiddenField metric_textListHide = item.FindControl(CT_METRIC_TEXTLISTHIDE) as HiddenField;
        if (metric_textList != null)
        {
            metric_textList.Text = string.Empty;
            metric_textListHide.Value = string.Empty;
        }
    }

    protected void metric_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        RadComboBox combo = (RadComboBox)sender;
        GridDataItem item = (GridDataItem)combo.NamingContainer;
        RadComboBox attrs = item.FindControl(CT_ATTRIBUTE) as RadComboBox;
        //LoadMetric(attrs.SelectedValue, combo.SelectedValue, combo.SelectedItem.Text, item);

    }

    protected void uxAttributeList_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
        DataRowView dataItem = (DataRowView)e.Item.DataItem;
        e.Item.Attributes["FieldType"] = dataItem["FieldType"].ToString();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        Button rb = (Button)sender;
        GridDataItem item = (GridDataItem)rb.NamingContainer;

        DataRowView dataRow = item.DataItem as DataRowView;
    }

    #endregion

    #region Methods
    private void DoBindData()
    {
        DataTable dt = new DataTable();
        if (string.IsNullOrEmpty(ClassificationId))
        {
            dt.Columns.Add(PAGE_GRID_OBJECT.RECORDID, typeof(int));
            dt.Columns.Add(PAGE_GRID_OBJECT.MERCHANT_CLASSIFICATION_ID);
            dt.Columns.Add(PAGE_GRID_OBJECT.MERCHANT_CLASSIFICATION_NAME);
            dt.Columns.Add(PAGE_GRID_OBJECT.ATTRIBUTE_ID);
            dt.Columns.Add(PAGE_GRID_OBJECT.ATTRIBUTE_NAME);
            dt.Columns.Add(PAGE_GRID_OBJECT.FROM_VALUE);
            dt.Columns.Add(PAGE_GRID_OBJECT.TO_VALUE);
            dt.Columns.Add(PAGE_GRID_OBJECT.OPERAND);
            dt.Columns.Add(PAGE_GRID_OBJECT.METRICVALUE);

            dt.Rows.Add(1, -1, string.Empty, -1, "", "", "", "", "");
        }
        if (AttributeTable == null)
            AttributeTable = dt;

        AttributeTable.PrimaryKey = new DataColumn[] { AttributeTable.Columns[PAGE_GRID_OBJECT.RECORDID] };

    }

    private void LoadMetric(string attrId, string operandId, string text, GridDataItem item)
    {
        string classAttrId = _ClassAttribute + attrId;
        RadComboBox metric = item.FindControl(CT_METRIC) as RadComboBox;
        HiddenField metric_textListHide = item.FindControl(CT_METRIC_TEXTLISTHIDE) as HiddenField;
        TextBox metric_textList = item.FindControl(CT_METRIC_TEXTLIST) as TextBox;
        PlaceHolder uxMetricTextList_Panel = item.FindControl(CT_METRIC_TEXTLIST_PANEL) as PlaceHolder;
        HtmlAnchor alink = uxMetricTextList_Panel.FindControl(CT_METRIC_FINDLINK) as HtmlAnchor;

        AttributeInformation config = (AttributeInformation)ViewState[classAttrId];

        if (config != null && !attrId.Equals("-1"))
        {
            if (config.Metrics != null)
            {
                metric.Visible = true;
                uxMetricTextList_Panel.Visible = false;
                List<Metric> list = config.Metrics.GetMetricByOperand(operandId);
                if ("=".Equals(text))
                    metric.CheckBoxes = true;
                else
                {
                    //list.Add(new Metric { Filter = "", Text = "", Value = "-1" });
                    metric.CheckBoxes = false;
                }
                metric.DataSource = list;
                metric.DataBind();

                foreach (RadComboBoxItem radItem in metric.Items)
                {
                    radItem.CssClass = "metric-item-checkbox";
                }
            }
            else
            {
                metric.Visible = false;
                uxMetricTextList_Panel.Visible = true;

                DataRowView dataRow = item.DataItem as DataRowView;
                if (item.DataItem.IsNotNullData())
                {
                    string metrics = dataRow[PAGE_GRID_OBJECT.METRICVALUE].ToString();
                    if (!string.IsNullOrEmpty(metrics) && metrics.Split(';').Count() > 2)
                        metric_textList.Text = metrics.Split(';').Count() + " " + GetLocalResourceObject("LiteralResourceFindMoreitem").ToString();
                    else
                        metric_textList.Text = metrics;

                    metric_textListHide.Value = metrics;
                }

                string queryString = this.Page.BuildSecureQueryString("SelectedAttributeID=" + attrId + "&OperandValue=" + operandId + "&SelectedMetricID=" + "" + "&FromEdit=0&FromChildModal=1&MetricControlID=" + metric_textListHide.ClientID);
                if (alink != null)
                {
                    alink.Attributes["onclick"] = "parent.OpenPopupModal(1, '" + ResolveUrl("~/") + "FindOwners.aspx?" + queryString + "', 'auto'); return false;";
                }
            }
        }

    }

    private void GetOperandMetric(string attributeId)
    {
        // parse xml
        string classAttrId = _ClassAttribute + attributeId;
        if (ViewState[classAttrId] == null)
        {

            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            parameters.Add(new FilterParameter("@AttributeID", attributeId, DbType.Int32));

            DataTable tb = WebServices.RiskServices.GetReports("spa_RM_MRS_Get_MetricOperandByAttribute", parameters);
            StringBuilder builder = new StringBuilder();
            string xml = string.Empty;
            if (tb.Rows.Count > 1)
            {
                foreach (DataRow row in tb.Rows)
                {
                    builder.Append(row.Field<string>(0));
                }
                xml = builder.ToString();
            }
            else
            {
                xml = tb.Rows[0].Field<string>(0);
            }

            AttributeInformation config = (AttributeInformation)GeneralFuncsLib.ConvertXMLToObject(xml, typeof(AttributeInformation));
            if (config.Operands != null)
                config.Operands.Items.Insert(0, new Operand { Text = "", Value = "-1" });

            ViewState[classAttrId] = config;
        }

    }

    public void InsertNew()
    {
        DataTable dt = AttributeTable;
        //Store current values
        StoreGridValues();

        int maxRec = 0;
        if (dt.Rows.Count > 0)
        {
            maxRec = Convert.ToInt32(dt.Compute("max([RecordID])", string.Empty));
        }

        DataRow row = dt.NewRow();
        row[PAGE_GRID_OBJECT.RECORDID] = ++maxRec;
        row[PAGE_GRID_OBJECT.ATTRIBUTE_ID] = -1;
        row[PAGE_GRID_OBJECT.FROM_VALUE] = 0;
        row[PAGE_GRID_OBJECT.TO_VALUE] = 0;
        dt.Rows.Add(row);

        uxAttributeDataGrid.MasterTableView.IsItemInserted = false;
        uxAttributeDataGrid.Rebind();

    }

    private void StoreGridValues()
    {
        foreach (GridDataItem item in uxAttributeDataGrid.Items)
        {
            string value = item.GetDataKeyValue(PAGE_GRID_OBJECT.RECORDID).ToString();
            DataTable dt = (DataTable)AttributeTable;
            DataRow row = dt.Rows.Find(value);
            if (row != null)
            {
                RadComboBox attr = item.FindControl(CT_ATTRIBUTE) as RadComboBox;
                AttributeRiskScoreMode fieldType = (AttributeRiskScoreMode)int.Parse(attr.SelectedItem != null ? attr.SelectedItem.Attributes["FieldType"].ToString() : "0");

                row[PAGE_GRID_OBJECT.ATTRIBUTE_ID] = attr.SelectedValue;

                if (fieldType == AttributeRiskScoreMode.Operand)
                {
                    RadComboBox oper = item.FindControl(CT_OPENRAND) as RadComboBox;
                    RadComboBox metric = item.FindControl(CT_METRIC) as RadComboBox;


                    row[PAGE_GRID_OBJECT.OPERAND] = oper.SelectedValue;

                    TextBox metric_textList = item.FindControl(CT_METRIC_TEXTLIST) as TextBox;
                    HiddenField metric_textListHide = item.FindControl(CT_METRIC_TEXTLISTHIDE) as HiddenField;

                    AttributeInformation config = (AttributeInformation)ViewState[_ClassAttribute + attr.SelectedValue];
                    if (config != null)
                    {
                        if (config.Metrics != null)
                        {
                            string metrics = (metric.CheckBoxes ? (metric.CheckedItems.Count > 0 ? string.Join(";", metric.CheckedItems.Select(t => t.Value)) : string.Empty) : metric.SelectedValue);
                            row[PAGE_GRID_OBJECT.METRICVALUE] = metrics;
                        }
                        else
                        {
                            row[PAGE_GRID_OBJECT.METRICVALUE] = metric_textList.Text.Trim();
                            row[PAGE_GRID_OBJECT.METRICVALUE] = metric_textListHide.Value.Trim();
                        }
                    }
                }
                else if (fieldType == AttributeRiskScoreMode.FromTo)
                {
                    TextBox To = item.FindControl(CT_TO) as TextBox;
                    TextBox From = item.FindControl(CT_FROM) as TextBox;
                    row[PAGE_GRID_OBJECT.FROM_VALUE] = (string.IsNullOrEmpty(From.Text) ? "0" : From.Text);
                    row[PAGE_GRID_OBJECT.TO_VALUE] = (string.IsNullOrEmpty(To.Text) ? "0" : To.Text);
                }
            }
        }
    }

    private void SetData(GridDataItem item)
    {
        RadComboBox attr = item.FindControl(CT_ATTRIBUTE) as RadComboBox;
        AttributeRiskScoreMode fieldType = (AttributeRiskScoreMode)int.Parse(attr.SelectedItem != null ? attr.SelectedItem.Attributes["FieldType"].ToString() : "0");
        DataRowView dataRow = item.DataItem as DataRowView;
        PlaceHolder metric_textListPanel = item.FindControl(CT_METRIC_TEXTLIST_PANEL) as PlaceHolder;
        RadComboBox oper = item.FindControl(CT_OPENRAND) as RadComboBox;
        RadComboBox metric = item.FindControl(CT_METRIC) as RadComboBox;
        TextBox metric_textList = item.FindControl(CT_METRIC_TEXTLIST) as TextBox;
        HiddenField metric_textListHide = item.FindControl(CT_METRIC_TEXTLISTHIDE) as HiddenField;
        if (fieldType == AttributeRiskScoreMode.Operand)
        {
            metric.Visible = true;
            string[] metrics = dataRow[PAGE_GRID_OBJECT.METRICVALUE].ToString().Split(';');
            oper.SelectedValue = dataRow[PAGE_GRID_OBJECT.OPERAND].ToString();
            LoadMetric(attr.SelectedValue, dataRow[PAGE_GRID_OBJECT.OPERAND].ToString(), oper.SelectedItem.Text, item);


            foreach (RadComboBoxItem chk in metric.Items)
            {
                if (metrics.Contains(chk.Value))
                    chk.Checked = true;

            }
            if (!"=".Equals(oper.SelectedItem.Text))
            {
                metric.SelectedValue = dataRow[PAGE_GRID_OBJECT.METRICVALUE].ToString();
            }

        }
        else if (fieldType == AttributeRiskScoreMode.FromTo)
        {
            metric_textListPanel.Visible = false;
            metric.Visible = true;
            TextBox To = item.FindControl(CT_TO) as TextBox;
            TextBox From = item.FindControl(CT_FROM) as TextBox;
            From.Text = FormatData.FormatNumber(dataRow[PAGE_GRID_OBJECT.FROM_VALUE], 0).Replace(",", "");
            To.Text = FormatData.FormatNumber(dataRow[PAGE_GRID_OBJECT.TO_VALUE], 0).Replace(",", "");
        }

    }

    public ClassificationAttribute GetData()
    {
        string data = string.Empty;
        ClassificationAttribute clsAtt = new ClassificationAttribute();
        clsAtt.Attributes = new List<Attribute>();
        foreach (GridDataItem item in uxAttributeDataGrid.Items)
        {
            RadComboBox attr = item.FindControl(CT_ATTRIBUTE) as RadComboBox;
            AttributeRiskScoreMode fieldType = (AttributeRiskScoreMode)int.Parse(attr.SelectedItem != null ? attr.SelectedItem.Attributes["FieldType"].ToString() : "0");

            Attribute aAttr = new Attribute();
            if (attr.SelectedValue != "-1")
            {
                aAttr.Value = attr.SelectedValue;

                if (fieldType == AttributeRiskScoreMode.Operand)
                {
                    RadComboBox oper = item.FindControl(CT_OPENRAND) as RadComboBox;
                    RadComboBox metric = item.FindControl(CT_METRIC) as RadComboBox;
                    TextBox metric_textList = item.FindControl(CT_METRIC_TEXTLIST) as TextBox;
                    HiddenField metric_textListHide = item.FindControl(CT_METRIC_TEXTLISTHIDE) as HiddenField;

                    aAttr.Operand = oper.SelectedValue;
                    if (metric.Visible)
                    {
                        string metrics = metric.CheckedItems.Count > 0 ? string.Join(";", metric.CheckedItems.Select(t => t.Value)) : metric.SelectedValue;
                        aAttr.Metrics = metrics;
                    }
                    else
                    {
                        aAttr.Metrics = metric_textListHide.Value.Trim(';');
                    }

                }
                else
                {
                    TextBox To = item.FindControl(CT_TO) as TextBox;
                    TextBox From = item.FindControl(CT_FROM) as TextBox;
                    aAttr.From = From.Text;
                    aAttr.To = To.Text;
                }

                clsAtt.Attributes.Add(aAttr);
            }

        }

        return clsAtt;
    }

    private void DisplayControl(GridDataItem item, AttributeRiskScoreMode mode)
    {
        RadComboBox Operand = item.FindControl(CT_OPENRAND) as RadComboBox;
        RadComboBox Metric = item.FindControl(CT_METRIC) as RadComboBox;
        TextBox AttTo = item.FindControl(CT_TO) as TextBox;
        TextBox AttFrom = item.FindControl(CT_FROM) as TextBox;
        PlaceHolder metric_textListPanel = item.FindControl(CT_METRIC_TEXTLIST_PANEL) as PlaceHolder;

        RadComboBox metric = item.FindControl(CT_METRIC) as RadComboBox;
        switch (mode)
        {
            case AttributeRiskScoreMode.Operand:
                AttTo.Text = AttFrom.Text = string.Empty;
                Operand.Enabled = Metric.Enabled = true;
                AttTo.Enabled = AttFrom.Enabled = !Operand.Enabled;
                metric_textListPanel.Visible = false;
                metric.Visible = true;
                break;
            case AttributeRiskScoreMode.FromTo:
                Operand.DataSource = new DataTable();
                Operand.DataBind();
                Metric.DataSource = new DataTable();
                Metric.DataBind();
                Operand.Enabled = Metric.Enabled = false;
                AttTo.Enabled = AttFrom.Enabled = !Operand.Enabled;
                metric_textListPanel.Visible = false;
                metric.Visible = true;
                break;
            case AttributeRiskScoreMode.None:
                Operand.Enabled = Metric.Enabled = AttTo.Enabled = AttFrom.Enabled = false;
                break;

        }

    }

    private void LoadOperand(string value, GridDataItem item)
    {
        string classAttrId = _ClassAttribute + value;
        RadComboBox operand = item.FindControl(CT_OPENRAND) as RadComboBox;
        RadComboBox metric = item.FindControl(CT_METRIC) as RadComboBox;
        AttributeInformation config = (AttributeInformation)ViewState[classAttrId];

        if (config != null)
        {
            if (config.Operands != null)
            {
                operand.DataSource = config.Operands.Items;
                operand.DataBind();
            }
            else
            {
                operand.DataSource = new DataTable();
                operand.DataBind();

            }
        }

        LoadMetric(value, "-1", "", item);

    }

    private DataTable GetAttributes()
    {
        DataTable tb = new DataTable();

        FilterParameterCollection prams = new FilterParameterCollection();
        prams.AddLoggedInUserParamsWithRecId();
        tb = WebServices.RiskServices.GetReports("spa_RM_MRS_Get_AttributeList", prams);

        //DataRow row = tb.NewRow();
        //row[PAGE_GRID_OBJECT.ATTRIBUTE_ID] = "-1";
        //row[PAGE_GRID_OBJECT.ATTRIBUTE_NAME] = GetLocalResourceObject("Attribute.SelectText");
        //row["FieldType"] = AttributeRiskScoreMode.None;
        //tb.Rows.InsertAt(row, 0);

        return tb;
    }

    public bool CheckDuplicateAttribute()
    {
        bool isValid = true;
        GridDataItemCollection rows = uxAttributeDataGrid.Items;
        if (rows.Count == 0)
            return true;

        for (int i = 0; i < rows.Count; i++)
        {
            GridDataItem row = rows[i];
            RadComboBox cbAttr = row.FindControl(CT_ATTRIBUTE) as RadComboBox;

            if (cbAttr.SelectedValue != "-1")
            {
                for (var j = i + 1; j < rows.Count; j++)
                {
                    GridDataItem row2 = rows[j];
                    RadComboBox cbAttr2 = row2.FindControl(CT_ATTRIBUTE) as RadComboBox;
                    if (cbAttr2.SelectedValue != "-1")
                    {
                        if (cbAttr.SelectedValue == cbAttr2.SelectedValue)
                        {
                            Page.IsIntruderDetected = true;
                            Page.IntruderLog.LogData4 += "&Attribute=DuplicateId:" + cbAttr2.SelectedValue;
                            Page.RaiseIntruderEvent(IntruderType.Control);
                            isValid = false;
                            break;
                        }
                    }

                }
            }
        }

        return isValid;
    }

    public bool CheckAtleastOneAttribute()
    {
        bool isValid = false;
        GridDataItemCollection rows = uxAttributeDataGrid.Items;

        if (rows.Count > 0)
        {
            for (int i = 0; i < rows.Count; i++)
            {
                GridDataItem row = rows[i];
                RadComboBox cbAttr = row.FindControl(CT_ATTRIBUTE) as RadComboBox;

                if (cbAttr.SelectedValue != "-1")
                {
                    isValid = true;
                    break;
                }
            }
        }

        if (!isValid)
        {
            Page.IsIntruderDetected = true;
            Page.IntruderLog.LogData4 += "&Attribute=MustSelectAtleastOneAttribute";
            Page.RaiseIntruderEvent(IntruderType.Control);
        }

        return isValid;
    }

    public bool ValidateDataAttribute()
    {
        bool isValid = true;
        GridDataItemCollection rows = uxAttributeDataGrid.Items;
        for (int i = 0; i < rows.Count; i++)
        {
            GridDataItem row = rows[i];
            RadComboBox cbAttr = row.FindControl(CT_ATTRIBUTE) as RadComboBox;
            RadComboBox cbOperand = row.FindControl(CT_OPENRAND) as RadComboBox;
            RadComboBox cbMetric = row.FindControl(CT_METRIC) as RadComboBox;
            // Text box from
            TextBox txtFrom = row.FindControl(CT_FROM) as TextBox;
            TextBox txtTo = row.FindControl(CT_TO) as TextBox;
            if (txtFrom.Enabled)
            {
                if (txtFrom.Text == "")
                {
                    //show message           
                    Page.IsIntruderDetected = true;
                    Page.IntruderLog.LogData4 += "&From=Empty";
                    Page.RaiseIntruderEvent(IntruderType.Control);
                    isValid = false;
                    break;
                }
            }
            if (txtTo.Enabled)
            {
                if (txtTo.Text == "")
                {
                    Page.IsIntruderDetected = true;
                    Page.IntruderLog.LogData4 += "&To=Empty";
                    Page.RaiseIntruderEvent(IntruderType.Control);
                    isValid = false;
                    break;
                }
                else if (txtFrom.Enabled)
                {
                    if (Convert.ToDecimal(txtFrom.Text) > Convert.ToDecimal(txtTo.Text))
                    {
                        Page.IsIntruderDetected = true;
                        Page.IntruderLog.LogData4 += "&To<From";
                        Page.RaiseIntruderEvent(IntruderType.Control);
                        isValid = false;
                        break;
                    }

                }
            }
            else if (cbOperand.Enabled)
            {
                if (cbOperand.SelectedValue == "-1")
                {
                    Page.IsIntruderDetected = true;
                    Page.IntruderLog.LogData4 += "&Operand=Empty";
                    Page.RaiseIntruderEvent(IntruderType.Control);
                    isValid = false;
                    break;

                }
            }
        }

        return isValid;
    }

    public void Rebind()
    {
        uxAttributeDataGrid.Rebind();
    }

    #endregion

    internal static class PAGE_GRID_OBJECT
    {
        public const string RECORDID = "RecordID";
        public const string MERCHANT_CLASSIFICATION_ID = "MerchantClassificationID";
        public const string MERCHANT_CLASSIFICATION_NAME = "MerchantClassificationName";
        public const string ATTRIBUTE_ID = "AttributeID";
        public const string ATTRIBUTE_NAME = "AttributeName";
        public const string FROM_VALUE = "FromValue";
        public const string TO_VALUE = "ToValue";
        public const string OPERAND = "Operand";
        public const string METRICVALUE = "MetricValue";

    }
}

