using AS.Core.Common.Utilities;
using AS.Security.WS.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Xml;
using AS.Controls.ASP.Net;

public partial class UserControls_HeaderMenuMasterUserControl : DynamicUserControl<HeaderMenuUserControl>
{

    public UserControls_HeaderMenuMasterUserControl()
    {
        GroupPage = "HeaderMenu";
    }

    public void LoadMenu(ASMenu menu)
    {
        base.LoadData(menu);
    }
}