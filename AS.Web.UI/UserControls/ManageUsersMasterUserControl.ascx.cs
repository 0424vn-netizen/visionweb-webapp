using AS.Core.Common.Utilities;
using AS.Security.WS.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Xml;

public partial class UserControls_ManageUsersMasterUserControl : DynamicUserControl<ManageUsersBaseUserControl>
{


    public UserControls_ManageUsersMasterUserControl()
    {
        GroupPage = "ManageUsers";
    }
    public bool SubmitUser(int clientID, string userId, int userType, string status)
    {
        return base.SubmitData(clientID, userId, userType, status);
    }

    public void ReloadUser(int clientID, string userId)
    {
        base.ReloadData(clientID, userId);
    }

    public void LoadUser(int clientID, string userId)
    {
        base.LoadData(clientID, userId);

    }



}