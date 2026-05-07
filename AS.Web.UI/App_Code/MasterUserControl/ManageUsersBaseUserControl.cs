using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;

/// <summary>
/// Summary description for CustomUserControl
/// </summary>
public abstract class ManageUsersBaseUserControl : ModuleUserControl
{

    protected abstract void LoadUser(int clientID, string userId);

    protected abstract void ReloadUser(int clientID, string userId);

    protected abstract bool SubmitUser(int clientID, string userId, int userType, string status);

    protected abstract bool ValidateUser(object extraInfo);


    public override void LoadData(params object[] paramters)
    {
        this.LoadUser((int)paramters[0], (string)paramters[1]);
    }

    public override void ReloadData(params object[] paramters)
    {
        this.ReloadUser((int)paramters[0], (string)paramters[1]);
    }

    public override bool SubmitData(params object[] paramters)
    {
        return this.SubmitUser((int)paramters[0], (string)paramters[1], (int)paramters[2], (string)paramters[3]);
    }

    public override bool ValidateData(object extraInfo)
    {
        return this.ValidateUser(extraInfo);
    }
}