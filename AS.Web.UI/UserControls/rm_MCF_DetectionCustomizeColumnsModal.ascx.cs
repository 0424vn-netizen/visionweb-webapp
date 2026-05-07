using AS.Common;
using AS.Common.DBManager;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using AS.Controls.Pages;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

public partial class UserControls_rm_MCF_DetectionCustomizeColumnsModal : GlobalUserControl
{
    private IEnumerable<ColumnDisplayedConfigurationItem> ColumnConfiguration { get; set; }
    private String ViewData { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            uxLeftGrid.DataBind();
            uxRightGrid.DataBind();
        }
    }

    protected void uxSubmit_Click(object sender, EventArgs e)
    {
        var displayedItems = uxRightGrid.Items.Cast<RadListBoxItem>().Select(x => x.Value).ToList();
        List<ColumnDisplayedConfigurationItem> cols = new List<ColumnDisplayedConfigurationItem>();
        int order = 4;
        foreach (var c in displayedItems)
        {
            cols.Add(new ColumnDisplayedConfigurationItem
                {
                    ColumnName = c,
                    ColumnText = string.Empty,
                    IsDisplayed = false,
                    OrderIndex = order++
                });
        }
        PersonalDataHelper.SaveConfigAsJSON(UserConfigNames.CONFIG_RISK_DETECTIONQUEUE_CUSTOMIZE_COLUMNS, cols);
        ((BaseMasterPage)this.Page.Master).AjaxAddResponseScript(string.Format(@"window.CustomizeColumnModal.SubmitColumnConfigurationModal();"));
    }

    /// <summary>
    /// Get Unused Columns
    /// </summary>
    private void GetUnusedColumn()
    {
        var data = new List<ColumnDisplayedConfigurationItem>();
        var displayColumns = GenerateDetectionQueue();
        if (ColumnConfiguration != null)
        {
            foreach (var col in ColumnConfiguration)
            {
                if (!displayColumns.Contains(col.ColumnName))
                {
                    data.Add(col);
                }
            }
        }
        uxLeftGrid.DataSource = data.OrderBy(o => o.OrderIndex).ToList();
        uxLeftGrid.DataBind();
    }

    /// <summary>
    /// Get Displayed Columns
    /// </summary>
    private void GetDisplayedColumn()
    {
        var displayColumns = GenerateDetectionQueue();
        var data = new List<ColumnDisplayedConfigurationItem>();
        List<ColumnDisplayedConfigurationItem> items = new List<ColumnDisplayedConfigurationItem>();
        foreach (string item in displayColumns)
        {
            if (ColumnConfiguration != null)
            {
                var col = ColumnConfiguration.FirstOrDefault(a => a.ColumnName == item);
                if (col != null)
                {
                    data.Add(col);
                }
            }
        }
        uxRightGrid.DataSource = data.ToList();
        uxRightGrid.DataBind();
    }

    /// <summary>
    /// Get Data Columns
    /// </summary>
    public void GetData()
    {
        ColumnConfiguration = RiskSessionManager.RiskReportDetectionQueueColumns;
        GetUnusedColumn();
        GetDisplayedColumn();
    }

    public void RebindData()
    {
        uxRightGrid.DataBind();
    }
    private List<string> GenerateDetectionQueue()
    {
        var cols = PersonalDataHelper.GetJSONConfig<List<ColumnDisplayedConfigurationItem>>(UserConfigNames.CONFIG_RISK_DETECTIONQUEUE_CUSTOMIZE_COLUMNS);
        if (cols == null)
        {
            PersonalDataHelper.SaveConfigAsJSON(UserConfigNames.CONFIG_RISK_DETECTIONQUEUE_CUSTOMIZE_COLUMNS, RiskSessionManager.RiskReportDetectionQueueColumns);
        }
        return PersonalDataHelper.GetJSONConfig<List<ColumnDisplayedConfigurationItem>>(UserConfigNames.CONFIG_RISK_DETECTIONQUEUE_CUSTOMIZE_COLUMNS).Select(c => c.ColumnName).ToList();
    }
}