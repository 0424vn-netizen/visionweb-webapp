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
	public partial class SponsoredEntityPortfolioSummary : ReportPage
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

		#region Grid events
		protected override void DoItemDataBound(ASGrid sender, GridItemEventArgs e)
		{
			if (sender == uxSponsoredEntityGrid && e.Item is GridDataItem)
			{
				GridDataItem dataItem = e.Item as GridDataItem;
				DataRowView dataRow = e.Item.DataItem as DataRowView;
				var sponsorCess = dataItem["SponsoredNumber"];
				var uxGotoSubsite = sponsorCess.FindControl("uxGotoSubsite");
				var uxSponsoredEntityName = sponsorCess.FindControl("uxSponsoredEntityName");
				var hasPermission = GeneralFuncsLib.HasUserPermission(WebSiteConstants.SEC_PERMISSION_JSACCESS);
				var entityId = dataRow["SponsoredNumber"].ToString();
				var entityName = dataRow["SponsoredName"].ToString();
				if (uxSponsoredEntityName != null)
				{
					if (hasPermission)
					{
						var encUserID = CryptorServices.Current.EncryptText(entityId);
						var button = string.Format("<a href=\"#\" type=\"button\" value=\"{0}\" onclick=\"siteJumpClick(this,'{1}')\">{0}</a>", entityName, encUserID);
						((Literal)uxSponsoredEntityName).Text = button;
					}
					else
					{
						((Literal)uxSponsoredEntityName).Text = dataRow["SponsoredName"].ToString();
					}
				}				

				if (uxGotoSubsite != null)
				{
					var reportDate = DateTime.Parse(dataRow["ReportDate"].ToString());
					var queryString = BuildSubsiteAssignmentSummaryLink(true, entityId, reportDate);
					((System.Web.UI.HtmlControls.HtmlAnchor)uxGotoSubsite).HRef = queryString;
				}
			}
		}
		#endregion

		protected override void DoNeedExportConfig(UxExport sender, ExportConfig exportConfig)
		{
			base.DoNeedExportConfig(sender, exportConfig);
			string fileName = GeneralFuncsLib.FormatFileName(uxSponsoredEntityGrid.GridName);
			exportConfig.FileName = HttpUtility.UrlEncode(fileName);
		}
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				DataBindSponsorEntities();
				var initData = GetQueryStringData();
				if (initData != null && initData.HasQueryData)
				{

					uxBeginDate.SelectedDate = initData.BeginDate;
					uxEndDate.SelectedDate = initData.EndDate;

					if (initData.IsDaily)
					{
						uxDaily.Checked = true;
						uxEntityList_Data.SetSelectedValue(initData.EntityIds.Split(','));
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
				}
			}
			uxBeginDate.MaxDate = uxEndDate.MaxDate = DateTime.Now;
			IsBindDataOnLoad = true;
		}
		protected void uxSearchButton_OnClick(object sender, EventArgs e)
		{			
			uxSponsoredEntityGrid.Rebind();
		}

        #endregion Methods
        protected void uxSponsoredEntityGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
			var data = GetFilterData();
			uxExportuxSponsoredEntity.GridSubTitle = string.Format("{0} {1}", data.SubTitle, data.DateRangeTitle);

			var currentUser = SessionManager.CurrentUser;
			FilterParameterCollection parameters = new FilterParameterCollection();

			parameters.Add(new FilterParameter("@UserMode", GeneralFuncsLib.GetUserMode(), DbType.AnsiString));
			parameters.Add(new FilterParameter("@UserID", currentUser.UserID, DbType.AnsiString));
			parameters.Add(new FilterParameter("@ASClientID", currentUser.ASClient, DbType.Int32));
			parameters.Add(new FilterParameter("@ListSponsoredNumber", data.EntityIds, DbType.String));
			parameters.Add(new FilterParameter("@BeginDate", data.BeginDate, DbType.Date));
			parameters.Add(new FilterParameter("@EndDate", data.EndDate, DbType.Date));
			uxSponsoredEntityGrid.DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, "GetReports", new object[] { "spa_RM_MCF_GetSponsoredEntityPortfolioSummary", ReportServices.ConvertToFilterParamWSArray(parameters) });
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

			var sponsors = WebServices.CsReportServices.GetReports("spa_cs_Dashboard_Portfolio_GetHierarchyInformation", parameters);
			return sponsors;
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

			var alertedPercentage = uxSponsoredEntityGrid.Columns.FindByUniqueName("AlertedPercentage");
			alertedPercentage.HeaderText = alertedPercentage.HeaderTooltip;

			var alertedNetAmount = uxSponsoredEntityGrid.Columns.FindByUniqueName("AlertedNetAmount");
			alertedNetAmount.HeaderText = alertedNetAmount.HeaderTooltip;			

			var workedCount = uxSponsoredEntityGrid.Columns.FindByUniqueName("WorkedCount");
			workedCount.HeaderText = workedCount.HeaderTooltip;

			var workedPercentage = uxSponsoredEntityGrid.Columns.FindByUniqueName("WorkedPercentage");
			workedPercentage.HeaderText = workedPercentage.HeaderTooltip;

			var workedNetAmount = uxSponsoredEntityGrid.Columns.FindByUniqueName("WorkedNetAmount");
			workedNetAmount.HeaderText = workedNetAmount.HeaderTooltip;

			var casesOpenedCount = uxSponsoredEntityGrid.Columns.FindByUniqueName("CasesOpenedCount");
			casesOpenedCount.HeaderText = casesOpenedCount.HeaderTooltip;

			var casesClosedCount = uxSponsoredEntityGrid.Columns.FindByUniqueName("CasesClosedCount");
			casesClosedCount.HeaderText = casesClosedCount.HeaderTooltip;

			var filterData = GetFilterData();
			var reportHeader = exportConfig.ReportHeader;
			exportConfig.ReportHeader = reportHeader + Environment.NewLine + string.Format("{0} {1}", filterData.SubTitle, filterData.DateRangeTitle);
			exportConfig.FileName = string.Format("{0}-{1}_{2}", reportHeader, filterData.SubTitle, filterData.DateRangeTitle);
		}
		private string BuildSubsiteAssignmentSummaryLink(bool isDaily, string entityId, DateTime reportDate)
		{
			var filterData = GetFilterData();
			var url = new StringBuilder();
			url.AppendFormat("isDaily={0}", isDaily);
			url.AppendFormat("&beginDate={0}", reportDate.ToString("yyyy/MM/dd"));
			url.AppendFormat("&endDate={0}", reportDate.ToString("yyyy/MM/dd"));
			url.AppendFormat("&entityIds={0}", entityId);

			//currentFilter
			url.AppendFormat("&sm_isDaily={0}", filterData.IsDaily);
			url.AppendFormat("&sm_beginDate={0}", filterData.BeginDate.ToString("yyyy/MM/dd"));
			url.AppendFormat("&sm_endDate={0}", filterData.EndDate.ToString("yyyy/MM/dd"));
			url.AppendFormat("&sm_entityIds={0}", filterData.EntityIds);

			var queryString = BuildSecureQueryString(url.ToString());
			return "SubsiteAssignmentSummary.aspx?" + queryString;
		}
		private SponsoredEntitySummaryViewModel GetFilterData()
		{
			var data = new SponsoredEntitySummaryViewModel()
			{
				BeginDate = uxBeginDate.SelectedDate.Value,
				IsDaily = uxDaily.Checked
			};

			if (uxDateRange.Checked)
			{
				data.EndDate = uxEndDate.SelectedDate.Value;
				data.EntityIds = uxEntityList.SelectedValue;
				data.SubTitle = uxEntityList.SelectedItem.Text;
				data.DateRangeTitle = string.Format("({0} - {1})", data.BeginDate.ToGenericDateString(), data.EndDate.ToGenericDateString());
			}
			else
			{
				var selectedItems = uxEntityList_Data.SelectedItems.Cast<ListItem>().ToList();
				if (selectedItems.Count > 1)
				{
					data.SubTitle = GetLocalResourceObject("MultipleSponsoredEntities").ToString();
				}
				else
				{
					data.SubTitle = string.Join(",", selectedItems.Select(x => x.Text).ToList());
				}

				data.EndDate = data.BeginDate;
				data.EntityIds = string.Join(",", selectedItems.Select(x => x.Value).ToList());
				data.DateRangeTitle = string.Format("({0})", data.BeginDate.ToGenericDateString());
			}

			if (data.EntityIds.Equals("-1"))
			{
				data.EntityIds = string.Empty;
				data.SubTitle = GetLocalResourceObject("AllSponsoredEntities").ToString();
			}

			return data;
		}
		private SponsoredEntitySummaryViewModel GetQueryStringData()
		{
			if (IsSecureQueryString)
			{
				var entityIds = SecureQueryString["entityIds"].ToString();
				var data = new SponsoredEntitySummaryViewModel()
				{
					IsDaily = SecureQueryString["isDaily"].Equals("true", StringComparison.OrdinalIgnoreCase),
					EntityIds = !string.IsNullOrEmpty(entityIds)? entityIds: "-1",
					HasQueryData = true
				};

				DateTime beginDate;
				var beginDateStr = SecureQueryString["beginDate"].ToString();
				
				if (!string.IsNullOrEmpty(beginDateStr) && DateTime.TryParse(beginDateStr, out beginDate))
					data.BeginDate = beginDate;
				else
					data.BeginDate = DateTime.Today;

				DateTime endDate;
				var endDateStr = SecureQueryString["endDate"].ToString();
				
				if (!string.IsNullOrEmpty(beginDateStr) && DateTime.TryParse(endDateStr, out endDate))
					data.EndDate = endDate;
				else
					data.EndDate = DateTime.Today;

				return data;
			}
			else
			{
				return new SponsoredEntitySummaryViewModel()
				{
					IsDaily = true,
					BeginDate = DateTime.Now,
					EndDate = DateTime.Now,
					EntityIds = "-1",
				};
			}
		}
		[WebMethod(EnableSession = true)]
		public static string GetSiteJumpUrl(string encUser)
		{
			if (string.IsNullOrEmpty(encUser))
				return string.Empty;

			string userId = CryptorServices.Current.DecryptText(encUser);

			if (string.IsNullOrEmpty(userId))
				return string.Empty;

			var hasPermission = GeneralFuncsLib.HasUserPermission(WebSiteConstants.SEC_PERMISSION_JSACCESS);
			if (!hasPermission)
				return string.Empty;

			var url = MerchantProfileHelper.CreateSiteJumpLink(userId);
			return url;
		}
		class SponsoredEntitySummaryViewModel
		{
			public bool IsDaily { get; set; }
			public DateTime BeginDate { get; set; }
			public DateTime EndDate { get; set; }
			public string EntityIds { get; set; }
			public string SubTitle { get; set; }
			public string DateRangeTitle { get; set; }
			public bool HasQueryData { get; set; }
		}
	}
}

