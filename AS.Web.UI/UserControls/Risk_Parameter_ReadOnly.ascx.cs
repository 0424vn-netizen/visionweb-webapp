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
using AS.Common;
using AS.Controls.UserControls;
using Telerik.Web.UI;
using System.Drawing;

public partial class UserControls_Risk_Parameter_ReadOnly : GlobalUserControl
{
    private const string PARAM_NAME = "ParameterName";
    private const string PARAM_COLOR = "ParameterColor";
    private const string PARAM_THRESHOLD = "ParameterThreshold";
    private const string PARAM_VALUE = "ParameterValue";
    private const string PARAM_DATATYPE = "ParameterDataType";
    private const string NBSP = "&nbsp;";
    private const string PARAM_DESC = "ParameterDescription";

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

    private bool _IsColorMode = false;
    /// <summary>
    /// Gets or sets a value indicating whether this instance is color mode.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is color mode; otherwise, <c>false</c>.
    /// </value>
    public bool IsColorMode
    {
        get { return this._IsColorMode; }
        set { this._IsColorMode = value; }
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
            this.uxParameterList.MasterTableView.NoMasterRecordsText = VeraCodeSolution.ValidateResponseData(value);
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
        this.uxParameterList.Rebind();
    }
    /// <summary>
    /// Rebind mastertableview of grid
    /// </summary>
    public void RebindMasterTableView()
    {
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

    #endregion

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (this._IsColorMode)
        {
            this.uxParameterList.Columns.FindByUniqueName(PARAM_COLOR).Visible = true;
            this.uxParameterList.Columns.FindByUniqueName(PARAM_THRESHOLD).Visible = false;
            this.uxParameterList.Columns.FindByUniqueName(PARAM_VALUE).Visible = false;
        }
        else
        {
            this.uxParameterList.Columns.FindByUniqueName(PARAM_COLOR).Visible = false;
            this.uxParameterList.Columns.FindByUniqueName(PARAM_THRESHOLD).Visible = true;
            this.uxParameterList.Columns.FindByUniqueName(PARAM_VALUE).Visible = true;
        }
        //46652 - AW - Multi-Currency Transaction Display
        uxParameterList.Columns.FindByUniqueName(PARAM_VALUE).HeaderText = uxParameterList.Columns.FindByUniqueName(PARAM_VALUE).HeaderText.ToCurrencySymbol();
        uxParameterList.Columns.FindByUniqueName(PARAM_VALUE).HeaderTooltip = uxParameterList.Columns.FindByUniqueName(PARAM_VALUE).HeaderTooltip.ToCurrencySymbol();

    }

    protected void uxParameterList_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        this.OnNeedDataSource(e);

