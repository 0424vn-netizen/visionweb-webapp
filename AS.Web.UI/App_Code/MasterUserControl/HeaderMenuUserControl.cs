using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using AS.Controls.ASP.Net;

/// <summary>
/// Summary description for CustomUserControl
/// </summary>
public abstract class HeaderMenuUserControl : ModuleUserControl
{
    protected abstract void LoadMenu(ASMenu menu);

    public override void LoadData(params object[] paramters)
    {
        this.LoadMenu((ASMenu)paramters[0]);
    }

    public override void ReloadData(params object[] paramters)
    {
        throw new NotImplementedException();
    }

    public override bool SubmitData(params object[] paramters)
    {
        throw new NotImplementedException();
    }

    public override bool ValidateData(object extraInfo)
    {
        throw new NotImplementedException();
    }
}