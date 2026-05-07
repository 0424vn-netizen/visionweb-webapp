using AS.Common.DBManager;
using AS.Security.WS.Entities;
using AS.Web.Business.PauseMerchantAlert.Interfaces;
using AS.Web.Business.PauseMerchantAlert.Models;
using System.Data;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System;

namespace AS.Web.Business.PauseMerchantAlert.Impl
{
    public class PauseMerchantAlertBusiness : IPauseMerchantAlertBusiness
    {
        private readonly IReportServices _reportService;

        public PauseMerchantAlertBusiness(IReportServices service)
        {
            _reportService = service;
        }

        public DataTable GetPauseMerchantAlertFilters(string userMode, User currentUser, string assignmentId)
        {
            FilterParameterCollection parameters = new FilterParameterCollection()
            {
                new FilterParameter("@UserMode", userMode, DbType.AnsiString),
                new FilterParameter("@UserID", currentUser.UserID, DbType.AnsiString),
                new FilterParameter("@ASClient", currentUser.ASClient, DbType.Int32),
                new FilterParameter("@SiteID", currentUser.SiteID, DbType.Int32),
                new FilterParameter("@AssignmentID", int.Parse(assignmentId), DbType.Int32),
            };
            DataTable dt = _reportService.GetReports("spa_RM_MCF_Get_Assignment_PauseDateRange", parameters);
            return dt;
        }

        public int DeleteAssignmentPauseDateRange(string userMode, User currentUser, string assignmentId, string rowGuid)
        {
            FilterParameterCollection parameters = new FilterParameterCollection()
            {
                new FilterParameter("@UserMode", userMode, DbType.AnsiString),
                new FilterParameter("@UserID", currentUser.UserID, DbType.AnsiString),
                new FilterParameter("@ASClient", currentUser.ASClient, DbType.Int32),
                new FilterParameter("@SiteID", currentUser.SiteID, DbType.Int32),
                new FilterParameter("@AssignmentID", int.Parse(assignmentId), DbType.Int32),
                new FilterParameter("@RowGUID", rowGuid, DbType.String)
            };
            _ = new FilterParameterCollection();
            return _reportService.ExecuteNonQueryCommand("spa_RM_MCF_Delete_Assignment_PauseDateRange", parameters, out _);
        }

        public DataTable GetAllMerchants(string merchantIdList, string userMode, User currentUser)
        {
            if (string.IsNullOrEmpty(merchantIdList))
            {
                merchantIdList = "";
            }

            FilterParameterCollection parameters = new FilterParameterCollection()
            {
                new FilterParameter("@UserMode", userMode, DbType.AnsiString),
                new FilterParameter("@UserID", currentUser.UserID, DbType.AnsiString),
                new FilterParameter("@ASClient", currentUser.ASClient, DbType.Int32),
                new FilterParameter("@SiteID", currentUser.SiteID, DbType.Int32),
                new FilterParameter("@MIDList", merchantIdList, DbType.String),
            };
            DataTable dt = _reportService.GetReports("spa_RM_MCF_GetMerchantList", parameters);
            return dt;
        }

        public int SaveAssignmentPauseDateRange(string userMode, User currentUser, string assignmentId, string data)
        {
            FilterParameterCollection parameters = new FilterParameterCollection()
            {
                new FilterParameter("@UserMode", userMode, DbType.AnsiString),
                new FilterParameter("@UserID", currentUser.UserID, DbType.AnsiString),
                new FilterParameter("@ASClient", currentUser.ASClient, DbType.Int32),
                new FilterParameter("@SiteID", currentUser.SiteID, DbType.Int32),
                new FilterParameter("@AssignmentID", int.Parse(assignmentId), DbType.Int32),
                new FilterParameter("@Data", data, DbType.String)
            };
            _ = new FilterParameterCollection();
            return _reportService.ExecuteNonQueryCommand("spa_RM_MCF_Save_Assignment_PauseDateRange", parameters, out _);
        }         

        private DataTable FilterMerchant(string userMode, User currentUser, string merchantName, int pageNo, int pageSize, bool isCountPageTotal = false)
        {
            FilterParameterCollection parameters = new FilterParameterCollection()
            {
                new FilterParameter("@UserMode", userMode, DbType.AnsiString),
                new FilterParameter("@UserID", currentUser.UserID, DbType.AnsiString),
                new FilterParameter("@ASClient", currentUser.ASClient, DbType.Int32),
                new FilterParameter("@SiteID", currentUser.SiteID, DbType.Int32),
                new FilterParameter("@MerchantName", merchantName, DbType.String),
                new FilterParameter("@IsCountPageTotal", isCountPageTotal, DbType.Boolean),
                new FilterParameter("@PageNo", pageNo, DbType.Int32),
                new FilterParameter("@PageSize", pageSize, DbType.Int32),
                new FilterParameter("@IsPaging", true, DbType.Boolean)
            };
            DataTable dt = _reportService.GetReports("spa_RM_MCF_GetMerchantList", parameters);
            return dt;
        }

        public PauseMerchantAlertResponse GetMerchantFilters(string userMode, User currentUser, string customfilterstring = null)
        {
            try
            {
                RequestObject requestObject = JsonConvert.DeserializeObject<RequestObject>(customfilterstring);
                if (requestObject.Filter != null && requestObject.Filter.Filters != null && requestObject.Filter.Filters.Count > 0 && requestObject.Filter.Filters[0].Value.Length >= 3)
                {
                    var data = FilterMerchant(userMode, currentUser, requestObject.Filter.Filters[0].Value, requestObject.Page, requestObject.PageSize);
                    List<MerchantRefModel> merchantList = new List<MerchantRefModel>();
                    int totalRows = 0;
                    int selectedDataKeys = 0;
                    if (data.Rows != null && data.Rows.Count > 0)
                    {
                        foreach (DataRow row in data.Rows)
                        {
                            if (requestObject.SelectedDataKeys.Contains(row.Field<string>("DataKey")))
                            {
                                selectedDataKeys++;
                            }
                            else
                            {
                                var merchantRef = new MerchantRefModel()
                                {
                                    DataKey = row.Field<string>("DataKey"),
                                    DataText = row.Field<string>("DataText")
                                };
                                merchantList.Add(merchantRef);
                            }
                        }

                        merchantList = data.AsEnumerable().Select(row => new MerchantRefModel
                        {
                            DataKey = row.Field<string>("DataKey"),
                            DataText = row.Field<string>("DataText")
                        }).ToList();
                    }

                    var total = FilterMerchant(userMode, currentUser, requestObject.Filter.Filters[0].Value, requestObject.Page, requestObject.PageSize, true);
                    if (data.Rows != null && data.Rows.Count > 0)
                    {
                        totalRows = total.Rows[0].Field<int>("TotalRows");
                        totalRows = totalRows - selectedDataKeys;
                    }

                    return new PauseMerchantAlertResponse(merchantList.ToArray(), totalRows);
                }

                var emptyList = new List<MerchantRefModel>();
                return new PauseMerchantAlertResponse(emptyList.ToArray(), 0);
            }
            catch (Exception ex)
            {                
                return new PauseMerchantAlertResponse(ex.Message);
            }
        }
    }



}