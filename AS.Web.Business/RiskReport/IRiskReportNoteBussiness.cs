using AS.Controls.Grid;
using AS.Security.WS.Entities;
using AS.Web.Business.RiskReport.Models;
using System.Data;

namespace AS.Web.Business.RiskReport
{
    public interface IRiskReportNoteBussiness
    {
        ASFuncInvoker GetRiskReportNotes(GetRiskReportNoteRequest request, User currentUser);
        bool DeleteRiskReportNote(DeleteRiskReportNoteRequest request, User currentUser);
        DataTable GetDetailRiskReportNote(GetDetailRiskReportNoteRequest request, User currentUser);
        bool UpdateRiskReportNote(UpdateRiskReportNoteRequest request, User currentUser);
    }
}
