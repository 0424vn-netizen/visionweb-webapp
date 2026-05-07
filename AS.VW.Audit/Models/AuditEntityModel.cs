using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Audit.Models
{
    [Serializable]
    public class AuditEntityModel
    {
        public string Id { get; set; }
        public AuditAction AuditAction { get; set; }
        public string ColumnName { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
    }

    [Serializable]
    public enum AuditAction
    {
        Added = 1,
        Updated = 2,
        Deleted = 3
    }

    [Serializable]
    public class AuditEntityLegacyModel
    {
        public string Key { get; set; }
        /// <summary>
        /// KeyLang = ActionName (Update/Delete...)
        /// </summary>
        public string KeyLang { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
    }
}
