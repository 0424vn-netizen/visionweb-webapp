using AS.Common;
using AS.Controls.Grid;
using AS.Controls.Telerik;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace AS.Web.UI.AppCode.General
{

    /// <summary>
    /// Summary description for ASRadControlHelper
    /// </summary>
    public static class ASRadControlHelper
    {
        public static void BindGridWithPaging(ASGrid grid, DataTable dt)
        {
            if (dt.Rows.Count > 0 && dt.Columns.Contains("TotalRows"))
                grid.VirtualItemCount = Convert.ToInt32(dt.Rows[0]["TotalRows"].ToString());

            grid.MasterTableView.AllowCustomPaging = true;
            grid.DataSource = dt;
        }

        public static string GetFilterValue(Pair filterPair, GridFilteringItem filterItem)
        {
            TextBox filterBox = filterItem[filterPair.Second.ToString()].Controls[0] as TextBox;
            if (filterBox != null)
                return filterBox.Text.Trim();

            ASRadComboBox filterCbBox = filterItem[filterPair.Second.ToString()].Controls[0] as ASRadComboBox;
            if (filterCbBox != null)
                return filterCbBox.SelectedValue.Trim();
            return string.Empty;
        }

        public static void ShowHidePagingControl(ASGrid grid)
        {
            if (grid.AS_DataSource == null || grid.AS_DataSource.Rows == null || grid.AS_DataSource.Rows.Count == 0)
            {
                grid.AllowSorting = false;
                grid.AllowPaging = false;
            }
            else
            {
                grid.AllowSorting = true;
                grid.AllowPaging = true;
            }
        }

        public static string[] GetSelectedValuesMultiChooser(ListItemCollection selectedItems)
        {
            List<string> result = new List<string>();
            foreach (ListItem item in selectedItems)
            {
                result.Add(item.Value);
            }
            return result.ToArray();
        }
       
        public static string HandleHtmlEncodeDecode(string title, bool isExporting)
        {
            string result = string.Empty;
            if (isExporting)
            {
                result = VeraCodeSolution.GetOutputHtmlString(title);
            }
            else
            {
                result = VeraCodeSolution.ValidateResponseData(title);
            }

            return result;
            
        }
        
    }
}