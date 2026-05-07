using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for UserRoleType
/// </summary>
public class UserRoleType
{
    #region Constants

    /// <summary>
    /// CS
    /// </summary>
    public const string CS_ROLE = "CS";
    /// <summary>
    /// MS
    /// </summary>
    public const string MS_ROLE = "MS";

    #endregion Constants

    #region Methods

    public static string GetUserRoleText(int systemId)
    {
        string role = string.Empty;
        switch (systemId)
        {
            case WebSiteConstants.AS_SYSTEM_CS:
                role = UserRoleType.CS_ROLE;
                break;
            case WebSiteConstants.AS_SYSTEM_MS:
                role = UserRoleType.CS_ROLE;
                break;
        }
        return role;
    }

    #endregion Methods
}
