using AS.Common;
using AS.Common.DBManager;
using AS.Common.Logger;
using AS.Controls.Exporter;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Controls.UserControls;
using AS.Web.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace As.VisionWeb.Web
{
    [PagePermission("PortfolioSummary")]
    public partial class SubsiteAssignmentSummary : ReportPage
    {
        #region Methods
        #region Base overrides
        protected override void PageInitialize()
        {
            if (IsIntruderDetected) return;
            this.GridIDs.Add("uxSponsoredEntityGrid");

            base.PageInitialize();
        }
        #endregion        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DataBindSponsorEntities();
                DataBindComboSubsiteAssignmentDaily();
                DataBindComboSubsiteAssignmentDateRange();

                var initData = GetQueryStringData(false);
                if (initData != null && initData.HasQueryData)
                {
                    uxBeginDate.SelectedDate = initData.BeginDate;
                    uxEndDate.SelectedDate = initData.EndDate;

                    if (initData.IsDaily)
                    {
                        uxDaily.Checked = true;
                        uxEntityList_Data.SetSelectedValue(initData.EntityIds.Split(','));
                        uxSubsiteAssignment_Data.SelectedValue = initData.SubsiteIds;
                    }
                    else
                    {
                        uxDateRange.Checked = true;
                        uxEntityList.SelectedValue = initData.EntityIds;
                    }
                }
                else
                {
                    uxDaily.Checked = true;
                    uxBeginDate.SelectedDate = DateTime.Today;
                    uxEndDate.SelectedDate = DateTime.Today;
                    uxEntityList_Data.SelectedValue = "-1";
                    uxSubsiteAssignment_Data.SelectedValue = "-1";                    
                }
                BuildSponsoredEntitySumaryLink(true);
            }
            uxBeginDate.MaxDate = uxEndDate.MaxDate = DateTime.Now;
            IsBindDataOnLoad = true;
        }

        protected void uxSearchButton_OnClick(object sender, EventArgs e)
        {
            BuildSponsoredEntitySumaryLink(false);
            uxSponsoredEntityGrid.Rebind();
        }

        #endregion Methods
        protected void uxSponsoredEntityGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var data = GetFilterData();
            var currentUser = SessionManager.CurrentUser;
            uxExportuxSponsoredEntity.GridSubTitle = string.Format("{0} {1}", data.SubTitle, data.DateRangeTitle);
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.Add(new FilterParameter("@UserMode", GeneralFuncsLib.GetUserMode(), DbType.AnsiString));
            parameters.Add(new FilterParameter("@UserID", currentUser.UserID, DbType.AnsiString));
            parameters.Add(new FilterParameter("@ASClientID", currentUser.ASClient, DbType.Int32));
            parameters.Add(new FilterParameter("@ListSponsoredNumber", data.EntityIds != "-1" ? data.EntityIds : null, DbType.String));
            parameters.Add(new FilterParameter("@ListAssignmentID", data.SubsiteIds != "-1" ? data.SubsiteIds : null, DbType.String));
            parameters.Add(new FilterParameter("@BeginDate", data.BeginDate, DbType.Date));
            parameters.Add(new FilterParameter("@EndDate", data.EndDate, DbType.Date));
            uxSponsoredEntityGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, "GetReports",
                new object[] { "spa_RM_MCF_GetSubsiteAssignmentSummary", ReportServices.ConvertToFilterParamWSArray(parameters) });
        }
        private DataTable GetSponsor()
        {
            var currentUser = SessionManager.CurrentUser;
            FilterParameterCollection parameters = new FilterParameterCollection();

            parameters.Add(new FilterParameter("@UserMode", GeneralFuncsLib.GetUserMode(), DbType.AnsiString));
            parameters.Add(new FilterParameter("@UserID", currentUser.UserID, DbType.AnsiString));
            parameters.Add(new FilterParameter("@ASClientID", currentUser.ASClient, DbType.Int32));
            parameters.Add(new FilterParameter("@HierarchyTypeFilter", 3, DbType.Int16));

            if (currentUser.SiteID >= 0)
            {
                parameters.Add(new FilterParameter("@SiteId", currentUser.SiteID, DbType.Int16));
            }

            return WebServices.CsReportServices.GetReports("spa_cs_Dashboard_Portfolio_GetHierarchyInformation", parameters);
        }
        private DataTable GetSubsites(string filterValue = "")
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            parameters.Add(new FilterParameter("@AssignmentType", (int)WebSiteEnums.AssignmentType.Subsite, DbType.Int32));
            parameters.Add(new FilterParameter("@IsReskinSite", true, DbType.Boolean));
            parameters.Add(new FilterParameter("@FilterValueList", filterValue, DbType.String));
            return WebServices.RiskServices.GetReports("spa_RM_MCF_GetAssignmentList", parameters);

        }
        protected void uxLinkButton_Command(object sender, CommandEventArgs e)
        {
            var hasPermission = GeneralFuncsLib.HasUserPermission(WebSiteConstants.SEC_PERMISSION_JSACCESS);
            if (!string.IsNullOrEmpty(e.CommandArgument.ToString()) && !string.IsNullOrEmpty(e.CommandName.ToString()) && hasPermission)
            {
                var url = MerchantProfileHelper.CreateSiteJumpLink(e.CommandArgument.ToString());
                if (!string.IsNullOrEmpty(url))
                    ClientScript.RegisterStartupScript(GetType(), "startup", string.Format("window.open('{0}');", url), true);
                else
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "showMessage", "alert('" + GetLocalResourceObject("InvalidUserAccount").ToString() + "')", true);
            }
        }

        private void DataBindComboSubsiteAssignmentDaily(bool isReload = false)
        {
            var multiAssignmentNameSelected = uxSubsiteAssignment_Data.SelectedItems.Cast<ListItem>().ToList();
            uxSubsiteAssignment_Data.Items.Clear();

            var selectedItems = uxEntityList_Data.SelectedItems.Cast<ListItem>().ToList();
            string filterValue = string.Join(",", selectedItems.Select(x => x.Value).ToList());

            var subsites = GetSubsites(filterValue);
            uxSubsiteAssignment_Data.Items.Add(new ListItem("All", "-1".ToString()));

            foreach (DataRow dr in subsites.Rows)
            {
                var id = dr["AssignmentID"].ToString();
                string name = dr["AssignmentName"].ToString();
                uxSubsiteAssignment_Data.Items.Add(new ListItem(name, id));
            }

            string[] selectedValue = subsites.Rows.Count > 0 ? multiAssignmentNameSelected.Select(x => x.Value).ToArray() : new string[] { "-1"};
            uxSubsiteAssignment_Data.SetSelectedValue(selectedValue);

            if (isReload)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "doneChange", "regisMultiChooseChange();", true);
            }
        }

        private void DataBindComboSubsiteAssignmentDateRange()
        {
            var cbAssignmentNameSelected = uxSubsiteAssignment.SelectedValue;

            uxSubsiteAssignment.Items.Clear();
            
            var subsites = GetSubsites(uxEntityList.SelectedValue);

            foreach (DataRow dr in subsites.Rows)
            {
                var id = dr["AssignmentID"].ToString();
                string name = dr["AssignmentName"].ToString();
                uxSubsiteAssignment.Items.Add(new RadComboBoxItem(name, id));
            }

            uxSubsiteAssignment.SelectedValue = cbAssignmentNameSelected;
            if (subsites.Rows.Count == 0)
                uxSubsiteAssignment.Text = "";
        }

        private void DataBindSponsorEntities()
        {
            var sponsorData = GetSponsor();
            uxEntityList_Data.Items.Clear();
            uxEntityList_Data.Items.Add(new ListItem("All", "-1".ToString()));

            foreach (DataRow dr in sponsorData.Rows)
            {
                var name = dr["EntityName"].ToString();
                var number = dr["EntityNumber"].ToString();
                uxEntityList_Data.Items.Add(new ListItem(name, number));
                uxEntityList.Items.Add(new RadComboBoxItem(name, number));
            }
        }
        protected void uxExportuxSponsoredEntity_NeedExportConfig(object sender, ExportConfig exportConfig)
        {
            var alertedCount = uxSponsoredEntityGrid.Columns.FindByUniqueName("AlertedCount");
            alertedCount.HeaderText = alertedCount.HeaderTooltip;

            var alertedNetAmount = uxSponsoredEntityGrid.Columns.FindByUniqueName("AlertedNetAmount");
            alertedNetAmount.HeaderText = alertedNetAmount.HeaderTooltip;

            var readyToWorkCount = uxSponsoredEntityGrid.Columns.FindByUniqueName("ReadyToWorkCount");
            readyToWorkCount.HeaderText = readyToWorkCount.HeaderTooltip;

            var readyToWorkNetAmount = uxSponsoredEntityGrid.Columns.FindByUniqueName("ReadyToWorkNetAmount");
            readyToWorkNetAmount.HeaderText = readyToWorkNetAmount.HeaderTooltip;

            var workedCount = uxSponsoredEntityGrid.Columns.FindByUniqueName("WorkedCount");
            workedCount.HeaderText = workedCount.HeaderTooltip;

            var workedNetAmount = uxSponsoredEntityGrid.Columns.FindByUniqueName("WorkedNetAmount");
            workedNetAmount.HeaderText = workedNetAmount.HeaderTooltip;

            var filterData = GetFilterData();
            var reportHeader = exportConfig.ReportHeader;
            exportConfig.ReportHeader = reportHeader + Environment.NewLine + string.Format("{0} {1}", filterData.SubTitle, filterData.DateRangeTitle);
            exportConfig.FileName = string.Format("{0}-{1}_{2}", reportHeader, filterData.SubTitle, filterData.DateRangeTitle);
        }
        private SubsiteAssignmentSummaryViewModel GetFilterData()
        {
            var data = new SubsiteAssignmentSummaryViewModel()
            {
                BeginDate = uxBeginDate.SelectedDate.Value,
                SubsiteIds = uxSubsiteAssignment.SelectedValue,
                IsDaily = uxDaily.Checked
            };

            if (uxDateRange.Checked)
            {
                data.EndDate = uxEndDate.SelectedDate.Value;
                data.EntityIds = uxEntityList.SelectedValue;
                data.EntityTitle = uxEntityList.SelectedItem != null ? uxEntityList.SelectedItem.Text : string.Empty;
                data.SubsiteTitle = uxSubsiteAssignment.SelectedItem != null ? uxSubsiteAssignment.SelectedItem.Text : string.Empty;
                data.DateRangeTitle = string.Format("({0} - {1})", data.BeginDate.ToGenericDateString(), data.EndDate.ToGenericDateString());
            }
            else
            {
                var selectedItems = uxEntityList_Data.SelectedItems.Cast<ListItem>().ToList();
                var selectedSubsites = uxSubsiteAssignment_Data.SelectedItems.Cast<ListItem>().ToList();

                data.EntityTitle = selectedItems.Count > 1 ? GetLocalResourceObject("MultipleSponsoredEntities").ToString() : string.Join(",", selectedItems.Select(x => x.Text).ToList());
                data.SubsiteTitle = selectedSubsites.Count > 1 ? GetLocalResourceObject("MultipleSubsitesAssignments").ToString() : string.Join(",", selectedSubsites.Select(x => x.Text).ToList());
                data.EndDate = data.BeginDate;
                data.EntityIds = string.Join(",", selectedItems.Select(x => x.Value).ToList());
                data.SubsiteIds = string.Join(",", selectedSubsites.Select(x => x.Value).ToList());
                data.DateRangeTitle = string.Format("({0})", data.BeginDate.ToGenericDateString());
            }

            if (data.EntityIds.Equals("-1"))
            {
                data.EntityIds = null;
                data.EntityTitle = GetLocalResourceObject("AllSponsoredEntities").ToString();
            }

            if (data.SubsiteIds.Equals("-1"))
            {
                data.SubsiteIds = null;
                data.SubsiteTitle = GetLocalResourceObject("AllSubsitesAssignments").ToString();
            }

            return data;
        }
        private void BuildSponsoredEntitySumaryLink(bool fromHistory)
        {
            string urlRedirect;
            if (fromHistory)
            {
                var data = GetQueryStringData(true);
                if (data != null && data.HasQueryData)
                {
                    urlRedirect = BuildQueryString(data);
                }
                else
                {
                    urlRedirect = "SponsoredEntityPortfolioSummary.aspx";
                }
            }
            else
            {
                var data = GetFilterData();
                urlRedirect = BuildQueryString(data);
            }            

            uxLinkSponsoredEntityPortfolioSummary.HRef = urlRedirect;
        }
        private string BuildQueryString(SubsiteAssignmentSummaryViewModel data)
        {
            var url = new StringBuilder();
            url.AppendFormat("isDaily={0}", data.IsDaily);
            url.AppendFormat("&beginDate={0}", data.BeginDate);
            url.AppendFormat("&endDate={0}", data.EndDate);
            url.AppendFormat("&entityIds={0}", data.EntityIds);

            var queryString = BuildSecureQueryString(url.ToString());
            return "SponsoredEntityPortfolioSummary.aspx?" + queryString;
        }
        private SubsiteAssignmentSummaryViewModel GetQueryStringData(bool isMaster)
        {
            var key = isMaster ? "sm_" : string.Empty;
            if (IsSecureQueryString)
            {
                var entityIds = SecureQueryString[key + "entityIds"].ToString();
                var data = new SubsiteAssignmentSummaryViewModel()
                {
                    IsDaily = SecureQueryString[key + "isDaily"].Equals("true", StringComparison.OrdinalIgnoreCase),
                    EntityIds = !string.IsNullOrEmpty(entityIds)? entityIds: "-1",
                    SubsiteIds = "-1",
                    HasQueryData = true
                };

                DateTime beginDate;
                var beginDateStr = SecureQueryString[key + "beginDate"].ToString();
                if (!string.IsNullOrEmpty(beginDateStr) && DateTime.TryParse(beginDateStr, out beginDate))
                    data.BeginDate = beginDate;
                else
                    data.BeginDate = DateTime.Today;

                DateTime endDate;
                var endDateStr = SecureQueryString[key + "endDate"].ToString();
                if (!string.IsNullOrEmpty(beginDateStr) && DateTime.TryParse(endDateStr, out endDate))
                    data.EndDate = endDate;
                else
                    data.EndDate = DateTime.Today;

                return data;
            }
            else
            {
                return new SubsiteAssignmentSummaryViewModel()
                {
                    IsDaily = true,
                    BeginDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    EntityIds = "-1",
                    SubsiteIds = "-1"
                };
            }
        }
        class SubsiteAssignmentSummaryViewModel
        {
            public bool IsDaily { get; set; }
            public DateTime BeginDate { get; set; }
            public DateTime EndDate { get; set; }
            public string EntityIds { get; set; }
            public string SubsiteIds { get; set; }
            public string SubTitle { get { return string.Format("{0} - {1}", EntityTitle, SubsiteTitle); } }
            public string EntityTitle { get; set; }
            public string SubsiteTitle { get; set; }
            public string DateRangeTitle { get; set; }
            public bool HasQueryData { get; set; }
        }

        protected void uxEntityList_Data_TextChanged(object sender, EventArgs e)
        {
            DataBindComboSubsiteAssignmentDaily(true);
        }

        protected void uxEntityList_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            DataBindComboSubsiteAssignmentDateRange();
        }
    }
}

