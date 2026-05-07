using System;
using System.Collections.Generic;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data;
using System.Text;
using AS.Common;
using System.Web;
using System.Xml.Serialization;

public enum FilterPageEnums
{
    DetectionQueue = 1,
    ManageAssignments = 2,
    PortfolioStatistics = 3
}

public enum FilterTypeEnums
{
    SavedFilter = 1,
    RecentFilter = 2
}

public enum OperatorEnums
{
    Equal,
    NotEqual,
    Like,
    RLike,
    LLike,
    In,
    Between,
    Great,
    Little,
    LittleEqual,
    GreatEqual,
    GreaterThan,
    LessThan,
    Underfined
}

public enum InputTypeEnums
{
    Underfined = 0,
    DatePicker = 4,
    Selection = 5,
    SingleSelection = 6,
    RangeRadio = 9,
    Text = 10,
    MultiSelection = 11,
    MultiChoose = 12,
    SubFilter = 13,
    DateTime = 14,
    OrderBy = 15,
    AutoComplete = 16,
    DropdownList = 17
}

public enum TaskListFilterKeys
{
    GetBy,

    MpaStatus,

    AssigneeId,

    IsAssigned,

    KeyerId,

    SalesRepCode,

    OrganizationCode,

    MyTaskIsDefault,

    IsFastTrack,
}

[Serializable]
public class AdvancedFilterConfig
{
    public FilterPageEnums FilterPage { get; set; }

    [XmlArray("KeywordItems")]
    public List<KeywordItem> KeywordItems { get; set; }

    [XmlArray("QuickFilters")]
    public List<AdvancedFilterInfo> QuickFilters { get; set; }
}

[Serializable]
public class AdvancedFilterInfo
{
    [XmlAttribute("FilterSearchID")]
    public long FilterSearchID { get; set; }

    [XmlAttribute("FilterSearchIDEncrypt")]
    public string FilterSearchIDEncrypt { get; set; }

    [XmlAttribute("FilterPage")]
    public int FilterPage { get; set; }

    [XmlAttribute("FilterName")]
    public string FilterName { get; set; }

    [XmlAttribute("FilterContent")]
    public string FilterContent { get; set; }

    [XmlAttribute("IsRecent")]
    public bool IsRecent { get; set; }

    public List<FilterItem> FilterItems { get; set; }

    [XmlAttribute("CreatedDTS")]
    public DateTime CreatedDTS { get; set; }

    public bool IsGenerate { get; set; }
}

[Serializable]
public class FilterItem
{
    [XmlIgnore]
    public FilterTypeEnums FilterType { get; set; }

    [XmlAttribute("Key")]
    public string Key { get; set; }

    [XmlAttribute("Value")]
    public string Value { get; set; }

    [XmlAttribute("Value2")]
    public string Value2 { get; set; }

    [XmlIgnore]
    public string DisplayName { get; set; }

    [XmlIgnore]
    public string DisplayValue { get; set; }

    [XmlIgnore]
    public string DisplayValueReal { get; set; }

    [XmlIgnore]
    public string DisplayAutoComplete { get; set; }

    [XmlAttribute("Operator")]
    public OperatorEnums Operator { get; set; }

    [XmlIgnore]
    public InputTypeEnums InputType { get; set; }

    [XmlIgnore]
    public string InputTypeValue { get { return InputType.ToString(); } }

    [XmlIgnore]
    public bool IsParent { get { return SubFilterItems != null && SubFilterItems.Count > 0; } }

    [XmlArray("SubFilterItems")]
    public List<FilterItem> SubFilterItems { get; set; }

    public void Add(FilterItem item)
    {
        if (SubFilterItems == null)
        {
            SubFilterItems = new List<FilterItem>();
        }
        SubFilterItems.Add(item);
    }

    public void AddRange(List<FilterItem> items)
    {
        if (SubFilterItems == null)
        {
            SubFilterItems = new List<FilterItem>();
        }
        SubFilterItems.AddRange(items);
    }

    [XmlIgnore]
    public string ParentName { get; set; }

    [XmlIgnore]
    public string ParentKey { get; set; }
}

[Serializable]
public class KeywordItem
{
    /// <summary>
    /// Key search (property in model)
    /// </summary>
    [XmlAttribute("Key")]
    public string Key { get; set; }

    /// <summary>
    /// Field name display
    /// </summary>
    [XmlAttribute("Display")]
    public string Display { get; set; }

    [XmlAttribute("Description")]
    public string Description { get; set; }

    /// <summary>
    /// Ref data (for select list)
    /// </summary>
    public List<Item> ValueItems { get; set; }

    /// <summary>
    /// Type input
    /// </summary>
    [XmlAttribute("InputType")]
    public InputTypeEnums InputType { get; set; }

    [XmlAttribute("FilterType")]
    public FilterTypeEnums FilterType { get; set; }

