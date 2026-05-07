using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// Summary description for FDCKeyValueTableTemplateRow
/// </summary>

namespace AS.Controls
{
    [Serializable]
    public class KeyValueTableTemplateRow : KeyValueTableRow
    {
        [Browsable(false)]
        [DefaultValue("")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateContainer(typeof(KeyValueTableItem))]
        public ITemplate ItemTemplate { get; set; }

        public override void BuildCell(System.Web.UI.WebControls.TableCell cellRight, System.Data.DataRowView dataRow)
        {
            ItemTemplate.InstantiateIn(cellRight);
        }
    }
}