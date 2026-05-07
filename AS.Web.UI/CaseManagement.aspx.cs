using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AS.Common.DBManager;
using AS.Common;
using System.Collections.Generic;
using Telerik.Web.UI;
using System.Web.Services;
using AS.Controls.Grid;
using AS.Web.Business;
using System.Text;
using AS.Controls.Pages;
using AS.Web.UI.Controls;

[PagePermission("ManCase,MSManCase")]
public partial class CaseManagement : ReportPage
{
    #region Enums
    enum DataBindAction
    {
        BindCommentList
    }
    enum PostBackAction
    {
        Submit
    }
    #endregion

    #region Properties
    private DataTable _dtTreeCS, _dtTreeAO, _dtTreeTD;
    string _sourceName = string.Empty;
    string _keyName = string.Empty;
    const string STATUS_OPEN = "Open";
    const string STATUS_CLOSED = "Closed";
    const string STATUS_PENDING = "Left Message";
    const string STATUS_WORKING = "Working";
    const string STATUS_NEW = "New";
    
    #endregion

    private void ProcessQueryString()
    {
        _sourceName = SecureQueryString["Source"];
        _keyName = SecureQueryString["Key"];

        string ticketID = SecureQueryString["TicketNumber"];
        string merchantNumber = SecureQueryString["MerchantNumer"];

        //CheckDataIntruders(_sourceName, _keyName.Split(','), new object[] { merchantNumber, ticketID });
    }

