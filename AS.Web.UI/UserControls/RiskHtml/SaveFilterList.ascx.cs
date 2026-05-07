using System;
using System.Collections.Generic;
using System.Linq;
using AS.Common.DBManager;
using System.Data;
using System.Web;
using Newtonsoft.Json;
using AS.Core.Common.Utilities;

public partial class UserControls_RiskHtml_SaveFilterList : GlobalUserControl
{
    public List<AdvancedFilterInfo> FilterSearchs { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

}