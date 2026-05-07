using AS.Controls.Grid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for FDCKeyValueTableRow
/// </summary>

namespace AS.Controls
{
    [Serializable]
    public abstract class KeyValueTableRow
    {
        public string UniqueName { get; set; }

        //public string CellLeftContent { get; set; }

        public string CellTitle { get; set; }

        public string Text { get
            
        {
            return this.CellTitle;
        }
            set
            {
                this.CellTitle = value;
            }
        }

        public string CellTitleTooltip { get; set; }

        public string DataField { get; set; }

        public FormatType FormatType { get; set; }

        public bool ExportAble { get; set; }

        public bool Visible { get; set; }

        public KeyValueTableRow()
        {
            FormatType = FormatType.DynamicString;
            ExportAble = true;
            Visible = true;
            UniqueName = string.Empty;
        }

        public abstract void BuildCell(TableCell cellRight, DataRowView dataRow);
    }
}