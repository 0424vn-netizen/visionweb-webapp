using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Security.WS.Entities;
using AS.Web.Business.General;
using AS.Web.Business.RiskReport.Models;
using AS.Web.Business.Shared.Constants;
using System.Data;

namespace AS.Web.Business.RiskReport
{
    public class RiskReportNoteBussiness : IRiskReportNoteBussiness
    {
        private readonly IReportServices _reportService;
        public RiskReportNoteBussiness(IReportServices service)
        {
            _reportService = service;
        }

        public ASFuncInvoker GetRiskReportNotes(GetRiskReportNoteRequest request, User currentUser)
        {
            if (request == null || currentUser == null)
            {
                return null;
            }
            var parameters = new FilterParameterCollection
            {
                new FilterParameter("@MerchantNumber", request.MerchantNumber, DbType.AnsiString),
                new FilterParameter("@NotesSourceList", request.NotesSourceList, DbType.AnsiString),
                new FilterParameter("@RoleList", request.RoleList, DbType.AnsiString),
                new FilterParameter("@UserList", request.UserList, DbType.AnsiString),
                new FilterParameter("@IsHasMerchantProfile", request.IsHasMerchantProfile, DbType.Boolean),
                new FilterParameter("@IsHasRiskReport", request.IsHasRiskReport, DbType.Boolean),
                new FilterParameter("@IsHasOpenNewCase", request.IsHasOpenNewCase, DbType.Boolean),
                new FilterParameter("@IsHasOpenRiskCase", request.IsHasOpenRiskCase, DbType.Boolean),
                new FilterParameter("@IsHasShadowUnderwriting", request.IsHasShadowUnderwriting, DbType.Boolean),
                new FilterParameter("@stOrder", request.MerchantNoteSort, DbType.AnsiString)
            }
            .AddParamCurrentInUser(currentUser, request.UserMode)
            .AddParamLanguageId(request.HasMultiLanguageFeature, request.CurrentLanguage)
            .AddParamExport(request.IsExporting);

            return new ASFuncInvoker(_reportService
                , RiskReportNoteConstants.GET_REPORT_METHOD_NAME
                , new object[] { "spa_RM_MCF_Get_RiskReport_MerchantNote"
                , ReportServices.ConvertToFilterParamWSArray(parameters) });
        }

        public bool DeleteRiskReportNote(DeleteRiskReportNoteRequest request, User currentUser)
        {
            if (request == null || currentUser == null)
            {
                return false;
            }
            var paras = new FilterParameterCollection
            {
                new FilterParameter("@MerchantNoteID", request.MerchantNoteID, DbType.String),
                new FilterParameter("@MerchantNumber", request.MerchantNumber, DbType.String)
            }
            .AddParamCurrentInUser(currentUser, request.UserMode);
            _reportService.GetReports("spa_MerchantNotes_DeleteNote", paras);
            return true;
        }
        public DataTable GetDetailRiskReportNote(GetDetailRiskReportNoteRequest request, User currentUser)
        {
            if (request == null || currentUser == null)
            {
                return null;
            }
            var paras = new FilterParameterCollection
            {
                new FilterParameter("@MerchantNoteID", request.MerchantNoteID, DbType.String),
                new FilterParameter("@MerchantNumber", request.MerchantNumber, DbType.String)
            }
           .AddParamCurrentInUser(currentUser, request.UserMode);
            return _reportService.GetReports("spa_RM_MCF_GetDetail_RiskReport_MerchantNote", paras);
        }

        public bool UpdateRiskReportNote(UpdateRiskReportNoteRequest request, User currentUser)
        {
            if (request == null || currentUser == null)
            {
                return false;
            }
            string comment = GeneralFuncsLib.EncryptComment(request.Comment, request.HdCardDetected);
            string commentPlainText = GeneralFuncsLib.EncryptComment(request.CommentPlainText, request.HdCardDetected);
            var paras = new FilterParameterCollection
            {
                new FilterParameter("@MerchantNoteID", request.MerchantNoteID, DbType.String),
                new FilterParameter("@MerchantNumber", request.MerchantNumber, DbType.String),
                new FilterParameter("@Comment", comment, DbType.String),
                new FilterParameter("@CommentPlainText", commentPlainText, DbType.String)
            }
            .AddParamCurrentInUser(currentUser, request.UserMode);
            _reportService.GetReports("spa_MerchantNotes_UpdateNote", paras);
            return true;
        }        
    }
}
