using AS.Common.DBManager;
using System.Collections.Generic;
using System.Data;

namespace AS.Web.Business.PCI
{
   public interface  IPciServices
    {
        DataTable GetReports(string spName, FilterParameterCollection _parameters);
        int ExecuteNonQueryCommand(string spName, FilterParameterCollection _parameters, out FilterParameterCollection OutputParams);
        string EncryptText(string encryptStr);
        string DecryptText(string decryptStr);
        void AddRequestHeader(string name, string value);
        void AddRequestHeaders(Dictionary<string, string> dictHeader);
        void InsertOrUpdateRequestHeader(string name, string value);
    }
}
