using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Security.WS.Mobile
{
    public partial class SecMobileService
    {
        private readonly SecMobileDao _secDAO = null;

        public SecMobileService(string connectionString)
        {
            _secDAO = new SecMobileDao(connectionString);
        }

        public MobileUser GetMobileUser(int clientId, string userId)
        {
            return _secDAO.GetMobileUser(clientId, userId);
        }
    }
}
