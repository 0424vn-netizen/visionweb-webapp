using AS.Security.WS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;

/// <summary>
/// Summary description for CustomUserControl
/// </summary>
public abstract class CreateNewUserBaseUserControl : ModuleUserControl
{
    private bool _isUpdateMode = false;

    public bool IsUpdateMode
    {
        get
        {
            return _isUpdateMode;
        }
        set
        {
            _isUpdateMode = value;
        }
    }

    protected abstract void LoadUser(int clientID, string userId, bool isUpdateMode, string[] permissionCodes, PermissionCollection accessPermissions);

    protected abstract void ReloadUser(int clientID, string userId, string[] permissionCodes);

    protected abstract bool SubmitUser(int clientID, string userId, string[] permissionCodes);

    protected abstract bool ValidateUser(object extraInfo);

    protected object GetExtraInfoByProperty(object extraInfo, string propertyName)
    {
        if (extraInfo == null) return null;
        PropertyInfo p = extraInfo.GetType().GetProperty(propertyName);
        if (p != null)
        {
            return p.GetValue(extraInfo);

        }

        return null;
    }



    public override void LoadData(params object[] paramters)
    {
        this.LoadUser((int)paramters[0], (string)paramters[1], (bool)paramters[2], (string[])paramters[3], (PermissionCollection)paramters[4]);
    }

    public override void ReloadData(params object[] paramters)
    {
        this.ReloadUser((int)paramters[0], (string)paramters[1], (string[])paramters[2]);
    }

    public override bool SubmitData(params object[] paramters)
    {
        return this.SubmitUser((int)paramters[0], (string)paramters[1], (string[])paramters[2]);
    }

    public override bool ValidateData(object extraInfo)
    {
        return this.ValidateUser(extraInfo);
    }
}