        //disable sorting if the grid has no records.
        this.uxParameterList.AllowSorting = true;
        if (this.DataSource is DataTable)
        {
            var table = this.DataSource as DataTable;
            if (table.Rows.Count == 0)
                this.uxParameterList.AllowSorting = false;
        }
    }

    protected void uxParameterList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            var headerItem = e.Item as GridHeaderItem;
            headerItem[PARAM_NAME].Style.Add(HtmlTextWriterStyle.TextAlign, "left !important");
            headerItem[PARAM_NAME].Style.Add(HtmlTextWriterStyle.PaddingBottom, "4px !important");
        }
        else if (e.Item is GridDataItem)
        {
            var dataItem = e.Item as GridDataItem;
            this.ProcessDataItem(dataItem);
        }
        else if (e.Item is GridGroupHeaderItem)
        {
            var gHeaderItem = e.Item as GridGroupHeaderItem;
            ProcessGroupHeaderItemStyle(gHeaderItem);
        }

        //fire event
        this.OnItemDataBound(e);
    }

    private void ProcessDataItem(GridDataItem dataItem)
    {

        if (!this._IsColorMode)
        {
            DataRowView rowView = dataItem.DataItem as DataRowView;
            if (rowView[PARAM_DATATYPE].GetType() == typeof(DBNull))
            {
                dataItem[PARAM_VALUE].Text = "<b>" + GetLocalResourceObject("Risk_Parameter_ReadOnly_ascx_cs_NA").ToString() + "</b>";
                dataItem[PARAM_VALUE].HorizontalAlign = HorizontalAlign.Center;
            }
            else
            {
                string indicatorValue = dataItem[PARAM_VALUE].Text;
                if (indicatorValue.Trim() == NBSP || indicatorValue.Trim() == string.Empty)
                {
                    dataItem[PARAM_VALUE].Text = "<b>" + GetLocalResourceObject("Risk_Parameter_ReadOnly_ascx_cs_NA").ToString() + "</b>";
                    dataItem[PARAM_VALUE].HorizontalAlign = HorizontalAlign.Center;
                }
                else
                {
                    string indicatorType = dataItem[PARAM_DATATYPE].Text;
                    if (indicatorType == SessionManager.CurrencySymbol)
                    {

                        dataItem[PARAM_VALUE].Text = VeraCodeSolution.DoVeraCode(
                                    indicatorType + NBSP + this.GetDouble4Precision(indicatorValue));
                    }
                    else
                        dataItem[PARAM_VALUE].Text = VeraCodeSolution.DoVeraCode(
                                        this.GetDouble4Precision(indicatorValue) + NBSP + indicatorType);
                }
            }

            string thresholdValue = dataItem[PARAM_THRESHOLD].Text;
            if (thresholdValue.Trim() == NBSP || thresholdValue.Trim() == string.Empty)
            {
                dataItem[PARAM_THRESHOLD].HorizontalAlign = HorizontalAlign.Center;
                dataItem[PARAM_THRESHOLD].Text = "<b>" + GetLocalResourceObject("Risk_Parameter_ReadOnly_ascx_cs_NA").ToString() + "</b>";
            }
            else
                dataItem[PARAM_THRESHOLD].Text = VeraCodeSolution.DoVeraCode(
                            SessionManager.CurrencySymbol + NBSP + this.GetDouble4Precision(thresholdValue));
        }
        else
        {
            string colorCode = dataItem[PARAM_COLOR].Text;
            if (colorCode.StartsWith("#"))
                dataItem[PARAM_COLOR].BackColor = Color.FromName(colorCode);
            dataItem[PARAM_COLOR].Text = string.Empty;
        }

        dataItem[PARAM_NAME].ToolTip = VeraCodeSolution.DoVeraCode(dataItem[PARAM_DESC].Text);
    }

    private void ProcessGroupHeaderItemStyle(GridGroupHeaderItem gHeaderItem)
    {
        gHeaderItem.Cells[0].Visible = false;
        gHeaderItem.Cells[1].Attributes.Add("colspan", "7");

        gHeaderItem.Cells[1].Style.Add(HtmlTextWriterStyle.TextAlign, "center");
        var groupText = gHeaderItem.Cells[1].Text;
        groupText = groupText.Substring(groupText.IndexOf(":") + 1).Trim();
        switch (groupText)
        {
            case "A":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_ReadOnly_ascx_cs_VolumeBatchTicket").ToString();
                break;
            case "B":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_ReadOnly_ascx_cs_Duplicates").ToString();
                break;
            case "C":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_ReadOnly_ascx_cs_Credits").ToString();
                break;
            case "D":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_ReadOnly_ascx_cs_Contract").ToString();
                break;
            case "E":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_ReadOnly_ascx_cs_CbRt").ToString();
                break;
            case "F":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_ReadOnly_ascx_cs_Attrition").ToString();
                break;
            case "G":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_ReadOnly_ascx_cs_ACHRejects").ToString();
                break;
            case "H":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_ReadOnly_ascx_cs_Auth").ToString();
                break;
            case "I":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_ReadOnly_ascx_cs_Deposits").ToString();
                break;
            case "J":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_ReadOnly_ascx_cs_BIN").ToString();
                break;
            case "K":
                gHeaderItem.Cells[1].Text = GetLocalResourceObject("Risk_Parameter_ReadOnly_ascx_cs_MerchantFilter").ToString();
                break;
            default:
                break;
        }
    }

    protected void ProcessGridGroupSplitterColumn(object sender, System.EventArgs e)
    {
        foreach (GridColumn column in uxParameterList.MasterTableView.RenderColumns)
        {
            if (column is GridGroupSplitterColumn)
            {
                column.Display = false;
            }
        }
        uxParameterList.Rebind();
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

    private void ProcessIndicatorColumn(TextBox txtParameterValue, Literal lblType, Label lblIndicatorDollar, HtmlGenericControl errControl, GridItemEventArgs e)
    {
        DataRowView dataRow = e.Item.DataItem as DataRowView;
        var dataItem = e.Item as GridDataItem;
    }

    private void ProcessThresholdColumn(TextBox txtThreshold, Label lblDollar, bool matchParameter, GridItemEventArgs e)
    {
        var dataItem = e.Item as GridDataItem;
        var rowView = e.Item.DataItem as DataRowView;
    }

    protected virtual void OnNeedDataSource(GridNeedDataSourceEventArgs e)
    {
        if (this.NeedDataSource != null)
            this.NeedDataSource(this, e);
    }

    protected virtual void OnItemDataBound(GridItemEventArgs e)
    {
        if (this.ItemDataBound != null)
            this.ItemDataBound(this, e);
    }

}
