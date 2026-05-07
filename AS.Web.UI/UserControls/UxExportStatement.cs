using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public partial class UserControls_UxExportStatement : AS.Controls.Global.Exporter
{   
    
    public bool IsOnTop { get; set; }
    
    protected override void OnPreRender(EventArgs e)
    {      
        this.GridTitle = this.GridTitle.Trim().TrimEnd('-');
        litGridTitle.Text = this.GridTitle.Trim().TrimEnd('-');
       

        if (IsOnTop)
        {
            h2GridTitle.Attributes.Add("class", "grid-title on-top");
        }
        else
        {
            h2GridTitle.Attributes.Add("class", "grid-title");
        }

        h2GridTitle.Attributes.Add("data-target", "#" + this.ClientID.Replace(this.ID, GridID));
        base.OnPreRender(e);
    }
}