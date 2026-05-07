using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for FDCKeyValueTableRowCollection
/// </summary>
namespace AS.Controls
{
    [Serializable]
    public sealed class FDCKeyValueTableRowCollection : AS.Controls.Bases.StateCollectionBase<KeyValueTableRow>, IEnumerable<KeyValueTableRow>
    {
        public new IEnumerator<KeyValueTableRow> GetEnumerator()
        {
            return ItemList.GetEnumerator();
        }

        public KeyValueTableRow this[string uniqueName]
        {
            get
            {
                return this.Single(s => !s.UniqueName.IsNullOrEmpty() && s.UniqueName.Equals(uniqueName, StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}