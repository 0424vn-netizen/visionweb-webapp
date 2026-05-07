using AS.WS.Mobile.Domain.Models;

namespace AS.WS.Mobile.Domain.Contracts
{
    public partial interface IMobileService
    {
        Merchants GetMerchantList(MobileParameters parameters);
    }
}
