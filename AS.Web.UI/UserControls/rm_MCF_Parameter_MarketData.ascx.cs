using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AS.Common.DBManager;
using System.Collections.Generic;
using Telerik.Web.UI;
using AS.Controls.UserControls;
using AS.Controls.Exporter;
using System.Drawing;
using AS.Common;
using System.Text;

public partial class UserControls_rm_MCF_Parameter_MarketData : GlobalUserControl
{
    #region constants

    private const string SP_GETDEFAULTRISKSCORE = "spa_RM_MCF_GetDefaultRiskScore";

    private const string PARAM_CHECKBOX = "ParameterCheckBox";
    private const string PARAM_NAME = "ParameterName";
    private const string IND_MAXVALUE = "IndicatorMax";
    private const string PARAM_VALUE = "ParameterValue";
    private const string PARAM_THRESHOLD = "ParameterThreshold";
    private const string THRESHOLD_MAXVAL = "ThresholdMaxValue";
    private const string THRESHOLD_MINVAL = "ThresholdMinValue";
    private const string MAX_VAL = "MaxValue";
    private const string MIN_VAL = "MinValue";
    private const string ERR_CONTROLID = "ErrControlID";
    private const string PRECISION = "Precision";
    //private const string THRESHOLD_PREC = "ThresholdPrecision";
    private const string NBSP = "&nbsp;";
    private string NA = string.Empty;
    private const string PARAM_KEY = "ParameterKey";
    private const string PARAM_PRECISION = "ParameterPrecision";
    private const string IND_MINVALUE = "IndicatorMin";
    private const string ACTIVITY_STATUS = "ActivityStatus";
    private const string IS_SELECTED = "IsSelected";
    private const string GROUP_NAME = "GroupNameDummy";
    private const string PARAM_THRESHOLDHIDE = "ParameterThresholdHide";
    private const string USEDBY_ASS = "UsedByAssignments";
    private const string USEDBY_RISKSCORE = "UsedByRiskScores";
    private const string PARAM_CODE = "ParameterCode";

    #endregion

    #region Properties

