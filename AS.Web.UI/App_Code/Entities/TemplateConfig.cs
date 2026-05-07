using AS.Controls.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TemplateConfig
/// </summary>
public class TemplateConfig
{
    public string OriginalKey { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public string AdditionData { get; set; }
    public TemplateConfigType Type { get; set; }
    public string FormatType { get; set; }
    public string Source { get; set; }
}
public enum TemplateConfigType
{
    Resource = 1,
    Data = 2,
    RiskReportExtend = 3,
    Loop = 4,
    Permission = 5
}
public enum TemplateConfigFormatType
{
    Text = 1,
    Currency = 2,
    Percent = 3,
    Phone = 4,
    SIC = 5,
    Address = 6,
    Date = 7,
    BatchLink = 8,
    StatusAccount = 9,
    WebSite = 10,
    TaxView = 11,
    RawText = 12,
    SiteAcessLink = 13,
    DDAView = 14,
    PartialData = 15,
    Hierarchy = 16,
    ChainLink = 17,
    ViewRiskReportLink = 18,
    SiteAcessLinkExport = 19,
    SiteJump = 20,
    SecondaryAccessChainLink = 21,
    PricingToolTip = 22,
    MC_ICA_AVS = 23,    
    ToolTipTitle = 24,
    NoRecords = 25,
    AgentLink = 26,
    Resource = 27,
    AccountStatus = 28,
    AccountEffectedDate = 29,
    OptStatusAction = 30
}
public class MerchantProfileConfig
{
    public string Processor { get; set; }
    public string MerchantSpaName { get; set; }
    public List<Section> Sections { get; set; }
}

public class MerchantProfileClientConfig
{
    public bool HasMultiProcessor { get; set; }
    public string DefaultProcessor { get; set; }
    public string DecryptColumns { get; set; }
    public List<MerchantColumn> Columns { get; set; }
}

public class Section 
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string HasPermissionCode { get; set; }
    public string TemplateKey { get; set; }
    public string TemplateFile { get; set; }
    public string ExportTemplate { get; set; }
    public bool IsHide { get; set; }
    public SectionType Type { get; set; }
    public string SpaName { get; set; }
    public bool IgnoreGetData { get; set; }
}
public class MerchantColumn
{
    public string UniqueName { get; set; }
    public string Key { get; set; }
    public string ASFormat { get; set; }
    public string ReSourceKey { get; set; }
    public int Width { get; set; }
    public string DefaultValue { get; set; }
    public string HeaderAlign { get; set; }
    public string ItemAlign { get; set; }
    public bool IsTaxView { get; set; }
}
public enum SectionType
{
    Normal = 1,
    CardInformation = 2
}