    protected override void PageInitialize()
    {
        this.GridIDs.Add("uxCommentList");
        base.PageInitialize();
        IsBindDataOnLoad = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        if (!IsSecureQueryString)
        {
            Response.Redirect("~/CaseSearch.aspx", true);
            return;
        }

        if (!IsPostBack)
        {
            if (SecureQueryString["isMe"] != "yes")
                ProcessQueryString();

            //Set session hierarchyfilter
            HierarchyFilterValue reportFiler = new HierarchyFilterValue();// SavedReportFilterValue;

            if (SavedReportFilterValue != null)
            {
                reportFiler.DateOption = SavedReportFilterValue.DateOption;// DateOptionMode.DateRange;
                reportFiler.DateOptionValue.From = SavedReportFilterValue.DateOptionValue.From;// DateTime.Now.GetFirstDayOfMonth();
                reportFiler.DateOptionValue.To = SavedReportFilterValue.DateOptionValue.To;// DateTime.Now;
                reportFiler.HierarchyMode = GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyMode;// GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyMode;
                reportFiler.ID = GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyID;// GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyID;
                reportFiler.Value = SecureQueryString["MerchantNumber"];// string.Empty;                            
            }
            else
            {
                reportFiler = new HierarchyFilterValue();
                reportFiler.DateOption = DateOptionMode.DateRange;
                reportFiler.DateOptionValue.From = DateTime.Now.GetFirstDayOfMonth();
                reportFiler.DateOptionValue.To = DateTime.Now;
                reportFiler.HierarchyMode = GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyMode;
                reportFiler.ID = GeneralFuncsLib.GetMerchantHierarchyInfo().HierarchyID;
                reportFiler.Value = SecureQueryString["MerchantNumber"];
            }

            SavedReportFilterValue = new HierarchyFilterValue();
            SavedReportFilterValue = reportFiler;
            uxOpenDate.Text = VeraCodeSolution.DoVeraCode(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"));

            uxTreeCS.DataTextField = uxTreeAO.DataTextField = uxTreeTD.DataTextField = "IssueName";
            uxTreeCS.DataValueField = uxTreeAO.DataValueField = uxTreeTD.DataValueField = "IssueID";
            uxTreeCS.DataFieldID = uxTreeAO.DataFieldID = uxTreeTD.DataFieldID = "GroupID";
            uxTreeCS.DataFieldParentID = uxTreeAO.DataFieldParentID = uxTreeTD.DataFieldParentID = "ParentGroupID";

            if (SecureQueryString["TicketNumber"] == null)  // for Create case, generate the new ticket ID
            {
                uxCommentList.Visible = false;
                uxTicketNumber.Text = VeraCodeSolution.DoVeraCode(GenerateTicketNumber());
            }
            else  // for Edit case, update the Issue's status
            {
                uxTicketNumber.Text = VeraCodeSolution.DoVeraCode(SecureQueryString["TicketNumber"]);
                uxCommentList.Visible = true;
            }
            SwithMerchantInfo();
            BindData();
        }
    }

    private void SwithMerchantInfo()
    {
        string spName = "spa_cs_GetBEProcessor";
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32));
        parameters.Add(new FilterParameter("@MerchantNumber",SecureQueryString["MerchantNumber"],DbType.String));
        parameters.Add(new FilterParameter("@SiteID", int.Parse(SecureQueryString["SiteID"]), System.Data.DbType.Int32));
        DataTable table = WebServices.CsReportServices.GetReports(spName, parameters);
        if (table != null && table.Rows.Count > 0)
        {
            string beProcessor = table.Rows[0]["BEProcessor"].ToString();
            if (beProcessor.Equals("FDR")) // Show FDR columns
            {
                uxMerchantInfo_FDR.Visible = true;
                uxMerchantInfo_PLANET.Visible = false;
                uxMerchantInfo_TSYS.Visible = false;
                uxMerchantInfo_ORION.Visible = false;
                uxMerchantInfo_FDR.Rebind();
                return;
            }
            else if (beProcessor.Equals("TSYS")) //   Show TSYS columns
            {
                uxMerchantInfo_FDR.Visible = false;
                uxMerchantInfo_PLANET.Visible = false;
                uxMerchantInfo_TSYS.Visible = true;
                uxMerchantInfo_ORION.Visible = false;
                uxMerchantInfo_TSYS.Rebind();
                return;
            }
            else if (beProcessor.Equals("PLANET")) //   Show TSYS columns
            {
                uxMerchantInfo_FDR.Visible = false;
                uxMerchantInfo_PLANET.Visible = true;
                uxMerchantInfo_TSYS.Visible = false;
                uxMerchantInfo_ORION.Visible = false;
                uxMerchantInfo_PLANET.Rebind();
                return;
            }
            else if (beProcessor.Equals("GLOBAL")) //   Show TSYS columns
            {
                uxMerchantInfo_FDR.Visible = false;
                uxMerchantInfo_PLANET.Visible = false;
                uxMerchantInfo_TSYS.Visible = false;
                uxMerchantInfo_ORION.Visible = true;
                uxMerchantInfo_ORION.Rebind();
                return;
            }
            else // Not show
            {
                uxMerchantInfo_FDR.Visible = false;
                uxMerchantInfo_PLANET.Visible = false;
                uxMerchantInfo_TSYS.Visible = false;
                uxMerchantInfo_ORION.Visible = false;
            }
        }
    }

    protected override void OnDataBindControls(Enum type, object sender)
    {
        switch ((DataBindAction)type)
        {
            case DataBindAction.BindCommentList:
            {
                FilterParameterCollection parameters = new FilterParameterCollection();
                parameters.AddLoggedInUserParams(-1);
                if (SecureQueryString["SiteID"] != null)
                    parameters.Add(new FilterParameter("@SiteID", SecureQueryString["SiteID"], DbType.Int32));

                if (SecureQueryString["TicketNumber"] != null)
                    parameters.Add(new FilterParameter("@TicketNumber", int.Parse(SecureQueryString["TicketNumber"]), DbType.Int32));

                string spa = "spa_cm_GetCommentForTicketCaseManagement";
                if (uxCommentList.AS_SortExpression.Trim() == string.Empty)
                {
                    uxCommentList.AS_SortExpression = "CreatedDate DESC";
                }
                uxCommentList.DataSourceInvoker = new ASFuncInvoker(WebServices.CsReportServices, "GetReports", new object[] { spa, ReportServices.ConvertToFilterParamWSArray(parameters) });
            }
            break;
        }
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        if (IsIntruderDetected) return;
        switch ((PostBackAction)type)
        {
            case PostBackAction.Submit:
                string comment = uxCommentText.Text.Trim();
                if (comment.Length > 7000)  // Check max length to 7000 characters
                    return;
                FilterParameterCollection parameters = new FilterParameterCollection();
                var issueList = new StringBuilder();
                IList<RadTreeNode> nodeCollection = uxTreeCS.CheckedNodes;
                foreach (RadTreeNode node in nodeCollection)
                {
                    if (node.Nodes.Count == 0)
                    {
                        issueList.Append(node.Value + ",");
                    }
                }
                nodeCollection = uxTreeAO.CheckedNodes;
                foreach (RadTreeNode node in nodeCollection)
                {
                    if (node.Nodes.Count == 0)
                    {
                        issueList.Append(node.Value + ",");
                    }
                }
                nodeCollection = uxTreeTD.CheckedNodes;
                foreach (RadTreeNode node in nodeCollection)
                {
                    if (node.Nodes.Count == 0)
                    {
                        issueList.Append(node.Value + ",");
                    }
                }

                // Manage Ticket ------------------------------------------------
                // If Status is Closed, we will show the pop-up and ask user for confirmation
                if (uxStatus.Text != STATUS_CLOSED)
                {
                    string spa = string.Empty;
                    string mode;
                    FilterParameterCollection tempParameters = null;
                    parameters.AddLoggedInUserParams(-1);
                    parameters.Add(new FilterParameter("@AssignedTo", uxUserAssigned.SelectedValue, DbType.AnsiString));
                    parameters.Add(new FilterParameter("@TicketNumber", int.Parse(uxTicketNumber.Text), DbType.Int32));
                    parameters.Add(new FilterParameter("@SiteID", int.Parse(SecureQueryString["SiteID"]), DbType.Int32));

                    if (SecureQueryString["TicketNumber"] != null) // for Edit case, we will update more information
                    {
                        parameters.Add(new FilterParameter("@Status", int.Parse(uxStatus.SelectedValue), DbType.Int32));
                        parameters.Add(new FilterParameter("@Resolution", int.Parse(uxResolution.SelectedValue), DbType.Int32));
                        spa = "spa_cm_UpdateTicketCaseManagement";
                        mode = "edit";
                    }
                    else
                    {
                        spa = "spa_cm_InsertTicketCaseManagement";
                        //parameters.Add(new FilterParameter("@SiteID", int.Parse(SecureQueryString["SiteID"]), DbType.Int32));
                        parameters.Add(new FilterParameter("@MerchantNumber", SecureQueryString["MerchantNumber"], DbType.AnsiString));
                        mode = "create";
                    }
                    if (issueList.Length > 0)
                    {
                        issueList.Remove(issueList.Length - 1, 1);
                        parameters.Add(new FilterParameter("@IssueList", issueList.ToString(), DbType.AnsiString));
                    }
                    else
                    {
                        parameters.Add(new FilterParameter("@IssueList", "", DbType.AnsiString));
                    }

                    WebServices.CsReportServices.ExecuteNonQueryCommand(spa, parameters, out tempParameters);
                    // End of Manage Ticket-----------------------------------------------

                    // Insert Comment--------------------------------------
                    parameters = new FilterParameterCollection();
                    parameters.AddLoggedInUserParams(-1);
                    
                    parameters.Add(new FilterParameter("@TicketNumber", int.Parse(uxTicketNumber.Text), DbType.Int32));
                    parameters.Add(new FilterParameter("@SiteID", int.Parse(SecureQueryString["SiteID"]), DbType.Int32));
                    if (comment.Length > 0)
                        parameters.Add(new FilterParameter("@CommentText", comment, DbType.String));
                    else
                    {
                        if (uxStatus.Text.ToLower() == "closed")
                            parameters.Add(new FilterParameter("@CommentText", "Ticket closed", DbType.String));
                        else if (uxStatus.Text.ToLower() == "open")
                            parameters.Add(new FilterParameter("@CommentText", "Ticket opened", DbType.String));
                    }
                    WebServices.CsReportServices.ExecuteNonQueryCommand("spa_cm_InsertCommentForTicketCaseManagement", parameters, out tempParameters);
                    // End of Comment--------------------------------------            
                    string queryStr = BuildSecureQueryString("mode=redirect&type=" + mode + "&TicketNumber=" + uxTicketNumber.Text + "&MerchantNumber=" + SecureQueryString["MerchantNumber"] + "&SiteID=" + SecureQueryString["SiteID"]);
                    AjaxAddResponseScript("setTimeout('OpenMessageWindow(\"CaseModal1.aspx?" + queryStr + "\")',500);");
                }
                else
                {
                    if (comment.Length > 0)
                    {
                        SessionManager.CurrentTicketComment = comment;
                    }
                    else
                    {
                        SessionManager.CurrentTicketComment = "";
                        if (uxStatus.Text.ToLower() == "closed")
                        {
                            SessionManager.CurrentTicketComment = "Ticket closed";
                        }
                        else
                        {
                            if (uxStatus.Text.ToLower() == "open")
                            {
                                SessionManager.CurrentTicketComment = "Ticket opened";
                            }
                        }
                    }

                    //ClientScript.RegisterStartupScript(this.GetType(), "myScript", "setTimeout('OpenMessageWindow(\"CaseModal2.aspx?" + BuildSecureQueryString("type=close&TicketNumber=" + SecureQueryString["TicketNumber"] + "&MerchantNumber=" + SecureQueryString["MerchantNumber"] + "&SiteID=" + SecureQueryString["SiteID"] + "&ato=" + uxUserAssigned.SelectedValue + "&res=" + uxResolution.SelectedValue + "&sta=" + uxStatus.SelectedValue + "&issueList=" + issueList.ToString()) + "\")',500);", true);
                    AjaxAddResponseScript("setTimeout('OpenMessageWindow(\"CaseModal2.aspx?" + BuildSecureQueryString("type=close&TicketNumber=" + SecureQueryString["TicketNumber"] + "&MerchantNumber=" + SecureQueryString["MerchantNumber"] + "&SiteID=" + SecureQueryString["SiteID"] + "&ato=" + uxUserAssigned.SelectedValue + "&res=" + uxResolution.SelectedValue + "&sta=" + uxStatus.SelectedValue + "&issueList=" + issueList.ToString()) + "\")',500);");
                }
                break;
        }
    }

    protected override void DoGridNeedDataSource(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if(sender == uxCommentList)
            OnDataBindControls(DataBindAction.BindCommentList);
    }

    //protected override void DoItemDataBound(AS.Controls.Grid.ASGrid sender, Telerik.Web.UI.GridItemEventArgs e)
    //{
        
    //}

    protected override void DoGridDataSourceReady(AS.Controls.Grid.ASGrid sender, EventArgs e)
    {
        DataTable dt = sender.DataSource as DataTable;
        if (dt.Rows.Count == 0)
        {
            uxCommentList.Visible = false;
        }
    }

    protected void uxStatus_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
    {
        BindResolution();
    }

    protected void uxSave_Click(object sender, EventArgs e)
    {
        OnPostBackActions(PostBackAction.Submit);
    }

    private void BindTicketInfo()
    {
        string spa = "spa_cm_GetRefValueCaseManagement";
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(-1);

        const string TICKET_NUMBER_PARA = "@TicketNumber";
        const string MODE_PARA = "@Mode";

        if (SecureQueryString["TicketNumber"] != null)  // for Edit case, we receive more information about Resolution and Status
        {
            uxEditPanel.Visible = true;  // Visible status and resolution combobox, also set visible the colums for table
            uxPanel1.Visible = true;
            uxPanel2.Visible = true;

            BindStatus();
            BindResolution();

            spa = "spa_cm_GetTicketInfoCaseManagement";
            parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            parameters.Add(new FilterParameter(TICKET_NUMBER_PARA, int.Parse(uxTicketNumber.Text), DbType.Int32));
            parameters.AddLanguageID();
            DataTable dt = WebServices.CsReportServices.GetReports(spa, parameters);
            uxOpenDate.Text = VeraCodeSolution.DoVeraCode(dt.Rows[0]["OpenedDate"].ToString());
            uxCloseDate.Text = VeraCodeSolution.DoVeraCode(dt.Rows[0]["ClosedDate"].ToString());
            uxLastUpdated.Text = VeraCodeSolution.DoVeraCode(dt.Rows[0]["LastUpdatedDate"].ToString());

            spa = "spa_cm_GetRefValueCaseManagement";
            parameters = new FilterParameterCollection();
            parameters.AddLoggedInUserParams(-1);
            parameters.Add(new FilterParameter(TICKET_NUMBER_PARA, int.Parse(uxTicketNumber.Text), DbType.Int32));
        }
        parameters.Add(new FilterParameter("@SiteID", int.Parse(SecureQueryString["SiteID"]), DbType.Int32));
        parameters.Add(new FilterParameter(MODE_PARA, "ASSIGNED", DbType.String));
        parameters.AddLanguageID();
        uxUserAssigned.DataSource = WebServices.CsReportServices.GetReports(spa, parameters);
        uxUserAssigned.DataBind();
        uxUserAssigned.ClearSelection();
        // Default set the first item in the list
        // If User is Client, we'll select the login user in the list, otherwise we'll select the first user in the list (sorted by alphabet)
        // In case of Editing, we'll select the assigned user (the first item of the result list)
        uxUserAssigned.SelectedIndex = 0;
        if (SecureQueryString["TicketNumber"] == null)  // for Create 
        {
            if (SessionManager.CurrentUserType != WebSiteEnums.UserHierarchyMode.AS && SessionManager.CurrentUserType != WebSiteEnums.UserHierarchyMode.CS)  // for Client
            {
                uxUserAssigned.FindItemByValue(SessionManager.CurrentUser.UserID).Selected = true;
            }
        }
    }

    private void BindIssue(RadTreeView uxControl, ref DataTable dt, string issueType)
    {
        string spa = "spa_cm_GetIssuesListForTicketCaseManagement";
        const string ISSUE_ID = "IssueID";
        const string ISSUE_NAME = "IssueName";
        const string PARENT_GROUP_ID = "ParentGroupID";

        DataTable tempDataTable;

        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(-1);
        parameters.Add(new FilterParameter("@IssueType", issueType, DbType.String));
        if (SecureQueryString["TicketNumber"] != null)
        {
            parameters.Add(new FilterParameter("@TicketNumber", int.Parse(SecureQueryString["TicketNumber"]), DbType.Int32));
        }
        parameters.Add(new FilterParameter("@SiteID", int.Parse(SecureQueryString["SiteID"]), DbType.Int32));
        tempDataTable = WebServices.CsReportServices.GetReports(spa, parameters);
        dt = tempDataTable.Copy();

        //Bind the issue to the Issue List.
        // 
        List<DataRow> listRows = new List<DataRow>();
        for (int i = 0; i < tempDataTable.Rows.Count; )
        {
            if (tempDataTable.Rows[i][PARENT_GROUP_ID].ToString() != string.Empty)
            {
                DataRow row = tempDataTable.NewRow();
                row.ItemArray = tempDataTable.Rows[i].ItemArray;
                listRows.Add(row);
                tempDataTable.Rows.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }
        for (int j = 0; j < tempDataTable.Rows.Count; j++)
        {
            if (tempDataTable.Rows[j][ISSUE_ID].ToString() != string.Empty)
            {
                RadTreeNode radNode = new RadTreeNode(VeraCodeSolution.ValidateResponseData(tempDataTable.Rows[j][ISSUE_NAME].ToString()), tempDataTable.Rows[j][ISSUE_ID].ToString());
                uxControl.Nodes.Add(radNode);
            }
            else
            {
                RadTreeNode radNodeParent = new RadTreeNode(VeraCodeSolution.ValidateResponseData(tempDataTable.Rows[j]["GroupName"].ToString()));
                for (int k = 0; k < listRows.Count; )
                {
                    if (listRows[k][PARENT_GROUP_ID].ToString() == tempDataTable.Rows[j]["GroupID"].ToString())
                    {
                        RadTreeNode radNodeChild = new RadTreeNode(VeraCodeSolution.ValidateResponseData(listRows[k][ISSUE_NAME].ToString()), listRows[k][ISSUE_ID].ToString());
                        radNodeParent.Nodes.Add(radNodeChild);
                        listRows.RemoveAt(k);
                    }
                    else
                    {
                        k++;
                    }
                }
                uxControl.Nodes.Add(radNodeParent);
            }
        }

        foreach (RadTreeNode node in uxControl.GetAllNodes())
        {
            uxTree_NodeDataBound(node, dt); // Bind Issue Status
        }
    }

    private void uxTree_NodeDataBound(RadTreeNode node, DataTable dt)
    {
        bool isEnabled = false;
        var query = from tempDt in dt.AsEnumerable()
                    where (tempDt["IssueID"].ToString() == node.Value && string.IsNullOrEmpty(tempDt["ParentGroupID"].ToString()))
                    select tempDt;
        isEnabled = (query.Count() == 0 ? false : true);
        node.Enabled = isEnabled;
    }

    private void BindData()
    {
        // Bind Issue for both Create and Update 
        BindIssue(uxTreeCS, ref _dtTreeCS, "CUSTOMERSERVICE");
        BindIssue(uxTreeAO, ref _dtTreeAO, "ACTIVATIONOUTCOME");
        BindIssue(uxTreeTD, ref _dtTreeTD, "TERMINALDOWNLOAD");
        BindTicketInfo();
        if (SecureQueryString["TicketNumber"] != null)  // for Update ticket
        {
            BindIssueStatus(uxTreeCS, _dtTreeCS); // Bind the issue's status of ticket
            BindIssueStatus(uxTreeAO, _dtTreeAO);
            BindIssueStatus(uxTreeTD, _dtTreeTD);
        }


        //uxMerchantInfo_FDR.MerchantNumber = SecureQueryString["MerchantNumber"];

        uxTreeCS.ExpandAllNodes();
        uxTreeCS.ShowLineImages = false;
        uxTreeAO.ExpandAllNodes();
        uxTreeAO.ShowLineImages = false;
        uxTreeTD.ExpandAllNodes();
        uxTreeTD.ShowLineImages = false;
    }

    private string GenerateTicketNumber()
    {
        string spa = "spa_cm_GetMaxTicketNumber";
        FilterParameterCollection parameters = new FilterParameterCollection();
        FilterParameterCollection outParameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient",SessionManager.CurrentUser.ASClient,DbType.Int32));
        parameters.Add(new FilterParameter("@TicketNumber", 0, DbType.Int32, true));
        WebServices.CsReportServices.ExecuteNonQueryCommand(spa, parameters, out outParameters);
        return outParameters[0].ParameterValue.ToString();
    }

    private void BindStatus()
    {
        string spa = "spa_cm_GetRefValueCaseManagement";
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(-1);
        parameters.Add(new FilterParameter("@Mode", "STATUS", DbType.String));
        parameters.Add(new FilterParameter("@TicketNumber", int.Parse(uxTicketNumber.Text), DbType.Int32));
        parameters.Add(new FilterParameter("@SiteID", int.Parse(SecureQueryString["SiteID"]), DbType.Int32));
        parameters.AddLanguageID();
        uxStatus.DataSource = WebServices.CsReportServices.GetReports(spa, parameters);
        uxStatus.DataBind();

        string selectedItem = uxStatus.SelectedItem.Text;
        //Select the approciate status in status list according to the current status
        switch (uxStatus.SelectedItem.Text)
        {
            case STATUS_NEW:  //Show Open and Closed status in list
                {
                    for (int i = 0; i < uxStatus.Items.Count; )
                    {
                        if (uxStatus.Items[i].Text != STATUS_OPEN && uxStatus.Items[i].Text != STATUS_CLOSED)
                        {
                            uxStatus.Items.Remove(i);
                        }
                        else
                        {
                            i++;
                        }
                    }
                    uxStatus.FindItemByText(STATUS_OPEN).Selected = true;

                }
                break;
            case STATUS_OPEN:  // Show Working, Pending and Closed status in list
            case STATUS_WORKING:
            case STATUS_PENDING:
                {
                    for (int i = 0; i < uxStatus.Items.Count; )
                    {
                        if (uxStatus.Items[i].Text != STATUS_WORKING && uxStatus.Items[i].Text != STATUS_PENDING && uxStatus.Items[i].Text != STATUS_CLOSED)
                        {
                            uxStatus.Items.Remove(i);
                        }
                        else
                        {
                            i++;
                        }
                    }
                }
                break;
            case STATUS_CLOSED: // Only show Closed status in list
                {
                    for (int i = 0; i < uxStatus.Items.Count; )
                    {
                        if (uxStatus.Items[i].Text != STATUS_CLOSED)
                        {
                            uxStatus.Items.Remove(i);
                        }
                        else
                        {
                            i++;
                        }
                    }
                }
                break;


        }
        // If status is Closed, then disable AssignedTo, Status and Resoultion combobox besides hide the input comment section and Submit button
        if (selectedItem == STATUS_CLOSED)
        {
            uxCommentPanel.Visible = false;
            uxSave.Visible = false;
            uxUserAssigned.Enabled = false;
            uxStatus.Enabled = false;
            uxResolution.Enabled = false;
        }
    }

    private void BindResolution()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        string spa = "spa_cm_GetResolutionByStatus";
        parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(-1);
        parameters.Add(new FilterParameter("@StatusID", int.Parse(uxStatus.SelectedValue), DbType.String));
        parameters.Add(new FilterParameter("@TicketNumber", int.Parse(SecureQueryString["TicketNumber"]), DbType.String)); ;
        parameters.Add(new FilterParameter("@SiteID", int.Parse(SecureQueryString["SiteID"]), DbType.Int32));
        parameters.AddLanguageID();
        uxResolution.DataSource = WebServices.CsReportServices.GetReports(spa, parameters);
        uxResolution.DataBind();
        string temp = uxResolution.Items[0].Text;

        // If the first item in list is not empty string, it means this ticket has resolution before. So we will select it.
        // We replace the empty string by the Blank Item in the list.
        if (uxStatus.SelectedItem.Text != STATUS_CLOSED)
        {
            if (temp != string.Empty)
            {
                uxResolution.Items[1].Text = Server.HtmlDecode("&nbsp;");
                uxResolution.Items.Remove(0);
                uxResolution.Items.FindItemByText(temp).Selected = true;
            }
            else
            {
                uxResolution.Items[0].Text = Server.HtmlDecode("&nbsp;");
            }
        }

    }

    private void BindIssueStatus(RadTreeView uxControl, DataTable dt)
    {
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i]["Checked"].ToString() == "1")  // the current node is checked in database
            {
                RadTreeNode node = uxControl.FindNodeByValue(dt.Rows[i]["IssueID"].ToString());
                if (node.Level != 0)  // If the node is a child of other node, we will enable/check the parent node and enable all the child nodes
                {
                    RadTreeNode parentNode = node.ParentNode;
                    parentNode.Enabled = true;
                    parentNode.Checked = true;
                    for (int j = 0; j < parentNode.Nodes.Count; j++)
                    {
                        parentNode.Nodes[j].Enabled = true;
                    }
                }

                node.Checked = true;  // Check the current node.

            }
        }
    }


}
