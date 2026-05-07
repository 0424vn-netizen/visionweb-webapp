using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Web.UI;

/// <summary>
/// Summary description for IExportKeyValueTable
/// </summary>
public interface IExportKeyValueTable
{
    string ExportKeyValueTableContent(string reportTitle);
    string ExportForMulti(string reportTitle, string path);
}