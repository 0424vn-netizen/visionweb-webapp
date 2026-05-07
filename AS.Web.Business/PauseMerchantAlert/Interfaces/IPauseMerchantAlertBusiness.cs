using AS.Security.WS.Entities;
using AS.Web.Business.PauseMerchantAlert.Models;
using System.Data;

namespace AS.Web.Business.PauseMerchantAlert.Interfaces
{
    public interface IPauseMerchantAlertBusiness
    {
        int DeleteAssignmentPauseDateRange(string userMode, User currentUser, string assignmentId, string rowGuid);

        DataTable GetPauseMerchantAlertFilters(string userMode, User currentUser, string assignmentId);

        DataTable GetAllMerchants(string merchantIdList, string userMode, User currentUser);

        int SaveAssignmentPauseDateRange(string userMode, User currentUser, string assignmentId, string data);

        PauseMerchantAlertResponse GetMerchantFilters(string userMode, User currentUser, string customfilterstring = null);
    }
}
