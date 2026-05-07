using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using AS.Controls.Pages;
using System.Configuration;
using System.Web.UI;

/// <summary>
/// Summary description for ExportKeyValueTable
/// </summary>
namespace AS.Controls
{
    public class ExportKeyValueTable : GlobalUserControl
    {
        private string EXPORT_EXCELL_PATH = ConfigurationManager.AppSettings["ExportTempFolder"].ToString();
        public string Title { get; set; }
        public string CssClass { get; set; }
        public string FileName { get; set; }
        public string DataSourceID { get; set; }
        private IExportKeyValueTable _data;
        public string TargetID { get; set; }
        public string ScrollID { get; set; }
        public bool IsChild { get; set; }
        public bool IsMultiExport { get; set; }
        private Control KeyValueControl
        {
            get
            {
                var control = FindControlRecursive(this.Parent, DataSourceID);
                if (!control.IsNullData())
                    return control;
                return new Control();
            }
        }

        private Control FindControlRecursive(Control rootControl, string controlID)
        {
            if (rootControl.ID == controlID) return rootControl;
            foreach (Control controlToSearch in rootControl.Controls)
            {
                Control controlToReturn = FindControlRecursive(controlToSearch, controlID);
                if (controlToReturn != null)
                    return controlToReturn;
            }
            return null;
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            base.Render(writer);
            string html;
            html = @"<div id='{0}' class='row'>
                            <div class='col-xs-12'>
                                <h2 class='grid-title {3}' data-toggle='collapse' data-target='{1}'>
                                    {2}
                                </h2>
                            </div>
                        </div>";

            if (IsChild)
            {
                Title = string.Format("<span class='text-muted cursor-default'>{0}</span>", Title);
            }

            html = string.Format(html, ScrollID, GetDataTargets(TargetID), Title, CssClass);
            writer.Write(html);
        }
        private string GetDataTargets(string targetId)
        {
            string result = string.Empty;
            if (!targetId.IsNullOrEmpty())
            {
                string[] targetIds = targetId.Split(',');
                foreach (var item in targetIds)
                {
                    var control = FindControlRecursive(this.Page, item);
                    //If it could not find the control ID, it will take the selected item 
                    if (!control.IsNullData())
                    {
                        result += "#" + control.ClientID + ",";
                    }
                    else
                        result += "#" + item + ",";
                }
                result = result.Trim(',');
            }
            return result;
        }

        public string DoExport()
        {
            var _data = KeyValueControl as IExportKeyValueTable;
            if (!_data.IsNullData())
            {
                if (IsMultiExport)
                    return _data.ExportForMulti(FileName?? Title, EXPORT_EXCELL_PATH);
                else
                    return _data.ExportKeyValueTableContent(FileName ?? Title);
            }
            return string.Empty;
        }
    }
}