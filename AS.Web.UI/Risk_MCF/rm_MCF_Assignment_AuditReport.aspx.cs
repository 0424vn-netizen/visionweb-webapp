using AS.Common;
using AS.Common.DBManager;
using AS.Controls.Grid;
using AS.Web.Business;
using System;
using System.Data;
using AS.Web.UI.Controls;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using AS.Controls.Pages;
using Telerik.Web.UI;

[PagePermission("AssignmentAuditReport,MSAssignmentAuditReport")]
public partial class rm_MCF_Assignment_AuditReport : ReportPage
{
	#region  ---- Enum ----
	enum PostBackAction
	{
		DoSearching,
	}
	enum DataBindAction
	{
		BindMessageGrid,
		BindAssignmentList,
		BindChangedByList,
	}
	#endregion  ---- Enum ----
	#region ---- Properties ----
	private HierarchyFilterValue _reportValue = null;
	public int AssignmentID
	{
		get
		{
			int assignmentId = 0;
			if (SecureQueryString.IsNotNullData() && !SecureQueryString["assignmentId"].IsNullOrEmpty())
			{
				int.TryParse(SecureQueryString["assignmentId"].ToString(), out assignmentId);
			}
			return assignmentId;
		}
	}
	private string UserChangedBy { get; set; }
	private DateTime? UpdateDate { get; set; }
	private bool IsSearch = false;
	#endregion ---- Properties ----
	#region ---- Event & Protected ----
	protected void Page_Load(object sender, EventArgs e)
	{
		if (IsIntruderDetected) return;
		IsBindDataOnLoad = true;
		if (AssignmentID != 0)
		{
			((MasterPageNormal)Page.Master).HideHeaderMenu = true;
		}
		if (!IsPostBack)
		{
			if (AssignmentID != 0)
			{
				GetModifyInfo(AssignmentID);
			}
			else if (RiskSessionManager.MCF_DQ_CurrentAssignmentID != null && RiskSessionManager.MCF_DQ_CurrentAssignmentID != 0)
			{
				GetModifyInfo(RiskSessionManager.MCF_DQ_CurrentAssignmentID ?? 0);
			}
			GetDateFilter();
			SetDateFilter();
			OnDataBindControls(DataBindAction.BindAssignmentList);
			OnDataBindControls(DataBindAction.BindChangedByList);
		}
	}
	protected override void DoNeedExportConfig(AS.Controls.UserControls.UxExport sender, AS.Controls.Exporter.ExportConfig exportConfig)
	{
		base.DoNeedExportConfig(sender, exportConfig);
		string headerName = GetLocalResourceObject("uxExporter.GridTitle").ToString();
		if (sender.ExportButtonType == AS.Controls.UserControls.UxExport.ExportType.Excel)
		{
			exportConfig.AllowHtmlEncoded = false;
		}

		exportConfig.ReportHeader = headerName;
		exportConfig.FileName = HttpUtility.UrlEncode(GeneralFuncsLib.FormatFileName(GetLocalResourceObject("uxExporter.GridTitle").ToString()));
	}
	protected override void DoGridNeedDataSource(ASGrid sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
	{
		OnDataBindControls(DataBindAction.BindMessageGrid, sender);
	}
	protected override void OnDataBindControls(Enum type, object sender)
	{
		switch ((DataBindAction)type)
		{
			case DataBindAction.BindMessageGrid:
				{
					SetDateFilter();
					FilterParameterCollection _parames = new FilterParameterCollection();
                    _parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
                    _parames.Add(new FilterParameter("@DateFilterMode", (int)this._reportValue.DateOption, DbType.Int32));
					_parames.Add(new FilterParameter("@BeginDate", this._reportValue.DateOptionValue.From, DbType.Date));
					DateTime endDate = this._reportValue.DateOption == DateOptionMode.DateRange ? this._reportValue.DateOptionValue.To : this._reportValue.DateOptionValue.From;
					_parames.Add(new FilterParameter("@EndDate", endDate, DbType.Date));
					_parames.Add(new FilterParameter("@ChangedBys", string.Join(",", uxUserNameList.SelectedItems.Cast<ListItem>().Select(x => x.Value).ToArray()),
													  DbType.AnsiString));
					_parames.Add(new FilterParameter("@AssignmentIDs", string.Join(",", uxAssignmentList.SelectedItems.Cast<ListItem>().Select(x => x.Value).ToArray()),
													  DbType.AnsiString));
					//1.Contain,2.Equal
					_parames.Add(new FilterParameter("@FieldActionFilterType", uxRdEqual.Checked ? 2 : 1, DbType.Int32));
					_parames.Add(new FilterParameter("@FieldAction", EscapeSpecialCharacter(uxFieldAction.Text), DbType.AnsiString));
					_parames.AddLanguageID();
					if (IsSearch)
					{
						uxReportGrid.MasterTableView.CurrentPageIndex = 0;
					}
					((ASGrid)sender).DataSourceInvoker = new ASFuncInvoker(WebServices.RiskServices, WebSiteConstants.GET_REPORT_METHOD_NAME, new object[] { "spa_RM_MCF_Get_AssignmentAuditReport", ReportServices.ConvertToFilterParamWSArray(_parames) });

					uxExporter.GridTitle = GetLocalResourceObject("uxExporter.GridTitle").ToString();
				}
				break;
			case DataBindAction.BindAssignmentList:
				{
					FilterParameterCollection parameters = new FilterParameterCollection();
					parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
					this.uxAssignmentList.DataTextField = "AssignmentName";
					this.uxAssignmentList.DataValueField = "AssignmentID";
					this.uxAssignmentList.DataSource = WebServices.RiskServices.GetReports("spa_RM_MCF_AR_GetAssignmentList", parameters);
					this.uxAssignmentList.DataBind();
					if (AssignmentID != 0)
					{
						this.uxAssignmentList.SelectedValue = AssignmentID.ToString();
					}
					else if (RiskSessionManager.MCF_DQ_CurrentAssignmentID != null && RiskSessionManager.MCF_DQ_CurrentAssignmentID != 0)
					{
						this.uxAssignmentList.SelectedValue = RiskSessionManager.MCF_DQ_CurrentAssignmentID.ToString();
					}
				}
				break;
			case DataBindAction.BindChangedByList:
				{
					FilterParameterCollection parameters = new FilterParameterCollection();
					parameters.Add(new FilterParameter("@UserName", "", DbType.String));
					parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
					this.uxUserNameList.DataTextField = "UserNameFull";
					this.uxUserNameList.DataValueField = "RecId";
					this.uxUserNameList.DataSource = WebServices.RiskServices.GetReports("spa_RM_MCF_Audit_GetUsers", parameters);
					this.uxUserNameList.DataBind();
					if (!string.IsNullOrEmpty(UserChangedBy))
					{
						this.uxUserNameList.SelectedValue = UserChangedBy;
					}
				}
				break;
		}
	}
	protected override void OnPostBackActions(Enum type, object sender)
	{
		switch ((PostBackAction)type)
		{
			case PostBackAction.DoSearching:
				{
					uxReportGrid.Rebind();
				}
				break;
		}
	}
	protected void uxSearch_Click(object sender, EventArgs e)
	{
		IsSearch = true;
		OnPostBackActions(PostBackAction.DoSearching);
		IsSearch = false;
	}
	protected void uxReportGrid_ItemDataBound(object sender, GridItemEventArgs e)
	{
		if (e.Item is GridDataItem)
		{
			GridDataItem dataItem = e.Item as GridDataItem;

			var rowItem = (e.Item.DataItem as DataRowView).Row;

			if (rowItem["NewValue"].IsNullOrEmpty())
			{
				dataItem["NewValue"].Text = Formatter.FormatDataToMDash(rowItem["NewValue"]).ToString();
			}
			if (rowItem["PreviousValue"].IsNullOrEmpty())
			{
				dataItem["PreviousValue"].Text = Formatter.FormatDataToMDash(rowItem["PreviousValue"]).ToString();
			}
		}
	}
	#endregion ---- Event & Protected ----
	#region ---- Private Methods ----
	/// <summary>
	/// Get Date from Session if not null and assign to control
	/// </summary>
	private void GetDateFilter()
	{
		if (UpdateDate != null)
		{

			this._reportValue = new HierarchyFilterValue();
			this._reportValue.DateOption = DateOptionMode.Daily;
			this._reportValue.DateOptionValue.To = UpdateDate ?? DateTime.Now;
			this._reportValue.DateOptionValue.From = UpdateDate ?? DateTime.Now;
		}
		else
			this._reportValue = SavedReportFilterValue;
		if (this._reportValue == null)
		{
			this._reportValue = new HierarchyFilterValue();
			this._reportValue.DateOption = DateOptionMode.Daily;
			this._reportValue.DateOptionValue.To = DateTime.Now;
			this._reportValue.DateOptionValue.From = DateTime.Now;
		}
		switch (_reportValue.DateOption)
		{
			case DateOptionMode.Daily:
				this.uxDaily.Checked = true;
				this.uxDate.SelectedDate = this._reportValue.DateOptionValue.From;
				break;
			case DateOptionMode.Monthly:
				this.uxMonthly.Checked = true;
				this.uxDate.SelectedDate = this._reportValue.DateOptionValue.From;
				break;
			case DateOptionMode.DateRange:
				this.uxRange.Checked = true;
				this.uxFromDate.SelectedDate = this._reportValue.DateOptionValue.From;
				this.uxEndDate.SelectedDate = this._reportValue.DateOptionValue.To;
				break;
		}
	}
	/// <summary>
	///  Set Date to Session
	/// </summary>
	private void SetDateFilter()
	{
		//Get report filter form session
		if (SavedReportFilterValue != null)
			this._reportValue = SavedReportFilterValue;
		else
			this._reportValue = new HierarchyFilterValue();
		// Set Date Option         
		if (uxDaily.Checked)
		{
			this._reportValue.DateOption = DateOptionMode.Daily;
			this._reportValue.DateOptionValue.From = this._reportValue.DateOptionValue.To = uxDate.SelectedDate.Value;
		}
		else if (uxMonthly.Checked)
		{
			this._reportValue.DateOption = DateOptionMode.Monthly;
			this._reportValue.DateOptionValue.From = uxDate.SelectedDate.Value;
		}
		else
		{
			this._reportValue.DateOption = DateOptionMode.DateRange;
			this._reportValue.DateOptionValue.From = uxFromDate.SelectedDate.Value;
			this._reportValue.DateOptionValue.To = uxEndDate.SelectedDate.Value;
		}

		// Set report filter
		if (IsSearch)
			SavedReportFilterValue = this._reportValue;


	}
	/// <summary>
	/// Change special charater
	/// </summary>
	/// <param name="text">text need change</param>
	/// <returns></returns>
	private string EscapeSpecialCharacter(string text)
	{
		return text.Replace("[", "[[]").Replace("%", "[%]").Replace(",", "[,]").Replace("_", "[_]");
	}
	private void GetModifyInfo(long assignmentID)
	{
		FilterParameterCollection parameters = new FilterParameterCollection();
		parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
		parameters.Add(new FilterParameter("@AssignmentID", assignmentID, DbType.Int32));
		parameters.Add(new FilterParameter("@FilterMode", 0, DbType.Int32));
		DataTable dt = WebServices.RiskServices.GetReports("spa_RM_MCF_GetAssignmentInfo", parameters);
		if (dt != null && dt.Rows.Count > 0)
		{
			UserChangedBy = dt.Rows[0]["UserRecId"].ToASString();
			if (!string.IsNullOrEmpty(dt.Rows[0]["LastModifiedDate"].ToASString()))
				UpdateDate = dt.Rows[0]["LastModifiedDate"].ASConverter<DateTime>();
		}
	}
	#endregion ---- Private Methods ----
}
