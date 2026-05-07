using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AS.WS.Entities
{
    public class DocumentTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int SourceId { get; set; }
        public string SourceIdList { get; set; }
        public bool IsSyncData { get; set; }
        public string CreatedBy { get; set; }
    }
}
