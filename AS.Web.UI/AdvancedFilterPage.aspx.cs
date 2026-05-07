using System;
using System.Collections.Generic;
using System.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using AS.Common.DBManager;
using System.Data;
using System.Web;
using Newtonsoft.Json;
using AS.Core.Common.Utilities;
using AS.Core.Common;
using System.Web.UI.WebControls;

public partial class AdvancedFilterPage : ReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.IsBindDataOnLoad = true;
    }

    [WebMethod(EnableSession = true)]
    public static ObjectResult SaveFilter(string page, string nameFilter, string filterContent)
    {
        return UserControls_UxAdvancedFilter.SaveFilter((int)Enum.Parse(typeof(FilterPageEnums), page), nameFilter, filterContent);
    }

    [WebMethod(EnableSession = true)]
    public static string RenameFilterById(long id, string name)
    {
        return UserControls_UxAdvancedFilter.UpdateFilterById(0, id, name, string.Empty);
    }

    [WebMethod(EnableSession = true)]
    public static string UpdateFilterById(string page, long id, string filter)
    {
        return UserControls_UxAdvancedFilter.UpdateFilterById((int)Enum.Parse(typeof(FilterPageEnums), page), id, string.Empty, filter);
    }

    [WebMethod(EnableSession = true)]
    public static string DeleteFilterById(long id)
    {
        return UserControls_UxAdvancedFilter.DeleteFilterById(id);
    }

    [WebMethod(EnableSession = true)]
    public static string GenerateItemSource(string page, string type, string key, string subValue)
    {
        return UserControls_UxAdvancedFilter.GenerateItemSource((int)Enum.Parse(typeof(FilterPageEnums), page), type, key, subValue);
    }

    [WebMethod(EnableSession = true)]
    public static string GenerateSavedFilterList(string page, string filterSection)
    {
        return UserControls_UxAdvancedFilter.GenerateSavedFilterList((int)Enum.Parse(typeof(FilterPageEnums), page), filterSection);
    }

    [WebMethod(EnableSession = true)]
    public static ObjectResult SaveAppliedFilter(string page, long id, string filterContent, string filterSection, bool isGenerate)
    {
        return UserControls_UxAdvancedFilter.SaveAppliedFilter((int)Enum.Parse(typeof(FilterPageEnums), page), id, filterContent, filterSection, isGenerate);
    }

    [WebMethod(EnableSession = true)]
    public static AdvancedFilterInfo GetInfoFilterItem(string page, long id, bool isRecent)
    {
        return UserControls_UxAdvancedFilter.GetInfoFilterItem((int)Enum.Parse(typeof(FilterPageEnums), page), id, isRecent);
    }

    [WebMethod(EnableSession = true)]
    public static List<FilterDataItem> GetDataAutoComplete(string page, string type, string key, string subValue, string filterText, string ignoreValue)
    {
        return UserControls_UxAdvancedFilter.GetDataAutoComplete((int)Enum.Parse(typeof(FilterPageEnums), page), type, key, subValue, filterText, ignoreValue);
    }
}