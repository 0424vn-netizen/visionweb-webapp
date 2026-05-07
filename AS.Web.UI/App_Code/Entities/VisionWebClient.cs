using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for VisionWebClient
/// </summary>
public class VisionWebClient
{
    public VisionWebClient()
    {
        ASClient = 0;
        ClientAbbreviation = string.Empty;
        ClientName = string.Empty;
        ActvStatus = string.Empty;
        DateCreated = DateTime.Today;
        CreatedBy = string.Empty;
        DateUpdated = DateTime.Today;
        UpdatedBy = string.Empty;
        MarketData = string.Empty;
        IsAutoCheckWork = false;
    }
    public int ASClient { get; set; }
    public string ClientAbbreviation { get; set; }
    public string ClientName { get; set; }
    public string ActvStatus { get; set; }
    public DateTime DateCreated { get; set; }
    public string CreatedBy { get; set; }
    public DateTime DateUpdated { get; set; }
    public string UpdatedBy { get; set; }
    public string MarketData { get; set; }
    public bool IsAutoCheckWork { get; set; }
    public bool TransHist30 { get; set; } 
}
