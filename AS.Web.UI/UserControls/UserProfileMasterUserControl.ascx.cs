using AS.Core.Common.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Xml;

public partial class UserControls_UserProfileMasterUserControl : DynamicUserControl<UserProfileBaseUserControl>
{
    public UserControls_UserProfileMasterUserControl()
    {
        GroupPage = "UserProfile";
    }
    public bool SubmitUser(int clientID, string userId)
    {
        return base.SubmitData(clientID, userId);
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