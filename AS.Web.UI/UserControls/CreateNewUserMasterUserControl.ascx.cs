using AS.Core.Common.Utilities;
using AS.Security.WS.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Xml;

public partial class UserControls_CreateNewUserMasterUserControl : DynamicUserControl<CreateNewUserBaseUserControl>
{
    public bool IsUpdateMode { get; set; }
    

    public UserControls_CreateNewUserMasterUserControl()
    {
        GroupPage = "CreateNewUser";
    }
    public bool SubmitUser(int clientID, string userId, string[] permissionCodes)
    {
        return base.SubmitData(clientID, userId, permissionCodes);
    }

    public void ReloadUser(int clientID, string userId, string[] permissionCodes)
    {
        base.ReloadData(clientID, userId, permissionCodes);
    }

    public void LoadUser(int clientID, string userId, bool isUpdateMode, string[] permissionCodes, PermissionCollection accessPermissions)
    {
        base.LoadData(clientID, userId, isUpdateMode, permissionCodes, accessPermissions);

    }



}