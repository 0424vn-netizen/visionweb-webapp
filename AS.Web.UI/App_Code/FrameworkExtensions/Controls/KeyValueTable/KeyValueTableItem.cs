using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for KeyValueTableItem
/// </summary>
namespace AS.Controls
{
    [Serializable]
    public class KeyValueTableItem : TableRow, IDataItemContainer, INamingContainer
    {
        public TableCell RightCell
        {
            get
            {
                return Cells[1];
            }
        }

        public TableCell LeftCell
        {
            get
            {
                return Cells[0];
            }
        }

        public object DataItem { get; set; }

        public int DataItemIndex { get; private set; }

        public int DisplayIndex { get; private set; }

        public string UniqueName { get; set; }
    }
}