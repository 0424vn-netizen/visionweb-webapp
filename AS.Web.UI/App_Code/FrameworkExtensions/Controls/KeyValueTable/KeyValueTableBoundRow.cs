using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for FDCKeyValueTableBoundRow
/// </summary>

namespace AS.Controls
{
    [Serializable]
    public class KeyValueTableBoundRow : KeyValueTableRow
    {
        public override void BuildCell(TableCell cellRight, DataRowView dataRow)
        {
            if (DataField == null || DataField == string.Empty)
                cellRight.Text = string.Empty;
            else
                cellRight.Text = FormatResolver.Format(FormatType, dataRow[DataField]);
        }
    }
}