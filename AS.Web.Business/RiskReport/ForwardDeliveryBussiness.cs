using AS.Common.DBManager;
using AS.Security.WS.Entities;
using AS.Web.Business.General;
using AS.Web.Business.RiskReport.Models;
using System.Data;

namespace AS.Web.Business.RiskReport
{
    public class ForwardDeliveryBussiness : IForwardDeliveryBussiness
    {
        private readonly IReportServices _reportService;
        public ForwardDeliveryBussiness(IReportServices service)
        {
            _reportService = service;
        }

        public DataTable GetForwardDelivery(GetForwardDeliveryRequest request, User currentUser)
        {
            if (request == null || currentUser == null)
            {
                return null;
            }
            var paras = new FilterParameterCollection
            {
                new FilterParameter("@MerchantNumber", request.MerchantNumber, DbType.String),
                new FilterParameter("@UserID", currentUser.UserID, DbType.AnsiString)
            }
           .AddParamCurrentInUser(currentUser, request.UserMode);
            return _reportService.GetReports("spa_RM_MCF_RiskReport_GetForwardDelivery", paras);
        }
        public bool CalculateForwardDelivery(CalculateForwardDeliveryRequest request, User currentUser)
        {
            if (request == null || currentUser == null)
            {
                return false;
            }
            var paras = new FilterParameterCollection
            {
                new FilterParameter("@MerchantNumber", request.MerchantNumber, DbType.AnsiString),
                new FilterParameter("@CreditTimeliness", request.CreditTimeliness, DbType.Int32),
                new FilterParameter("@NDX", request.NDX, DbType.Int32),
                new FilterParameter("@NDXPercent", request.NDXPercent, DbType.Int32),
                new FilterParameter("@FEAction", (int)request.EnumFWDAction, DbType.Int32),
                new FilterParameter("@UserID", currentUser.UserID, DbType.AnsiString)
            }
            .AddParamCurrentInUser(currentUser, request.UserMode);

            _reportService.ExecuteNonQueryCommand("spa_RM_MCF_RiskReport_CalculateForwardDelivery", paras, out _);
            return true;
        }


    }
}
