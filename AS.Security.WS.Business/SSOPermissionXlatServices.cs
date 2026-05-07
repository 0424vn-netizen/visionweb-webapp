using AS.Security.WS.Entities;
using AS.Security.WS.Data;
using System.Collections.Generic;

namespace AS.Security.WS.Business
{
    public class SsoPermissionXlatServices
    {
        readonly SsoPermissionXlatDao _SSOPermissionXlatDAO;
        public SsoPermissionXlatServices(string connString)
        {
            _SSOPermissionXlatDAO = new SsoPermissionXlatDao(connString);
        }

        #region SSOPermissionXlat Methods

        public SsoPermissionXlatCollection GetSSOPermissionXlat(int PartnerID, int ClientID, int PermissionLevel)
        {
            return _SSOPermissionXlatDAO.GetSSOPermissionXlat(PartnerID, ClientID, PermissionLevel);
        }
        #endregion
    }
}