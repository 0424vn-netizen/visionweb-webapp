using AS.Common.DBManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.WS.Entities
{
    public class HashColEntity
    {
        public string ColumnName { get; set; }
        public string HashValue { get; set; }
        public DataTable RawData { get; set; }
        public HashColEntity(string column)
        {
            ColumnName = column;
        }

        public HashColEntity(string column, string value)
        {
            ColumnName = column;
            HashValue = value;
        }

        public void AddHashValue(string value)
        {
            if (!string.IsNullOrEmpty(value))
                HashValue = string.Format("{0},{1}", HashValue, value);
        }

        public void TrimHash()
        {
            if(!string.IsNullOrEmpty(HashValue))
                HashValue = HashValue.TrimStart(',');
        }
    }

    public class HashDataModel
    {
        public List<HashColEntity> HashColumns { get; set; }
        public List<string> DecryptColumns { get; set; }
    }

    public class ServiceRequestModel
    {
        public FilterParameterCollection Params { get; set; }
        public int AsClientId { get; set; }
        public bool IsExport { get; set; }
        public bool IsHash { get; set; }
        public List<HashColEntity> HashColumns { get; set; } = new List<HashColEntity>();
        public List<string> DecryptColumns { get; set; } = new List<string>();
        public string HashColumnsConfig { get; set; } 
        public string HashClientsConfig { get; set; }
    }

    public class ReportRequestModel : ICloneable
    {
        public FilterParameterCollection Params { get; set; }
        public int AsClientId { get; set; }
        public bool IsExport { get; set; }
        public List<string> EncryptedColumnRequest { get; set; } = new List<string>();
        public bool IsHash { get; set; }
        public List<HashColEntity> HashColumns { get; set; } = new List<HashColEntity>();
        public List<string> DecryptColumns { get; set; } = new List<string>();
        public string HashColumnsConfig { get; set; }
        public string HashClientsConfig { get; set; }

        public object Clone()
        {
            return new ReportRequestModel
            {
                Params = Params,
                AsClientId = AsClientId,
                IsExport = IsExport,
                EncryptedColumnRequest = EncryptedColumnRequest,
                IsHash = IsHash,
                HashColumns = HashColumns,
                DecryptColumns = DecryptColumns,
                HashColumnsConfig = HashColumnsConfig,
                HashClientsConfig = HashClientsConfig
            };
        }
    }
}