    private int _AssignmentID = 0;
    /// <summary>
    /// Gets the assignment ID.
    /// </summary>
    /// <value>The assignment ID.</value>
    public int AssignmentID
    {
        get
        {
            if (Request.Params["assignmentid"] != null)
                _AssignmentID = Convert.ToInt32(Request.Params["assignmentid"]);
            return _AssignmentID;

        }

    }
    /// <summary>
    /// Gets or sets a value indicating whether [temp disable duplicates].
    /// </summary>
    /// <value>
    /// 	<c>true</c> if [temp disable duplicates]; otherwise, <c>false</c>.
    /// </value>
    public bool TempDisableDuplicates
    {
        get
        {
            return this.ViewState["TempDisableDuplicates"] != null ?
                (bool)this.ViewState["TempDisableDuplicates"] : false;
        }
        set
        {
            this.ViewState["TempDisableDuplicates"] = value;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is from assignment.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is from assignment; otherwise, <c>false</c>.
    /// </value>
    public bool IsFromAssignment
    {
        get
        {
            return this.ViewState["UxParam_IsFromAssignment"] != null ?
                (bool)this.ViewState["UxParam_IsFromAssignment"] : false;
        }
        set
        {
            this.ViewState["UxParam_IsFromAssignment"] = value;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether [show row number column].
    /// </summary>
    /// <value>
    /// 	<c>true</c> if [show row number column]; otherwise, <c>false</c>.
    /// </value>
    public bool ShowRowNumberColumn
    {
        get { return this.uxParameterList.Columns.FindByUniqueName("ParameterRowNumber").Visible; }
        set { this.uxParameterList.Columns.FindByUniqueName("ParameterRowNumber").Visible = value; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether [show export].
    /// </summary>
    /// <value><c>true</c> if [show export]; otherwise, <c>false</c>.</value>
    public bool ShowExport
    {
        get { return this.uxExportTop.Visible; }
        set
        {
            this.uxExportTop.Visible = value;
            this.uxExportBottom.Visible = value;
        }
    }

    /// <summary>
    /// Get a changes parameter list
    /// Note: this function get a changed parameter list which is edited (select checkbox) by user on the UI
    /// </summary>
    public List<RiskParameter> ParameterChangeList
    {
        get
        {
            return this.Session[this.ClientID + "ParameterChangeList"] != null ?
                (List<RiskParameter>)this.Session[this.ClientID + "ParameterChangeList"] : null;
        }
        private set
        {
            this.Session[this.ClientID + "ParameterChangeList"] = value;
        }
    }

    /// <summary>
    /// Get all RiskParameter list.
    /// </summary>
    public List<RiskParameter> ParameterList
    {
        get
        {
            return this.Session[this.ClientID + "ParameterList"] != null ?
                this.Session[this.ClientID + "ParameterList"] as List<RiskParameter> : null;
        }
        private set
        {
            this.Session[this.ClientID + "ParameterList"] = value;
        }
    }

    /// <summary>
    /// Gets the selected parameters (checked parameters).
    /// </summary>
    /// <value>The selected parameters.</value>
    public List<RiskParameter> SelectedParameters
    {
        get
        {
            if (this.ParameterList != null)
            {
                var selParams = this.ParameterList.Where(p => p.ActivityStatus == 1);
                return selParams.ToList();
            }

            return null;
        }
    }

    /// <summary>
    /// Gets or sets the no record text.
    /// </summary>
    /// <value>The no record text.</value>
    public string NoRecordText
    {
        get
        {
            return this.uxParameterList.MasterTableView.NoMasterRecordsText;
        }
        set
        {
            this.uxParameterList.MasterTableView.NoMasterRecordsText = value;
        }
    }

    /// <summary>
    /// Data source of grid
    /// </summary>
    public object DataSource
    {
        get { return this.uxParameterList.DataSource; }
        set { this.uxParameterList.DataSource = value; }
    }
    /// <summary>
    /// Rebind all grid
    /// </summary>
    public void Rebind()
    {
        this.ParameterInfo.Clear();
        this.uxParameterList.Rebind();
    }
    /// <summary>
    /// Rebind mastertableview of grid
    /// </summary>
    public void RebindMasterTableView()
    {
        this.ParameterInfo.Clear();
        this.uxParameterList.MasterTableView.Rebind();
    }

    /// <summary>
    /// NeedDataSource event for UxParameter
    /// </summary>
    public event GridNeedDataSourceEventHandler NeedDataSource;
    /// <summary>
    /// ItemDataBound event for UxParameter
    /// </summary>
    public event GridItemEventHandler ItemDataBound;
    /// <summary>
    /// NeedExportConfig event for UxParameter
    /// </summary>
    public event NeedExportConfigHandler NeedExportConfig;
    /// <summary>
    /// Occurs when [process save].
    /// </summary>
    public event EventHandler ProcessSave;

    private List<string> _ParameterInfo = new List<string>();
    /// <summary>
    /// Contain the RiskParameter informations for generate to client browser
    /// </summary>
    private List<string> ParameterInfo
    {
        get
        {

            if (Session[this.ClientID + "_ParameterInfo"] == null)
                Session[this.ClientID + "_ParameterInfo"] = new List<string>();

            return (List<string>)Session[this.ClientID + "_ParameterInfo"];
        }
        set
        {
            Session[this.ClientID + "_ParameterInfo"] = value;
        }
    }

    private string _MarketData = string.Empty;

    public string MarketData
    {
        get { return _MarketData; }
        set { _MarketData = value; }
    }


    /// <summary>
    /// Indicate that this control is in sorting or not.
    /// </summary>
    private bool _IsSort = false;

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        NA = GetLocalResourceObject("Risk_Parameter_MarketData_ascx_cs_NA").ToString();
    }
    protected string setParamterType(object sender)
    {
        if (sender != null)
        {
            string result = sender.ToString().ToLower();
            if (result.Equals("days"))
                return GetLocalResourceObject("Risk_Parameter_MarketData_ascx_cs_Days").ToString();
            return result;
        }
        return string.Empty;
    }

    private FilterParameterCollection GetDefaultRiskScoreParameters()
    {
        var filterParameters = new FilterParameterCollection();
        return filterParameters;
    }

    private FilterParameterCollection GetAssListUseRiskScoreParameters()
    {
        var filterParameters = new FilterParameterCollection();
        filterParameters
            .Add("ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32)
            .Add("UserID", SessionManager.CurrentUser.UserID, DbType.AnsiString);

        return filterParameters;
    }

    protected void btSave_Click(object sender, EventArgs e)
    {
        OnProcessSave(e);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        //visible columns
        this.uxParameterList.Columns.FindByUniqueName(PARAM_CODE).Visible = false;

        if (this.IsPostBack)
        {
            var list = this.GetAllParameterFromUI();
            //update all RiskParameter list
            this.ParameterList = list;
        }

    }

    public List<RiskParameter> GetAllParameterFromUI()
    {
        var dataItems = this.uxParameterList.MasterTableView.Items;
        var list = new List<RiskParameter>();
        foreach (GridDataItem dataItem in dataItems)
        {
            RiskParameter p = new RiskParameter();
            string key = dataItem[PARAM_KEY].Text.Trim();
            RiskParameter referer = this.ParameterList.GetRiskParameterByKey(key);
            p.GetDataFromGridDataItem(dataItem, referer);
            list.Add(p);
        }
        return list;
    }

    //mode: -1=all, 0=not selected 1=selected
    public string GetParameterKeysFromUI(int mode)
    {
        var dataItems = this.uxParameterList.MasterTableView.Items;
        StringBuilder sb = new StringBuilder();

        foreach (GridDataItem dataItem in dataItems)
        {
            HtmlInputCheckBox chkBox = dataItem[PARAM_CHECKBOX].FindControl("chkRowIndex") as HtmlInputCheckBox;
            if (mode == -1 || (mode == 0 && chkBox != null && !chkBox.Checked) || (mode == 1 && chkBox != null && chkBox.Checked))
            {
                sb.AppendFormat("{0},", dataItem[PARAM_KEY].Text.Trim());
            }
        }

        return sb.ToString().TrimEnd(',');
    }

    private List<RiskParameter> GetAllParameterFromTable(DataTable table)
    {
        List<RiskParameter> paramList = new List<RiskParameter>();
        for (int i = 0; i < table.Rows.Count; i++)
        {
            RiskParameter p = new RiskParameter();
            p.GetDataFromTableRowNRT(table.Rows[i]);
            paramList.Add(p);
        }
        return paramList;
    }

    private List<RiskParameter> GetAllParameterFromTable2(DataTable table)
    {
        List<RiskParameter> paramList = new List<RiskParameter>();
        for (int i = 0; i < table.Rows.Count; i++)
        {
            RiskParameter p = new RiskParameter();
            p.GetDataFromTableRow2NRT(table.Rows[i]);
            paramList.Add(p);
        }
        return paramList;
    }



    protected void uxParameterList_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        this.OnNeedDataSource(e);
        if (!e.IsFromDetailTable)
        {
            if (e.RebindReason == GridRebindReason.PostBackEvent)
                this._IsSort = true;

            DataTable table = null;

            if (this.DataSource != null)
            {
                this.ParameterInfo.Clear();

                table = (this.DataSource is DataTable ? this.DataSource as DataTable : (this.DataSource as DataView).Table);

                //if (this.ShowRiskScore && !this.IsFromAssignment)
                //{
                //    for (int i = 0; i < table.Rows.Count; i++)
                //        table.Rows[i][ACTIVITY_STATUS] = 0;
                //}

                //get all parameters for the first time and save into session
                if (!this._IsSort)
                    this.ParameterList = GetAllParameterFromTable(table);

                //disable sorting if the grid has no records.
                this.uxParameterList.AllowSorting = true;
                if (table.Rows.Count == 0)
                    this.uxParameterList.AllowSorting = false;
                

            }

            if (table == null || table.Rows.Count == 0)
                this.uxParameterList.Columns.FindByUniqueName(PARAM_CHECKBOX).HeaderText = string.Empty;
        }
    }
    protected void uxParameterList_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
    {
        GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
        DataRowView dataRow = e.DetailTableView.ParentItem.DataItem as DataRowView;
        switch (e.DetailTableView.Name)
        {
            case "ParameterDetail":
                {
                    string userId = dataItem.GetDataKeyValue("ParameterKey").ToString();
                    string[] columns = { "ParameterKey:int" };
                    e.DetailTableView.DataSource = CreateDummyData(columns, 1);
                    e.DetailTableView.ParentItem.ChildItem.Expanded = true;
                    break;
                }
        } 
    }
    protected void uxClose_Click(object sender, EventArgs e)
    {
        int index = int.Parse(uxCurrentParam.Value);
        GridDataItem item = uxParameterList.MasterTableView.Items[index].ChildItem.NestedTableViews[0].Items[0];
        Literal lblParamfilter = item.FindControl("lblParamfilter") as Literal;
       
        AS.Controls.Global.Container container = item.FindControl("asContainer") as AS.Controls.Global.Container;
        container.HeaderText = container.HeaderText.Replace("display:none;", "");

        string paramkey = uxPramKey.Value;
        lblParamfilter.Text = UserControls_rm_MCF_ParameterFilter_TransactionCode.GetSelectedValuesString_MarketData(MarketData, paramkey)["Label"];

        ((NonReportPage)this.Page).AjaxAddResponseScript("setCssClass();");

    }
    List<int> _arr = new List<int>();
    protected void uxParameterList_PreRender(object sender, EventArgs e)
    {
        HideExpandColumnRecursive(uxParameterList.MasterTableView);
    }


    public void HideExpandColumnRecursive(GridTableView tableView)
    {
        
        GridItem[] nestedViewItems = tableView.GetItems(GridItemType.NestedView);

        int c = 0;
        foreach (GridNestedViewItem nestedViewItem in nestedViewItems)
        {
            if (_arr.Contains(c))
            {
                nestedViewItem.NestedTableViews[0].ParentItem.Expanded = true;
            }
            c++;
        }
        
    } 

    #region Dummy Data
    public DataTable CreateDummyData(string[] columns, int rowCount)
    {
        DataTable tb = new DataTable("tb");
        DataColumn col = null;
        DataRow row = null;
        foreach (string colName in columns)
        {
            if (colName.IndexOf(":") > 0)
            {
                string[] colNameDetail = colName.Split(':');
                switch (colNameDetail[1].ToLower())
                {
                    case "int":
                        col = new DataColumn(colNameDetail[0], typeof(int));
                        tb.Columns.Add(col);
                        break;
                    case "datetime":
                        col = new DataColumn(colNameDetail[0], typeof(DateTime));
                        tb.Columns.Add(col);
                        break;
                    case "string":
                        col = new DataColumn(colNameDetail[0], typeof(string));
                        tb.Columns.Add(col);
                        break;
                    case "decimal":
                        col = new DataColumn(colNameDetail[0], typeof(decimal));
                        tb.Columns.Add(col);
                        break;
                    case "bool":
                        col = new DataColumn(colNameDetail[0], typeof(bool));
                        tb.Columns.Add(col);
                        break;
                }
            }
            else
            {
                col = new DataColumn(colName);
                tb.Columns.Add(col);
            }
        }

        for (int i = 1; i <= rowCount; i++)
        {
            row = tb.NewRow();
            foreach (string colName in columns)
            {

                if (colName.IndexOf(":") > 0)
                {
                    string[] colNameDetail = colName.Split(':');
                    switch (colNameDetail[1].ToLower())
                    {
                        case "int":
                            row[colNameDetail[0]] = i;
                            break;
                        case "datetime":
                            row[colNameDetail[0]] = DateTime.Now;
                            break;
                        case "string":
                            row[colNameDetail[0]] = colNameDetail[0] + " " + i;
                            break;
                        case "decimal":
                            row[colNameDetail[0]] = 123.456f;
                            break;
                        case "bool":
                            row[colNameDetail[0]] = i % 2 == 0;
                            break;
                    }
                }
                else
                {
                    row[colName] = colName + " " + i;
                }
            }
            tb.Rows.Add(row);
        }
        return tb;
    }

    #endregion
    protected void uxParameterList_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem && e.Item.OwnerTableView.Name == "ParentGrid")
        {
            var headerItem = e.Item as GridHeaderItem;
            ProcessHeaderItemStyle(headerItem);

        }
        else if (e.Item is GridDataItem && e.Item.OwnerTableView.Name == "ParentGrid")
        {
            var dataItem = e.Item as GridDataItem;
            ProcessDataItemStyle(dataItem);
            dataItem[PARAM_NAME].ToolTip = VeraCodeSolution.GetOutputHtmlString(dataItem["ParameterDescription"].Text);

            //================== process Indicator column==========================
            TextBox txtParameterValue = e.Item.FindControl("txtParameterValue") as TextBox;
            Literal lblType = e.Item.FindControl("lblParameterType") as Literal;
            RegularExpressionValidator RequiredFieldValidator1 = e.Item.FindControl("RequiredFieldValidator1") as RegularExpressionValidator;
            Label lblIndicatorDollar = e.Item.FindControl("lblIndicatorDollar") as Label;
            HtmlGenericControl spanErrMessage = e.Item.FindControl("spanErrMsg") as HtmlGenericControl;

            //=================== process Threshold column =======================
            TextBox txtThreshold = e.Item.FindControl("txtThreshold") as TextBox;
            DataRowView rowView = e.Item.DataItem as DataRowView;
            Label lblDollar = e.Item.FindControl("lblDollar") as Label;
            Label lblpercent = e.Item.FindControl("lblpercent") as Label;
            HtmlGenericControl spanErrMessageThs = e.Item.FindControl("spanErrMsgThs") as HtmlGenericControl;

            TextBox txtThresholdLow = e.Item.FindControl("txtThresholdLow") as TextBox;
            TextBox txtThresholdHigh = e.Item.FindControl("txtThresholdHigh") as TextBox;

            Label lblDollarThresholdLow = e.Item.FindControl("lblDollarThresholdLow") as Label;
            Label lblPercentThresholdLow = e.Item.FindControl("lblPercentThresholdLow") as Label;

            Label lblDollarThresholdHigh = e.Item.FindControl("lblDollarThresholdHigh") as Label;
            Label lblPercentThresholdHigh = e.Item.FindControl("lblPercentThresholdHigh") as Label;

            HtmlGenericControl spanErrMsgThresholdLow = e.Item.FindControl("spanErrMsgThresholdLow") as HtmlGenericControl;
            HtmlGenericControl spanErrMsgThresholdHigh = e.Item.FindControl("spanErrMsgThresholdHigh") as HtmlGenericControl;

            DataRowView row = e.Item.DataItem as DataRowView;
            var ParameterThresholdType = row["ParameterThresholdType"].ToString();
            var ParameterThreshold = row["ParameterThreshold"].ToString();
            var ParameterThresholdLow = row["ParameterThreshold"].ToString();
            var ParameterThresholdHigh = row["ParameterThresholdHigh"].ToString();
            TextBox txtFrom = e.Item.FindControl("txtFrom") as TextBox;
            TextBox txtTo = e.Item.FindControl("txtTo") as TextBox;
            if (row["ParameterValue"] != DBNull.Value && !row["ParameterValue"].ToString().IsNullOrEmpty()
               && row["ParameterValueHigh"] != DBNull.Value && !row["ParameterValueHigh"].ToString().IsNullOrEmpty())
            {
                var ParameterValue = row["ParameterValue"].ToString();
                var ParameterValueHigh = row["ParameterValueHigh"].ToString();
                HtmlGenericControl div = e.Item.FindControl("div1") as HtmlGenericControl;
                HtmlGenericControl uxMerchantfilter = e.Item.FindControl("uxMerchantFilter") as HtmlGenericControl;
                div.Visible = false;
                uxMerchantfilter.Visible = true;
                
                txtFrom.Text = VeraCodeSolution.DoVeraCode(Convert.ToInt32(GetParameterNumber(ParameterValue)).ToString());
                txtTo.Text = VeraCodeSolution.DoVeraCode(Convert.ToInt32(GetParameterNumber(ParameterValueHigh)).ToString());
            }

            if (row["FilterTypeModal"] != DBNull.Value)
            {
                e.Item.OwnerTableView.DetailTables[0].Visible = true;
                _arr.Add(e.Item.ItemIndex);
            }
            else
            {
                e.Item.OwnerTableView.DetailTables[0].Visible = false;
            }
            bool isNA = false;
            if (ParameterThresholdType == "LowHigh")
            {
                if (ParameterThresholdLow.IsNullOrEmpty() && ParameterThresholdHigh.IsNullOrEmpty())
                {
                    isNA = true;
                }
            }
            else
            {
                if (ParameterThreshold.IsNullOrEmpty())
                {
                    isNA = true;
                }
            }


            if (isNA)
            {
                ((PlaceHolder)e.Item.FindControl("uxThresholdNormal")).Visible =
                         ((PlaceHolder)e.Item.FindControl("uxThresholdLowHigh")).Visible = !isNA;
                ((PlaceHolder)e.Item.FindControl("lblNA")).Visible = isNA;
            }
            else
            {
                switch (ParameterThresholdType)
                {
                    case "LowHigh":
                        ((PlaceHolder)e.Item.FindControl("uxThresholdNormal")).Visible = false;
                        ((PlaceHolder)e.Item.FindControl("uxThresholdLowHigh")).Visible = true;

                        ProcessThresholdColumn(txtThresholdLow, lblDollarThresholdLow, lblPercentThresholdLow, spanErrMsgThresholdLow, e);
                        ProcessThresholdColumn(txtThresholdHigh, lblDollarThresholdHigh, lblPercentThresholdHigh, spanErrMsgThresholdHigh, e);

                        break;
                    default:
                        ((PlaceHolder)e.Item.FindControl("uxThresholdNormal")).Visible = true;
                        ((PlaceHolder)e.Item.FindControl("uxThresholdLowHigh")).Visible = false;

                        ProcessThresholdColumn(txtThreshold, lblDollar, lblpercent, spanErrMessageThs, e);
                        break;
                }
            }

            ProcessIndicatorColumn(txtParameterValue, lblType, lblIndicatorDollar, spanErrMessage, e);

            //ProcessThresholdColumn(txtThreshold, lblDollar, lblpercent, spanErrMessageThs, e);

            ProcessCheckBoxColumn(txtParameterValue, txtThreshold, txtThresholdLow, txtThresholdHigh, txtFrom, txtTo, e);

            
        }
        else if (e.Item is GridGroupHeaderItem && e.Item.OwnerTableView.Name == "ParentGrid")
        {
            var gHeaderItem = e.Item as GridGroupHeaderItem;
            ProcessGroupHeaderItemStyle(gHeaderItem);

        }
        if (e.Item is GridDataItem && e.Item.OwnerTableView.Name == "ParameterDetail" && e.Item.OwnerTableView.Visible)
        {
            Literal lblParamfilter = e.Item.FindControl("lblParamfilter") as Literal;
            AS.Controls.Global.Container asContainer = (AS.Controls.Global.Container)e.Item.FindControl("asContainer");
            
            string ht = string.Empty;         
            
            //AS.Controls.ASContainer ascontainer = e.Item.OwnerTableView.DetailTables[0].Items[0].FindControl("asContainer")
            //HtmlInputCheckBox cb = e.Item.Parent.Parent.Parent.Parent.Parent.FindControl("chkRowIndex") as HtmlInputCheckBox;
            
            GridDataItem dataItem = (GridDataItem)e.Item;
            DataRowView row = e.Item.DataItem as DataRowView;
            GridDataItem parentItem = e.Item.OwnerTableView.ParentItem;
            HtmlInputCheckBox cbx = parentItem.FindControl("chkRowIndex") as HtmlInputCheckBox;
            if (cbx.Checked)
            {
                ht = "<table width=\"100%\"><tr><td style=\"padding-left: 15px; height:32px;\">{0}</td><td Visible=\"true\"><div class=\"EditButtonAssignment\" style=\"float:right;\" onclick=\"parameter_ShowFilter({1})\"></div></td></tr></table>";
            }
            else
            {
                ht = "<table width=\"100%\"><tr><td style=\"padding-left: 15px; height:32px;\">{0}</td><td Visible=\"true\"><div class=\"EditButtonAssignment\" style=\"display:none; float:right;\" onclick=\"parameter_ShowFilter({1})\"></div></td></tr></table>";
            }
            //string a = e.Item.OwnerTableView.CssClass;
            //dataItem.CssClass = parentItem.CssClass;
            string paramId = parentItem.GetDataKeyValue("ParameterKey").ToString();//.GetDataKeyValue("ParameterKey").ToString(); 
            lblParamfilter.Text = UserControls_rm_MCF_ParameterFilter_TransactionCode.GetSelectedValuesString_MarketData(MarketData, paramId)["Label"];
            ht = string.Format(ht, "Auth Transaction Code", "'rm_MCF_ParameterFilter_TransactionCodeModal.aspx?{0}','{1}','{2}', 850, 725");
            string queryString = Page.BuildSecureQueryString(string.Format("MarketData=" + MarketData + "&ParamID="+paramId));
            asContainer.HeaderText = VeraCodeSolution.DoVeraCode(string.Format(ht, queryString, e.Item.OwnerTableView.ParentItem.ItemIndex, paramId));                  

        }
        //fire event
        this.OnItemDataBound(e);
    }

    protected void uxExport_OnNeedExportConfig(object sender, ExportConfig exportConfig)
    {
        uxParameterList.Columns.FindByUniqueName("ParameterCheckBox").Visible = false;
        uxParameterList.Columns.FindByUniqueName("ParameterValue").Visible = false;
        uxParameterList.Columns.FindByUniqueName("ParameterThreshold").Visible = false;
        UxExport uxExport = sender as UxExport;
        exportConfig.FileName = GetLocalResourceObject("Risk_Parameter_MarketData_ascx_cs_ExportFilename").ToString();
        exportConfig.ReportHeader = GetLocalResourceObject("Risk_Parameter_MarketData_ascx_cs_ExportHeader").ToString();
        if (this.NeedExportConfig != null)
            this.NeedExportConfig(sender, exportConfig);
    }

    private void ProcessGroupHeaderItemStyle(GridGroupHeaderItem gHeaderItem)
    {
        gHeaderItem.Cells[0].Visible = false;
        gHeaderItem.Cells[1].Attributes.Add("colspan", "9");

        gHeaderItem.Cells[1].Style.Add(HtmlTextWriterStyle.TextAlign, "center");
        var groupText = gHeaderItem.Cells[1].Text;
        groupText = groupText.Substring(groupText.IndexOf(":") + 1).Trim();
        switch (groupText)
        {
            case "A":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_MarketData_ascx_cs_VolumeBatchTicket").ToString();
                break;
            case "B":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_MarketData_ascx_cs_Duplicates").ToString();
                break;
            case "C":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_MarketData_ascx_cs_Credits").ToString();
                break;
            case "D":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_MarketData_ascx_cs_Contract").ToString();
                break;
            case "E":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_MarketData_ascx_cs_CbRt").ToString();
                break;
            case "F":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_MarketData_ascx_cs_Attrition").ToString();
                break;
            case "G":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_MarketData_ascx_cs_ACHRejects").ToString();
                break;
            case "H":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_MarketData_ascx_cs_Auth").ToString();
                break;
            case "I":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_MarketData_ascx_cs_Deposits").ToString();
                break;
            case "J":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_MarketData_ascx_cs_BIN").ToString();
                break;
            case "K":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_MarketData_ascx_cs_MerchantFilter").ToString();
                break;
            default:
                break;
        }
    }

    private void ProcessHeaderItemStyle(GridHeaderItem headerItem)
    {
        if (this.Request.Browser.Type.Contains("IE"))
            headerItem[PARAM_CHECKBOX].Text = VeraCodeSolution.DoVeraCode(headerItem[PARAM_CHECKBOX].Text.Replace("-3px", "-6px"));
        headerItem[PARAM_NAME].Style.Add(HtmlTextWriterStyle.TextAlign, "left !important");
        headerItem[PARAM_NAME].Style.Add(HtmlTextWriterStyle.PaddingBottom, "4px !important");
        if (this.uxParameterList.DataSource == null ||
            (this.uxParameterList.DataSource != null && (this.uxParameterList.DataSource as DataTable).Rows.Count == 0))
        {
            headerItem[PARAM_CHECKBOX].Text = string.Empty;
            headerItem[PARAM_CHECKBOX].Style.Add(HtmlTextWriterStyle.PaddingLeft, "0px");
            this.uxParameterList.Columns.FindByUniqueName(PARAM_CHECKBOX).HeaderText = string.Empty;
        }
        else
            headerItem[PARAM_CHECKBOX].Style.Add("text-align", "left");

    }

    private void ProcessDataItemStyle(GridDataItem dataItem)
    {

        dataItem[PARAM_CHECKBOX].Style.Add("border-left", "0");
        dataItem[PARAM_CHECKBOX].Style.Add("margin-left", "0");
        dataItem[PARAM_CHECKBOX].Style.Add("padding-left", "0");

    }

    private void ProcessIndicatorColumn(TextBox txtParameterValue, Literal lblType, Label lblIndicatorDollar, HtmlGenericControl errControl, GridItemEventArgs e)
    {
        DataRowView dataRow = e.Item.DataItem as DataRowView;
        var dataItem = e.Item as GridDataItem;
        var type = lblType.Text.Replace(NBSP, string.Empty).Trim();
        string paramKey = dataItem[PARAM_KEY].Text;

        if (txtParameterValue.Text.Trim() != string.Empty)
        {
            //add to check indicator max, min, precision, error controlID value
            if (dataItem[IND_MAXVALUE].Text.Replace(NBSP, string.Empty).Trim() != string.Empty)
                txtParameterValue.Attributes[MAX_VAL] = dataRow[IND_MAXVALUE].ToString();
            txtParameterValue.Attributes[MIN_VAL] = dataRow[IND_MINVALUE].ToString();
            txtParameterValue.Attributes[PRECISION] = dataRow[PARAM_PRECISION].ToString();
            txtParameterValue.Attributes[ERR_CONTROLID] = errControl.ClientID;

            if (this._IsSort)
                txtParameterValue.Text = VeraCodeSolution.DoVeraCode(this.GetIndicatorFromParameterList(paramKey));

            if (type == "$")
            {
                lblType.Text = NBSP;
                lblIndicatorDollar.Visible = true;
                lblIndicatorDollar.Text = VeraCodeSolution.DoVeraCode(NBSP + type);
                txtParameterValue.Text = this.GetDouble4Precision(txtParameterValue.Text.Trim().TrimStart('-'));
            }
            else
            {
                lblType.Visible = true;
                //lblIndicatorDollar.Visible = false;
                lblIndicatorDollar.Text = VeraCodeSolution.DoVeraCode(NBSP + NBSP + NBSP);
                //lblIndicatorDollar.Attributes.CssStyle["visibility"] = "hidden";

                if (type == "#" || type == "%")
                    txtParameterValue.Text = VeraCodeSolution.DoVeraCode(this.GetDouble4Precision(txtParameterValue.Text).Trim().TrimStart('-'));
                else if (type == "days")
                    txtParameterValue.Text = VeraCodeSolution.DoVeraCode(this.GetIntegerNumber(txtParameterValue.Text).Trim().TrimStart('-'));
                else if (type.Length == 0)
                {
                    //lblIndicatorDollar.Visible = false;
                    txtParameterValue.Visible = true;
                    txtParameterValue.Text = VeraCodeSolution.DoVeraCode(NA);
                    txtParameterValue.Font.Bold = true;
                    dataItem[PARAM_VALUE].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            if (dataItem["IndicatorNegative"].Text.ToString() != "" && dataItem["IndicatorNegative"].Text.ToString() == "Yes")
            {
                HtmlGenericControl div = e.Item.FindControl("div1") as HtmlGenericControl;
                div.Attributes.CssStyle["color"] = "Red";
                lblIndicatorDollar.Visible = true;
                txtParameterValue.ForeColor = Color.Red;
                if (type == "$")
                {
                    lblIndicatorDollar.Text = VeraCodeSolution.DoVeraCode("$(");
                }
                else
                {
                    lblIndicatorDollar.Text = VeraCodeSolution.DoVeraCode(NBSP + NBSP + "(");
                }
                lblType.Text = ")" + lblType.Text;
            }
        }
        else
        {
            txtParameterValue.Visible = false;

            lblType.Visible = true;
            lblType.Text = "<b>" + GetLocalResourceObject("Risk_Parameter_MarketData_ascx_cs_NA").ToString() + "</b>";
            HtmlGenericControl div = e.Item.FindControl("div1") as HtmlGenericControl;
            div.Attributes.CssStyle["text-align"] = "center";
            dataItem[PARAM_VALUE].HorizontalAlign = HorizontalAlign.Center;
        }
    }

    private void ProcessThresholdColumn(TextBox txtThreshold, Label lblDollar, Label lblpercent, HtmlGenericControl errControl, GridItemEventArgs e)
    {
        var dataItem = e.Item as GridDataItem;
        var rowView = dataItem.DataItem as DataRowView;

        var type = lblpercent.Text.Replace(NBSP, string.Empty).Trim();
        string paramKey = dataItem[PARAM_KEY].Text;

        Type DBNull = Type.GetType("System.DBNull");
        bool hasNoThreshold = false;

        hasNoThreshold = rowView[PARAM_THRESHOLD].GetType() == DBNull || rowView["ThresholdMax"].GetType() == DBNull
                        || decimal.Parse(rowView["ThresholdMax"].ToString()) == 0;

        //mean that this field is 'empty' but allow user to enter value
        if (dataItem[PARAM_THRESHOLDHIDE].Text == "0dds0") // current sys dont have this case 
        {
            if (dataItem[THRESHOLD_MAXVAL].Text.Replace(NBSP, string.Empty).Trim() != string.Empty)
                txtThreshold.Attributes[MAX_VAL] = VeraCodeSolution.DoVeraCode(dataItem[THRESHOLD_MAXVAL].Text);

            if (this.ParameterChangeList != null && this.ParameterChangeList.Count > 0)
            {
                //in this case update
                RiskParameter param = this.ParameterChangeList.GetRiskParameterByName(dataItem["ParameterNameHide"].Text.Trim());
                if (param != null)
                {
                    if (param.ThresholdValue != null && !double.IsNaN(param.ThresholdValue.Value))
                    {
                        txtThreshold.Text = VeraCodeSolution.DoVeraCode(param.ThresholdValue.Value.ToString("#,#0.0000").Trim().TrimStart('-'));
                    }
                    else
                        txtThreshold.Text = string.Empty;
                }
                else
                    txtThreshold.Text = string.Empty;
            }
            else
            {
                txtThreshold.Text = string.Empty;
            }

        }
        else if (hasNoThreshold)
        {
            ((PlaceHolder)e.Item.FindControl("lblNA")).Visible = true;
            lblDollar.Visible = false;
            txtThreshold.Visible = false;
            lblpercent.Visible = false;
        }
        else
        {
            //add to check threshold min, max value
            if (dataItem[THRESHOLD_MAXVAL].Text.Replace(NBSP, string.Empty).Trim() != string.Empty)
                txtThreshold.Attributes[MAX_VAL] = VeraCodeSolution.DoVeraCode(dataItem[THRESHOLD_MAXVAL].Text);
            txtThreshold.Attributes[MIN_VAL] = VeraCodeSolution.DoVeraCode(dataItem[THRESHOLD_MINVAL].Text);
            txtThreshold.Attributes[ERR_CONTROLID] = VeraCodeSolution.DoVeraCode(errControl.ClientID);
            if (this._IsSort)
                txtThreshold.Text = VeraCodeSolution.DoVeraCode(GetThresholdFromParameterList(paramKey).Trim().TrimStart('-'));

            txtThreshold.Text = VeraCodeSolution.DoVeraCode(this.GetDouble4Precision(txtThreshold.Text).Trim().TrimStart('-'));
            //dataItem[PARAM_THRESHOLD].HorizontalAlign = HorizontalAlign.Center;

            if (type == "$")
            {
                lblDollar.Text = NBSP;
                lblDollar.Visible = true;
                lblDollar.Text = VeraCodeSolution.DoVeraCode(NBSP + type);
                lblpercent.Visible = false;
            }
            else
            {
                //lblDollar.Visible = false;
                lblpercent.Visible = true;
                //lblIndicatorDollar.Visible = false;
                lblDollar.Text = VeraCodeSolution.DoVeraCode(NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP);
                //lblIndicatorDollar.Attributes.CssStyle["visibility"] = "hidden";

                if (type == "#" || type == "%")
                    txtThreshold.Text = VeraCodeSolution.DoVeraCode(this.GetDouble4Precision(txtThreshold.Text).Trim().TrimStart('-'));
                else if (type == "days")
                    txtThreshold.Text = VeraCodeSolution.DoVeraCode(this.GetIntegerNumber(txtThreshold.Text).Trim().TrimStart('-'));
            }
        }
        
        if (dataItem["ThresholdNegative"].Text.ToString() != "" && dataItem["ThresholdNegative"].Text.ToString() == "Yes" && txtThreshold.Text != "")
        {
            HtmlGenericControl div = e.Item.FindControl("dvThreshold") as HtmlGenericControl;
            div.Attributes.CssStyle["color"] = "Red";
            txtThreshold.ForeColor = Color.Red;
            //lblDollar.Text = lblDollar.Text + "(";
            //lblpercent.Text = ")";
            lblpercent.Visible = true;
            if (type == "$")
            {
                lblDollar.Text = VeraCodeSolution.DoVeraCode(NBSP + NBSP + "$(");
                lblpercent.Text = ")";
            }
            else
            {
                lblDollar.Text = VeraCodeSolution.DoVeraCode(NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + NBSP + "(");
                lblpercent.Text = ")" + lblpercent.Text;
            }
        }
    }

    private void ProcessCheckBoxColumn(TextBox txtParameterValue, TextBox txtThreshold, TextBox txtThresholdLow, TextBox txtThresholdHigh, TextBox txtFrom, TextBox txtTo, GridItemEventArgs e)
    {
        DataRowView dataRow = e.Item.DataItem as DataRowView;
        var dataItem = e.Item as GridDataItem;
        bool isNew = false;
        string paramKey = dataItem[PARAM_KEY].Text;
        string paramValue = txtParameterValue.Text;

        string paramThreshold = dataItem[PARAM_THRESHOLDHIDE].Text;
        HtmlInputCheckBox checkBox = e.Item.FindControl("chkRowIndex") as HtmlInputCheckBox;
        string indicatorID = string.Empty;
        string thresholdID = string.Empty;
        string activityStatus = string.Empty;
        if (!this.IsFromAssignment)
            activityStatus = this._IsSort ? GetActivityStatusFromParameterList(paramKey)
                                             : dataItem[ACTIVITY_STATUS].Text.Trim();
        else
            activityStatus = this._IsSort ? GetActivityStatusFromParameterList(paramKey)
                                             : dataItem[IS_SELECTED].Text.Trim();

        indicatorID = !isNew && paramValue.Trim() == string.Empty ? "new parameter" : txtParameterValue.ClientID;

        thresholdID = txtThreshold.ClientID;

        checkBox.Attributes["onclick"] = string.Format("parameterCheckBox_Click(this, '{0}', '{1}', '{2}', {3});",
            paramKey, indicatorID, thresholdID, (txtThreshold.Text.Trim() == string.Empty).ToString().ToLower());

        if (activityStatus == "1")
        {
            checkBox.Checked = true;
        }
        else
        {
            checkBox.Checked = false;
        }

        txtParameterValue.Enabled = txtThreshold.Enabled = txtThresholdLow.Enabled = txtThresholdHigh.Enabled = txtFrom.Enabled = txtTo.Enabled=checkBox.Checked;

        if (this.TempDisableDuplicates == true && dataItem[GROUP_NAME].Text == "B") //"B" mean Duplicates group.
        {
            checkBox.Disabled = true;
            dataItem.ToolTip = GetLocalResourceObject("Risk_Parameter_MarketData_ascx_cs_DuplicatesSection").ToString();
        }

        string assList = string.Empty;
        assList = dataRow[USEDBY_ASS].ToString().Replace(",", ", ");

        string isRiskScore = dataRow["UsedByRiskScores"].ToString().ToLower();

        this.ParameterInfo.Add(
        string.Format("{{ CbClientId:\"{0}\", ParamKey:\"{1}\", IndClientId:\"{2}\", ThsClientId:\"{3}\", IsUseThs:{4}, AssList:\"{5}\", RiskScoreList:\"{6}\", ThsLowID:\"{7}\", ThsHighID:\"{8}\", IndFromID:\"{9}\", IndToID:\"{10}\" }}",
                checkBox.ClientID, paramKey, txtParameterValue.ClientID, txtThreshold.ClientID, (txtThreshold.Text.Trim() != string.Empty).ToString().ToLower(), assList, isRiskScore, txtThresholdLow.ClientID, txtThresholdHigh.ClientID, txtFrom.ClientID, txtTo.ClientID));

    }

    private string GetActivityStatusFromParameterList(string paramKey)
    {
        var param = this.ParameterList.Where(p => p.Key == paramKey);
        return param.First().ActivityStatus.ToString();
    }

    private string GetThresholdFromParameterList(string paramKey)
    {
        var param = this.ParameterList.Where(p => p.Key == paramKey).FirstOrDefault();
        return param == null || param.ThresholdValue == null ? string.Empty : param.ThresholdValue.Value.ToString("#,#0.0000");
    }

    private string GetIndicatorFromParameterList(string paramKey)
    {
        var param = this.ParameterList.Where(p => p.Key == paramKey).FirstOrDefault();

        return param == null || param.IndicatorValue == null ? string.Empty : param.IndicatorValue.Value.ToString("#,#0.0000");
    }

    protected virtual void OnNeedDataSource(Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (this.NeedDataSource != null)
            this.NeedDataSource(this, e);
    }

    protected virtual void OnItemDataBound(Telerik.Web.UI.GridItemEventArgs e)
    {
        if (this.ItemDataBound != null)
            this.ItemDataBound(this, e);
    }

    protected virtual void OnProcessSave(EventArgs e)
    {
        if (this.ProcessSave != null)
            this.ProcessSave(this, e);
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        //RiskParameter infor list
        string paramInfo = "ParameterInfo = [];";
        foreach (string info in this.ParameterInfo)
        {
            paramInfo += "ParameterInfo.push(" + info + ");";
        }

        this.Page.ClientScript.RegisterStartupScript(
            this.GetType(), "jsParameterInfo", paramInfo, true);

        var ram = RadAjaxManager.GetCurrent(this.Page);
        if (ram != null)
            ram.ResponseScripts.Add("populateCheckBoxAllCtrl();");

        //check the checkbox all
        bool isAllChecked = this.ParameterList.Count(p => p.ActivityStatus == 0) == 0;
        if (isAllChecked)
            this.Page.ClientScript.RegisterStartupScript(
                this.GetType(), "jsTickCheckBoxAll", "populateCheckBoxAllCtrl();", true);

        //using for market data
        //string usedMarketData = "usedMarketData = " + SessionManager.IsUsingMarketData.ToString();
        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "jsUsedMarketData", "var usedMarketData = " + RiskSessionManager.IsUsingMarketData.ToString().ToLower(), true);
    }

    private string GetIntegerNumber(string intStr)
    {
        if (intStr.Contains("."))
            return intStr.Substring(0, intStr.IndexOf('.'));

        return intStr;
    }
    private string GetDouble4Precision(string doubleStr)
    {
        if (doubleStr.Contains("."))
        {
            if (!doubleStr.EndsWith("0000"))
                return double.Parse(doubleStr).ToString("#,#0.0000");
        }

        return double.Parse(doubleStr).ToString("#,#0");
    }

    protected double? GetParameterNumber(string number)
    {
        if (number == "")
            return null;
        else if (number == "0dds0")
            return double.NaN;

        return double.Parse(number);
    }
}
