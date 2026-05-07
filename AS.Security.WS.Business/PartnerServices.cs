using AS.Security.WS.Entities;
using AS.Security.WS.Data;

namespace AS.Security.WS.Business
{
    public class PartnerServices
    {
        readonly PartnerDao _PartnerDAO;
        public PartnerServices(string connString)
        {
            _PartnerDAO = new PartnerDao(connString);
        }

        #region Partner Methods

        public PartnerCollection GetPartners(string PartnerName)
        {
            return _PartnerDAO.GetPartners(PartnerName);
        }

        #endregion
    }
}