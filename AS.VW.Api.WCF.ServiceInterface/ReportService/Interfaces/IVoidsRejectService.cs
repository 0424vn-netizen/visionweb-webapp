using AS.VW.Api.Model.Dto;
using AS.VW.Api.Model.Filter;
using AS.VW.Api.Model.Report;
using AS.VW.Api.Business.Repository;
using AS.VW.Api.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Api.WCF.ServiceInterface
{
    [ServiceContract]
    [AS.WCF.SecuredService]
    public interface IVoidsRejectService
    {
        [OperationContract]
        [ApiPermission(PermissionCode = WebApiSettings.PERMISSION_REPORTING)]
        ReportResponse<VoidRejectHierarchySummary> GetVoidsRejectsSummary(GenericReportFilter req);

        [OperationContract]
        [ApiPermission(PermissionCode = WebApiSettings.PERMISSION_REPORTING)]
        ReportResponse<VoidRejectDetail> GetVoidsRejectsDetail(MerchantSummaryFilter req);
    }
}
