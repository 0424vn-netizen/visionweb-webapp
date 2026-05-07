using AS.Common.DBManager;
using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.SessionState;

public class ExportQueueInsert : IHttpHandler, IReadOnlySessionState
{
    public void ProcessRequest(HttpContext context)
    {
        if (!context.User.Identity.IsAuthenticated)
        {
            context.Response.StatusCode = 401;
            return;
        }
        var form = context.Request.Form;

        string pageName = form["pageName"];
        string fileName = form["fileName"] ?? string.Empty;
        string fileType = form["fileType"];
        string filterParamsB64 = form["filterParams"] ?? string.Empty;
        string filterParams = filterParamsB64.Length > 0 ? System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(filterParamsB64)) : string.Empty;

        string spaName = null;
        int catCode = 0, subCatCode = 0;
        var lookupParams = new FilterParameterCollection();
        lookupParams.AddLoggedInUserReportingParams();
        lookupParams.Add(new FilterParameter("@PageName", pageName, DbType.AnsiString));
        DataTable dt = WebServices.RiskServices.GetReports("spa_ExportingService_GetExtractSubCategory", lookupParams);

        if (dt != null && dt.Rows.Count > 0)
        {
            var row = dt.AsEnumerable().FirstOrDefault();
            if (row != null)
            {
                spaName = row["SPAName"].ToString();
                catCode = row["CategoryCode"].ToInt();
                subCatCode = row["SubCategoryCode"].ToInt();
            }
        }

        if (string.IsNullOrEmpty(spaName) || string.IsNullOrEmpty(fileType))
        {
            context.Response.StatusCode = 400;
            return;
        }

        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams();
        parameters.Add(new FilterParameter("@FileName", fileName, DbType.AnsiString));
        parameters.Add(new FilterParameter("@FileType", fileType, DbType.AnsiString));
        parameters.Add(new FilterParameter("@FilterContent", filterParams ?? string.Empty, DbType.AnsiString));
        parameters.Add(new FilterParameter("@CategoryCode", catCode, DbType.Int32));
        parameters.Add(new FilterParameter("@SubCategoryCode", subCatCode, DbType.Int32));
        parameters.Add(new FilterParameter("@SPAName", spaName, DbType.AnsiString));

        WebServices.CsReportServices.GetReports("spa_ExportingService_InsertProcessQueue", parameters);

        context.Response.ContentType = "application/json";
        context.Response.Write("{\"success\":true}");
    }

    public bool IsReusable { get { return false; } }
}