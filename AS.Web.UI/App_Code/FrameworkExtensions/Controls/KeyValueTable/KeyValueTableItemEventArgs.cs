using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for FDCKeyValueTableItemEventArgs
/// </summary>

namespace AS.Controls
{
    [Serializable]
    public class KeyValueTableItemEventArgs : EventArgs
    {
        public KeyValueTableItem Item { get; private set; }

        public KeyValueTableItemEventArgs(KeyValueTableItem item)
        {
            Item = item;
        }
    }
}