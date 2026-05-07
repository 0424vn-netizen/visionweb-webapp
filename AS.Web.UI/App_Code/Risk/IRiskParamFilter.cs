using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for IRiskParamFilter
/// </summary>


public interface IRiskParamFilter
{
    WebSiteEnums.ParamFilterMode Mode { get; set; }
    string PrimaryID { get; set; }//Can be: Assignment ID, ParameterDefineID,  AdhocID base on the mode
    string ParamID { get; set; }//can be null
    string FilterID { get; set; }//preserve
    int Save();//return error code, if needed or else return 0
    event EventHandler SelectedChanged;
}