    [XmlAttribute("Operator")]
    public OperatorEnums Operator { get; set; }

    [XmlAttribute("IsGetDataSource")]
    public bool IsGetDataSource { get; set; }

    /// <summary>
    /// Function name validation
    /// </summary>
    public string ClientValidationFunction
    {
        get
        {
            if (this.InputType != InputTypeEnums.Underfined)
            {
                switch (InputType)
                {
                    case InputTypeEnums.DatePicker:
                        return "aperia.searchControl.validate.validateDateFormat";
                    case InputTypeEnums.Selection:
                        if (!IsParent && IsMultiValue)
                        {
                            return "aperia.searchControl.validate.multipleSelection";
                        }
                        return "aperia.searchControl.validate.selection";
                    default:
                        break;
                }
            }
            return "aperia.defaultValidate";
        }
        set { }
    }

    /// <summary>
    /// Default all attribute search is show
    /// </summary>
    [XmlAttribute("IsHidden")]
    public bool IsHidden { get; set; }

    /// <summary>
    /// Default all attribute search only allow to select one.
    /// </summary>
    [XmlAttribute("IsMultiValue")]
    public bool IsMultiValue { get; set; }

    [XmlArray("SubKeywordItems")]
    public List<KeywordItem> KeywordItems { get; set; }

    public bool IsParent { get { return InputType == InputTypeEnums.SubFilter; } }

    [XmlArray("ValidationRules")]
    public List<AdvancedValidationRule> ValidationRules { get; set; }

    [XmlAttribute("IsDefaultFilter")]
    public bool IsDefaultFilter { get; set; }

    public string DefaultValue { 
        get 
        {
            if (Key == "DateTime" && IsDefaultFilter)
                return DateTime.Now.ToString("MM/dd/yyyy");
            if (Key == "Export" && IsDefaultFilter)
                return "Excel";
            if (Key == "CaseType" && IsDefaultFilter)
                return "2";
            return string.Empty; 
        } 
    }

    public string DefaultDisplayValue
    {
        get
        {
            if (Key == "DateTime" && IsDefaultFilter)
                return DateTime.Now.ToString("MM/dd/yyyy");
            if (Key == "Export" && IsDefaultFilter)
                return "Excel";
            if (Key == "CaseType" && IsDefaultFilter)
                return AdvancedFilter_GeneralFunction.GetResourceValue("Risk"); ;
            return string.Empty;
        }
    }

    public string ParentKey { get; set; }

    [XmlAttribute("Value")]
    public string Value { get; set; }

    [XmlAttribute("ParentName")]
    public string ParentName { get; set; }

    [XmlAttribute("TitleMultiChoose")]
    public string TitleMultiChoose { get; set; }

    [XmlAttribute("IsRequiredWhenChangeDate")]
    public bool IsRequiredWhenChangeDate { get; set; }
}

[Serializable]
public class Item
{
    [XmlAttribute("Key")]
    public string Key { get; set; }

    [XmlAttribute("Value")]
    public string Value { get; set; }

    [XmlAttribute("DisplayResource")]
    public string DisplayResource { get; set; }

    public string Display
    {
        get
        {
            return AdvancedFilter_GeneralFunction.GetResourceValue(DisplayResource);
        }
        set { this.Display = value; }
    }

    /// <summary>
    /// Default all values of filter is show
    /// </summary>
    [XmlAttribute("IsHidden")]
    public bool IsHidden { get; set; }
}

[Serializable]
public class AdvancedValidationRule
{
    [XmlAttribute("ControlToValidateID")]
    public string ControlToValidateID { get; set; }

    [XmlAttribute("ObjectControlToValidateID")]
    public string ObjectControlToValidateID { get; set; }

    [XmlAttribute("Rule")]
    public string Rule { get; set; }

    [XmlAttribute("MessageResource")]
    public string MessageResource { get; set; }

    public string Message 
    {
        get
        {
            return AdvancedFilter_GeneralFunction.GetResourceValue(MessageResource);
        }
        set { this.Message = value; }
    }
}

public class BoardingWidgetFilter
{
    public List<FilterItem> AdvandFilter { get; set; }
}

public class ObjectResult 
{
    public string Id { get; set; }
    public string ErrorMessage { get; set; }
}

public class FilterDataItem
{
    public string Key { get; set; }
    public string Value { get; set; }
    public string DisplayValue { get; set; }
    public string DisplayText { get; set; }
}

public static class AdvancedFilter_GeneralFunction
{
    public static string GetResourceValue(string name)
    {
        if (string.IsNullOrEmpty(name))
            return "";
        // Fix can not switch language in Static method
        if(SessionManager.CurrentLanguage == 2)
        {
            return HttpContext.GetGlobalResourceObject("AdvancedFilterResource", name, System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")).ToSafeString();
        }

        return HttpContext.GetGlobalResourceObject("AdvancedFilterResource", name).ToSafeString();
    }
}