using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ColumnDisplayedConfigurationItem
/// </summary>
[Serializable()]
public class ColumnDisplayedConfigurationItem
{
    public string ColumnName { get; set; }
    public string ColumnText { get; set; }
    public bool IsDisplayed { get; set; }
    public int OrderIndex { get; set; }
}

[Serializable()]
public class TransVolumeUserDataItem
{
    public List<string> DisplayedColumns { get; set; }
}
