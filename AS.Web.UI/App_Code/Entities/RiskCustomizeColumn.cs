using AS.Controls.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for rm_MCF_ColumnSetting
/// </summary>
public class RiskCustomizeColumn
{
    public string Key { get; set; }
    public string ASFormat { get; set; }
    public FormatType ASFormatType { get; set; }
    public string CustomFormat { get; set; }
    public string ReSourceKey { get; set; }
    public bool IsDefault { get; set; }
    public int Width { get; set; }
    public int OrderNo { get; set; }
    public string DefaultValue { get; set; }
    public bool IsHide { get; set; }
    public string ToolTipKey { get; set; }
    public bool IsAutoText { get; set; }
    public bool HasBorder { get; set; }
    public string DisplayDashValue { get; set; }
    public string Vertical { get; set; }
}