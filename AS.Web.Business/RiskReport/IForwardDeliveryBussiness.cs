using AS.Security.WS.Entities;
using AS.Web.Business.RiskReport.Models;
using System.Data;

namespace AS.Web.Business.RiskReport
{
    public interface IForwardDeliveryBussiness
    {
        DataTable GetForwardDelivery(GetForwardDeliveryRequest request, User currentUser);
        bool CalculateForwardDelivery(CalculateForwardDeliveryRequest request, User currentUser);
    }
}
